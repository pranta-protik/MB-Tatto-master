using UnityEngine;
using HomaGames.HomaConsole.Core.Attributes;
using Singleton;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public enum TattooType
{
   Flower  ,
   Skull , 
   Pinup , 
   Celebs ,
   Money ,
   Calligraphy

}
public class UAManager : Singleton<UAManager>
{
    

    [DebuggableField("General/SkyBox", Order = 10)]
    public Color SkyColor , BottomColor;
    [DebuggableField("General/SkyBox", Order = 10)]
    public Color HeightFogColor;
   // [DebuggableField("General/Pillar", Order = 10)]
   // public Color PillarColor;
    [DebuggableField("Character/Character Change", Order = 1)]
    public int HandId;
    [DebuggableField("Character/Hand Color", Order = 1)]
    public Color hnadColor;
    [DebuggableField("Level No", CustomName = "Level No")]
    public int LevelNo;
    [DebuggableField("Tattoo", CustomName = "Tattto")]
    public TattooType eTattooType;
    [DebuggableField]
    public bool EnableWater;
    public bool IsEndReached;
    [DebuggableField("Road Color", CustomName = "Road Color")]
    public Color RoadColor;
    public GameObject water;

    GameObject g_Water;
    public List<GameObject> m_Roads = new List<GameObject>();

    Material[] mat;
    private void Update()
    {
        foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (gameObj.name == "Road Mesh Holder")
            {
                if(!m_Roads.Contains(gameObj))
                m_Roads.Add(gameObj);
            }
        }

       
        
      if(m_Roads[0] != null)
        {
          mat = m_Roads[0].GetComponent<MeshRenderer>().materials;
          mat[0].color = RoadColor;

       }
    }

    [DebuggableAction("Play Given Level")]
    public void Play()
    {
        GameManager.Instance.PlaySpecificLevel(LevelNo - 1);
        SceneManager.LoadScene("Main");
    }
    [DebuggableAction]
    public void EnableWaterSurface()
    {
       g_Water =  Instantiate(water);
    }
    [DebuggableAction]
    public void DisableWaterSurface()
    {
        GameObject _LevelWater;
        if(g_Water != null)
        Destroy(g_Water);    
        _LevelWater = GameObject.Find("water");

        if (_LevelWater != null)
            _LevelWater.SetActive(false);
    }

    [DebuggableAction]
    public void UASwitchTattoo()
    {
        switch(eTattooType)
        {
            case TattooType.Flower:
                GameManager.Instance.UASetTattoo(0);

            break;
            case TattooType.Skull:
                GameManager.Instance.UASetTattoo(01);

            break;
            case TattooType.Pinup:
                GameManager.Instance.UASetTattoo(02);

                break;
            
            case TattooType.Celebs:
                GameManager.Instance.UASetTattoo(03);

                break;
            case TattooType.Money:
                GameManager.Instance.UASetTattoo(04);

                break;
            case TattooType.Calligraphy:
                GameManager.Instance.UASetTattoo(05);

                break;
        }
    }
    [DebuggableAction]
    public void  GreenScreen()
    {
        BottomColor = Color.green;
        SkyColor = Color.green;
        GameObject environmentObj = GameObject.Find("Env");
        environmentObj.SetActive(false);
        RenderSettings.skybox.SetColor("_SkyColor2", SkyColor);
        RenderSettings.skybox.SetColor("_SkyColor3", BottomColor);
    }

}
