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
using GameAnalyticsSDK;
using UnityEngine.Serialization;

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
    [Header("Ad Section")] 
    public bool isBannerAdEnabled;
    public bool isInterstitialAdEnabled;
    public int interstitialAdStartLevel;
    public EGameMode gameMode;
    public int currentTattooGunLevel;
    public List<GameObject> levelPrefabs = new List<GameObject>();
    public List<int> likes = new List<int>();
    public List<FollowerInfoSet> followers = new List<FollowerInfoSet>();
    public List<HandGroup> handGroups = new List<HandGroup>();
    public PlayerPathFollower playerPathFollower;
    
    [HideInInspector] public int currentLevelNo;
    [HideInInspector] public GameObject currentLevelPrefab;
    [HideInInspector] public PathCreator pathCreator;
    [HideInInspector] public bool hasGameStarted;
    [HideInInspector] public bool isGameOver;
    [HideInInspector] public bool isGoldenTattooGunActivated;
    [HideInInspector] public bool isWrestling;
    [HideInInspector] public Camera mainCamera;
    
    [SerializeField] private GameObject customTattooDrawScreen;
    [SerializeField] private Transform wrestlingCameraTransform;
    [SerializeField] private int specificLevelId;
    
    [Header("Tattoo Gun Section")]
    [SerializeField] private List<GameObject> tattooGuns =  new List<GameObject>();
    [SerializeField] private Color goldenTattooGunMaterialColor;
    [SerializeField] private float goldenTattooGunMaterialSmoothness;
    [SerializeField] private float goldenTattooGunMaterialMetallic;
    [SerializeField] private GameObject tattooGunSpawnEffect;
    [SerializeField] private GameObject tattooEffect;
    [SerializeField] private PostProcessHandler postProcessHandler;
    
    private static readonly int SHPropSmoothness = Shader.PropertyToID("_Glossiness");
    private static readonly int SHPropMetallic = Shader.PropertyToID("_Metallic");
    
    private int _handId;
    private HandBehaviour _mainHandBehaviour;
    private CameraController _cameraController;
    private GameObject _pathObj;
    private GameObject _bossParent;
    private GameObject _currentBoss;
    private GameObject _fightingRing;
    private GameObject _wrestlingPivot;
    private Leveldetails _levelDetails;
    private float _timeLeft;
    private float _timerInitialValue;
    private bool _isClicked;

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

        mainCamera = Camera.main;

        if (mainCamera != null)
        {
            _cameraController = mainCamera.GetComponent<CameraController>();
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
        
        playerPathFollower.enabled = false;

        currentTattooGunLevel = PlayerPrefs.GetInt("CoolnessUpgradeLevel", 1) - 1;

        if (currentTattooGunLevel >= GetTotalTattooGunAmount())
        {
            currentTattooGunLevel %= GetTotalTattooGunAmount();
            
            isGoldenTattooGunActivated = true;
        }

        tattooGuns[currentTattooGunLevel].SetActive(true);

        if (isGoldenTattooGunActivated)
        {
            UpdateTattooGunMaterial(tattooGuns[currentTattooGunLevel].GetComponent<MeshRenderer>());
        }

        UAManager.Instance.handId = PlayerPrefs.GetInt("SelectedHandCardId", 0);
        
        // Banner Ad
        if (isBannerAdEnabled)
        {
            Events.onBannerAdLoadedEvent+= OnBannerAdLoadedEvent;
            HomaBelly.Instance.LoadBanner();    
        }
    }

    private void UpdateTattooGunMaterial(MeshRenderer meshRenderer)
    {
        Material material = meshRenderer.material;
        
        material.mainTexture = null;
        material.color = goldenTattooGunMaterialColor;
        material.SetFloat(SHPropSmoothness, goldenTattooGunMaterialSmoothness);
        material.SetFloat(SHPropMetallic, goldenTattooGunMaterialMetallic);
    }

    private void OnBannerAdLoadedEvent(string obj)
    {
        HomaBelly.Instance.ShowBanner();
    }

    private void Update()
    {
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

        if (isWrestling)
        {
            if (Input.GetMouseButton(0))
            {
                if (_wrestlingPivot != null)
                {
                    _wrestlingPivot.transform.DOKill();
                }
            }

            if (!isGameOver)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    _timeLeft = 0.4f;

                    if (_timerInitialValue < 1f)
                    {
                        _timerInitialValue += 0.12f;

                        if (_wrestlingPivot != null)
                        {
                            _wrestlingPivot.GetComponent<Rotator>().enabled = false;
                            _wrestlingPivot.transform.DOLocalRotate(new Vector3(_wrestlingPivot.transform.eulerAngles.x + _timerInitialValue + 16f, 0f, 0f),
                                0.1f);
                        }

                        mainCamera.transform.DOShakePosition(1.5f, 0.01f);
                        mainCamera.DOFieldOfView(55, 2f);
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
                            if (_wrestlingPivot != null)
                            {
                                _wrestlingPivot.transform.DOLocalRotate(new Vector3(-22f, 0f, 0f), 1.5f).SetEase(Ease.InSine);
                            }

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
        tattooGuns[currentTattooGunLevel == 0 ? GetTotalTattooGunAmount() - 1 : currentTattooGunLevel - 1].SetActive(false);
        tattooGuns[currentTattooGunLevel].SetActive(true);

        if (isGoldenTattooGunActivated)
        {
            UpdateTattooGunMaterial(tattooGuns[currentTattooGunLevel].GetComponent<MeshRenderer>());
        }
        
        tattooGunSpawnEffect.GetComponent<ParticleSystem>().Play();

        int currentTattooLevel = PlayerPrefs.GetInt("CurrentTattooTypeLevel" + _levelDetails.tattooId, 0);
        PlayerPrefs.SetInt("CurrentTattooTypeLevel" + _levelDetails.tattooId, currentTattooLevel + 1);
        
        _mainHandBehaviour.ResetHandTattooStatus();
    }

    #endregion
    
    public void FinishWrestling()
    {
        UiManager.Instance.tapFastPanel.SetActive(false);
        mainCamera.DOKill();
        mainCamera.transform.DOShakePosition(1f, .1f);
        mainCamera.DOFieldOfView(70f, 1f);
        isGameOver = true;
        _wrestlingPivot.transform.GetChild(1).parent = null;
        _mainHandBehaviour.transform.parent.DOLocalMoveY(0.8f, 0.3f);
        _mainHandBehaviour.tattooHand.transform.parent.DOLocalMoveY(0.8f, 0.3f);
        _wrestlingPivot.transform.DOLocalRotate(new Vector3(-40f, -20f, 20f), 0.3f);
        _mainHandBehaviour.mainHandAnimator.Play("gesture04");
        _mainHandBehaviour.tattooHandAnimator.Play("gesture04");
        StartCoroutine(UiManager.Instance.CrossOpponentOnInfluenceMeter());
    }
    
    public void PlaySpecificLevel(int levelId)
    {
        currentLevelNo = levelId;
        PlayerPrefs.SetInt("current_scene", currentLevelNo);
    }

    #region Level Setup

    private void LoadLevelPrefab()
    {
        currentLevelNo = PlayerPrefs.GetInt("current_scene", 0);
        currentLevelPrefab = Instantiate(LevelPrefabManager.Instance.GetCurrentLevelPrefab());

        string levelId = (PlayerPrefs.GetInt("current_scene_text", 0) + 1).ToString();
        float duration = Time.time - PlayerPrefs.GetFloat("StartTime", 0);
        
        // Level Events
        // Level Reached Event
        HomaBelly.Instance.TrackDesignEvent("Levels:Reached:" + levelId, duration);

        SetLevelDetails(currentLevelPrefab);
    }

    private void SetLevelDetails(GameObject levelPrefab)
    {
        _levelDetails = levelPrefab.GetComponent<Leveldetails>();

        _mainHandBehaviour.tattooGroupId = _levelDetails.tattooId;
    }
    
    public void UASetTattoo(int id)
    {
        _mainHandBehaviour.tattooGroupId = id;
        _mainHandBehaviour.UAResetHandTattooStatus();
    }

    #endregion

    #region Gameplay Initial Setup

    public void StartGameplay()
    {
         GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Game Start");
        // Invoke this method everytime the user starts the gameplay at any level
        DefaultAnalytics.GameplayStarted();
        
        // Invoke this every time player starts the level. Levels should start at 1
        
        string levelId = (PlayerPrefs.GetInt("current_scene_text", 0) + 1).ToString();
        
        // Progression events
        // Level Start Event
        DefaultAnalytics.LevelStarted(levelId);

        PlayerPrefs.SetFloat("LevelStartTime", Time.time);
        
        UiManager.Instance.ClearUIOnGameStart();
        UiManager.Instance.MovePriceTag();

        playerPathFollower.transform.DOMoveX(-0.55f, .5f).OnComplete(EnableTattooGun);
    }

    private void EnableCustomTattooDrawingScreen()
    {
        mainCamera.gameObject.SetActive(false);
        customTattooDrawScreen.SetActive(true);
        UiManager.Instance.DisablePriceTag();
    }

    public GameObject customTattooObj;
    public void SetCustomTattooOnTattooHand(Texture customTattoo)
    {
        customTattooObj = _mainHandBehaviour.tattooHand.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(5).gameObject;
        customTattooObj.SetActive(true);
        customTattooObj.GetComponent<MeshRenderer>().material.DOFade(0, 0);
        customTattooObj.GetComponent<MeshRenderer>().material.mainTexture = customTattoo;
        
        UiManager.Instance.EnablePriceTag();
        StartCoroutine(DelayPlayerControlRoutine());
    }

    private void EnableTattooGun()
    {
        tattooEffect.SetActive(true);
        tattooGuns[currentTattooGunLevel].GetComponent<Animator>().enabled = true;
        Invoke(nameof(EnableCustomTattooDrawingScreen), 1f);
    }

    IEnumerator DelayPlayerControlRoutine()
    {
        // Normal Gameplay
        // tattooEffect.SetActive(true);
        // _mainHandBehaviour.DrawDefaultTattoo();

        // Playable Ad Gameplay
        customTattooObj.GetComponent<MeshRenderer>().material.DOFade(1, 1.8f);
        
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
    
    public void UASpawnHand(int id)
    {
        foreach (HandGroup hand in handGroups)
        {
            if (hand.mainHand.GetComponent<HandController>().handId == id)
            {
                hand.mainHand.gameObject.SetActive(true);
                hand.tattooHand.gameObject.SetActive(true);
                _mainHandBehaviour = hand.mainHand.transform.GetChild(0).GetComponent<HandBehaviour>();
                _cameraController.player = hand.mainHand.gameObject;
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
        postProcessHandler.RemoveAllEffects();
        
        _currentBoss = _bossParent.transform.GetChild(bossHandId).gameObject;
        
        Transform mainHandTransform = _mainHandBehaviour.transform.parent;
        Transform tattooHandTransform = _mainHandBehaviour.tattooHand.transform.parent;
        Transform playerTransform = mainHandTransform.parent;
        Transform mainCameraTransform = mainCamera.transform;
        Transform endTransform = _bossParent.transform.parent;
        
        playerTransform.parent = _wrestlingPivot.transform;
        _currentBoss.transform.parent = _wrestlingPivot.transform;
        mainCameraTransform.parent = endTransform;

        playerTransform.DOLocalMove(new Vector3(.35f, -.28f, .03f), 0.5f).OnComplete(() =>
        {
            playerTransform.localPosition = new Vector3(1.547f, -.304f, 0.037f);
            mainHandTransform.localPosition = new Vector3(0.48f, 5.74f, 3.12f);
            tattooHandTransform.localPosition = new Vector3(0.48f, 5.74f, 3.12f);
            
            _cameraController.enabled = false;
            // mainCameraTransform.position = wrestlingCameraTransform.position;
            // mainCameraTransform.eulerAngles = wrestlingCameraTransform.eulerAngles;
            // mainCamera.fieldOfView = 75f;
            mainCameraTransform.DOMove(wrestlingCameraTransform.position, .1f);
            mainCameraTransform.DORotate(wrestlingCameraTransform.eulerAngles, .11f);
            mainCamera.DOFieldOfView(75f, 0.1f);
            
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
        });
        // playerTransform.localPosition = new Vector3(1.736f, -0.145f, 0.072f);
        // mainHandTransform.localPosition = new Vector3(0.48f, 5.74f, 3.12f);
        // tattooHandTransform.localPosition = new Vector3(0.48f, 5.74f, 3.12f);

        // _cameraController.enabled = false;
        // mainCameraTransform.position = wrestlingCameraTransform.position;
        // mainCameraTransform.eulerAngles = wrestlingCameraTransform.eulerAngles;
        // mainCamera.fieldOfView = 75f;

        // mainHandTransform.localEulerAngles = new Vector3(0f, -90f, 9f);
        // tattooHandTransform.localEulerAngles = new Vector3(0f, -90f, 9f);
        //
        // _mainHandBehaviour.mainHandAnimator.Play("Wrestle");
        // _mainHandBehaviour.tattooHandAnimator.Play("Wrestle");
        // _fightingRing.SetActive(true);
        // _currentBoss.SetActive(true);
        // _currentBoss.transform.GetComponent<Animator>().enabled = true;
        //
        // endTransform.GetChild(4).GetComponent<EndDetector>().endEffect = _currentBoss.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0)
        //     .GetChild(1).GetComponent<ParticleSystem>();
        // endTransform.GetChild(4).GetComponent<EndDetector>().endEffect.Play();
        // _wrestlingPivot.GetComponent<Rotator>().enabled = true;
        // UiManager.Instance.tapFastPanel.SetActive(true);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        float duration = Time.time - PlayerPrefs.GetFloat("StartTime", 0);
        
        // Session
        // Session Player Event
        if (PlayerPrefs.GetInt("GameOpenCount", 0) < 100)
        {
            HomaBelly.Instance.TrackDesignEvent("Session:"+PlayerPrefs.GetInt("GameOpenCount", 0)+":Played", duration);
        }
    }
}