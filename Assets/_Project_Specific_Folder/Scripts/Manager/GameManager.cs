using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using DG.Tweening;
using SuperCop.Scripts;
using PathCreation.Examples;
using System;
using UnityEngine.Serialization;

[Serializable]
public class HandGroup
{
    public GameObject mainHand;
    public GameObject tattooHand;
}
public class GameManager : Singleton<GameManager>
{
    private int _handId;
    [SerializeField] private List<HandGroup> handGroups = new List<HandGroup>();
    private Collsion _mainHandCollision;
    private Camera _mainCamera;
    private CameraController _cameraController;
    private TextureManager _textureManager;

    public Texture LastTattoTexture;
    public string TextureName;

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
    
    // public TextureManager m_textureManager;
    public CameraController cam;
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
    
    [SerializeField] private int specificLevelId;

    public override void Start()
    {
        _mainCamera = Camera.main;
        
        if (_mainCamera!=null)
        {
            _cameraController = _mainCamera.GetComponent<CameraController>();    
        }
        
        _handId = PlayerPrefs.GetInt("SelectedHandCardId");
        
        GameObject mainHandObj = handGroups[_handId].mainHand;
        mainHandObj.SetActive(true);
        handGroups[_handId].tattooHand.SetActive(true);
        _mainHandCollision = mainHandObj.GetComponent<Collsion>();
        
        _cameraController.player = mainHandObj;

        _textureManager = TextureManager.Instance;
        
        
        SavedLevelNo = PlayerPrefs.GetInt("current_scene_text", 0);
        UiManager.Instance.LevelText.text = (SavedLevelNo + 1).ToString();
        int currentLevel = PlayerPrefs.GetInt("current_scene");
        LoadLvlPrefab();
        p.enabled = false;
        base.Start();
        TattooVsLevel();
        
#if UNITY_EDITOR
        PlaySpecificLevel(specificLevelId);
#endif
    }
    
