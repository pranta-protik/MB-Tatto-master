using System.Linq;
using UnityEngine;

public class TattooFrame : MonoBehaviour
{
    [SerializeField] private int frameId;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject foreGround;

    private void Start()
    {
        if (PlayerPrefs.GetString(PlayerPrefsKey.TATTOO_FRAME + frameId, "") == "")
        {
            return;
        }

        foreGround.SetActive(false);
        meshRenderer.materials[0].mainTexture =
            TextureManager.Instance.allTattoos.SingleOrDefault(obj => obj.name == PlayerPrefs.GetString(PlayerPrefsKey.TATTOO_FRAME + frameId));
    }
}