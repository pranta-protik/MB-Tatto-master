using UnityEngine;
using UnityEngine.Serialization;

public class EndDetector : MonoBehaviour
{
    [FormerlySerializedAs("EndParticle")] public ParticleSystem endEffect;
    [FormerlySerializedAs("Confetti")] public ParticleSystem confettiEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EndIt"))
        {
            if (HapticsManager.Instance.IsHapticsAllowed)
            {
                Handheld.Vibrate();   
            }
            endEffect.Play();
            confettiEffect.gameObject.SetActive(true);
            GameManager.Instance.FinishWrestling();
        }
    }
}
