using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InstagramPost : MonoBehaviour
{
    [SerializeField] private int commentAmount = 7;
    [SerializeField] private List<Sprite> commentSprites;
    [SerializeField] private GameObject comment;
    [SerializeField] private Transform commentSpawnTransform;
    [SerializeField] private float popUpDistance = 3f;
    
    private readonly List<int> _commentIndexList = new List<int>();
    
    private IEnumerator Start()
    {
        for (int i = 0; i < commentSprites.Count; i++)
        {
            _commentIndexList.Add(i);
        }
        
        for (int i = 0; i < commentAmount; i++)
        {
            GameObject commentObj = Instantiate(comment, commentSpawnTransform.position, Quaternion.identity, commentSpawnTransform);
            Image commentImage = commentObj.GetComponent<Image>();
            
            int index = Random.Range(0, _commentIndexList.Count);

            int commentIndex = _commentIndexList[index];
            _commentIndexList.Remove(commentIndex);
            
            commentImage.sprite = commentSprites[commentIndex];
            commentImage.DOFade(0f, 2f);
            
            commentObj.transform.DOMoveY(commentObj.transform.position.y + popUpDistance, 2f);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
