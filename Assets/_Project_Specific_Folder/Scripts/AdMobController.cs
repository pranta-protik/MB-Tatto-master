using UnityEngine;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

public class AdMobController : MonoBehaviour
{
   private void Start()
   {
#if UNITY_IOS
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
         {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
         }
#endif
   }
}
