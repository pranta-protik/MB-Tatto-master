using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Fountain : MonoBehaviour, IAdUpgrade
{
    [SerializeField] private List<Renderer> renderersToBlack;
    [SerializeField] private List<GameObject> waterEffects;
    [SerializeField] private GameObject water;
    [SerializeField] private WatchAdPlatform watchAdPlatform;

    private List<Texture2D> _originalTextures;
    private List<Color> _originalColors;
    private bool _hasUsedNullTexture;
    private bool _isUnlocked;

    private void Start()
    {
        _isUnlocked = PlayerPrefs.GetInt(PlayerPrefsKey.FOUNTAIN_UNLOCK_STATUS, 0) == 1;
        
        watchAdPlatform.Init(!_isUnlocked);
        SetUnlockStatus(_isUnlocked);
    }

    private void SetUnlockStatus(bool isUnlocked)
    {
        if (!isUnlocked)
        {
            _originalTextures = new List<Texture2D>();
            _originalColors = new List<Color>();

            for (int i = 0, count = renderersToBlack.Count; i < count; i++)
            {
                _originalTextures.Add(renderersToBlack[i].material.mainTexture as Texture2D);
                _originalColors.Add(renderersToBlack[i].material.color);

                renderersToBlack[i].material.mainTexture = null;
                renderersToBlack[i].material.color = Color.black;
            }

            _hasUsedNullTexture = true;

            foreach (GameObject waterEffect in waterEffects)
            {
                waterEffect.SetActive(false);
            }
            
            water.transform.localPosition = Vector3.zero;
        }
        else if (_hasUsedNullTexture)
        {
            for (int i = 0, count = renderersToBlack.Count; i < count; i++)
            {
                renderersToBlack[i].material.mainTexture = _originalTextures[i];
                renderersToBlack[i].material.color = _originalColors[i];
            }

            _hasUsedNullTexture = false;

            foreach (GameObject waterEffect in waterEffects)
            {
                waterEffect.SetActive(true);
            }

            water.transform.DOLocalMoveY(0.4f, 10f).SetEase(Ease.Linear);
        }
    }

    public void UnlockStation()
    {
        PlayerPrefs.SetInt(PlayerPrefsKey.FOUNTAIN_UNLOCK_STATUS, 1);
        SetUnlockStatus(true);
    }
}
