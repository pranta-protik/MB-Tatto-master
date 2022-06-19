using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using UnityEngine.SceneManagement;
using DG.Tweening;
using SuperCop.Scripts;
using PathCreation.Examples;
using System;
using System.Globalization;
using Facebook.Unity;
using MoreMountains.NiceVibrations;

[Serializable]
public class ItemPacks
{
    public GameObject MainHand;
    public GameObject CopyHand;
}
public class GameManager : Singleton<GameManager>
{
    public Texture LastTattoTexture;
    public string TextureName;

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
    public Transform bossWall;
    public int currentHandId;
    public int baseLikes;
    public int baseFollowers;
    public List<int> likes = new List<int>();
    public List<string> followers = new List<string>();

    int SavedLevelNo;
    GameObject Path;
    public float timer = 0.0f;
    [SerializeField] private int amarIcchaLevel;


    public override void Start()
    {
// #if UNITY_EDITOR
//
//         levelNo = amarIcchaLevel;
//         PlayerPrefs.SetInt("current_scene", levelNo);
//
// #endif
        
        // First time hand enable
        HandNumber = PlayerPrefs.GetInt("SelectedHandCardId");
        Hands[HandNumber].MainHand.gameObject.SetActive(true);
        Hands[HandNumber].CopyHand.gameObject.SetActive(true);
        CollsionScript = Hands[HandNumber].MainHand.GetComponent<Collsion>();
        // GameObject.FindGameObjectWithTag("Player").GetComponent<Collsion>();
        if (!cam.player) cam.player = GameObject.FindGameObjectWithTag("Player");


        //   HandNumber = PlayerPrefs.GetInt("SelectedHandId");
        // SpawnHand(HandNumber);
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
        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     PlayerPrefs.DeleteAll();
        // }
        
        if(StartGame && !GameEnd)
        {
            timer += Time.deltaTime;
        }

        if (Boss == null)
        {
            Boss = GameObject.Find("npc_Boss");
        }

        if (PivotParent == null)
        {
            PivotParent = GameObject.Find("Parent");
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

        SetLevelDetails(currentLvlPrefab);
    }
    public void Reset()
    {
       
    }

    public void StartIt()
    {
        UiManager.Instance.selectionMenuButton.gameObject.SetActive(false);
        // UiManager.Instance.hand.gameObject.SetActive(false);
        UiManager.Instance.shop.SetActive(false);
        UiManager.Instance.LevelText.transform.parent.gameObject.SetActive(false);
        CollsionScript.StiackerMat.mainTexture = CollsionScript.Default;
        UiManager.Instance.StartUI.SetActive(false);
        PivotParent = GameObject.FindGameObjectWithTag("PivotParent");
        Boss = GameObject.FindGameObjectWithTag("EndIt");
        p.gameObject.transform.DOMoveX(.1f, .5f).OnComplete(() =>
        {
            TattoMachine.transform.GetComponentInChildren<Animator>().enabled = true;
            StartCoroutine(DelayStart());
        });
    }

    IEnumerator DelayStart()
    {
        TattoMachine.transform.GetChild(1).gameObject.SetActive(true);
        CollsionScript.ChangeMaterials();
        yield return new WaitForSeconds(.5f);
        UiManager.Instance.ShowPriceTag();
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
    private void SetLevelDetails(GameObject levelPrefab)
    {
        Leveldetails levelDetails = levelPrefab.GetComponent<Leveldetails>();
        SetTttos(levelDetails);
    }

    private void SetTttos(Leveldetails levelDetails)
    {
        int TattoSet;
        TattoSet = levelDetails.Id;
        switch (TattoSet)
        {
            // [Header("Ref : Flower == 1 Skull == 2 PinUp == 3 Celebs == 4 Money==5  Caligraphy==6")]
            //vehicle set
            case 1:
                // CollsionScript.Tattos = new Texture[15];
                CollsionScript.Default = m_textureManager.DefaultFlower;
                CollsionScript.Tattos = m_textureManager.FlowerExpensiveTattos;
                CollsionScript.CheapTttos = m_textureManager.FlowerCheapTattos;

                CollsionScript.GoodBlue = m_textureManager.FlowerGoodBlue;
                CollsionScript.BadBlue = m_textureManager.FlowerBadBlue;

                CollsionScript.GoodYellow = m_textureManager.FlowerGoodYellow;
                CollsionScript.BadYellow = m_textureManager.FlowerBadYellow;
                break;
            case 2:
                // CollsionScript.Tattos = new Texture[15];
                CollsionScript.Default = m_textureManager.DefaultSkull;
                CollsionScript.Tattos = m_textureManager.SkullExpensiveTattos;
                CollsionScript.CheapTttos = m_textureManager.SkullCheapTattos;

                CollsionScript.GoodBlue = m_textureManager.SkullGoodBlue;
                CollsionScript.BadBlue = m_textureManager.SkullBadBlue;

                CollsionScript.GoodYellow = m_textureManager.SkullGoodYellow;
                CollsionScript.BadYellow = m_textureManager.SkullBadYellow;

                break;
            case 3:
                // CollsionScript.Tattos = new Texture[6];
                CollsionScript.Default = m_textureManager.DefaultPinup;
                CollsionScript.Tattos = m_textureManager.PinnupGirlExpensiveTattos;
                CollsionScript.CheapTttos = m_textureManager.PinnupGirlCheapTattos;


                CollsionScript.GoodBlue = m_textureManager.PinnupGirlGoodBlue;
                CollsionScript.BadBlue = m_textureManager.PinnupGirlBadBlue;

                CollsionScript.GoodYellow = m_textureManager.PinnupGirlGoodYellow;
                CollsionScript.BadYellow = m_textureManager.PinnupGirlBadYellow;
                break;
            case 4:
                // CollsionScript.Tattos = new Texture[6];
                CollsionScript.Default = m_textureManager.DefaultCeleb;
                CollsionScript.Tattos = m_textureManager.CelebExpensiveTattos;
                CollsionScript.CheapTttos = m_textureManager.CelebCheapTattos;

                CollsionScript.GoodBlue = m_textureManager.CelebGoodBlue;
                CollsionScript.BadBlue = m_textureManager.CelebBadBlue;

                CollsionScript.GoodYellow = m_textureManager.CelebGoodYellow;
                CollsionScript.BadYellow = m_textureManager.CelebBadYellow;

                break;
            case 5:
                // CollsionScript.Tattos = new Texture[6];
                CollsionScript.Default = m_textureManager.DefaultMoney;
                CollsionScript.Tattos = m_textureManager.MoneyExpensiveTattos;
                CollsionScript.CheapTttos = m_textureManager.MoneyCheapTattos;

                CollsionScript.GoodBlue = m_textureManager.MoneyGoodBlue;
                CollsionScript.BadBlue = m_textureManager.MoneyBadBlue;

                CollsionScript.GoodYellow = m_textureManager.MoneyGoodYellow;
                CollsionScript.BadYellow = m_textureManager.MoneyBadYellow;

                break;
            case 6:
                // CollsionScript.Tattos = new Texture[6];
                CollsionScript.Default = m_textureManager.DefaultCaligraphy;
                CollsionScript.Tattos = m_textureManager.CaligraphyExpensiveTattos;
                CollsionScript.CheapTttos = m_textureManager.CaligraphyCheapTattos;



                CollsionScript.GoodBlue = m_textureManager.CaligraphyGoodBlue;
                CollsionScript.BadBlue = m_textureManager.CaligraphyBadBlue;

                CollsionScript.GoodYellow = m_textureManager.CaligraphyGoodYellow;
                CollsionScript.BadYellow = m_textureManager.CaligraphyBadYellow;

                break;
            case 7:

                break;
        }
    }

    public void TattooVsLevel()
    {


        //if (levelNo == 0)
        //{
        //    CollsionScript.Default = m_textureManager.DefaultFlower;
        //    CollsionScript.Tattos = m_textureManager.FlowerExpensiveTattos;
        //    CollsionScript.CheapTttos = m_textureManager.FlowerCheapTattos;
        //}
        //if (levelNo == 1)
        //{
        //    CollsionScript.Default = m_textureManager.DefaultSkull;
        //    CollsionScript.Tattos = m_textureManager.SkullExpensiveTattos;
        //    CollsionScript.CheapTttos = m_textureManager.SkullCheapTattos;
        //}
        //if (levelNo == 2)
        //{
        //    CollsionScript.Default = m_textureManager.DefaultPinup;
        //    CollsionScript.Tattos = m_textureManager.PinnupGirlExpensiveTattos;
        //    CollsionScript.CheapTttos = m_textureManager.PinnupGirlCheapTattos;
        //}
        //if (levelNo == 3)
        //{
        //    CollsionScript.Default = m_textureManager.DefaultFlower;
        //    CollsionScript.Tattos = m_textureManager.FlowerExpensiveTattos;
        //    CollsionScript.CheapTttos = m_textureManager.FlowerCheapTattos;
        //}
        //if (levelNo == 4)
        //{
        //    CollsionScript.Default = m_textureManager.DefaultPinup;
        //    CollsionScript.Tattos = m_textureManager.PinnupGirlExpensiveTattos;
        //    CollsionScript.CheapTttos = m_textureManager.PinnupGirlCheapTattos;
        //}
        //if (levelNo == 5)
        //{
        //    CollsionScript.Default = m_textureManager.DefaultCeleb;
        //    CollsionScript.Tattos = m_textureManager.CelebExpensiveTattos;
        //    CollsionScript.CheapTttos = m_textureManager.CelebCheapTattos;
        //}
        //if (levelNo ==6)
        //{
        //    CollsionScript.Default = m_textureManager.DefaultFlower;
        //    CollsionScript.Tattos = m_textureManager.FlowerExpensiveTattos;
        //    CollsionScript.CheapTttos = m_textureManager.FlowerCheapTattos;
        //}
        //if (levelNo == 7)
        //{
        //    CollsionScript.Default = m_textureManager.DefaultSkull;
        //    CollsionScript.Tattos = m_textureManager.SkullExpensiveTattos;
        //    CollsionScript.CheapTttos = m_textureManager.SkullCheapTattos;
        //}
        //if (levelNo == 8)
        //{
        //    CollsionScript.Default = m_textureManager.DefaultSkull;
        //    CollsionScript.Tattos = m_textureManager.SkullExpensiveTattos;
        //    CollsionScript.CheapTttos = m_textureManager.SkullCheapTattos;
        //}


        //if (levelNo == 10)
        //{
        //    CollsionScript.Default = m_textureManager.defaultCartoon;
        //    CollsionScript.Tattos = m_textureManager.CartoonExpensiveTattos;
        //    CollsionScript.CheapTttos = m_textureManager.CartoonCheapTattos;

        //}
        //if (levelNo == 13)
        //{
        //    CollsionScript.Default = m_textureManager.DefaultMoney;
        //    CollsionScript.Tattos = m_textureManager.MoneyExpensiveTattos;
        //    CollsionScript.CheapTttos = m_textureManager.MoneyCheapTattos;
        //}


    }

   public void SpawnHand(int handId)
    {
        foreach (ItemPacks hand in Hands)
        {
            if (hand.MainHand.GetComponent<HandController>().handId == handId)
            {
                hand.MainHand.gameObject.SetActive(true);
                hand.CopyHand.gameObject.SetActive(true);
                CollsionScript = hand.MainHand.GetComponent<Collsion>(); 
                // GameObject.FindGameObjectWithTag("Player").GetComponent<Collsion>();
                cam.player = hand.MainHand.gameObject;
                SetLevelDetails(currentLvlPrefab);
            }
            else
            {
                hand.MainHand.gameObject.SetActive(false);
                hand.CopyHand.gameObject.SetActive(false);
            }
        }
        // for (int i = 0; i <Hands.Count; i++)
        // {
        //     // Activate the selected weapon 
        //     
        //     if (i == handId)
        //     {
        //         Hands[i].MainHand.gameObject.SetActive(true);
        //         Hands[i].CopyHand.gameObject.SetActive(true);
        //         CollsionScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Collsion>();
        //         cam.player = Hands[i].MainHand.gameObject;
        //         SetLevelDetails(currentLvlPrefab);
        //
        //     }
        //     else
        //     {
        //         Hands[i].MainHand.gameObject.SetActive(false);
        //         Hands[i].CopyHand.gameObject.SetActive(false);
        //     }
        // }

        //GameObject g = Instantiate(Hands[i].MainHand, transform.position, Quaternion.identity);
       // GameObject g1 = Instantiate(Hands[i].CopyHand, transform.position, Quaternion.identity);
        //g.transform.parent = p.transform; g1.transform.parent = p.transform;
        //g.transform.DOScale(SpwanPos.transform.localScale, 0); g1.transform.DOScale(SpwanPos.transform.localScale, 0);
        //g.transform.DOLocalMove(SpwanPos.transform.localPosition, 0); g1.transform.DOLocalMove(SpwanPos.transform.localPosition, 0);
        //g.transform.DOLocalRotate(SpwanPos.transform.localEulerAngles, 0); g1.transform.DOLocalRotate(SpwanPos.transform.localEulerAngles, 0);
        //CollsionScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Collsion>();
        //if (!cam.player) cam.player = GameObject.FindGameObjectWithTag("Player");
    }
}