    private void PlaySpecificLevel(int levelId)
    {
        levelNo = levelId;
        PlayerPrefs.SetInt("current_scene", levelNo);
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

    public void StartIt()
    {
        UiManager.Instance.selectionMenuButton.gameObject.SetActive(false);
        // UiManager.Instance.hand.gameObject.SetActive(false);
        UiManager.Instance.shop.SetActive(false);
        UiManager.Instance.LevelText.transform.parent.gameObject.SetActive(false);
        // CollsionScript.StiackerMat.mainTexture = CollsionScript.Default;
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
        // CollsionScript.DrawDefaultTattoo();
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
            // [Header("Ref : Flower == 1, Skull == 2, PinUpGirl == 3, Celebrity == 4, Money==5, Calligraphy==6")]
            case 1:
                _mainHandCollision.defaultTattoo = _textureManager.flowerDefaultTattoo;
                
                _mainHandCollision.expensiveTattoos = _textureManager.flowerExpensiveTattoos;
                _mainHandCollision.expensiveBlueTattoos = _textureManager.flowerExpensiveBlueTattoos;
                _mainHandCollision.expensiveYellowTattoos = _textureManager.flowerExpensiveYellowTattoos;
                _mainHandCollision.expensiveColorTattooIdSequences = _textureManager.flowerExpensiveColorTattooIdSequences;
                
                _mainHandCollision.cheapTattoos = _textureManager.flowerCheapTattoos;
                _mainHandCollision.cheapBlueTattoos = _textureManager.flowerCheapBlueTattoos;
                _mainHandCollision.cheapYellowTattoos = _textureManager.flowerCheapYellowTattoos;
                _mainHandCollision.cheapColorTattooIdSequences = _textureManager.flowerCheapColorTattooIdSequences;
                
                break;
            
            case 2:
                _mainHandCollision.defaultTattoo = _textureManager.skullDefaultTattoo;
                
                _mainHandCollision.expensiveTattoos = _textureManager.skullExpensiveTattoos;
                _mainHandCollision.expensiveBlueTattoos = _textureManager.skullExpensiveBlueTattoos;
                _mainHandCollision.expensiveYellowTattoos = _textureManager.skullExpensiveYellowTattoos;
                _mainHandCollision.expensiveColorTattooIdSequences = _textureManager.skullExpensiveColorTattooIdSequences;
                
                _mainHandCollision.cheapTattoos = _textureManager.skullCheapTattoos;
                _mainHandCollision.cheapBlueTattoos = _textureManager.skullCheapBlueTattoos;
                _mainHandCollision.cheapYellowTattoos = _textureManager.skullCheapYellowTattoos;
                _mainHandCollision.cheapColorTattooIdSequences = _textureManager.skullCheapColorTattooIdSequences;

                break;
            
            case 3:
                _mainHandCollision.defaultTattoo = _textureManager.pinupGirlDefaultTattoo;
                
                _mainHandCollision.expensiveTattoos = _textureManager.pinupGirlExpensiveTattoos;
                _mainHandCollision.expensiveBlueTattoos = _textureManager.pinupGirlExpensiveBlueTattoos;
                _mainHandCollision.expensiveYellowTattoos = _textureManager.pinupGirlExpensiveYellowTattoos;
                _mainHandCollision.expensiveColorTattooIdSequences = _textureManager.pinupGirlExpensiveColorTattooIdSequences;
                
                _mainHandCollision.cheapTattoos = _textureManager.pinupGirlCheapTattoos;
                _mainHandCollision.cheapBlueTattoos = _textureManager.pinupGirlCheapBlueTattoos;
                _mainHandCollision.cheapYellowTattoos = _textureManager.pinupGirlCheapYellowTattoos;
                _mainHandCollision.cheapColorTattooIdSequences = _textureManager.pinupGirlCheapColorTattooIdSequences;
                
                break;
            
            case 4:
                _mainHandCollision.defaultTattoo = _textureManager.celebrityDefaultTattoo;
                
                _mainHandCollision.expensiveTattoos = _textureManager.celebrityExpensiveTattoos;
                _mainHandCollision.expensiveBlueTattoos = _textureManager.celebrityExpensiveBlueTattoos;
                _mainHandCollision.expensiveYellowTattoos = _textureManager.celebrityExpensiveYellowTattoos;
                _mainHandCollision.expensiveColorTattooIdSequences = _textureManager.celebrityExpensiveColorTattooIdSequences;
                
                _mainHandCollision.cheapTattoos = _textureManager.celebrityCheapTattoos;
                _mainHandCollision.cheapBlueTattoos = _textureManager.celebrityCheapBlueTattoos;
                _mainHandCollision.cheapYellowTattoos = _textureManager.celebrityCheapYellowTattoos;
                _mainHandCollision.cheapColorTattooIdSequences = _textureManager.celebrityCheapColorTattooIdSequences;

                break;
            
            case 5:
                _mainHandCollision.defaultTattoo = _textureManager.moneyDefaultTattoo;
                
                _mainHandCollision.expensiveTattoos = _textureManager.moneyExpensiveTattoos;
                _mainHandCollision.expensiveBlueTattoos = _textureManager.moneyExpensiveBlueTattoos;
                _mainHandCollision.expensiveYellowTattoos = _textureManager.moneyExpensiveYellowTattoos;
                _mainHandCollision.expensiveColorTattooIdSequences = _textureManager.moneyExpensiveColorTattooIdSequences;
                
                _mainHandCollision.cheapTattoos = _textureManager.moneyCheapTattoos;
                _mainHandCollision.cheapBlueTattoos = _textureManager.moneyCheapBlueTattoos;
                _mainHandCollision.cheapYellowTattoos = _textureManager.moneyCheapYellowTattoos;
                _mainHandCollision.cheapColorTattooIdSequences = _textureManager.moneyCheapColorTattooIdSequences;

                break;
            
            case 6:
                _mainHandCollision.defaultTattoo = _textureManager.calligraphyDefaultTattoo;
                
                _mainHandCollision.expensiveTattoos = _textureManager.calligraphyExpensiveTattoos;
                _mainHandCollision.expensiveBlueTattoos = _textureManager.calligraphyExpensiveBlueTattoos;
                _mainHandCollision.expensiveYellowTattoos = _textureManager.calligraphyExpensiveYellowTattoos;
                _mainHandCollision.expensiveColorTattooIdSequences = _textureManager.calligraphyExpensiveColorTattooIdSequences;
                
                _mainHandCollision.cheapTattoos = _textureManager.calligraphyCheapTattoos;
                _mainHandCollision.cheapBlueTattoos = _textureManager.calligraphyCheapBlueTattoos;
                _mainHandCollision.cheapYellowTattoos = _textureManager.calligraphyCheapYellowTattoos;
                _mainHandCollision.cheapColorTattooIdSequences = _textureManager.calligraphyCheapColorTattooIdSequences;

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
        foreach (HandGroup hand in handGroups)          
        {
            if (hand.mainHand.GetComponent<HandController>().handId == handId)
            {
                hand.mainHand.gameObject.SetActive(true);
                hand.tattooHand.gameObject.SetActive(true);
                // CollsionScript = hand.mainHand.GetComponent<Collsion>(); 
                //// GameObject.FindGameObjectWithTag("Player").GetComponent<Collsion>();
                cam.player = hand.mainHand.gameObject;
                SetLevelDetails(currentLvlPrefab);
            }
            else
            {
                hand.mainHand.gameObject.SetActive(false);
                hand.tattooHand.gameObject.SetActive(false);
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
