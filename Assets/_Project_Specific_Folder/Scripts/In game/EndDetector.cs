using UnityEngine;
using MoreMountains.NiceVibrations;
using UnityEngine.Serialization;

public class EndDetector : MonoBehaviour
{
    [FormerlySerializedAs("EndParticle")] public ParticleSystem endEffect;
    [FormerlySerializedAs("Confetti")] public ParticleSystem confettiEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EndIt"))
        {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
            endEffect.Play();
            confettiEffect.gameObject.SetActive(true);
            GameManager.Instance.FinishWrestling();
        }
    }
}
