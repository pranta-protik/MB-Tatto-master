using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager sharedInstance = null;

    [FormerlySerializedAs("sfxAuidoSource")] public AudioSource sfxAudioSource;
    [SerializeField] private AudioSource backgroundAudioSource;
    
    [Header("Scanning SFX")]
    public AudioClip Collectable;

    public static SoundManager SharedManager()
    {
        return sharedInstance;
    }

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    void Start()
    {
   
        sfxAudioSource = GetComponent<AudioSource>();
        //backgroundAudioSource =  GetComponent<AudioSource>();
    }


    public void PlaySFX(AudioClip audioClip)
    {
        sfxAudioSource.PlayOneShot(audioClip);
    }

    public void StopSFX()
    {
        sfxAudioSource.Stop();
    }
    public void PlaySFXDelayed(AudioClip audioClip)
    {
        StartCoroutine(delayedSFX(audioClip));
    }

    private IEnumerator delayedSFX(AudioClip audioClip)
    {
        yield return new WaitForSeconds(1f);
        sfxAudioSource.PlayOneShot(audioClip);
    }

}
