using UnityEngine;

namespace Plugins.Lanski.Subjective
{
    internal class GameplayKeyWatcher
    {
        public GameplayKeyWatcher(KeyCode key)
        {
            _key = key;
        }

        public void update()
        {
            _keyWasHeldLastUpdate = Input.GetKey(_key);
        }
        
        public bool get_key_down()
        {
            return Input.GetKey(_key) && !_keyWasHeldLastUpdate;
        }

        public bool get_key_up() => !Input.GetKey(_key) && _keyWasHeldLastUpdate;

        bool _keyWasHeldLastUpdate;
        readonly KeyCode _key;
    }
}