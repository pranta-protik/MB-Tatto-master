using Singleton;

public class AdManager : Singleton<AdManager>
{
    public bool isBannerAdEnabled;
    public bool isInterstitialAdEnabled;
    public int interstitialAdStartLevel;
}