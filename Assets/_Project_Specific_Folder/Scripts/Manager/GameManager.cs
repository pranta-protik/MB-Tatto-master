using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using DG.Tweening;
using PathCreation.Examples;
using System;
using PathCreation;

[Serializable]
public class HandGroup
{
    public GameObject mainHand;
    public GameObject tattooHand;
}

public class GameManager : Singleton<GameManager>
{
    public enum EGameMode
    {
        Complete,
        Test
    }
    
    public int totalLevelNo = 50;
    public List<GameObject> levelPrefabs = new List<GameObject>();
    public List<int> likes = new List<int>();
    public List<string> followers = new List<string>();

    [HideInInspector] public int currentLevelNo;
    [HideInInspector] public GameObject currentLevelPrefab;
    [HideInInspector] public PathCreator pathCreator;
    [HideInInspector] public bool hasGameStarted;
    [HideInInspector] public bool isGameOver;

    [SerializeField] private List<HandGroup> handGroups = new List<HandGroup>();
    [SerializeField] private PathFollower playerPathFollower;
    [SerializeField] private GameObject tattooGun;

    private int _handId;
    private Collsion _mainHandCollision;
    private Camera _mainCamera;
    private CameraController _cameraController;
    private TextureManager _textureManager;
    private GameObject _pathObj;
    // public GameObject Boss;
    // public GameObject PivotParent;

    public EGameMode gameMode;
    [SerializeField] private int specificLevelId;

    public override void Start()
    {
        base.Start();

        _mainCamera = Camera.main;

        if (_mainCamera != null)
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

        LoadLevelPrefab();

        if (_pathObj == null)
        {
            _pathObj = GameObject.Find("pathWAY");
            pathCreator = _pathObj.GetComponent<PathCreator>();
            _pathObj.GetComponent<RoadMeshCreator>().refresh();    
        }
        
        playerPathFollower.enabled = false;

#if UNITY_EDITOR
        if (gameMode == EGameMode.Test)
        {
            PlaySpecificLevel(specificLevelId);    
        }
#endif
    }

    private void Update()
    {
        // if (Boss == null)
        // {
        //     Boss = GameObject.Find("npc_Boss");
        // }
        //
        // if (PivotParent == null)
        // {
        //     PivotParent = GameObject.Find("Parent");
        // }
    }

    private void PlaySpecificLevel(int levelId)
    {
        currentLevelNo = levelId;
        PlayerPrefs.SetInt("current_scene", currentLevelNo);
    }

    #region Level Setup

    private void LoadLevelPrefab()
    {
        currentLevelNo = PlayerPrefs.GetInt("current_scene", 0);
        currentLevelPrefab = Instantiate(LevelPrefabManager.Instance.GetCurrentLevelPrefab());

        SetLevelDetails(currentLevelPrefab);
    }

    private void SetLevelDetails(GameObject levelPrefab)
    {
        Leveldetails levelDetails = levelPrefab.GetComponent<Leveldetails>();
        SetTattoosBasedOnId(levelDetails.tattooId);
    }

    private void SetTattoosBasedOnId(int tattooId)
    {
        switch (tattooId)
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

    #endregion

    #region Gameplay Initial Setup

    public void StartGameplay()
    {
        UiManager.Instance.ClearUIOnGameStart();
        // PivotParent = GameObject.FindGameObjectWithTag("PivotParent");
        // Boss = GameObject.FindGameObjectWithTag("EndIt");
        playerPathFollower.transform.DOMoveX(.1f, .5f).OnComplete(() =>
        {
            tattooGun.transform.GetComponentInChildren<Animator>().enabled = true;
            StartCoroutine(DelayPlayerControlRoutine());
        });
    }

    IEnumerator DelayPlayerControlRoutine()
    {
        tattooGun.transform.GetChild(1).gameObject.SetActive(true);
        _mainHandCollision.DrawDefaultTattoo();

        yield return new WaitForSeconds(.5f);

        UiManager.Instance.ShowPriceTag();
        tattooGun.transform.DOMoveZ(-0.98f, .3f);

        hasGameStarted = true;
        playerPathFollower.enabled = true;
    }

    public void SpawnHand(int handId)
    {
        foreach (HandGroup hand in handGroups)
        {
            if (hand.mainHand.GetComponent<HandController>().handId == handId)
            {
                hand.mainHand.gameObject.SetActive(true);
                hand.tattooHand.gameObject.SetActive(true);
                _mainHandCollision = hand.mainHand.GetComponent<Collsion>();
                _cameraController.player = hand.mainHand.gameObject;
                SetLevelDetails(currentLevelPrefab);
            }
            else
            {
                hand.mainHand.gameObject.SetActive(false);
                hand.tattooHand.gameObject.SetActive(false);
            }
        }
    }

    #endregion


    public void ZoomEffect()
    {
        StartCoroutine(CamZoomInAndOutRoutine());
    }

    private IEnumerator CamZoomInAndOutRoutine()
    {

        Camera.main.DOFieldOfView(58, 1);
        yield return new WaitForSeconds(1);
        Camera.main.DOFieldOfView(70, .5f);
    }
}