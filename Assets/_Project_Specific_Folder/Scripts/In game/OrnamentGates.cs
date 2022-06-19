using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrnamentGates : MonoBehaviour
{
    public enum EOrnamentType
    {
        Ring,
        Bracelet
    }
    
    [Header("Ref : Flower1 == 0 Flower2 == 1 Skull 1 == 2 Skull 2 == 3")]
    public int ornamentId;
    public EOrnamentType ornamentType;
    
    private List<Sprite> _flowerOrnamentSprites;
    private List<Sprite> _skullOrnamentSprites;
    private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        _spriteRenderer = transform.GetChild(2).GetComponent<SpriteRenderer>();
        
        if (ornamentType == EOrnamentType.Ring)
        {
            _flowerOrnamentSprites = OrnamentManager.Instance.flowerRingSprites;
            _skullOrnamentSprites = OrnamentManager.Instance.skullRingSprites;
        }
        else if (ornamentType == EOrnamentType.Bracelet)
        {
            _flowerOrnamentSprites = OrnamentManager.Instance.flowerBraceletSprites;
            _skullOrnamentSprites = OrnamentManager.Instance.skullBraceletSprites;
        }

        switch (ornamentId)
        {
            case 0:
                _spriteRenderer.sprite = _flowerOrnamentSprites[0];
                break;
            case 1:
                _spriteRenderer.sprite = _flowerOrnamentSprites[1];
                break;
            case 2:
                _spriteRenderer.sprite = _skullOrnamentSprites[0];
                break;
            case 3:
                _spriteRenderer.sprite = _skullOrnamentSprites[1];
                break;
        }
    }
}
