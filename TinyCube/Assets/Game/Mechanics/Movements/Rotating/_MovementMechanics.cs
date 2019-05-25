using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _MovementMechanics
    {
        void Remember_last_rotation() =>
            OnGameplay()
            .Do((Has_rotation r, Interpolates_rotation ir) =>
            {
                ir.LastRotation = r.Rotation;
            })
        ;

        void Apply_interpolated_rotation() =>
            OnPresentation()
            .Do((Has_rotation rd, Interpolates_rotation ir) =>
            {
                var lr = ir.LastRotation;
                var cr = rd.Rotation;
                var r = PresentationTimeRatio;
                
                Transform.rotation = Quaternion.Lerp(lr, cr, r);
            })
        ;
    }
}