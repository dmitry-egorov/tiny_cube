using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _Movement
    {
        void Remember_last_rotation() =>
            Do((Has_rotation r, Interpolates_rotation ir) =>
            {
                ir.LastRotation = r.rotation;
            })
        ;

        void Apply_interpolated_rotation() =>
            Do((Has_rotation rd, Interpolates_rotation ir) =>
            {
                var lr = ir.LastRotation;
                var cr = rd.rotation;
                var r = presentation_time_ratio;
                
                transform.rotation = Quaternion.Lerp(lr, cr, r);
            })
        ;
    }
}