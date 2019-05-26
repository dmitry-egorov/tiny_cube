using System;
using Plugins.Lanski.Subjective;
using Plugins.UnityExtensions;
using UnityEngine;

public class Marks_a_path: MarkingComponent
{
    public Marks_a_point start;
    public Marks_a_point end;

    public Vector3 GetPositionAt(float distance)
    {
        var d = GetDirection();
        var sp = start.GetPosition();
        return sp + d * distance;
    }
    
    public float GetHeightAt(float distance) => GetPositionAt(distance).y;

    public Vector3 GetDirection() => GetOffset().normalized;

    public Vector3 GetOffset()
    {
        var sp = start.GetPosition();
        var ep = end.GetPosition();
        return ep - sp;
    }

    public Quaternion GetRotationFor(Direction od)
    {
        var d = GetDirection();
        var s = od == Direction.Forward ? 1f : -1f;
        return Quaternion.LookRotation(d * s);
    }

    public float GetLength() => GetOffset().magnitude;

    public bool TryGetConnectedPointAt(PathSide side, out Marks_a_point point)
    {
        var p = GetPointAt(side);
        if (!p.TryGetComponent<Connects_to>(out var c))
        {
            point = default;
            return false;
        }

        point = c.point;
        return true;
    }

    public bool TryGetConnectedAt(PathSide side, out Marks_a_path new_path, out PathSide new_path_side)
    {
        if (!TryGetConnectedPointAt(side, out var /* connected point */ cp))
        {
            new_path = default;
            new_path_side = default;
            return false;
        }

        new_path = cp.path;
        new_path_side = new_path.GetSideOf(cp);
        return true;
    }

    public PathSide GetSideOf(Marks_a_point point)
    {
        if (point == start)
            return PathSide.Start;

        if (point == end)
            return PathSide.End;
        
        throw new ArgumentException("Point doesn't belong to the path");
    }

    public Marks_a_point GetPointAt(PathSide side) => side == PathSide.Start ? start : end;
}