using UnityEngine;

namespace Plugins.Lanski.Subjective
{
    internal class GameplayKeyWatcher
    {
        public GameplayKeyWatcher(KeyCode key)
        {
            _key = key;
        }

        public void Update()
        {
            _keyWasHeldLastUpdate = Input.GetKey(_key);
        }
        
        public bool GetKeyDown()
        {
            return Input.GetKey(_key) && !_keyWasHeldLastUpdate;
        }

        bool _keyWasHeldLastUpdate;
        readonly KeyCode _key;
    }
}