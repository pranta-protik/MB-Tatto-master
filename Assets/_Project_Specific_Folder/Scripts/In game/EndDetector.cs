using System;
using System.Collections;
using UnityEngine;
using MoreMountains.NiceVibrations;
using UnityEngine.Serialization;

public class EndDetector : MonoBehaviour
{
    [FormerlySerializedAs("EndParticle")] public ParticleSystem endEffect;

    public ParticleSystem Confetti;
    public GameObject Cam;
    public GameObject End;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EndIt"))
        {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
            endEffect.Play();
            GameManager.Instance.FinishWrestling();
            // Cam.gameObject.SetActive(true);
            
            Confetti.gameObject.SetActive(true);
        }
    }
    public IEnumerator EnableEndUi()
    {
        yield return new WaitForSeconds(3f);
        // GameManager.Instance.FakeCam.transform.parent = End.transform;
        // GameManager.Instance.FakeCam.gameObject.transform.DOLocalMove(new Vector3(0.32f, 1.06f, 0.08f), .8f);
        // GameManager.Instance.FakeCam.gameObject.transform.DOLocalRotate(new Vector3(0, 90, 0), .8f);
        
        // GameManager.Instance.TextureName = GameManager.Instance.CollsionScript.StiackerMat.mainTexture.name;
        // GameManager.Instance.LastTattoTexture = GameManager.Instance.CollsionScript.StiackerMat.mainTexture;
        
        
        // UiManager.Instance.CompleteUI.gameObject.SetActive(true);
    }
}
