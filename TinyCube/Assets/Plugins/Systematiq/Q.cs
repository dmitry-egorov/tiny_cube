using Plugins.Systematiq;

public static class Q
{
    public static QRegistrationContext OnPresentationUpdate() => new QRegistrationContext(MechanicStage.Presentation);
    public static QRegistrationContext OnGameplayUpdate() => new QRegistrationContext(MechanicStage.Gameplay);
    public static Subject Subject => QManager.Subject;
    public static float DeltaTime => QManager.DeltaTime;
    public static float PresentationTimeRatio => QManager.PresentationTimeRatio;
}