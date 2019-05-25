using Plugins.Lanski.Subjective;
using UnityEngine;

public class Located: SubjectComponent
{
    public Vector3 Location;

    public float GetHeight()
    {
        return Location.y;
    }

    public void SetHeight(float nh)
    {
        Location.y = nh;
    }
}
