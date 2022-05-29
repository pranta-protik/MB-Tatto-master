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
    public int Level;


    [Header("Bools")]
    public bool IsGameOver;
    public bool StartGame;
    public bool IsVideo;
    public bool IsLevelEnd;
    public bool GameOver;
    public bool GameEnd;

    [Header("GameObject Refs")]
    public GameObject Boss;
    public GameObject TattoMachine;

    [Header("Scripts Refs")]
    public PathCreation.PathCreator pathCreator;
    public PathCreation.Examples.PathFollower p;
    public Collsion CollsionScript;
    public TextureManager m_textureManager;

    [Header("Transforms")]
    public GameObject FianlCamPos;
    public Transform FinalCamPos;
    public GameObject PivotParent;

    public int currentHandId;

    int SavedLevelNo;
    GameObject Path;

    public override void Start()
    {
        if(levelNo == 0)
        {
            CollsionScript.Tattos = m_textureManager.FlowerExpensiveTattos;
            CollsionScript.CheapTttos = m_textureManager.FlowerCheapTattos;
        }
        if (levelNo == 1)
        {
            CollsionScript.Tattos = m_textureManager.SkullExpensiveTattos;
            CollsionScript.CheapTttos = m_textureManager.SkullCheapTattos;
        }
        if (levelNo == 2)
        {
            CollsionScript.Tattos = m_textureManager.PinnupGirlExpensiveTattos;
            CollsionScript.CheapTttos = m_textureManager.PinnupGirlCheapTattos;
        }
        if (levelNo == 3)
        {
            CollsionScript.Tattos = m_textureManager.FlowerExpensiveTattos;
            CollsionScript.CheapTttos = m_textureManager.FlowerCheapTattos;
        }
        if (levelNo == 4)
        {
            CollsionScript.Tattos = m_textureManager.PinnupGirlExpensiveTattos;
            CollsionScript.CheapTttos = m_textureManager.PinnupGirlCheapTattos;
        }
        SavedLevelNo = PlayerPrefs.GetInt("current_scene_text", 0);
        UiManager.Instance.LevelText.text = (SavedLevelNo + 1).ToString();
        int currentLevel = PlayerPrefs.GetInt("current_scene");
        LoadLvlPrefab();
        p.enabled = false;
        base.Start();

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
