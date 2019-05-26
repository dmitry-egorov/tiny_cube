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
                if (!GetKeyDown(KeyCode.P)) return;

                Debug.Break();
            })
            ;
        }
    }
}