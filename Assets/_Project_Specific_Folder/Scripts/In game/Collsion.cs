using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using MoreMountains.NiceVibrations;
using PathCreation.Examples;


public class Collsion : MonoBehaviour
{
    private class CollectedGoodTattoosAttributes
    {
        public readonly Texture2D collectedGoodTattoo;
        public readonly int collectedGoodTattooLevel;

        public CollectedGoodTattoosAttributes(Texture2D collectedGoodTattoo, int collectedGoodTattooLevel)
        {
            this.collectedGoodTattoo = collectedGoodTattoo;
            this.collectedGoodTattooLevel = collectedGoodTattooLevel;
        }
    }
    public GameObject tattooHand;
    public Animator scoreAnimator;
    public AnimatorOverrideController animatorOverrideController;
    public AnimationClip[] animationClips;
    public Color goodGateScorePopUpColor;
    public Color badGateScorePopUpColor;
    
    [HideInInspector] public Animator mainHandAnimator;
    [HideInInspector] public Animator tattooHandAnimator;
    [HideInInspector] public Texture2D defaultTattoo;
    [HideInInspector] public List<Texture2D> expensiveTattoos;
    [HideInInspector] public List<Texture2D> cheapTattoos;
    [HideInInspector] public List<Texture2D> expensiveBlueTattoos;
    [HideInInspector] public List<Texture2D> expensiveYellowTattoos;
    [HideInInspector] public List<Texture2D> cheapBlueTattoos;
    [HideInInspector] public List<Texture2D> cheapYellowTattoos;
    [HideInInspector] public List<int> expensiveColorTattooIdSequences;
    [HideInInspector] public List<int> cheapColorTattooIdSequences;
    
    private HandController _tattooHandController;
    private HandController _mainHandController;
    private ParticleSystem _hurtEffect;
    private ParticleSystem _shineEffect;
    private readonly List<int> _animationIndexes = new List<int>();
    private static readonly int Gesture = Animator.StringToHash("Gesture");
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private static readonly int SHPropTexture = Shader.PropertyToID("_MainTex");
    private MaterialPropertyBlock _mpb;
    private MaterialPropertyBlock Mpb => _mpb ??= new MaterialPropertyBlock();
    private bool _shouldChangeTattoo;
    private bool _hasGoneThroughGoodGate;
    [SerializeField] private List<CollectedGoodTattoosAttributes> _collectedGoodTattoosAttributes = new List<CollectedGoodTattoosAttributes>();
    private int _currentExpensiveTattooLevel;
    private int _currentCheapTattooLevel;
    private PathFollower _playerPathFollower;
    private float _playerInitialSpeed;


    public Camera cam;
    public Text LevelText, ColorText;


    public GameObject SecondHand;
    // public Texture Burnt;
    public Texture[] Tattos, CheapTttos;
    public Texture[] BadBlue, GoodBlue, GoodYellow, BadYellow;
    public Material StiackerMat;
    public int min = 0, max = 255;
    public Texture Default;

    [SerializeField] bool IsGoodGate;
    Vector3 Startpos;
    public float Multiplier;
    public bool StartTapRoutine;
    
    bool IsYellow, IsBlue;
    private float timeLeft = .4f;
    [SerializeField] public ParticleSystem Ps;
    bool m_FirstClick;
    [SerializeField] bool m_isTapping;

    [SerializeField] int i; public int SavedTattooNo;
    private int _targetTattooValue;
    private float _currentTattooValue;
    private bool _shouldUpdateCash;
    private bool _isUnlockScreenEnabled;
    private float _incrementAmount;
    [SerializeField]int LastLevel;
    [SerializeField] bool IsGood;

    [SerializeField] int j = 2;
    [SerializeField] int m_i;
    public List<Texture> Dummy = new List<Texture>();

    public int lastGateId;
    private new Camera _camera;
    
    private static readonly int IsWrestling = Animator.StringToHash("isWrestling");


    
    


    [Header("Hand Ornaments Section")]
    public List<GameObject> rings = new List<GameObject>();
    public List<GameObject> bracelets = new List<GameObject>();
    

