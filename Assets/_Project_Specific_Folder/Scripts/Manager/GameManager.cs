using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using DG.Tweening;
using PathCreation.Examples;
using System;
using MySDK;
using PathCreation;
using HomaGames.HomaBelly;

public enum ERotationAxis
{
    X,
    Y
}

[Serializable]
public class HandGroup
{
    public GameObject mainHand;
    public GameObject tattooHand;
}

[Serializable]
public struct FollowerInfoSet
{
    public int value;
    public string scale;
    [Range(0, 999)] public int randomRange;
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
    public List<FollowerInfoSet> followers = new List<FollowerInfoSet>();
    

    [HideInInspector] public int currentLevelNo;
    [HideInInspector] public GameObject currentLevelPrefab;
    [HideInInspector] public PathCreator pathCreator;
    [HideInInspector] public bool hasGameStarted;
    [HideInInspector] public bool isGameOver;
    public int currentTattooGunLevel;

    [SerializeField] private List<HandGroup> handGroups = new List<HandGroup>();
    [SerializeField] private PathFollower playerPathFollower;
    [SerializeField] private List<GameObject> tattooGuns =  new List<GameObject>();
    [SerializeField] private GameObject tattooGunSpawnEffect;
    [SerializeField] private GameObject tattooEffect;
    [SerializeField] private Transform wrestlingCameraTransform;

    private int _handId;
    private GameObject _groundFog;
    private HandBehaviour _mainHandBehaviour;
    private Camera _mainCamera;
    private CameraController _cameraController;
    private GameObject _pathObj;
    private GameObject _bossParent;
    private GameObject _currentBoss;
    private GameObject _fightingRing;
    private GameObject _wrestlingPivot;
    private Leveldetails _levelDetails;
    private bool _isWrestling;
    private float _timeLeft;
    private float _timerInitialValue;
    private bool _isClicked;

    public EGameMode gameMode;
    [SerializeField] private int specificLevelId;
    
    public override void Start()
    {
        //Main Menu screen is loaded
        DefaultAnalytics.MainMenuLoaded();
        
#if UNITY_EDITOR
        if (gameMode == EGameMode.Test)
        {
            PlaySpecificLevel(specificLevelId);
        }
#endif
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
        _mainHandBehaviour = mainHandObj.transform.GetChild(0).GetComponent<HandBehaviour>();

        _cameraController.player = mainHandObj;

        LoadLevelPrefab();

        if (_pathObj == null)
        {
            _pathObj = GameObject.Find("PlayerPath");
            pathCreator = _pathObj.GetComponent<PathCreator>();
            _pathObj.GetComponent<RoadMeshCreator>().refresh();
        }
        _groundFog = GameObject.FindGameObjectWithTag("Environment");

        playerPathFollower.enabled = false;

        currentTattooGunLevel = PlayerPrefs.GetInt("CoolnessUpgradeLevel", 1) - 1;
        tattooGuns[currentTattooGunLevel].SetActive(true);

        // Setting Default
        UAManager.Instance.SkyColor = new Color32(184, 190,255,255);
        if (_groundFog != null)
        {
            _groundFog.GetComponent<Renderer>().material.SetColor("_HeightFogColor", new Color(121, 132, 255, 255));   
        }
        else
        {
            Debug.Log("Ground Fog Not Found");
        }
        UAManager.Instance.HandId = PlayerPrefs.GetInt("SelectedHandCardId", 0);
    }

    private void Update()
    {
        // For Homa UA 
        // SpawnHand(UAManager.Instance.HandId);
        // For Homa UA 
        RenderSettings.skybox.SetColor("_SkyColor2", UAManager.Instance. SkyColor);
        // For Homa UA 
        if (_groundFog != null)
        {
            _groundFog.GetComponent<Renderer>().material.SetColor("_HeightFogColor", UAManager.Instance.HeightFogColor);   
        }
        else
        {
            Debug.Log("Ground Fog Not Found");
        }

        if (_bossParent == null)
        {
            _bossParent = GameObject.Find("Bosses");
            _fightingRing = _bossParent.transform.parent.GetChild(3).gameObject;
        }

        if (_wrestlingPivot == null)
        {
            _wrestlingPivot = GameObject.Find("WrestlingPivot");
        }

        if (isGameOver)
        {
            return;
        }
        
        if (Input.GetMouseButton(0))
        {
            if (_wrestlingPivot != null)
            {
                _wrestlingPivot.transform.DOKill();
            }
        }
        
        if (_isWrestling && !isGameOver)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                _timeLeft = 0.4f;

                if (_timerInitialValue < 1f)
                {
                    _timerInitialValue += 0.12f;

                    _wrestlingPivot.GetComponent<Rotator>().enabled = false;
                    _wrestlingPivot.transform.DOLocalRotate(new Vector3(_wrestlingPivot.transform.eulerAngles.x + _timerInitialValue + 16f, 0f, 0f), 0.1f);
                    _mainCamera.transform.DOShakePosition(1.5f, 0.01f);
                    _mainCamera.DOFieldOfView(55, 2f);
                    _isClicked = true;
                }
            }
            else
            {
                if (_isClicked)
                {
                    _timeLeft -= Time.deltaTime;

                    if (_timeLeft < 0)
                    {
                        _timeLeft = 0.4f;
                        _wrestlingPivot.transform.DOLocalRotate(new Vector3(-22f, 0f, 0f), 1.5f).SetEase(Ease.InSine);
                        _isClicked = false;
                    }
                }
            }

