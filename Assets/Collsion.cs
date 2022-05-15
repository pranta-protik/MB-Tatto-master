using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains.NiceVibrations;
public class Collsion : MonoBehaviour
{
    public ParticleSystem HeatEffect;

    public Text LevelText , ColorText;
    public Animator anim , anim1;

    public GameObject SecondHand;
    public Texture Burnt;
    public Texture[] Tattos , CheapTttos;
    public Texture[] BadBlue, GoodBlue, GoodYellow, BadYellow;
    public Material StiackerMat;
    public int min = 0, max = 255;
    public Texture Default;
   [SerializeField] bool IsGoodGate;
    Vector3 Startpos;
    public GameObject Boss;
    public float Multiplier;
    public bool StartTapRoutine;
    public Animator Opps,wow;
    bool IsYellow, IsBlue;
    private float timeLeft = .4f;
    [SerializeField] public ParticleSystem Ps;
    bool m_FirstClick;
    [SerializeField] bool m_isTapping;
    private void Start()
    {
        StiackerMat.mainTexture = Default;
        StiackerMat.DOFade(0, 0);
        Startpos = transform.localPosition;

    }
    private void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;

        if(Input.GetMouseButton(0))
        {
            DOTween.Kill(GameManager.Instance.PivotParent.transform);
            m_isTapping = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            m_isTapping = false;
        }

   

