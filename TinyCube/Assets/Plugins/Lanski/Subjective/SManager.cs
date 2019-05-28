using System;
using System.Collections.Generic;
using Plugins.Lanski.ExtraCollections;
using Plugins.Lanski.Reflections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Profiling;

namespace Plugins.Lanski.Subjective
{
    internal static class SManager
    {
        public static Subject Subject => _subject;

        public static float DeltaTime => _deltaTime;
        public static float PresentationTimeRatio
        {
            get
            {
                Assert.IsTrue(_currentMechanicStage == MechanicStage.Presentation);
                return _presentationTimeRatio;
            }
        }

        public static bool GetKey(KeyCode key) => Input.GetKey(key);
        public static bool GetKeyDown(KeyCode key)
        {
            if (_currentMechanicStage != MechanicStage.Gameplay)
                return Input.GetKeyDown(key);

            return GetOrCreateKeyWatcher(key).GetKeyDown();
        }

        public static ShuffleList<Subject>.Token Register(Subject subject)
        {
            if (_subjects == null) _subjects = new ShuffleList<Subject>();
            return _subjects.Add(subject);
        }
    
        public static void Unregister(ShuffleList<Subject>.Token token)
        {
            Assert.IsNotNull(_subjects);
            _subjects.Remove(token);
        }
        
        public static void SetToGameplayRegistration() => _currentRegistrationStage = MechanicStage.Gameplay;

        public static void SetToPresentationRegistration() => _currentRegistrationStage = MechanicStage.Presentation;

        public static void Register
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

        public static void Register<T1>
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
                    var c1 = Subject.get<T1>();

                    action(c1);
                },
                included, 
                excluded
            );
        }
    
        public static void Register<T1, T2>
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
                    var c1 = Subject.get<T1>();
                    var c2 = Subject.get<T2>();

                    action(c1, c2);
                },
                included, 
                excluded
            );
        }
    
        public static void Register<T1, T2, T3>
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
                    var c1 = Subject.get<T1>();
                    var c2 = Subject.get<T2>();
                    var c3 = Subject.get<T3>();

                    action(c1, c2, c3);
                },
                included, 
                excluded
            );
        }
    
        public static void Register<T1, T2, T3, T4>
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
                    var c1 = Subject.get<T1>();
                    var c2 = Subject.get<T2>();
                    var c3 = Subject.get<T3>();
                    var c4 = Subject.get<T4>();

                    action(c1, c2, c3, c4);
                },
                included, 
                excluded
            );
        }

        public static void Execute(float dt, float fdt)
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

                        foreach (var keyWatcher in _keyWatchers)
                        {
                            keyWatcher.Update();
                        }
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

        static GameplayKeyWatcher GetOrCreateKeyWatcher(KeyCode key)
        {
            if (!_keyWatchersMap.TryGetValue(key, out var w))
            {
                w = new GameplayKeyWatcher(key);
                _keyWatchersMap.Add(key, w);
                _keyWatchers.Add(w);
            }
            return w;
        }

        static void Initialize()
        {
            if (_gameplayMechanics != null) return;
        
            _keyWatchersMap = new Dictionary<KeyCode, GameplayKeyWatcher>();
            _keyWatchers = new List<GameplayKeyWatcher>();
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
        static Dictionary<KeyCode, GameplayKeyWatcher> _keyWatchersMap;
        static List<GameplayKeyWatcher> _keyWatchers;
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