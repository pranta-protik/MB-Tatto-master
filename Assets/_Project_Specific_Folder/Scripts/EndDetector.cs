using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.NiceVibrations;
public class EndDetector : MonoBehaviour
{
    public ParticleSystem EndParticle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EndIt"))
        {
            Camera.main.transform.DOShakePosition(1f, .1f);
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            GameManager.Instance.PivotParent.transform.GetChild(01).transform.parent = null;
            GameManager.Instance.PivotParent.transform.DOLocalRotate(new Vector3(-40, -20, 20f), .3f);
            //GameManager.Instance.p.transform.DOLocalRotate(new Vector3(0, 90 - 40), .1f);
            //FindObjectOfType<Collsion>().transform.parent.transform.DOLocalRotate(new Vector3(-7, 90 - 50), .1f);
            FindObjectOfType<Collsion>().StartTapRoutine = false;
            FindObjectOfType<Collsion>().anim.Play("g 0 0"); 
            FindObjectOfType<Collsion>().anim1.Play("g 0 0");
            EndParticle.Play();
            UiManager.Instance.TapFastPanel.gameObject.SetActive(false);
            StartCoroutine(EnableEndUi());
            GameManager.Instance.IsGameOver = true;
        }
           
    }
    public IEnumerator EnableEndUi()
    {
        yield return new WaitForSeconds(.7f);
        StorageManager.Instance.SetTotalScore(); StorageManager.Instance.GetTotalScore();
        UiManager.Instance.CompleteUI.gameObject.SetActive(true);
    }
}