            if (StartTapRoutine)
        {

            if (Input.GetMouseButtonDown(0))
            {
                
                timeLeft = .4f;
                if (UiManager.Instance.timerInitvalue < 1f)
                {
                    UiManager.Instance.timerInitvalue += 0.12f;

                    UiManager.Instance.Timer.fillAmount = UiManager.Instance.timerInitvalue;

                    UiManager.Instance.Timer.fillAmount = UiManager.Instance.timerInitvalue;
                    GameManager.Instance.PivotParent.GetComponent<MySDK.Rotator>().enabled = false;
                    GameManager.Instance.PivotParent.transform.DOLocalRotate(new Vector3((GameManager.Instance.PivotParent.transform.eulerAngles.x + UiManager.Instance.timerInitvalue + 8f), 0, 0), .1f);
                    
                    Camera.main.transform.DOShakePosition(1.5f, .01f);
                    Camera.main.DOFieldOfView(50, 2);
                    m_FirstClick = true;
                }




            }
            else
            {
               
                if (m_FirstClick)
                {
                    timeLeft -= Time.deltaTime;

                    if (timeLeft < 0)
                    {
                        timeLeft = .4f;
                   
                        GameManager.Instance.PivotParent.transform.DOLocalRotate(new Vector3(-22, 0, 0), 1.5f).SetEase(Ease.InSine);
                        
                        
                    }
                }
            }
           

            if (UiManager.Instance.timerInitvalue > 0f)
            {
                UiManager.Instance.timerInitvalue -= 0.0071f;
                UiManager.Instance.Timer.fillAmount = UiManager.Instance.timerInitvalue;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("GoodGate"))
        {;
          
            StartCoroutine(AnimationDelayRoutine());
            GameManager.Instance.Level++;
          
            IsGoodGate = true;
            other.GetComponent<BoxCollider>().enabled = false;
            if (IsYellow)
            {
                if (GameManager.Instance.Level == 5)
                {
                    StiackerMat.DOFade(0, .3f).OnComplete(() =>
                    {
                        StiackerMat.mainTexture = GoodYellow[01];
                        StiackerMat.DOFade(1, .5f);

                    });
                }
                else
                    Invoke("UpdateTexture", .2f);
            }
            else if(IsBlue)
            {
                if (GameManager.Instance.Level == 5)
                {
                    StiackerMat.DOFade(0, .3f).OnComplete(() =>
                    {
                        StiackerMat.mainTexture = GoodBlue[01];
                        StiackerMat.DOFade(1, .5f);
                    });
                }
                else
                    Invoke("UpdateTexture", .2f);
            }
            else
            {
                Invoke("UpdateTexture", .2f);
            }

        }
        if (other.gameObject.CompareTag("BadGate"))
        {
         


            StartCoroutine(AnimationDelayRoutine());
            GameManager.Instance.Level++;
          
            IsGoodGate = false;
            if (IsYellow)
            {
                if (GameManager.Instance.Level == 5)
                {
                    StiackerMat.DOFade(0, .3f).OnComplete(() =>
                    {
                        StiackerMat.mainTexture = BadYellow[01];
                        StiackerMat.DOFade(1, .5f);

                    });
                }
                else
                    Invoke("UpdateTextureCheap", .2f);
            }
            else if (IsBlue)
            {
                if (GameManager.Instance.Level == 5)
                {
                    StiackerMat.DOFade(0, .3f).OnComplete(() =>
                    {
                        StiackerMat.mainTexture = BadBlue[01];
                        StiackerMat.DOFade(1, .5f);
                    });
                }
                else
                    Invoke("UpdateTextureCheap", .2f);
            }
            else
            {
                Invoke("UpdateTextureCheap", .2f);
            }
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            HeatEffect.Play();
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            StartCoroutine(SpeedSlowDownRoutine());
            StartCoroutine(UiManager.Instance.FdeDelayRoutine()); Invoke("RemoveMat" , .2f);
            anim1.Play("Hurt"); anim.Play("Hurt");
            other.GetComponent<BoxCollider>().enabled = false;
        }
        if (other.gameObject.CompareTag("Spike"))
        {
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            StartCoroutine(SpeedSlowDownRoutine());
            StartCoroutine(UiManager.Instance.FdeDelayRoutine()); 
            anim1.Play("Hurt"); anim.Play("Hurt");
            other.GetComponent<BoxCollider>().enabled = false;
        }
        if (other.gameObject.CompareTag("Lava"))
        {
            
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            StartCoroutine(SpeedSlowDownRoutine());
            StartCoroutine(UiManager.Instance.FdeDelayRoutine());
            anim1.Play("Hurt"); anim.Play("Hurt");
            other.GetComponent<BoxCollider>().enabled = false;
        }

        if (other.gameObject.CompareTag("Yellow"))
        {
            StartCoroutine(AnimationDelayRoutine());
            if (IsGoodGate)
            {
                if(GameManager.Instance.Level == 4)
                {
                    StiackerMat.DOFade(0, .3f).OnComplete(() =>
                    {
                        StiackerMat.mainTexture = GoodYellow[0];
                        StiackerMat.DOFade(1, .5f);
                        IsYellow = true;
                    });
                }
                else if (GameManager.Instance.Level == 5)
                {
                    StiackerMat.DOFade(0, .3f).OnComplete(() =>
                    {
                        StiackerMat.mainTexture = GoodYellow[01];
                        StiackerMat.DOFade(1, .5f);
                      
                    });
                }
            }
            else
            {
                if (GameManager.Instance.Level == 4)
                {
                    StiackerMat.DOFade(0, .3f).OnComplete(() =>
                    {
                        StiackerMat.mainTexture = BadYellow[0];
                        StiackerMat.DOFade(1, .5f);
                        IsYellow = true;
                    });
                }
                else  if (GameManager.Instance.Level == 5)
                {
                    StiackerMat.DOFade(0, .3f).OnComplete(() =>
                    {
                        StiackerMat.mainTexture = BadYellow[01];
                        StiackerMat.DOFade(1, .5f);
                    });
                }
            }
        }
        if (other.gameObject.CompareTag("Blue"))
        {
            StartCoroutine(AnimationDelayRoutine());
            if (IsGoodGate)
            {
                if (GameManager.Instance.Level == 4)
                {
                    StiackerMat.DOFade(0, .3f).OnComplete(() =>
                    {
                        StiackerMat.mainTexture = GoodBlue[0];
                        StiackerMat.DOFade(1, .5f);
                        IsBlue = true;
                    });
                }
             else   if (GameManager.Instance.Level == 5)
                {
                    StiackerMat.DOFade(0, .3f).OnComplete(() =>
                    {
                        StiackerMat.mainTexture = GoodBlue[01];
                        StiackerMat.DOFade(1, .5f);
                    });
                }
            }
            else
            {
                if (GameManager.Instance.Level == 4)
                {
                    StiackerMat.DOFade(0, .3f).OnComplete(() =>
                    {
                        StiackerMat.mainTexture = BadBlue[0];
                        StiackerMat.DOFade(1, .5f);
                    });
                    IsBlue = true;
                }
               else if (GameManager.Instance.Level == 5)
                {
                    StiackerMat.DOFade(0, .3f).OnComplete(() =>
                    {
                        StiackerMat.mainTexture = BadBlue[01];
                        StiackerMat.DOFade(1, .5f);
                    });
                }
            }
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            StartCoroutine(StopRoutine(other.gameObject));
        }
    }
    public IEnumerator AnimationDelayRoutine()
    {
        yield return new WaitForSeconds(.55f);
        RandomAnimationPlay();
    }
    public IEnumerator SpeedSlowDownRoutine()
    {
        GameManager.Instance.p.speed = GameManager.Instance.p.MaxSpeed = .8f;
        yield return new WaitForSeconds(.6f);
        GameManager.Instance.p.MaxSpeed = 3;
    }

