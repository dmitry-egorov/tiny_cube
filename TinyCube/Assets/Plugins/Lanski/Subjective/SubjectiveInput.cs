using System.Collections.Generic;
using UnityEngine;

namespace Plugins.Lanski.Subjective
{
    internal static class SubjectiveInput
    {
        static GameplayKeyWatcher GetOrCreateKeyWatcher(KeyCode key)
        {
            _keyWatchersMap = _keyWatchersMap ?? new Dictionary<KeyCode, GameplayKeyWatcher>();
            _keyWatchers = _keyWatchers ?? new List<GameplayKeyWatcher>();
            
            if (!_keyWatchersMap.TryGetValue(key, out var w))
            {
                w = new GameplayKeyWatcher(key);
                _keyWatchersMap.Add(key, w);
                _keyWatchers.Add(w);
            }
            return w;
        }
        
        static Dictionary<KeyCode, GameplayKeyWatcher> _keyWatchersMap ;
        static List<GameplayKeyWatcher> _keyWatchers;

        public static void Update()
        {
            foreach (var keyWatcher in _keyWatchers)
            {
                keyWatcher.Update();
            }
        }

        internal static bool GetKey(KeyCode key) => Input.GetKey(key);

        internal static bool GetKeyDown(KeyCode key)
        {
            if (SubjectiveManager.CurrentMechanicStage != MechanicStage.Gameplay)
                return Input.GetKeyDown(key);

            return GetOrCreateKeyWatcher(key).GetKeyDown();
        }
    }
}