using Plugins.Systematiq;
using UnityEngine;

public class Systematiq : MonoBehaviour
{
    public float GameplayDeltaTime;
    public float PresentationRatio;

    void Update()
    {
        var fdt = GameplayDeltaTime != 0.0f ? GameplayDeltaTime : Time.fixedDeltaTime;
        var dt = Time.deltaTime;
        QManager.Execute(dt, fdt);

        PresentationRatio = Q.PresentationTimeRatio;
    }

}
