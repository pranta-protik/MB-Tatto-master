using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using TMPro;
using UnityEngine.UI;
public class UiManager : Singleton<UiManager>
{
    public TMP_Text LevelText;
    public GameObject StartUI, EndUi, CompleteUI , FadeIn;
    public GameObject TapFastPanel;

    public GameObject fillbarTimer;
    public Image Timer;
    public float timerInitvalue;
    public TMP_Text TotalText, PointText , NormalCoin;
    public override void Start()
    {
        base.Start();

        LevelText.text = SceneLoadCounter.Instance.SceneLoadCount.ToString();
    }
    public IEnumerator FdeDelayRoutine()
    {
        FadeIn.SetActive(true);
        yield return new WaitForSeconds(.3f);
        FadeIn.SetActive(false);
     
    }


}