    void RandomAnimationPlay() {
        
        if(GameManager.Instance.Level % 2 == 0)
        {
            anim.Play("g 0"); anim1.Play("g 0");
        }
     else
        {
            int i = Random.Range(0, 2);
            if (i == 0)
            {
                anim.Play("g 1"); anim1.Play("g 1");
            }
            else
            {
                anim.Play("g"); anim1.Play("g");
            }
        }
     
    }
    public IEnumerator StopRoutine(GameObject g )

    {
        //Camera.main.transform.parent = g.transform.root;
     
        transform.GetComponent<Controller>().enabled = false; anim1.transform.GetComponent<Controller>().enabled = false;
        anim.transform.DOLocalMoveX(-1.66f, .1f); anim1.transform.DOLocalMoveX(-1.66f, .1f);      
        transform.root.parent = g.transform.root;     
        Camera.main.transform.DOLocalMove(GameManager.Instance.FianlCamPos.transform.localPosition, .7f);
        Camera.main.transform.DOLocalRotate(GameManager.Instance.FianlCamPos.transform.localEulerAngles, .7f);
        yield return new WaitForSeconds(.8f);
        transform.DOLocalRotate(new Vector3(0, -90, 9), .1f); anim1.transform.DOLocalRotate(new Vector3(0, -90, 9), .1f);
        Boss.transform.GetComponent<Animator>().enabled = true;
        GameManager.Instance.p.enabled = false;
        anim.Play("Wrestle"); anim1.Play("Wrestle");
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        Camera.main.transform.parent = g.transform.root;
        transform.parent.parent = GameManager.Instance.PivotParent.transform; Boss.transform.parent = GameManager.Instance.PivotParent.transform;
       
        this.transform.parent.DOLocalMove(new Vector3(0.296f, -0.038f, -0.038f), .3f).OnComplete(() => { Ps.Play(); }); 
            Camera.main.transform.DOLocalMoveX(-2.2f, .3f);
        yield return new WaitForSeconds(.2f);
        GameManager.Instance.PivotParent.transform.GetComponent<MySDK.Rotator>().enabled = true;

       
       
        StartTapRoutine = true;
        UiManager.Instance.TapFastPanel.SetActive(true);

    }

    public void ChangeMaterials()
    {
        StiackerMat.DOFade(1, 1.8f);
    }
    public void RemoveMat()
    {
        StiackerMat.DOFade(0, .3f).OnComplete(() =>
        {
            StiackerMat.mainTexture = Burnt;
            StiackerMat.DOFade(1, .5f);
        });
    }
    public void UpdateTexture()
    {
        
            StiackerMat.DOFade(0, .3f).OnComplete(() =>
            {
                wow.Play("opps");
                StiackerMat.mainTexture = Tattos[GameManager.Instance.Level - 1];
                StiackerMat.DOFade(1, .5f);
            });

    }
    public void ApplyBurntTexture()
    {

        StiackerMat.DOFade(0, .3f).OnComplete(() =>
        {
            
            StiackerMat.mainTexture = Tattos[GameManager.Instance.Level - 1];
            StiackerMat.DOFade(1, .5f);
        });

    }
    public IEnumerator GoodGateRot()
    {
       // GetComponent<Controller>().enabled = false;
        transform.DOLocalMove(new Vector3(-1.35f, 3.15f, -2.67f), .1f); anim1.transform.DOLocalMove(new Vector3(-1.35f, 3.15f, -2.67f), .1f);
        yield return new WaitForSeconds(1f);
      //  GetComponent<Controller>().enabled = true;
        transform.DOLocalMove(Startpos, .1f);
        anim1.transform.DOLocalMove(Startpos, .1f);

    }
    public void UpdateTextureCheap()
    {
        StiackerMat.DOFade(0, .3f).OnComplete(() => {
            Opps.Play("opps");
            StiackerMat.mainTexture = CheapTttos[GameManager.Instance.Level - 1];
            StiackerMat.DOFade(1, .5f);
        });
    }
}
