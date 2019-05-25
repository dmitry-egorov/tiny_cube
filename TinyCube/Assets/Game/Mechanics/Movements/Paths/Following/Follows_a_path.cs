using System;
using Plugins.Lanski.FunctionalExtensions;
using Plugins.Lanski.Subjective;
using Plugins.UnityExtensions;
using UnityEngine;
using static Direction;

public class Follows_a_path: SubjectComponent
{
    public Marks_a_path Path;
    public bool Rotates = true;
    public float DistanceWalked;
    public Direction Direction;

    public void Reverse()
    {
        Direction = Direction == Forward ? Backward : Forward;
    }

    public Vector3 GetPosition() => Path.GetPositionAt(DistanceWalked);
    public float GetHeight() => Path.GetHeightAt(DistanceWalked);
    public float GetLength() => Path.GetLength();
    public Quaternion GetRotation() => Path.GetRotation(Direction);

    public bool TryGetWalkedBeyond(out PathSide side, out float extra_distance)
    {
        var /* path's length */ pl = GetLength();
                    
        var wd = DistanceWalked;
        var /* walked past end */   wpe = wd > pl;
        var /* walked past start */ wpb = wd < 0;

        if (!wpe && !wpb)
        {
            side = default;
            extra_distance = default;
            return false;
        }
                    
        extra_distance = wpe ? wd - pl : -wd;
        side = wpe ? PathSide.End : PathSide.Start;
        return true;
    }

    public bool TryGetConnectedAt(PathSide side, out Marks_a_path new_path, out PathSide path_side)
    {
        return side == PathSide.End
            ? TryGetNext(out new_path, out path_side)
            : TryGetPrev(out new_path, out path_side)
        ;
    }
    public bool TryGetNext(out Marks_a_path new_path, out PathSide path_side)
    {
        if 
        (
            !Path.TryGetComponent(out Has_next_path nf) ||
            !nf.Next.TryGet(out var np)
        )
        {
            new_path = default;
            path_side = default;
            return false;
        }
        
        new_path = np;
        path_side = nf.Side;
        return true;
    }

    public bool TryGetPrev(out Marks_a_path new_path, out PathSide path_side)
    {
        if 
        (
            !Path.TryGetComponent(out Has_prev_path nf) ||
            !nf.Prev.TryGet(out var np)
        )
        {
            new_path = default;
            path_side = default;
            return false;
        }
        
        new_path = np;
        path_side = nf.Side;
        return true;
    }

    public void SetReverseDistance(float ed)
    {
        DistanceWalked = Path.GetLength() - ed;
    }

    public void SetDistance(float ed)
    {
        DistanceWalked = ed;
    }

    public void ResetToStart()
    {
        DistanceWalked = 0;
    }

    public void ResetToEnd()
    {
        DistanceWalked = GetLength();
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