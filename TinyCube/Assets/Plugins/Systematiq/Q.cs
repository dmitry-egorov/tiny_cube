using Plugins.Systematiq;

public static class Q
{
    public static QRegistrationContext Presentation() => new QRegistrationContext(MechanicStage.Presentation);
    public static QRegistrationContext Gameplay() => new QRegistrationContext(MechanicStage.Gameplay);
    public static Subject Subject => QManager.Subject;
    public static float DeltaTime => QManager.DeltaTime;
    public static float PresentationTimeRatio => QManager.PresentationTimeRatio;
}