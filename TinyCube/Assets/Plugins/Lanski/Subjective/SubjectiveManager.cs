using System;
using System.Collections.Generic;
using Plugins.Lanski.ExtraCollections;
using Plugins.Lanski.Reflections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Profiling;

namespace Plugins.Lanski.Subjective
{
    public class SubjectiveManager : MonoBehaviour
    {
        public int GameplayFramerate;
        public int PresentationFramerate;

        void Start()
        {
            if (PresentationFramerate != 0)
            {
                Application.targetFrameRate = PresentationFramerate;
            }
        }
    
        void Update()
        {
            var fdt = GameplayFramerate != 0 ? 1.0f / GameplayFramerate : Time.fixedDeltaTime;
            var dt = Time.deltaTime;
            Execute(dt, fdt);
        }
        
        internal static Subject subject => _subject;

        internal static float delta_time => _deltaTime;
        internal static float presentation_time_ratio
        {
            get
            {
                Assert.IsTrue(_currentMechanicStage == MechanicStage.Presentation);
                return _presentationTimeRatio;
            }
        }

        internal static MechanicStage CurrentMechanicStage => _currentMechanicStage;

        internal static ShuffleList<Subject>.Token Register(Subject subject)
        {
            if (_subjects == null) _subjects = new ShuffleList<Subject>();
            return _subjects.Add(subject);
        }
    
        internal static void Unregister(ShuffleList<Subject>.Token token)
        {
            Assert.IsNotNull(_subjects);
            _subjects.Remove(token);
        }

        internal static void SetToGameplayRegistration() => _currentRegistrationStage = MechanicStage.Gameplay;

        internal static void SetToPresentationRegistration() => _currentRegistrationStage = MechanicStage.Presentation;

        internal static void Register
        (
            string name, 
            Action action,
            TypeMask included, 
            TypeMask excluded
        )
        {
            Assert.IsNotNull(_gameplayMechanics);
            Assert.IsNotNull(_presentationMechanics);

            SelectMechanicsListForRegistration().Add(new RegisteredMechanic()
            {
                action = action,
                name = name,
                included = included,
                excluded = excluded
            });;
        }

        internal static void Register<T1>
        (
            string name, 
            Action<T1> action,
            TypeMask included, 
            TypeMask excluded
        )
            where T1 : SubjectComponent
        {
            included.add<T1>();
            Register(name, () =>
                {
                    var c1 = subject.get<T1>();

                    action(c1);
                },
                included, 
                excluded
            );
        }
    
        internal static void Register<T1, T2>
        (
            string name, 
            Action<T1, T2> action,
            TypeMask included, 
            TypeMask excluded
        )
            where T1 : SubjectComponent
            where T2 : SubjectComponent
        {
            included.add<T1>();
            included.add<T2>();
            Register(name, () =>
                {
                    var c1 = subject.get<T1>();
                    var c2 = subject.get<T2>();

                    action(c1, c2);
                },
                included, 
                excluded
            );
        }
    
        internal static void Register<T1, T2, T3>
        (
            string name, 
            Action<T1, T2, T3> action,
            TypeMask included, 
            TypeMask excluded
        )
            where T1 : SubjectComponent
            where T2 : SubjectComponent
            where T3 : SubjectComponent
        {
            included.add<T1>();
            included.add<T2>();
            included.add<T3>();
            Register(name, () =>
                {
                    var c1 = subject.get<T1>();
                    var c2 = subject.get<T2>();
                    var c3 = subject.get<T3>();

                    action(c1, c2, c3);
                },
                included, 
                excluded
            );
        }
    
        internal static void Register<T1, T2, T3, T4>
        (
            string name, 
            Action<T1, T2, T3, T4> action,
            TypeMask included, 
            TypeMask excluded
        )
            where T1 : SubjectComponent
            where T2 : SubjectComponent
            where T3 : SubjectComponent
            where T4 : SubjectComponent
        {
            included.add<T1>();
            included.add<T2>();
            included.add<T3>();
            included.add<T4>();
            
            Register(name, () =>
                {
                    var c1 = subject.get<T1>();
                    var c2 = subject.get<T2>();
                    var c3 = subject.get<T3>();
                    var c4 = subject.get<T4>();

                    action(c1, c2, c3, c4);
                },
                included, 
                excluded
            );
        }

        internal static void Execute(float dt, float fdt)
        {
            Initialize();
        
            if (_subjects == null)
                return;

            _presentationTime += Time.deltaTime;

            ExecuteGameplayMechanics(fdt);
            ExecutePresentationMechanics(dt, fdt);
        }
        
        static void ExecuteGameplayMechanics(float fdt)
        {
            _deltaTime = fdt;
            _currentMechanicStage = MechanicStage.Gameplay;

            try
            {
                while (_gameplayTime < _presentationTime)
                {
                    Profiler.BeginSample("Gameplay Mechanics");
                    try
                    {
                        ExecuteMechanics(_gameplayMechanics);

                        SubjectiveInput.Update();
                    }
                    finally
                    {
                        Profiler.EndSample();
                    
                        _gameplayTime += fdt;
                    }
                }
            }
            finally
            {
                _currentMechanicStage = MechanicStage.Unknown;
            }
        }

        static void ExecutePresentationMechanics(float dt, float fdt)
        {
            Profiler.BeginSample("Presentation Mechanics");
            try
            {
                _currentMechanicStage = MechanicStage.Presentation;
                _deltaTime = dt;
                _presentationTimeRatio = (float)((_presentationTime - (_gameplayTime - fdt)) / fdt);
                    
                ExecuteMechanics(_presentationMechanics);
            }
            finally
            {
                _currentMechanicStage = MechanicStage.Unknown;
                Profiler.EndSample();
            }
        }

        static void ExecuteMechanics(List<RegisteredMechanic> mechanics)
        {
            foreach (var m in mechanics)
            {
                Profiler.BeginSample(m.name);
                try
                {
                    foreach (var s in _subjects)
                    {
                        _subject = s;

                        if (!s.contains_all(m.included))
                            continue;
                        if (s.contains_any(m.excluded))
                            continue;

                        m.action();
                    }
                }
                finally
                {
                    Profiler.EndSample();
                }
            }
        }

        static List<RegisteredMechanic> SelectMechanicsListForRegistration() => 
            _currentRegistrationStage == MechanicStage.Gameplay ? _gameplayMechanics : _presentationMechanics
        ;

        static void Initialize()
        {
            if (_gameplayMechanics != null) return;
        
            
            _gameplayMechanics = new List<RegisteredMechanic>();
            _presentationMechanics = new List<RegisteredMechanic>();

            var /* late systems */ ls = new List<ISystem>();
            foreach (var s in AppDomain.CurrentDomain.InstantiateAllDerivedTypes<ISystem>())
            {
                if (s.GetType().IsDefined(typeof(LateAttribute), false))
                {
                    ls.Add(s);
                    continue;
                }
                
                SetToPresentationRegistration();
                s.Register();
            }

            foreach (var s in ls)
            {
                SetToPresentationRegistration();
                s.Register();
            }
        }

        static ShuffleList<Subject> _subjects;
        static List<RegisteredMechanic> _gameplayMechanics;
        static List<RegisteredMechanic> _presentationMechanics;
        static MechanicStage _currentRegistrationStage;

        static MechanicStage _currentMechanicStage;
        static float _presentationTimeRatio;
        static float _deltaTime;
        static double _presentationTime;
        static double _gameplayTime;
        static Subject _subject;

        struct RegisteredMechanic
        {
            public string name;
            public Action action;
            public TypeMask included;
            public TypeMask excluded;
        }
    }
}
