using UnityEngine;

public class HandCard : MonoBehaviour
{
    public int handId;
    public GameObject lockImage;
    public GameObject handImage;
    public GameObject selectionImage;
    public int unlockStatus;
    public int selectionId;

    private void Start()
    {
        lockImage = transform.GetChild(0).gameObject;
        handImage = transform.GetChild(1).gameObject;
        selectionImage = transform.GetChild(2).gameObject;

        unlockStatus = PlayerPrefs.GetInt("HandCardUnlockStatus" + handId, 0);
        selectionId = PlayerPrefs.GetInt("SelectedHandCardId", 0);

        if (handId == 0)
        {
            unlockStatus = 1;
        }

        if (unlockStatus == 0)
        {
            lockImage.SetActive(true);
            handImage.SetActive(false);
        }
        else
        {
            lockImage.SetActive(false);
            handImage.SetActive(true);
        }

        selectionImage.SetActive(handId == selectionId);
    }

    public void UnlockHandCard(int id)
    {
        PlayerPrefs.SetInt("HandCardUnlockStatus" + id, 1);
        PlayerPrefs.SetInt("SelectedHandCardId", id);
        unlockStatus = 1;
        lockImage.SetActive(false);
        handImage.SetActive(true);
        selectionImage.SetActive(true);
        GameManager.Instance.SpawnHand(id);
    }

    public void OnHandCardClick()
    {
        SelectionMenu selectionMenu = transform.parent.parent.GetComponent<SelectionMenu>();
        foreach (GameObject handCard in selectionMenu.handCards)
        {
            handCard.GetComponent<HandCard>().selectionImage.SetActive(false);
        }
        
        PlayerPrefs.SetInt("SelectedHandCardId", handId);
        selectionImage.SetActive(true);
        GameManager.Instance.SpawnHand(handId);
    }
}
