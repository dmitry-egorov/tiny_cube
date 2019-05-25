using JetBrains.Annotations;
using Plugins.Lanski.Subjective;
using UnityEngine;

namespace DefaultNamespace
{
    [UsedImplicitly]
    public class _DebugMechanics: SSystem
    {
        public override void Register()
        {
            OnPresentation()
            .Do(() =>
            {
                if (!GetKeyDown(KeyCode.P)) return;

                Debug.Break();
            })
            ;
        }
    }
}