using Plugins.UnityExtensions;
using UnityEngine;

namespace Plugins.Lanski.Subjective
{
    internal class MouseDragWatcher
    {
        public void update()
        {
            if (_key_watcher.get_key_down())
            {
                _start_point = Input.mousePosition.xy();
            }
            
            _key_watcher.update();
        }

        public bool try_get_complete(out MouseDrag d)
        {
            if (_key_watcher.get_key_up())
            {
                d = new MouseDrag()
                {
                    start_position = _start_point.Value,
                    end_position = Input.mousePosition.xy()
                };
                return true;
            }

            d = default;
            return false;
        }

        Vector2? _start_point;
        GameplayKeyWatcher _key_watcher = new GameplayKeyWatcher(KeyCode.Mouse0);
    }
}