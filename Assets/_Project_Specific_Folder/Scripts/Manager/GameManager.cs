using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using UnityEngine.SceneManagement;
using DG.Tweening;
using SuperCop.Scripts;
using PathCreation.Examples;
using System;

[Serializable]
public class ItemPacks
{
    public GameObject MainHand;
    public GameObject CopyHand;
}
public class GameManager : Singleton<GameManager>
{
    public int HandNumber;
    public List<ItemPacks> Hands = new List<ItemPacks>();


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
    public Camera FakeCam;

    [Header("Scripts Refs")]
    public PathCreation.PathCreator pathCreator;
    public PathCreation.Examples.PathFollower p;
    public Collsion CollsionScript;
    public TextureManager m_textureManager;
    public SharkAttack.CameraController cam;
    [Header("Transforms")]
    public GameObject FianlCamPos;
    public Transform FinalCamPos;
    public GameObject PivotParent;
    public Transform SpwanPos;

    public int currentHandId;

    int SavedLevelNo;
    GameObject Path;
    public float timer = 0.0f;

    public override void Start()
    {

        HandNumber = PlayerPrefs.GetInt("SelectedHandId");
        SpawnHand(HandNumber);
        SavedLevelNo = PlayerPrefs.GetInt("current_scene_text", 0);
        UiManager.Instance.LevelText.text = (SavedLevelNo + 1).ToString();
        int currentLevel = PlayerPrefs.GetInt("current_scene");
        LoadLvlPrefab();
        p.enabled = false;
        base.Start();
        TattooVsLevel();
  

    }
    private void Update()
    {

        if(StartGame && !GameEnd)
        {
            timer += Time.deltaTime;
           
        }


        if (Path == null)
        {
            Path = GameObject.Find("pathWAY");
            pathCreator = Path.GetComponent<PathCreation.PathCreator>();
            Path.GetComponent<RoadMeshCreator>().refresh();
        }

    }
    public void SetTotalTime()
    {
        int currentTime = PlayerPrefs.GetInt("TotalTime", 0);
        int Time= currentTime + Mathf.RoundToInt(timer);
       
        PlayerPrefs.SetInt("TotalTime", Time);
        print(PlayerPrefs.GetInt("TotalTime",0));
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
        UiManager.Instance.LevelText.transform.parent.gameObject.SetActive(false);
        CollsionScript.StiackerMat.mainTexture = CollsionScript.Default;
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

    public void TattooVsLevel()
    {


        if (levelNo == 0)
        {
            CollsionScript.Default = m_textureManager.DefaultFlower;
            CollsionScript.Tattos = m_textureManager.FlowerExpensiveTattos;
            CollsionScript.CheapTttos = m_textureManager.FlowerCheapTattos;
        }
        if (levelNo == 1)
        {
            CollsionScript.Default = m_textureManager.DefaultSkull;
            CollsionScript.Tattos = m_textureManager.SkullExpensiveTattos;
            CollsionScript.CheapTttos = m_textureManager.SkullCheapTattos;
        }
        if (levelNo == 2)
        {
            CollsionScript.Default = m_textureManager.DefaultPinup;
            CollsionScript.Tattos = m_textureManager.PinnupGirlExpensiveTattos;
            CollsionScript.CheapTttos = m_textureManager.PinnupGirlCheapTattos;
        }
        if (levelNo == 3)
        {
            CollsionScript.Default = m_textureManager.DefaultFlower;
            CollsionScript.Tattos = m_textureManager.FlowerExpensiveTattos;
            CollsionScript.CheapTttos = m_textureManager.FlowerCheapTattos;
        }
        if (levelNo == 4)
        {
            CollsionScript.Default = m_textureManager.DefaultPinup;
            CollsionScript.Tattos = m_textureManager.PinnupGirlExpensiveTattos;
            CollsionScript.CheapTttos = m_textureManager.PinnupGirlCheapTattos;
        }
        if (levelNo == 7)
        {
            CollsionScript.Default = m_textureManager.DefaultCeleb;
            CollsionScript.Tattos = m_textureManager.CelebExpensiveTattos;
            CollsionScript.CheapTttos = m_textureManager.CelebCheapTattos;
        }
        if (levelNo == 10)
        {
            CollsionScript.Default = m_textureManager.defaultCartoon;
            CollsionScript.Tattos = m_textureManager.CartoonExpensiveTattos;
            CollsionScript.CheapTttos = m_textureManager.CartoonCheapTattos;

        }
        if (levelNo == 13)
        {
            CollsionScript.Default = m_textureManager.DefaultMoney;
            CollsionScript.Tattos = m_textureManager.MoneyExpensiveTattos;
            CollsionScript.CheapTttos = m_textureManager.MoneyCheapTattos;
        }


    }

   public void SpawnHand(int i)
    {
        GameObject g = Instantiate(Hands[i].MainHand, transform.position, Quaternion.identity);
        GameObject g1 = Instantiate(Hands[i].CopyHand, transform.position, Quaternion.identity);
        g.transform.parent = p.transform; g1.transform.parent = p.transform;
        g.transform.DOScale(SpwanPos.transform.localScale, 0); g1.transform.DOScale(SpwanPos.transform.localScale, 0);
        g.transform.DOLocalMove(SpwanPos.transform.localPosition, 0); g1.transform.DOLocalMove(SpwanPos.transform.localPosition, 0);
        g.transform.DOLocalRotate(SpwanPos.transform.localEulerAngles, 0); g1.transform.DOLocalRotate(SpwanPos.transform.localEulerAngles, 0);
        CollsionScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Collsion>();
        if (!cam.player) cam.player = GameObject.FindGameObjectWithTag("Player");
    }
}
