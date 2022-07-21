using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TattooSeatSelectionPanel : MonoBehaviour
{
    public Action<int> TattooSeatUnlockAction;
        
    [SerializeField] private RectTransform selectionPanel;
    [SerializeField] private float targetYAnchorPosition;
    
    private Image _backgroundImage;

    private void Awake()
    {
        _backgroundImage = GetComponent<Image>();
    }

    public void ShowPanel()
    {
        selectionPanel.DOAnchorPosY(targetYAnchorPosition, 1f);
        _backgroundImage.enabled = true;
    }
    
    private void HidePanel()
    {
        selectionPanel.DOAnchorPosY(-targetYAnchorPosition, 1f);
        _backgroundImage.enabled = false;
    }

    public void OnTattooSeatSelectionButtonClick(int id)
    {
        TattooSeatUnlockAction?.Invoke(id);
        HidePanel();
    }
    
}
