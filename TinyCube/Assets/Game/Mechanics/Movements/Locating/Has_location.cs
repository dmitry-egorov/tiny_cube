using Plugins.Lanski.Subjective;
using UnityEngine;

public class Has_location: SubjectComponent
{
    public Vector3 Location;

    public float GetHeight() => Location.y;

    public void SetHeight(float nh)
    {
        Location.y = nh;
    }
}
