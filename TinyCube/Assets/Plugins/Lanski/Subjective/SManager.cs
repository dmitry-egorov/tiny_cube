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

        public static int Register(Subject subject)
        {
            if (_subjects == null) _subjects = new ShuffleList<Subject>();
            return _subjects.Add(subject);
        }
    
        public static void Unregister(int index)
        {
            Assert.IsNotNull(_subjects);
            _subjects.RemoveAt(index);
        }
        
        public static void Register
        (
            MechanicStage stage, 
            string name, 
            IEnumerable<Type> included,
            IEnumerable<Type> excluded, 
            Action action
        )
        {
            Assert.IsNotNull(_gameplayMechanics);
            Assert.IsNotNull(_presentationMechanics);

            var inc = included.ToArray();
            var exc = excluded.ToArray();
        
            SelectMechanicsList(stage).Add((name, () =>
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
            MechanicStage stage, 
            string name, 
            IEnumerable<Type> included,
            IEnumerable<Type> excluded, 
            Action<T1> action
        )
            where T1 : SubjectComponent
        {
            Assert.IsNotNull(_gameplayMechanics);
            Assert.IsNotNull(_presentationMechanics);

            var inc = included.ToArray();
            var exc = excluded.ToArray();
        
            SelectMechanicsList(stage).Add((name, () =>
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
            MechanicStage stage, 
            string name, 
            IEnumerable<Type> included, 
            IEnumerable<Type> excluded, 
            Action<T1, T2> action
        )
            where T1 : SubjectComponent
            where T2 : SubjectComponent
        {
            Assert.IsNotNull(_gameplayMechanics);
            Assert.IsNotNull(_presentationMechanics);
        
            var inc = included.ToArray();
            var exc = excluded.ToArray();
        
            SelectMechanicsList(stage)
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
            MechanicStage stage, 
            string name, 
            IEnumerable<Type> included, 
            IEnumerable<Type> excluded, 
            Action<T1, T2, T3> action
        )
            where T1 : SubjectComponent
            where T2 : SubjectComponent
            where T3 : SubjectComponent
        {
            Assert.IsNotNull(_gameplayMechanics);
            Assert.IsNotNull(_presentationMechanics);
        
            var inc = included.ToArray();
            var exc = excluded.ToArray();
        
            SelectMechanicsList(stage)
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

        public static void Execute(float dt, float fdt)
        {
            Initialize();
        
            if (_subjects == null)
                return;

            _presentationTime += Time.deltaTime;

            ExecuteGameplayMechanics();
            ExecutePresentationMechanics();

            void ExecuteGameplayMechanics()
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
            
            void ExecutePresentationMechanics()
            {
                Profiler.BeginSample("Presentation Mechanics");

                _currentMechanicStage = MechanicStage.Presentation;
                _deltaTime = dt;
                _presentationTimeRatio = (float)((_presentationTime - (_gameplayTime - fdt)) / fdt);

                try
                {
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
        }

        static List<(string name, Action action)> SelectMechanicsList(MechanicStage stage) => 
            stage == MechanicStage.Gameplay ? _gameplayMechanics : _presentationMechanics
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
            foreach (var s in AppDomain.CurrentDomain.InstantiateAllDerivedTypes<ISystem>())
            {
                s.Register();
            }
        }

        static ShuffleList<Subject> _subjects;
        static List<(string name, Action action)> _gameplayMechanics;
        static List<(string name, Action action)> _presentationMechanics;
        static Dictionary<KeyCode, GameplayKeyWatcher> _keyWatchersMap;
        static List<GameplayKeyWatcher> _keyWatchers;

        static MechanicStage _currentMechanicStage;
        static float _presentationTimeRatio;
        static float _deltaTime;
        static double _presentationTime;
        static double _gameplayTime;
    }
}