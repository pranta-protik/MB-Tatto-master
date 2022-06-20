using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bracelet : MonoBehaviour
{
    [Header("Ref : Flower1 == 0 Flower2 == 1 Skull 1 == 2 Skull 2 == 3 ")]
    public int Id;

    private void Start()
    {
        // switch (Id)
        // {
        //     case 0:
        //         transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = TextureManager.Instance.braceletFlowerTextures[0];
        //         break;
        //     case 1:
        //         transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = TextureManager.Instance.braceletFlowerTextures[1];
        //         break;
        //     case 2:
        //         transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = TextureManager.Instance.braceletSkullTextures[0];
        //         break;
        //     case 3:
        //         transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = TextureManager.Instance.braceletSkullTextures[1];
        //         break;
        // }
    }
}
