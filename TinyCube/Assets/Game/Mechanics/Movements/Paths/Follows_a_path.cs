using System;
using Plugins.Lanski.FunctionalExtensions;
using Plugins.Lanski.Subjective;
using Plugins.UnityExtensions;
using UnityEngine;
using UnityEngine.Serialization;
using static Direction;

[RequireComponent(typeof(Can_follow_a_path))]
public class Follows_a_path: SubjectComponent
{
    public Marks_a_waypoint_level path;
    public float distance;
    public Direction direction;

    public void Reverse()
    {
        direction = direction == Forward ? Backward : Forward;
    }

    public Vector3 GetPosition() => path.GetPositionAt(distance);
    public float GetHeight() => path.GetHeightAt(distance);
    public float GetLength() => path.GetLength();
    public Quaternion GetRotation() => path.GetRotationFor(direction);

//    public bool TryGetConnectedAt(PathSide side, out Marks_a_path new_path, out PathSide new_path_side) => 
//        path.TryGetConnectedAt(side, out new_path, out new_path_side)
//    ;

//    public bool TryGetConnectedPointAt(PathSide side, out Marks_a_point point) => 
//        path.TryGetConnectedPointAt(side, out point)
//    ;

    public bool TryGetWalkedBeyond(out PathSide side, out float extra_distance)
    {
        var /* path's length */ pl = GetLength();
                    
        var d = distance;
        var /* walked past end */   wpe = d > pl;
        var /* walked past start */ wpb = d < 0;

        if (!wpe && !wpb)
        {
            side = default;
            extra_distance = default;
            return false;
        }
                    
        extra_distance = wpe ? d - pl : -d;
        side = wpe ? PathSide.End : PathSide.Start;
        return true;
    }

    public void SetReverseDistance(float ed)
    {
        distance = path.GetLength() - ed;
    }

    public void SetDistance(float ed)
    {
        distance = ed;
    }

    public void ResetToStart()
    {
        distance = 0;
    }

    public void ResetToEnd()
    {
        distance = GetLength();
    }

    public void SetDistanceFrom(PathSide side, float ed)
    {
        switch (side)
        {
            case PathSide.Start:
                SetDistance(ed);
                break;
            case PathSide.End:
                SetReverseDistance(ed);
                break;
            default:
                throw new ArgumentException("Unknown value", nameof(side));
        }
    }

    public void ResetTo(PathSide side)
    {
        switch (side)
        {
            case PathSide.Start:
                ResetToStart();
                break;
            case PathSide.End:
                ResetToEnd();
                break;
            default:
                throw new ArgumentException("Unknown value", nameof(side));
        }
    }
}