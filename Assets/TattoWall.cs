using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TattoWall : MonoBehaviour
{
    public List<Transform> FramePos = new List<Transform>();
    public Transform StartPos;
    public GameObject FramePrefab;
    public Texture[] SavedTattos;
    public int count;

    [SerializeField] int i;

    private void Awake()
    {
        count += 1;

        int totalEntered = PlayerPrefs.GetInt("totalEntered", 0);
        int No = totalEntered +count;

        PlayerPrefs.SetInt("totalEntered", No);




    }
   
    private void Start()
    {
        StartCoroutine(EnableEndUi());

        i = PlayerPrefs.GetInt("totalEntered", 0);


        for (int j = 0; j < i; j++)
        {
            if(j == i - 1)
            {
                GameObject g = Instantiate(FramePrefab, StartPos.transform.position, Quaternion.identity);
                g.transform.DOLocalMove(FramePos[j].transform.position, 1.5f); 
                g.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GameManager.Instance.LastTattoTexture;
                g.transform.DOLocalRotate(new Vector3(0, -90, 0), 0);
            }
            else
            {
                GameObject g = Instantiate(FramePrefab, FramePos[j].transform.position, Quaternion.identity);
                g.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GameManager.Instance.LastTattoTexture;
                g.transform.DOLocalRotate(new Vector3(0, -90, 0), 0);
            }
           
          
        }






    }
    public IEnumerator EnableEndUi()
    {
        if (GameManager.Instance.levelNo == 0)
        {
            UiManager.Instance.UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 50;
        }
        else if (GameManager.Instance.levelNo == 1)
        {
            UiManager.Instance.UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 50;
        }
        else
        {
            UiManager.Instance.UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 33;
        }
        yield return new WaitForSeconds(5f);
        

         UiManager.Instance.UnlockPanel.gameObject.SetActive(true);

    }
}
