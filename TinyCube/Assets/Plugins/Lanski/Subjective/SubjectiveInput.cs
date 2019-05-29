using System.Collections.Generic;
using UnityEngine;

namespace Plugins.Lanski.Subjective
{
    internal static class SubjectiveInput
    {
        internal static void update()
        {
            foreach (var keyWatcher in key_watchers)
            {
                keyWatcher.update();
            }

            _drag_watcher?.update();
        }

        internal static bool get_key(KeyCode key) => Input.GetKey(key);

        internal static bool get_key_down(KeyCode key) =>
            SubjectiveManager.CurrentMechanicStage != MechanicStage.Gameplay 
            ? Input.GetKeyDown(key) 
            : key_watcher_for(key).get_key_down()
        ;

        internal static bool try_get_mouse_drag_complete(out MouseDrag d) => drag_watcher.try_get_complete(out d);


        static GameplayKeyWatcher key_watcher_for(KeyCode key)
        {
            if (!key_watchers_map.TryGetValue(key, out var w))
            {
                w = new GameplayKeyWatcher(key);
                key_watchers_map.Add(key, w);
                key_watchers.Add(w);
            }
            return w;
        }

        static List<GameplayKeyWatcher> key_watchers => 
            _key_watchers ?? (_key_watchers = new List<GameplayKeyWatcher>())
        ;

        static Dictionary<KeyCode, GameplayKeyWatcher> key_watchers_map => 
            _key_watchers_map ?? (_key_watchers_map = new Dictionary<KeyCode, GameplayKeyWatcher>())
        ;

        static MouseDragWatcher drag_watcher => _drag_watcher ?? (_drag_watcher = new MouseDragWatcher()); 

        static Dictionary<KeyCode, GameplayKeyWatcher> _key_watchers_map ;
        static List<GameplayKeyWatcher> _key_watchers;
        static MouseDragWatcher _drag_watcher;
    }
}