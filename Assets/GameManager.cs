using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class GameManager : Singleton<GameManager>
{
    public bool StartGame;
    public PathCreation.Examples.PathFollower p;
    public GameObject FianlCamPos;
    public bool GameOver , GameEnd;
    public GameObject TattoMachine;
    public Collsion CollsionScript;

    public Transform FinalCamPos;
    public int Level;
    public GameObject PivotParent;
    public bool IsGameOver;

    public bool IsVideo;
    public override void Start()
    {
        p.enabled = false;
        base.Start();
        PlayerPrefs.SetInt("current_scene", SceneManager.GetActiveScene().buildIndex);
    }

    public void Reset()
    {
       
    }

    public void StartIt()
    {


        UiManager.Instance.StartUI.SetActive(false);
        p.gameObject.transform.DOMoveX(.1f, .5f).OnComplete(() => { TattoMachine.transform.GetComponentInChildren<Animator>().enabled = true;
            StartCoroutine(DelayStart());
        });
                                                                                               
        }
    IEnumerator DelayStart()
    {
        TattoMachine.transform.GetChild(1).gameObject.SetActive(true);
        CollsionScript.ChangeMaterials();
        yield return new WaitForSeconds(.5f);
        TattoMachine.transform.DOMoveZ(-0.98f, .3f);
    
        StartGame = true;
        p.enabled = true;
    }

    private void TattoMachineMechanics()
    {
       
        TattoMachine.transform.GetComponentInChildren<Animator>().enabled = false;
    }

    public void ZoomEffect()
    {
        StartCoroutine(CamZoomInAndOutRoutine());
    }
    public IEnumerator CamZoomInAndOutRoutine()
    {
   
        Camera.main.DOFieldOfView(58, 1); 
        yield return new WaitForSeconds(1);
        Camera.main.DOFieldOfView(70, .5f);
    }
}
