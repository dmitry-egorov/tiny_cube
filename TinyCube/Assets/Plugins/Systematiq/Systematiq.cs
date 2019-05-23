using Plugins.Systematiq;
using UnityEngine;

public class Systematiq : MonoBehaviour
{
    public int GameplayFramerate;
    public int PresentationFramerate;

    void Start()
    {
        if (PresentationFramerate != 0)
        {
            Application.targetFrameRate = PresentationFramerate;
        }
    }
    
    void Update()
    {
        var fdt = GameplayFramerate != 0 ? 1.0f / GameplayFramerate : Time.fixedDeltaTime;
        var dt = Time.deltaTime;
        QManager.Execute(dt, fdt);
    }

}
