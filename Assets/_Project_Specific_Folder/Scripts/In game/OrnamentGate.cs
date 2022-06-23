using UnityEngine;

public class OrnamentGate : MonoBehaviour
{
    public enum EOrnamentType
    {
        Ring,
        Bracelet
    }
    
    [Header("Ref: Flower = 0, Skull = 1")]
    public int ornamentGroupId;
    
    [Header("Ref: Design1 = 0, Design2 = 1")]
    public EOrnamentType ornamentType;
    public int ornamentDesignId;

    private SpriteRenderer _spriteRenderer;
    private OrnamentManager _ornamentManager;

    private void Start()
    {
        _spriteRenderer = transform.GetChild(2).GetComponent<SpriteRenderer>();
        _ornamentManager = OrnamentManager.Instance;
        
        if (ornamentType == EOrnamentType.Ring)
        {
            _spriteRenderer.sprite = _ornamentManager.ornamentSpriteGroups[ornamentGroupId].ringSprites[ornamentDesignId];
        }
        else if (ornamentType == EOrnamentType.Bracelet)
        {
            _spriteRenderer.sprite = _ornamentManager.ornamentSpriteGroups[ornamentGroupId].braceletSprites[ornamentDesignId];
        }
    }
}
