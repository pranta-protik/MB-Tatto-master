using Singleton;

public class HapticsManager : Singleton<HapticsManager>
{
    public bool IsHapticsAllowed { get; set; }
}