    private void Start()
    {
        _mainHandController = GetComponent<HandController>();
        _tattooHandController = tattooHand.GetComponent<HandController>();
        
        _hurtEffect = transform.GetChild(2).GetComponent<ParticleSystem>();
        _shineEffect = transform.GetChild(3).GetComponent<ParticleSystem>();

        mainHandAnimator = GetComponent<Animator>();
        tattooHandAnimator = tattooHand.GetComponent<Animator>();
        
        animatorOverrideController = new AnimatorOverrideController
        {
            runtimeAnimatorController = mainHandAnimator.runtimeAnimatorController
        };
        
        for (int clipIndex = 0; clipIndex < animationClips.Length; clipIndex++)
        {
            _animationIndexes.Add(clipIndex);
        }

        _skinnedMeshRenderer = tattooHand.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        _skinnedMeshRenderer.material.DOFade(0, 0f);
        
        _collectedGoodTattoosAttributes.Add(new CollectedGoodTattoosAttributes(defaultTattoo, -1));

        _playerPathFollower = GetComponentInParent<PathFollower>();
        _playerInitialSpeed = _playerPathFollower.maxSpeed;
        
        
        _camera = Camera.main;
        cam = GameManager.Instance.FakeCam;
        Startpos = transform.localPosition;

    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;

        // if (Input.GetMouseButton(0))
        // {
        //     if (GameManager.Instance.PivotParent != null)
        //     {
        //         DOTween.Kill(GameManager.Instance.PivotParent.transform);   
        //     }
        //     m_isTapping = true;
        // }
        //
        // if (Input.GetMouseButtonUp(0))
        // {
        //     m_isTapping = false;
        // }
        //
        //
        //
        // if (StartTapRoutine)
        // {
        //
        //     if (Input.GetMouseButtonDown(0) || (Input.GetKeyDown("space")))
        //     {
        //         timeLeft = .4f;
        //         if (UiManager.Instance.timerInitvalue < 1f)
        //         {
        //             UiManager.Instance.timerInitvalue += 0.12f;
        //
        //             UiManager.Instance.Timer.fillAmount = UiManager.Instance.timerInitvalue;
        //
        //             UiManager.Instance.Timer.fillAmount = UiManager.Instance.timerInitvalue;
        //             GameManager.Instance.PivotParent.GetComponent<MySDK.Rotator>().enabled = false;
        //             GameManager.Instance.PivotParent.transform.DOLocalRotate(
        //                 new Vector3((GameManager.Instance.PivotParent.transform.eulerAngles.x + UiManager.Instance.timerInitvalue + 8f), 0, 0), .1f);
        //
        //             Camera.main.transform.DOShakePosition(1.5f, .01f);
        //             Camera.main.DOFieldOfView(50, 2);
        //             m_FirstClick = true;
        //         }
        //     }
        //     else
        //     {
        //
        //         if (m_FirstClick)
        //         {
        //             timeLeft -= Time.deltaTime;
        //
        //             if (timeLeft < 0)
        //             {
        //                 timeLeft = .4f;
        //
        //                 GameManager.Instance.PivotParent.transform.DOLocalRotate(new Vector3(-22, 0, 0), 1.5f).SetEase(Ease.InSine);
        //             }
        //         }
        //     }
        //
        //
        //     if (UiManager.Instance.timerInitvalue > 0f)
        //     {
        //         UiManager.Instance.timerInitvalue -= 0.0071f;
        //         UiManager.Instance.Timer.fillAmount = UiManager.Instance.timerInitvalue;
        //     }
        // }
    }
    
    private void CommonObstacleHitEffects()
    {
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        StartCoroutine(SpeedSlowDownRoutine());
    }
    