            if (_timerInitialValue > 0)
            {
                _timerInitialValue -= 0.0071f;
            }
        }
    }
    
    public void PlayPoseAnimation(int index)
    {
        _mainHandBehaviour.PlayPoseAnimation(index);
    }

    public void ResetPose()
    {
        _mainHandBehaviour.ResetPose();
    }

    #region Upgrade Buttons Functionality

    public int GetTotalTattooGunAmount()
    {
        return tattooGuns.Count;
    }

    public void UpgradeTattooGun()
    {
        tattooGuns[currentTattooGunLevel - 1].SetActive(false);
        tattooGuns[currentTattooGunLevel].SetActive(true);
        tattooGunSpawnEffect.GetComponent<ParticleSystem>().Play();

        int currentTattooLevel = PlayerPrefs.GetInt("CurrentTattooTypeLevel" + _levelDetails.tattooId, 0);
        PlayerPrefs.SetInt("CurrentTattooTypeLevel" + _levelDetails.tattooId, currentTattooLevel + 1);
    }

    #endregion
    
    public void FinishWrestling()
    {
        UiManager.Instance.tapFastPanel.SetActive(false);
        _mainCamera.DOKill();
        _mainCamera.transform.DOShakePosition(1f, .1f);
        _mainCamera.DOFieldOfView(70f, 1f);
        isGameOver = true;
        _wrestlingPivot.transform.GetChild(1).parent = null;
        _mainHandBehaviour.transform.parent.DOLocalMoveY(0.8f, 0.3f);
        _mainHandBehaviour.tattooHand.transform.parent.DOLocalMoveY(0.8f, 0.3f);
        _wrestlingPivot.transform.DOLocalRotate(new Vector3(-40f, -20f, 20f), 0.3f);
        _mainHandBehaviour.mainHandAnimator.Play("gesture04");
        _mainHandBehaviour.tattooHandAnimator.Play("gesture04");
        StartCoroutine(UiManager.Instance.CrossOpponentOnInfluenceMeter());
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
        _levelDetails = levelPrefab.GetComponent<Leveldetails>();
        _mainHandBehaviour.tattooGroupId = _levelDetails.tattooId;
    }

    #endregion

    #region Gameplay Initial Setup

    public void StartGameplay()
    {
        // Invoke this method everytime the user starts the gameplay at any level
        DefaultAnalytics.GameplayStarted();
        // Invoke this every time player starts the level. Levels should start at 1
        DefaultAnalytics.LevelStarted(PlayerPrefs.GetInt("current_scene_text"+1));

        UiManager.Instance.ClearUIOnGameStart();
        UiManager.Instance.MovePriceTag();
        
        playerPathFollower.transform.DOMoveX(-0.55f, .5f).OnComplete(() =>
        {
            tattooGuns[currentTattooGunLevel].GetComponent<Animator>().enabled = true;
            StartCoroutine(DelayPlayerControlRoutine());
        });
    }

    IEnumerator DelayPlayerControlRoutine()
    {
        tattooEffect.SetActive(true);
        _mainHandBehaviour.DrawDefaultTattoo();

        yield return new WaitForSeconds(.5f);
        
        tattooGuns[currentTattooGunLevel].transform.parent.DOMoveZ(-0.98f, .3f);

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
                _mainHandBehaviour = hand.mainHand.transform.GetChild(0).GetComponent<HandBehaviour>();
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

    public void WrestlingSetup(int bossHandId)
    {
        _currentBoss = _bossParent.transform.GetChild(bossHandId).gameObject;
        
        Transform mainHandTransform = _mainHandBehaviour.transform.parent;
        Transform tattooHandTransform = _mainHandBehaviour.tattooHand.transform.parent;
        Transform playerTransform = mainHandTransform.parent;
        Transform mainCameraTransform = _mainCamera.transform;
        Transform endTransform = _bossParent.transform.parent;
        
        playerTransform.parent = _wrestlingPivot.transform;
        _currentBoss.transform.parent = _wrestlingPivot.transform;
        mainCameraTransform.parent = endTransform;
        
        playerTransform.localPosition = new Vector3(1.6f, -0.28f, 0.03f);
        mainHandTransform.localPosition = new Vector3(0.48f, 5.74f, 3.12f);
        tattooHandTransform.localPosition = new Vector3(0.48f, 5.74f, 3.12f);

        _cameraController.enabled = false;
        mainCameraTransform.position = wrestlingCameraTransform.position;
        mainCameraTransform.eulerAngles = wrestlingCameraTransform.eulerAngles;
        _mainCamera.fieldOfView = 75f;

        mainHandTransform.localEulerAngles = new Vector3(0f, -90f, 9f);
        tattooHandTransform.localEulerAngles = new Vector3(0f, -90f, 9f);
        
        _mainHandBehaviour.mainHandAnimator.Play("Wrestle");
        _mainHandBehaviour.tattooHandAnimator.Play("Wrestle");
        _fightingRing.SetActive(true);
        _currentBoss.SetActive(true);
        _currentBoss.transform.GetComponent<Animator>().enabled = true;

        endTransform.GetChild(4).GetComponent<EndDetector>().endEffect = _currentBoss.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0)
            .GetChild(1).GetComponent<ParticleSystem>();
        endTransform.GetChild(4).GetComponent<EndDetector>().endEffect.Play();
        _wrestlingPivot.GetComponent<Rotator>().enabled = true;
        UiManager.Instance.tapFastPanel.SetActive(true);
        _isWrestling = true;
    }
}