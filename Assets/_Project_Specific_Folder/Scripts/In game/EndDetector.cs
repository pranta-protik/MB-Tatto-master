using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.NiceVibrations;
public class EndDetector : MonoBehaviour
{
    public ParticleSystem EndParticle , Confetti;
    public GameObject Cam;
    public GameObject TattoWall;
    public int SavedTattooNo;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EndIt"))
        {
            Camera.main.transform.DOShakePosition(1f, .1f);
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            GameManager.Instance.PivotParent.transform.GetChild(01).transform.parent = null;
            GameManager.Instance.PivotParent.transform.DOLocalRotate(new Vector3(-40, -20, 20f), .3f);
            

            GameManager.Instance.CollsionScript.StartTapRoutine = false;
            GameManager.Instance.CollsionScript.anim.Play("g 0 0");
            GameManager.Instance.CollsionScript.anim1.Play("g 0 0");
            EndParticle.Play();
            UiManager.Instance.TapFastPanel.gameObject.SetActive(false);       
            GameManager.Instance.IsGameOver = true;
            Cam.gameObject.SetActive(true);


            // all shit saving
            GameManager.Instance.GameEnd = true;
            GameManager.Instance.SetTotalTime();
            StorageManager.Instance.SetTotalScore();
            StorageManager.Instance.GetTotalScore();
            Confetti.gameObject.SetActive(true);

            // Tattoo wall mechanics
            StartCoroutine(EnableEndUi());
        }
           
    }
    public IEnumerator EnableEndUi()
    {


        yield return new WaitForSeconds(3f);
       
        GameManager.Instance.FakeCam.gameObject.transform.DOLocalMove(new Vector3(16.03f, 1.31f, 0), .8f);
        GameManager.Instance.FakeCam.gameObject.transform.DOLocalRotate(new Vector3(0, 90, 0), .8f);

     
        SavedTattooNo = PlayerPrefs.GetInt("SavedTattooNo");

        GameManager.Instance.TextureName = GameManager.Instance.CollsionScript.StiackerMat.mainTexture.name;
        GameManager.Instance.LastTattoTexture = GameManager.Instance.CollsionScript.StiackerMat.mainTexture;
        PlayerPrefs.SetString("TattooFrame" + SavedTattooNo, GameManager.Instance.TextureName);
        SavedTattooNo++;
        PlayerPrefs.SetInt("SavedTattooNo" , SavedTattooNo);

        int tattooValue = StorageManager.GetTattooValue();
        tattooValue += StorageManager.Instance.RewardValue;
        StorageManager.SaveTattooValue(tattooValue);
        TattoWall.SetActive(true);
        

        // UiManager.Instance.CompleteUI.gameObject.SetActive(true);
    }
}
