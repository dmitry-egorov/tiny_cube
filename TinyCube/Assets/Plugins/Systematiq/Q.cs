public static class Q
{
    public static QRegistrationContext OnPresentationUpdate() => new QRegistrationContext();
    public static Subject Subject => QManager.Subject;
    public static float DeltaTime => QManager.DeltaTime;
}