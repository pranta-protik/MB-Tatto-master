using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using UnityEngine.SceneManagement;
using DG.Tweening;
using SuperCop.Scripts;
using PathCreation.Examples;
public class GameManager : Singleton<GameManager>
{
    [Header("Level prefabs List")]
    public List<GameObject> LevelPrefabs = new List<GameObject>();

    public int levelNo;
    public GameObject currentLvlPrefab;

    GameObject Path;
    public PathCreation.PathCreator pathCreator;

    public PathCreation.Examples.PathFollower p;
    public GameObject Boss;
    int SavedLevelNo;



    public bool StartGame;

    public GameObject FianlCamPos;
    public bool GameOver , GameEnd;
    public GameObject TattoMachine;
    public Collsion CollsionScript;

    public Transform FinalCamPos;
    public int Level;
    public GameObject PivotParent;
    public bool IsGameOver;

    public bool IsVideo , IsLevelEnd;
    public override void Start()
    {
        SavedLevelNo = PlayerPrefs.GetInt("current_scene_text", 0);
        UiManager.Instance.LevelText.text = (SavedLevelNo + 1).ToString();
        int currentLevel = PlayerPrefs.GetInt("current_scene");
        LoadLvlPrefab();
        p.enabled = false;
        base.Start();
        PlayerPrefs.SetInt("current_scene", SceneManager.GetActiveScene().buildIndex);
    }
    private void Update()
    {
        if (Path == null)
        {
            Path = GameObject.Find("pathWAY");
            pathCreator = Path.GetComponent<PathCreation.PathCreator>();
            Path.GetComponent<RoadMeshCreator>().refresh();
        }

    }
    public void LoadLvlPrefab()
    {

        levelNo = PlayerPrefs.GetInt("current_scene", 0);
        /*#if UNITY_EDITOR

                levelNo = amarIcchaLevel;
                PlayerPrefs.SetInt("current_scene", levelNo);

        #endif*/

        currentLvlPrefab = Instantiate(LevelPrefabManager.Instance.GetCurrentLevelPrefab());


    }
    public void Reset()
    {
       
    }

    public void StartIt()
    {


        UiManager.Instance.StartUI.SetActive(false);
        PivotParent = GameObject.FindGameObjectWithTag("PivotParent");
        Boss = GameObject.FindGameObjectWithTag("EndIt");
        p.gameObject.transform.DOMoveX(.1f, .5f).OnComplete(() => { 
            TattoMachine.transform.GetComponentInChildren<Animator>().enabled = true;
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
