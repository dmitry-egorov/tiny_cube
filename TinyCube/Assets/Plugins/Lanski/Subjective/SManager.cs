using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.Lanski.ArrayExtensions;
using Plugins.Lanski.ExtraCollections;
using Plugins.Lanski.Reflections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Profiling;

namespace Plugins.Lanski.Subjective
{
    internal static class SManager
    {
        public static Subject Subject {get; private set;}

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
            IEnumerable<Type> included = null, 
            IEnumerable<Type> excluded = null
        )
        {
            Assert.IsNotNull(_gameplayMechanics);
            Assert.IsNotNull(_presentationMechanics);

            var inc = (included ?? Enumerable.Empty<Type>()).ToArray();
            var exc = (excluded ?? Enumerable.Empty<Type>()).ToArray();
        
            SelectMechanicsListForRegistration().Add((name, () =>
            {
                if (inc.QAny(t => !Subject.Has(t)))
                    return;
                if (exc.QAny(t => Subject.Has(t)))
                    return;

                action();
            }));
        }

        public static void Register<T1>
        (
            string name, 
            Action<T1> action,
            IEnumerable<Type> included = null, 
            IEnumerable<Type> excluded = null
        )
            where T1 : SubjectComponent
        {
            Assert.IsNotNull(_gameplayMechanics);
            Assert.IsNotNull(_presentationMechanics);

            var inc = (included ?? Enumerable.Empty<Type>()).ToArray();
            var exc = (excluded ?? Enumerable.Empty<Type>()).ToArray();
        
            SelectMechanicsListForRegistration().Add((name, () =>
            {
                if (inc.QAny(t => !Subject.Has(t)))
                    return;
                if (exc.QAny(t => Subject.Has(t)))
                    return;

                if (!Subject.TryGet(out T1 c1))
                    return;

                action(c1);
            }));
        }
    
        public static void Register<T1, T2>
        (
            string name, 
            Action<T1, T2> action,
            IEnumerable<Type> included = null, 
            IEnumerable<Type> excluded = null
        )
            where T1 : SubjectComponent
            where T2 : SubjectComponent
        {
            Assert.IsNotNull(_gameplayMechanics);
            Assert.IsNotNull(_presentationMechanics);
        
            var inc = (included ?? Enumerable.Empty<Type>()).ToArray();
            var exc = (excluded ?? Enumerable.Empty<Type>()).ToArray();
        
            SelectMechanicsListForRegistration()
            .Add((name, () =>
            {
                if (inc.QAny(t => !Subject.Has(t)))
                    return;
                if (exc.QAny(t => Subject.Has(t)))
                    return;

                if (!Subject.TryGet(out T1 c1))
                    return;
                if (!Subject.TryGet(out T2 c2))
                    return;

                action(c1, c2);
            }));
        }
    
        public static void Register<T1, T2, T3>
        (
            string name, 
            Action<T1, T2, T3> action, 
            IEnumerable<Type> included = null, 
            IEnumerable<Type> excluded = null
        )
            where T1 : SubjectComponent
            where T2 : SubjectComponent
            where T3 : SubjectComponent
        {
            Assert.IsNotNull(_gameplayMechanics);
            Assert.IsNotNull(_presentationMechanics);
        
            var inc = (included ?? Enumerable.Empty<Type>()).ToArray();
            var exc = (excluded ?? Enumerable.Empty<Type>()).ToArray();
        
            SelectMechanicsListForRegistration()
            .Add((name, () =>
            {
                if (inc.QAny(t => !Subject.Has(t)))
                    return;
                if (exc.QAny(t => Subject.Has(t)))
                    return;

                if (!Subject.TryGet(out T1 c1))
                    return;
                if (!Subject.TryGet(out T2 c2))
                    return;
                if (!Subject.TryGet(out T3 c3))
                    return;

                action(c1, c2, c3);
            }));
        }
    
        public static void Register<T1, T2, T3, T4>
        (
            string name, 
            Action<T1, T2, T3, T4> action, 
            IEnumerable<Type> included = null, 
            IEnumerable<Type> excluded = null
        )
            where T1 : SubjectComponent
            where T2 : SubjectComponent
            where T3 : SubjectComponent
            where T4 : SubjectComponent
        {
            Assert.IsNotNull(_gameplayMechanics);
            Assert.IsNotNull(_presentationMechanics);
        
            var inc = (included ?? Enumerable.Empty<Type>()).ToArray();
            var exc = (excluded ?? Enumerable.Empty<Type>()).ToArray();
        
            SelectMechanicsListForRegistration()
            .Add((name, () =>
            {
                if (inc.QAny(t => !Subject.Has(t)))
                    return;
                if (exc.QAny(t => Subject.Has(t)))
                    return;

                if (!Subject.TryGet(out T1 c1))
                    return;
                if (!Subject.TryGet(out T2 c2))
                    return;
                if (!Subject.TryGet(out T3 c3))
                    return;
                if (!Subject.TryGet(out T4 c4))
                    return;

                action(c1, c2, c3, c4);
            }));
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
                        foreach (var m in _gameplayMechanics)
                        {
                            Profiler.BeginSample(m.name);
                            try
                            {
                                foreach (var s in _subjects)
                                {
                                    Subject = s;
                                    m.action();
                                }
                            }
                            finally
                            {
                                Profiler.EndSample();
                            }
                        }

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
                    
                foreach (var m in _presentationMechanics)
                {
                    Profiler.BeginSample(m.name);
                    try
                    {
                        foreach (var s in _subjects)
                        {
                            Subject = s;
                            m.action();
                        }
                    }
                    finally
                    {
                        Profiler.EndSample();
                    }
                }
            }
            finally
            {
                _currentMechanicStage = MechanicStage.Unknown;
                Profiler.EndSample();
            }
        }

        static List<(string name, Action action)> SelectMechanicsListForRegistration() => 
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
            _gameplayMechanics = new List<(string, Action)>();
            _presentationMechanics = new List<(string, Action)>();

            var late_systems = new List<ISystem>();
            foreach (var s in AppDomain.CurrentDomain.InstantiateAllDerivedTypes<ISystem>())
            {
                if (s.GetType().IsDefined(typeof(LateAttribute), false))
                {
                    late_systems.Add(s);
                    continue;
                }
                
                SetToPresentationRegistration();
                s.Register();
            }

            foreach (var s in late_systems)
            {
                SetToPresentationRegistration();
                s.Register();
            }
        }

        static ShuffleList<Subject> _subjects;
        static List<(string name, Action action)> _gameplayMechanics;
        static List<(string name, Action action)> _presentationMechanics;
        static Dictionary<KeyCode, GameplayKeyWatcher> _keyWatchersMap;
        static List<GameplayKeyWatcher> _keyWatchers;
        static MechanicStage _currentRegistrationStage;

        static MechanicStage _currentMechanicStage;
        static float _presentationTimeRatio;
        static float _deltaTime;
        static double _presentationTime;
        static double _gameplayTime;
    }
}