    private void OnTriggerEnter(Collider other)
    {
        #region Normal Gates
        if (other.gameObject.CompareTag("GoodGate"))
        {
            _hasGoneThroughGoodGate = true;
            other.GetComponent<BoxCollider>().enabled = false;
            Gates gate = other.GetComponentInParent<Gates>();
            
            NormalGateEnteringEffects(gate, true);
            
            // Went through special good gates
            if (gate.isSpecial)
            {
                UiManager.Instance.priceTag.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f).SetLoops(4, LoopType.Yoyo);
            }
            else
            {
                _currentExpensiveTattooLevel = gate.gateLevel + 1;
                _collectedGoodTattoosAttributes.Add(new CollectedGoodTattoosAttributes(expensiveTattoos[gate.gateLevel], gate.gateLevel));
                StartCoroutine(UpdateTattooTexture(expensiveTattoos[gate.gateLevel]));
            }
        }
        
        if (other.gameObject.CompareTag("BadGate"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            Gates gate = other.GetComponentInParent<Gates>();
            
            NormalGateEnteringEffects(gate, false);
            
            UiManager.Instance.priceTag.GetComponent<Image>().DOColor(Color.red, 0.5f).SetLoops(2, LoopType.Yoyo);
            
            // Went through last bad gate
            if (gate.isLast)
            {
                StartCoroutine(UpdateTattooTexture(cheapTattoos[gate.gateLevel]));    
            }
            else
            {
                _shouldChangeTattoo = !_shouldChangeTattoo;

                if (_shouldChangeTattoo)
                {
                    // Went through bad gate after visiting good gates
                    if (_hasGoneThroughGoodGate)
                    {
                        if (_collectedGoodTattoosAttributes.Count > 1)
                        {
                            CollectedGoodTattoosAttributes collectedGoodTattoosAttribute =
                                _collectedGoodTattoosAttributes[_collectedGoodTattoosAttributes.Count - 2];
                            
                            _currentExpensiveTattooLevel = collectedGoodTattoosAttribute.collectedGoodTattooLevel + 1;
                            
                            StartCoroutine(UpdateTattooTexture(collectedGoodTattoosAttribute.collectedGoodTattoo));
                            _collectedGoodTattoosAttributes.Remove(collectedGoodTattoosAttribute);
                        }
                    }
                    else
                    {
                        if (_currentCheapTattooLevel < cheapTattoos.Count)
                        {
                            _currentCheapTattooLevel += 1;
                        }
                        
                        StartCoroutine(UpdateTattooTexture(cheapTattoos[_currentCheapTattooLevel - 1]));    
                    }
                }    
            }
        }
        #endregion

        #region Color Gates
        if (other.gameObject.CompareTag("Blue"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            CommonGateEnteringEffects();

            if (_hasGoneThroughGoodGate)
            {
                if (expensiveColorTattooIdSequences.Contains(_currentExpensiveTattooLevel))
                {
                    StartCoroutine(UpdateTattooTexture(expensiveBlueTattoos[expensiveColorTattooIdSequences.IndexOf(_currentExpensiveTattooLevel)]));
                }
            }
            else
            {
                if (cheapColorTattooIdSequences.Contains(_currentCheapTattooLevel))
                {
                    StartCoroutine(UpdateTattooTexture(cheapBlueTattoos[cheapColorTattooIdSequences.IndexOf(_currentCheapTattooLevel)]));
                }
            }
        }
        
        if (other.gameObject.CompareTag("Yellow"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            CommonGateEnteringEffects();

            if (_hasGoneThroughGoodGate)
            {
                if (expensiveColorTattooIdSequences.Contains(_currentExpensiveTattooLevel))
                {
                    StartCoroutine(UpdateTattooTexture(expensiveYellowTattoos[expensiveColorTattooIdSequences.IndexOf(_currentExpensiveTattooLevel)]));
                }
            }
            else
            {
                if (cheapColorTattooIdSequences.Contains(_currentCheapTattooLevel))
                {
                    StartCoroutine(UpdateTattooTexture(cheapYellowTattoos[cheapColorTattooIdSequences.IndexOf(_currentCheapTattooLevel)]));
                }
            }
        }
        #endregion

        #region Ornament Gates
        if(other.gameObject.CompareTag("Ring"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            CommonGateEnteringEffects();
            rings[other.gameObject.GetComponent<OrnamentGates>().ornamentId].gameObject.SetActive(true);
        }
        if (other.gameObject.CompareTag("Bracelet"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            CommonGateEnteringEffects();
            bracelets[other.gameObject.GetComponent<OrnamentGates>().ornamentId].gameObject.SetActive(true);
        }
        #endregion

        #region Obstacles
        if (other.gameObject.CompareTag("Remove"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            CommonObstacleHitEffects();
            
            _hurtEffect.Play();
            StartCoroutine(UiManager.Instance.HurtScreenRoutine());
            RemoveTattoo();
            
            mainHandAnimator.Play("Hurt");
            tattooHandAnimator.Play("Hurt");
        }
        #endregion
        

        if (other.gameObject.CompareTag("Spike"))
        {
            // GameManager.Instance.Level = other.GetComponent<DownGrade>().DownGradeAmmount;
            UiManager.Instance.priceTag.GetComponent<Image>().DOColor(Color.red, 0.5f).SetLoops(2, LoopType.Yoyo);
            DownGradeTexture(GameManager.Instance.Level, other.gameObject);
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
            StartCoroutine(SpeedSlowDownRoutine());
            StartCoroutine(UiManager.Instance.FdeDelayRoutine());
            tattooHandAnimator.Play("Hurt");
            mainHandAnimator.Play("Hurt");
            other.GetComponent<BoxCollider>().enabled = false;
            
            GameManager.Instance.Level--;
            
            if (IsGood)
            {
                m_i = Dummy.Count;
                
                StiackerMat.DOFade(0, .3f).OnComplete(() =>
                {
                    if (j < Dummy.Count+1)
                    {
                        StiackerMat.mainTexture = Dummy[m_i - j];
                        j++;
                    }
                    else
                    {
                        StiackerMat.mainTexture = Default;
                    }
                        
                    StiackerMat.DOFade(1, .5f);
                });       
            }
            else
            {
                StiackerMat.DOFade(0, .3f).OnComplete(() =>
                {
                    if (GameManager.Instance.Level == 0)
                    {
                        StiackerMat.mainTexture = CheapTttos[GameManager.Instance.Level];
                    }
                    else
                    {
                        StiackerMat.mainTexture = CheapTttos[GameManager.Instance.Level - 1];   
                    }
                    StiackerMat.DOFade(1, .5f);
                });
            }
        }

        if (other.gameObject.CompareTag("Lava"))
        {
            UiManager.Instance.priceTag.GetComponent<Image>().DOColor(Color.red, 0.5f).SetLoops(2, LoopType.Yoyo);
            DownGradeTexture(GameManager.Instance.Level, other.gameObject);
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
            StartCoroutine(SpeedSlowDownRoutine());
            StartCoroutine(UiManager.Instance.FdeDelayRoutine());
            tattooHandAnimator.Play("Hurt");
            mainHandAnimator.Play("Hurt");
            other.GetComponent<BoxCollider>().enabled = false;
            
            GameManager.Instance.Level--;
            
            if (IsGood)
            {
                m_i = Dummy.Count;
                
                StiackerMat.DOFade(0, .3f).OnComplete(() =>
                {
                    if (j < Dummy.Count+1)
                    {
                        StiackerMat.mainTexture = Dummy[m_i - j];
                        j++;
                    }
                    else
                    {
                        StiackerMat.mainTexture = Default;
                    }
                        
                    StiackerMat.DOFade(1, .5f);
                });       
            }
            else
            {
                StiackerMat.DOFade(0, .3f).OnComplete(() =>
                {
                    if (GameManager.Instance.Level == 0)
                    {
                        StiackerMat.mainTexture = CheapTttos[GameManager.Instance.Level];
                    }
                    else
                    {
                        StiackerMat.mainTexture = CheapTttos[GameManager.Instance.Level - 1];   
                    }
                    StiackerMat.DOFade(1, .5f);
                });
            }
        }

        

        if (other.gameObject.CompareTag("Finish"))
        {
            Debug.Log("level end trigger");
            // GameManager.Instance.IsLevelEnd = true;
            if (StorageManager.Instance.currentLevelScore <= 0)
            {
                StorageManager.Instance.currentLevel = PlayerPrefs.GetInt("current_scene");
                StorageManager.Instance.currentLevelText = PlayerPrefs.GetInt("current_scene_text", 0);
                StorageManager.Instance.currentLevelScore = 500;
            }

            // Camera.main.transform.gameObject.SetActive(false);
            // cam.gameObject.SetActive(true);

            // StartCoroutine(StopRoutine(other.gameObject));

            //c.enabled = false;
            //c1.enabled = false;
            //GameManager.Instance.p.enabled = false;

            //c.transform.DOLocalMoveX(-1.66f, .1f);
            //c1.transform.DOLocalMoveX(-1.66f, .1f);
            //transform.root.parent = other.transform.root;
            //cam.transform.DOLocalMove(GameManager.Instance.FinalCamPos.transform.localPosition, 0.7f);
            //cam.transform.DOLocalRotate(GameManager.Instance.FinalCamPos.transform.localEulerAngles, 0.7f).OnComplete(() =>
            //{
            //    c.transform.DOLocalRotate(new Vector3(0f, -90f, 9f), 0.1f);
            //    c1.transform.DOLocalRotate(new Vector3(0f, -90f, 9f), 0.1f);
            //    anim.Play("Wrestle");
            //    anim1.Play("Wrestle");
            //    MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            //    cam.transform.parent = other.transform.root;
            //    transform.parent.parent = GameManager.Instance.PivotParent.transform;
            //    GameManager.Instance.Boss.transform.parent = GameManager.Instance.PivotParent.transform;
            //    GameManager.Instance.Boss.transform.GetComponent<Animator>().enabled = true;

            //    transform.parent.DOLocalMove(new Vector3(0.296f, -0.038f, -0.038f), 0.3f).OnComplete(() =>
            //    {
            //        FindObjectOfType<EndDetector>().EndParticle.Play();
            //    });

            //    cam.transform.DOLocalMoveX(cam.transform.position.x + 2.5f, 0.3f).OnComplete(() =>
            //    {
            //        GameManager.Instance.PivotParent.transform.GetComponent<MySDK.Rotator>().enabled = true;
            //        StartTapRoutine = true;
            //        UiManager.Instance.TapFastPanel.SetActive(true);
            //    });
            //});

            // Camera.main.transform.DOLocalMove(GameManager.Instance.FianlCamPos.transform.localPosition, .7f);
            // Camera.main.transform.DOLocalRotate(GameManager.Instance.FianlCamPos.transform.localEulerAngles, .7f).OnComplete(() => {
            //
            //     c.transform.DOLocalRotate(new Vector3(0, -90, 9), .1f);
            //     c1.transform.DOLocalRotate(new Vector3(0, -90, 9), .1f);
            //     GameManager.Instance.Boss.transform.GetComponent<Animator>().enabled = true;
            //    
            //     anim.Play("Wrestle");
            //     anim1.Play("Wrestle");
            //     MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            //     Camera.main.transform.parent = other.transform.root;
            //     transform.parent.parent = GameManager.Instance.PivotParent.transform;
            //     GameManager.Instance.Boss.transform.parent = GameManager.Instance.PivotParent.transform;
            //
            //     this.transform.parent.DOLocalMove(new Vector3(0.296f, -0.038f, -0.038f), .3f).OnComplete(() => { FindObjectOfType<EndDetector>().EndParticle.Play(); });
            //     Camera.main.transform.DOLocalMoveX(Camera.main.transform.position.x + 2.5f, .3f).OnComplete(() => {
            //
            //         GameManager.Instance.PivotParent.transform.GetComponent<MySDK.Rotator>().enabled = true;
            //         StartTapRoutine = true;
            //         UiManager.Instance.TapFastPanel.SetActive(true);
            //
            //     });
            //
            //
            //
            // });
        }

        if (other.gameObject.CompareTag("DecisionTrigger"))
        {
            UiManager.Instance.haptics.SetActive(false);
            UiManager.Instance.scoreText.transform.parent.gameObject.SetActive(false);
            GameManager.Instance.p.enabled = false;
            mainHandAnimator.Play("idle");
            tattooHandAnimator.Play("idle");
            mainHandAnimator.transform.DOLocalMoveX(0, .2f); 
            tattooHandAnimator.transform.DOLocalMoveX(0, .2f);
            _mainHandController.enabled = false;
            _tattooHandController.enabled = false;

            GameObject mobile = other.transform.GetChild(2).gameObject;
           
            mobile.transform.DOLocalMoveY(0.88f, 0.5f);

            mobile.transform.DORotate(new Vector3(-50f, 270f, 90f), 0.5f).OnComplete(() =>
            {
                mobile.transform.DOLocalMove(new Vector3(-.66f, 0.88f, 0.113f), 1f).OnComplete(() =>
                {
                    UiManager.Instance.transitionScreen.SetActive(true);
                    UiManager.Instance.transitionScreen.GetComponent<Image>().DOFade(1f, 0.5f).OnComplete(() =>
                    {
                        _camera.transform.DORotate(new Vector3(46f, 90f, 0f), 0.01f).OnComplete(() =>
                        {
                            mobile.SetActive(false);
                            UiManager.Instance.mobileScreen.SetActive(true);
                            UiManager.Instance.transitionScreen.SetActive(false);
                            UiManager.Instance.mobileScreen.transform.GetChild(8).GetComponent<Image>().DOFade(0f, 0.5f).OnComplete(() =>
                            {
                                UiManager.Instance.mobileScreen.transform.GetChild(8).gameObject.SetActive(false);
                            });

                            int lastPhotoNo = PlayerPrefs.GetInt("SnapshotsTaken", 0);

                            if (lastPhotoNo > 0)
                            {
                                string filename = $"{Application.persistentDataPath}/Snapshots/" + lastPhotoNo + ".png";

                                byte[] savedSnapshot = File.ReadAllBytes(filename);
                                Texture2D loadedTexture = new Texture2D(720, 720, TextureFormat.ARGB32, false);
                                loadedTexture.LoadImage(savedSnapshot);

                                UiManager.Instance.mobileScreen.transform.GetChild(2).GetChild(0).GetComponent<RawImage>().texture = loadedTexture;
                            }
                            else
                            {
                                UiManager.Instance.mobileScreen.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                            }

                            UiManager.Instance.mobileScreen.transform.GetChild(3).DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.5f).SetLoops(-1, LoopType.Yoyo);
                            UiManager.Instance.mobileScreenSlider.transform.GetChild(2).GetChild(0).DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f)
                                .SetLoops(-1, LoopType.Yoyo);
                            UiManager.Instance.mobileScreen.transform.GetChild(7).GetComponent<RectTransform>().DOAnchorPosY(340, 1f)
                                .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

                            UiManager.Instance.isMobileActive = true;
                        });
                    });
                });
            });
            // mobile.transform.DOLocalMoveY(1f, 0.5f).OnComplete(() =>
            // {
            //     mobile.transform.DOLocalRotate(new Vector3(-50f, 270f, 90f), 0.5f).OnComplete(() =>
            //     {
            //         mobile.transform.DOLocalMoveX(-.65f, 1f).OnComplete(() =>
            //         {
            //             _camera.transform.DORotate(new Vector3(46f, 90f, 0f), 0.01f).OnComplete(() =>
            //             {
            //                 UiManager.Instance.mobileScreen.SetActive(true);
            //                 UiManager.Instance.mobileScreen.transform.GetChild(3).DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetLoops(-1, LoopType.Yoyo);
            //                 UiManager.Instance.mobileScreenSlider.transform.GetChild(2).GetChild(0).DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f)
            //                     .SetLoops(-1, LoopType.Yoyo);
            //                 UiManager.Instance.mobileScreen.transform.GetChild(7).GetComponent<RectTransform>().DOAnchorPosY(340, 1f)
            //                     .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            //                 
            //                 mobile.SetActive(false);
            //                 UiManager.Instance.mobileScreen.transform.GetChild(8).GetComponent<Image>().color = Color.black;
            //                 UiManager.Instance.mobileScreen.transform.GetChild(8).GetComponent<Image>().DOFade(0f, 1f);
            //                 UiManager.Instance.isMobileActive = true;
            //                 UiManager.Instance.mobileScreen.transform.GetChild(8).gameObject.SetActive(false);
            //             });
            //         });
            //     });
            // });
            // UiManager.Instance.decisionScreen.SetActive(true);
            // UiManager.Instance.cashCounter.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("$" + StorageManager.GetTotalCoin());
            // UiManager.Instance.cashCounter.SetActive(false);
        }
    }

    #region Gate Effects

    private void CommonGateEnteringEffects()
    {
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        StartCoroutine(AnimationDelayRoutine());
    }
    
    private void NormalGateEnteringEffects(Gates gate, bool isGood)
    {
        CommonGateEnteringEffects();

        int score;
        string scoreText;
        Color color;
        
        if (isGood)
        {
            score = gate.gateCost;
            scoreText = "+" + gate.gateCost;
            color = goodGateScorePopUpColor;
        }
        else
        {
            score = gate.gateCost * -1;
            scoreText = "-" + gate.gateCost;
            color = badGateScorePopUpColor;
        }
        
        StorageManager.Instance.IncreaseScore(score);
        _shineEffect.Play();

        scoreAnimator.transform.GetChild(0).gameObject.SetActive(true);
        scoreAnimator.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = scoreText;
        scoreAnimator.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().color = color;

        scoreAnimator.Play("PopUp");
    }

    #endregion
    
    
    public IEnumerator BookRoutine()
    {
       EndDetector e =  FindObjectOfType<EndDetector>();
        e.Cam.gameObject.SetActive(true);
        e.Confetti.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);
        e.Book.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        e.Book.transform.DOLocalRotate(new Vector3(45.5f, -90.19f, 13.45f), .2f);
        e.Book.GetComponent<Animator>().Play("open");
        yield return new WaitForSeconds(.2f);


        if (PlayerPrefs.GetInt("FirstTime", 0) == 0)
        {
            PlayerPrefs.SetInt("FirstTime", 1);
        }
        else if (PlayerPrefs.GetInt("FirstTime") == 1)
            e.PageToFlipRef.SetActive(true);

        yield return new WaitForSeconds(.3f);
        e.Book.transform.GetChild(2).gameObject.SetActive(true);
        SavedTattooNo = PlayerPrefs.GetInt("SavedTattooNo");

        GameManager.Instance.TextureName = GameManager.Instance.CollsionScript.StiackerMat.mainTexture.name;
        GameManager.Instance.LastTattoTexture = GameManager.Instance.CollsionScript.StiackerMat.mainTexture;
        PlayerPrefs.SetString("TattooFrame" + SavedTattooNo, GameManager.Instance.TextureName);
        PlayerPrefs.SetInt("TattoCost" + SavedTattooNo, StorageManager.Instance.currentLevelScore);
        SavedTattooNo++;
        PlayerPrefs.SetInt("SavedTattooNo", SavedTattooNo);
    }

    
    
    


    // public IEnumerator StopRoutine(GameObject g)
    //
    // {
    //     //Camera.main.transform.parent = g.transform.root;
    //
    //
    //     c.enabled = false;
    //     c1.enabled = false;
    //     c.transform.DOLocalMoveX(-1.66f, .1f);
    //     c1.transform.DOLocalMoveX(-1.66f, .1f);
    //     transform.root.parent = g.transform.root;
    //     Camera.main.transform.DOLocalMove(GameManager.Instance.FianlCamPos.transform.localPosition, .7f);
    //     Camera.main.transform.DOLocalRotate(GameManager.Instance.FianlCamPos.transform.localEulerAngles, .7f);
    //     yield return new WaitForSeconds(.8f);
    //     c.transform.DOLocalRotate(new Vector3(0, -90, 9), .1f);
    //     c1.transform.DOLocalRotate(new Vector3(0, -90, 9), .1f);
    //     GameManager.Instance.Boss.transform.GetComponent<Animator>().enabled = true;
    //     GameManager.Instance.p.enabled = false;
    //     anim.Play("Wrestle");
    //     anim1.Play("Wrestle");
    //     MMVibrationManager.Haptic(HapticTypes.MediumImpact);
    //     Camera.main.transform.parent = g.transform.root;
    //     transform.parent.parent = GameManager.Instance.PivotParent.transform;
    //     GameManager.Instance.Boss.transform.parent = GameManager.Instance.PivotParent.transform;
    //
    //     this.transform.parent.DOLocalMove(new Vector3(0.296f, -0.038f, -0.038f), .3f).OnComplete(() => { FindObjectOfType<EndDetector>().EndParticle.Play(); });
    //     Camera.main.transform.DOLocalMoveX(Camera.main.transform.position.x + .5f, .3f);
    //     yield return new WaitForSeconds(.2f);
    //     GameManager.Instance.PivotParent.transform.GetComponent<MySDK.Rotator>().enabled = true;
    //     StartTapRoutine = true;
    //     UiManager.Instance.TapFastPanel.SetActive(true);
    //
    // }
    
    public void DownGradeTexture(int ammount, GameObject g)
    {
        
        StorageManager.Instance.IncreaseScore(-g.GetComponentInParent<DownGrade>().Cost);
        scoreAnimator.Play("opps");

        scoreAnimator.transform.GetChild(0).gameObject.SetActive(true);
        scoreAnimator.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "-" + g.GetComponentInParent<DownGrade>().Cost.ToString();
        scoreAnimator.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().color = Color.red;
        // MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
  
    }

    #region Tattoo Drawing

    public void DrawDefaultTattoo()
    {
        Mpb.SetTexture(SHPropTexture, defaultTattoo);
        _skinnedMeshRenderer.SetPropertyBlock(Mpb);
        _skinnedMeshRenderer.material.DOFade(1, 1.8f);
    }

    private IEnumerator UpdateTattooTexture(Texture2D tattooTexture)
    {
        yield return new WaitForSeconds(0.2f);
        
        _skinnedMeshRenderer.material.DOFade(0, 0.3f).OnComplete(() =>
        {
            Mpb.SetTexture(SHPropTexture, tattooTexture);
            _skinnedMeshRenderer.SetPropertyBlock(Mpb);
            _skinnedMeshRenderer.material.DOFade(1, 0.5f);
        });
    }

    private void RemoveTattoo()
    {
        _skinnedMeshRenderer.material.DOFade(0, 0.3f).OnComplete(() =>
        {
            Mpb.SetTexture(SHPropTexture, TextureManager.Instance.handBurntTexture);
            _skinnedMeshRenderer.SetPropertyBlock(Mpb);
            _skinnedMeshRenderer.material.DOFade(1, 0.5f);
        });
    }

    #endregion
    
    #region Hand Animation

    private IEnumerator AnimationDelayRoutine()
    {
        yield return new WaitForSeconds(0.55f);
        PlayRandomAnimation();
    }
    
    private void PlayRandomAnimation()
    {
        int index;
        
        if (_animationIndexes.Count > 1)
        {
            int randomValue = Random.Range(0, _animationIndexes.Count);
            index = _animationIndexes[randomValue];
            _animationIndexes.RemoveAt(randomValue);
        }
        else
        {
            index = _animationIndexes[0];
            _animationIndexes.Clear();
            for (int k = 0; k < animationClips.Length; k++)
            {
                _animationIndexes.Add(k);
            }
        }
        animatorOverrideController["Take 001"] = animationClips[index];
        mainHandAnimator.runtimeAnimatorController = animatorOverrideController;
        tattooHandAnimator.runtimeAnimatorController = animatorOverrideController;
        mainHandAnimator.SetTrigger(Gesture);
        tattooHandAnimator.SetTrigger(Gesture);
    }

    private IEnumerator SpeedSlowDownRoutine()
    {
        _playerPathFollower.maxSpeed = _playerInitialSpeed / _playerPathFollower.speedDecrementFactor;
        _playerPathFollower.speed = _playerPathFollower.maxSpeed;
        yield return new WaitForSeconds(_playerPathFollower.speedDecrementDuration);
        _playerPathFollower.maxSpeed = _playerInitialSpeed;
    }
    #endregion
}