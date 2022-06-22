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
    
    [Header("Hand Ornaments Section")]
    public List<GameObject> rings = new List<GameObject>();
    public List<GameObject> bracelets = new List<GameObject>();
    
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
    private Camera _camera;

    // public float Multiplier;
    // public bool StartTapRoutine;
    // private static readonly int IsWrestling = Animator.StringToHash("isWrestling");
    
    private void Start()
    {
        _camera = Camera.main;
        
        _mainHandController = transform.parent.GetComponent<HandController>();
        _tattooHandController = tattooHand.transform.parent.GetComponent<HandController>();
        
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
        
        ResetHandTattooStatus();

        _playerPathFollower = GetComponentInParent<PathFollower>();
        _playerInitialSpeed = _playerPathFollower.maxSpeed;
    }

    private void ResetHandTattooStatus()
    {
        _hasGoneThroughGoodGate = false;
        _shouldChangeTattoo = false;
        _currentExpensiveTattooLevel = 0;
        _currentCheapTattooLevel = 0;
        _collectedGoodTattoosAttributes.Clear();
        _collectedGoodTattoosAttributes.Add(new CollectedGoodTattoosAttributes(defaultTattoo, -1));
    }

    private void OnTriggerEnter(Collider other)
    {
        #region Normal Gates
        if (other.gameObject.CompareTag("GoodGate"))
        {
            _hasGoneThroughGoodGate = true;
            other.GetComponent<BoxCollider>().enabled = false;
            Gates gate = other.GetComponentInParent<Gates>();
            
            CommonGateEnteringEffects();
            _shineEffect.Play();
            UpdateScore(gate.gateCost, true);

            // Went through special good gates
            if (gate.isSpecial)
            {
                UiManager.Instance.PriceTagScaleEffect();
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
            
            CommonGateEnteringEffects();
            _shineEffect.Play();
            UpdateScore(gate.gateCost, false);
            
            // Went through last bad gate
            if (gate.isLast)
            {
                UiManager.Instance.isBadTattoo = true;
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
                        DowngradeExpensiveTattoo();
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
            rings[other.gameObject.GetComponent<OrnamentGate>().ornamentId].gameObject.SetActive(true);
        }
        if (other.gameObject.CompareTag("Bracelet"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            CommonGateEnteringEffects();
            bracelets[other.gameObject.GetComponent<OrnamentGate>().ornamentId].gameObject.SetActive(true);
        }
        #endregion

        #region Obstacles
        if (other.gameObject.CompareTag("Remove"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            CommonObstacleHitEffects();
            
            _hurtEffect.Play();
            
            RemoveTattoo();
            ResetHandTattooStatus();
        }
        
        if (other.gameObject.CompareTag("Spike"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            ObstacleHitEffects(other.GetComponent<Obstacle>().decrementAmount);
        }
        
        if (other.gameObject.CompareTag("Lava"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            ObstacleHitEffects(other.GetComponent<Obstacle>().decrementAmount);
        }
        
        #endregion

        #region Level End Triggers

        if (other.gameObject.CompareTag("FinishLine"))
        {
            other.GetComponent<Collider>().enabled = false;
            UiManager.Instance.ClearUIOnFinishLine();
            
            _playerPathFollower.enabled = false;
            mainHandAnimator.Play("idle");
            tattooHandAnimator.Play("idle");
            mainHandAnimator.transform.parent.DOLocalMoveX(0, .2f); 
            tattooHandAnimator.transform.parent.DOLocalMoveX(0, .2f);
            _mainHandController.enabled = false;
            _tattooHandController.enabled = false;
            
            GameObject mobileObj = other.transform.GetChild(2).gameObject;
            
            mobileObj.transform.DOLocalMoveY(0.88f, 0.5f);
            mobileObj.transform.DORotate(new Vector3(-50f, 270f, 90f), 0.5f).OnComplete(() =>
            {
                mobileObj.transform.DOLocalMove(new Vector3(-.66f, 0.88f, 0.113f), 1f).OnComplete(() =>
                {
                    UiManager.Instance.transitionScreen.SetActive(true);
                    UiManager.Instance.transitionScreen.GetComponent<Image>().DOFade(1f, 0.5f).OnComplete(() =>
                    {
                        _camera.transform.DORotate(new Vector3(46f, 90f, 0f), 0.01f).OnComplete(() =>
                        {
                            mobileObj.SetActive(false);
                            UiManager.Instance.EnableMobileScreenUI();
                        });
                    });
                });
            });
        }

        #endregion
    }

    #region Gate Effects

    private void CommonGateEnteringEffects()
    {
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        if (_mainHandController.rotationAxis == ERotationAxis.X)
        {
            _mainHandController.RotateHandAlongXAxis();
            _tattooHandController.RotateHandAlongXAxis();
        }
        else if (_mainHandController.rotationAxis == ERotationAxis.Y)
        {
            _mainHandController.RotateHandAlongYAxis();
            _tattooHandController.RotateHandAlongYAxis();
        }

        StartCoroutine(AnimationDelayRoutine());
    }

    private void CommonObstacleHitEffects()
    {
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        StartCoroutine(SpeedSlowDownRoutine());
        StartCoroutine(UiManager.Instance.HurtScreenRoutine());
        mainHandAnimator.Play("Hurt");
        tattooHandAnimator.Play("Hurt");
    }
    
    private void ObstacleHitEffects(int amount)
    {
        CommonObstacleHitEffects();
            
        UpdateScore(amount, false);

        if (_hasGoneThroughGoodGate)
        {
            DowngradeExpensiveTattoo();
        }
        else
        {
            if (_currentCheapTattooLevel > 1)
            {
                _currentCheapTattooLevel -= 1;
                 
                StartCoroutine(UpdateTattooTexture(cheapTattoos[_currentCheapTattooLevel - 1]));
            }
        }
    }
    
    private void UpdateScore(int cost, bool isGood)
    {
        int score = StorageManager.Instance.GetCurrentScore();
        string scoreText;
        Color color;
        
        if (isGood)
        {
            score += cost;
            scoreText = "+" + cost;
            color = goodGateScorePopUpColor;
        }
        else
        {
            score += cost * -1;
            scoreText = "-" + cost;
            color = badGateScorePopUpColor;
        }
        
        StorageManager.Instance.SetCurrentScore(score);
        UiManager.Instance.UpdatePriceTag(score);
        
        scoreAnimator.transform.GetChild(0).gameObject.SetActive(true);
        scoreAnimator.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(scoreText);
        scoreAnimator.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = color;

        scoreAnimator.Play("PopUp");
    }

    private void DowngradeExpensiveTattoo()
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

    #endregion

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