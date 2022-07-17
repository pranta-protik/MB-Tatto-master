using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlock : MonoBehaviour
{
    public GameObject[] Characters;

    public Animator anim;
    void Awake()
    {
        int index = Random.Range(0, Characters.Length);
        Characters[index].gameObject.SetActive(true);
        anim = Characters[index].GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
