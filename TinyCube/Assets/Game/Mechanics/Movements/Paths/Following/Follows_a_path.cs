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
    public float GetDistance() => Path.GetDistance();
    public Quaternion GetRotation() => Path.GetRotation(Direction);
    
    public bool TryGetNext(out Marks_a_path new_path, out ConnectionSide connection_side)
    {
        if 
        (
            !Path.TryGetComponent(out Has_next_path nf) ||
            !nf.Next.TryGet(out var np)
        )
        {
            new_path = default;
            connection_side = default;
            return false;
        }
        
        new_path = np;
        connection_side = nf.Side;
        return true;
    }

    public bool TryGetPrev(out Marks_a_path new_path, out ConnectionSide connection_side)
    {
        if 
        (
            !Path.TryGetComponent(out Has_prev_path nf) ||
            !nf.Prev.TryGet(out var np)
        )
        {
            new_path = default;
            connection_side = default;
            return false;
        }
        
        new_path = np;
        connection_side = nf.Side;
        return true;
    }
}