using JetBrains.Annotations;
using Plugins.Lanski.Subjective;
using UnityEngine;

namespace DefaultNamespace
{
    [UsedImplicitly]
    public class DebugSystem: SubjectiveSystem
    {
        public override void Register()
        {
            presentation();
            act(() =>
            {
                if (!key_press_started(KeyCode.P)) return;

                Debug.Break();
            })
            ;
        }
    }
}