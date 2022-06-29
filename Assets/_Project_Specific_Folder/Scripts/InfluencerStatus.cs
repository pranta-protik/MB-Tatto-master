using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class InfluencerStatus : MonoBehaviour
{
    public int influencerId;
    public int influencerHandId;
    public Sprite influencerIcon;
    [SerializeField] private float influencerFollowerValue;
    [SerializeField] private string influenderFollowerScale;
    
    private TMP_Text _influencerFollowerText;
    private float _startFollowerValue;

    private void Start()
    {
        _startFollowerValue = influencerFollowerValue;
        _influencerFollowerText = transform.GetChild(2).GetComponent<TMP_Text>();

        if (PlayerPrefs.GetInt("InfluencerStatus" + influencerId, 0) == 1)
        {
            if (PlayerPrefs.GetInt("InfluncerFightStatus" + influencerId, 0) == 1)
            {
                transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f);
                _influencerFollowerText.SetText("0");
            }
            else
            {
                _influencerFollowerText.SetText(influencerFollowerValue + " " + influenderFollowerScale);
            }
        }
        else
        {
            _influencerFollowerText.SetText(influencerFollowerValue + " " + influenderFollowerScale);
        }
    }

    public IEnumerator DecreaseFollowers()
    {
        while (influencerFollowerValue > 0)
        {
            influencerFollowerValue += ((0 - _startFollowerValue) / 1.5f) * Time.deltaTime;
            influencerFollowerValue = Mathf.Clamp(influencerFollowerValue, 0, _startFollowerValue);
            
            _influencerFollowerText.SetText(Mathf.RoundToInt(influencerFollowerValue) + " " + influenderFollowerScale);
            
            yield return new WaitForSeconds(0.001f);
        }

        _influencerFollowerText.SetText("0");
        
        Transform followerDropTextTransform = transform.GetChild(2).GetChild(0);
        followerDropTextTransform.DOKill();
        followerDropTextTransform.gameObject.SetActive(false);
        
        transform.GetChild(0).DOScale(new Vector3(1f, 1f, 1f), 0.5f);
    }
}
