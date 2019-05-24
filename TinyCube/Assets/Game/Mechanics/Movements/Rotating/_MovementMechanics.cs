using Plugins.UnityExtensions;
using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _MovementMechanics
    {
        void Remember_last_rotation() =>
            OnGameplay()
            .Do((Rotated r, Interpolates_rotation ir) =>
            {
                ir.LastRotation = r.Rotation;
            })
        ;

        void Apply_interpolated_rotation() =>
            OnPresentation()
            .Do((Rotated rd, Interpolates_rotation ir) =>
            {
                var lr = ir.LastRotation;
                var cr = rd.Rotation;
                var r = PresentationTimeRatio;
                
                Transform.rotation = Quaternion.Lerp(lr, cr, r);
            })
        ;
    }
}