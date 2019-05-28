using Plugins.Lanski.Subjective;
using UnityEngine;

namespace Game.Mechanics.Movements.Rotating
{
    public partial class _RotatingMechanics: SubjectiveMechanics
    {
        public static void remember_last_rotation() =>
        act((Has_rotation r, Interpolates_rotation ir) =>
        {
            ir.LastRotation = r.rotation;
        });

        public static void apply_interpolated_rotation() =>
        act((Has_rotation rd, Interpolates_rotation ir) =>
        {
            var lr = ir.LastRotation;
            var cr = rd.rotation;
            var r = presentation_time_ratio;
            
            transform.rotation = Quaternion.Lerp(lr, cr, r);
        });
    }
}