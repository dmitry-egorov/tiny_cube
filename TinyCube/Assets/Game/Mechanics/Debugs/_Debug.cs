using JetBrains.Annotations;
using Plugins.Lanski.Subjective;
using UnityEngine;

namespace DefaultNamespace
{
    [UsedImplicitly]
    public class _Debug: SSystem
    {
        public override void Register()
        {
            Presentation();
            Do(() =>
            {
                if (!get_input_key_down(KeyCode.P)) return;

                Debug.Break();
            })
            ;
        }
    }
}