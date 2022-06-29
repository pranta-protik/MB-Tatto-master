using UnityEngine;
using HomaGames.HomaConsole.Core.Attributes;
using Singleton;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Serialization;

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
    public Color SkyColor, BottomColor;

    [DebuggableField("General/SkyBox", Order = 10)]
    public Color HeightFogColor;

    // [DebuggableField("General/Pillar", Order = 10)]
    // public Color PillarColor;
    
    [FormerlySerializedAs("HandId")] [DebuggableField("Character/Character Change", Order = 1)]
    public int handId;

    [FormerlySerializedAs("hnadColor")] [DebuggableField("Character/Hand Color", Order = 1)]
    public Color handColor;

    [FormerlySerializedAs("LevelNo")] [DebuggableField("Level/Level No", CustomName = "Level No")]
    public int levelNo;

    [DebuggableField("Tattoo", CustomName = "Tattoo")]
    public TattooType eTattooType;

    [FormerlySerializedAs("IsEndReached")] public bool isEndReached;

    [DebuggableField("Road Color", CustomName = "Road Color")]
    public Color RoadColor;

    [DebuggableField("Gate Color", CustomName = "Gate Color")]
    public Color GetaColor;

    [DebuggableField("Border Color", CustomName = "Border Color")]
    public Color PathBorder;

    public GameObject water;

    private GameObject _waterObj;
    public List<GameObject> m_Roads = new List<GameObject>();
    public List<GameObject> PathBorders = new List<GameObject>();
    public GameObject[] Gates;
    public GameObject priceTag;
    private Material[] _roadMaterials;
    public GameObject _groundFog, _pillar;
    public bool EnableUA;
    
    private static readonly int FogColor = Shader.PropertyToID("_HeightFogColor");
    private static readonly int SkyColor2 = Shader.PropertyToID("_SkyColor2");
    private static readonly int SkyColor3 = Shader.PropertyToID("_SkyColor3");
    private static readonly int MainColor = Shader.PropertyToID("_Color");

    public override void Start()
    {
        if (EnableUA)
        {
            // Setting Default
            SkyColor = new Color32(170, 177, 255, 255);
            BottomColor = new Color32(0, 51, 255, 255);
           
            _groundFog = GameObject.Find("Env");

            if (_groundFog!=null)
            {
                _groundFog.transform.GetChild(0).GetComponent<Renderer>().material.SetColor(FogColor, new Color(121, 132, 255, 255));   
            }
            // _groundFog.transform.GetChild(01).GetComponent<Renderer>().material.SetColor("_Color", new Color(145, 190, 255, 255));
        }
    }

    private void Update()
    {
        if (EnableUA)
        {
            RenderSettings.skybox.SetColor(SkyColor2, UAManager.Instance.SkyColor);
            RenderSettings.skybox.SetColor(SkyColor3, UAManager.Instance.BottomColor);
            GameObject hand = GameManager.Instance.handGroups[PlayerPrefs.GetInt("SelectedHandCardId")].mainHand.GetComponentInChildren<SkinnedMeshRenderer>()
                .gameObject;
            hand.GetComponent<Renderer>().material.SetColor(MainColor, UAManager.Instance.handColor);
            GameObject.Find("Env").gameObject.transform.GetChild(0).GetComponent<Renderer>().material.SetColor(FogColor, HeightFogColor);

            GameManager.Instance.UASpawnHand(handId);
            UASwitchTattoo();
            
            foreach (var gameObj in (GameObject[]) FindObjectsOfType(typeof(GameObject)))
            {
                if (gameObj.name == "Road Mesh Holder")
                {
                    if (!m_Roads.Contains(gameObj))
                        m_Roads.Add(gameObj);
                }
            }

            foreach (var gameObj in (GameObject[]) FindObjectsOfType(typeof(GameObject)))
            {
                if (gameObj.name == "PathBorder")
                {
                    if (!PathBorders.Contains(gameObj))
                        PathBorders.Add(gameObj);
                }
            }

            PathBorders[0].GetComponent<MeshRenderer>().material.SetColor(MainColor, PathBorder);
            PathBorders[01].GetComponent<MeshRenderer>().material.SetColor(MainColor, PathBorder);
            Gates = GameObject.FindGameObjectsWithTag("Gates");
            foreach (GameObject g in Gates)
            {
                if (g.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>() != null)
                    g.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = GetaColor;
                else
                {
                    g.transform.GetComponentInChildren<SpriteRenderer>().color = GetaColor;
                }
            }

            if (m_Roads[0] != null)
            {
                _roadMaterials = m_Roads[0].GetComponent<MeshRenderer>().materials;
                _roadMaterials[0].color = RoadColor;

            }
            
            
        }
    }

    [DebuggableAction("Reset Game")]
    public void ResetGame()
    {
        SceneManager.LoadScene("Main");
    }

    [DebuggableAction(("UI/Hide UI"))]
    public void Yes()
    {
        priceTag.SetActive(false);
    }

    [DebuggableAction(("UI/Hide UI"))]
    public void No()
    {
        priceTag.SetActive(true);
    }

    [DebuggableAction(("Level/Level No"))]
    public void Must_Press_Play()
    {
        GameManager.Instance.PlaySpecificLevel(levelNo - 1);
        SceneManager.LoadScene("Main");
    }

    [DebuggableAction]
    public void EnableWaterSurface()
    {
        _waterObj = Instantiate(water);
    }

    [DebuggableAction]
    public void DisableWaterSurface()
    {
        if (_waterObj != null)
        {
            Destroy(_waterObj);    
        }
        
        GameObject _LevelWater = GameObject.Find("water");

        if (_LevelWater != null)
        {
            _LevelWater.SetActive(false);    
        }
    }
    
    private void UASwitchTattoo()
    {
        switch (eTattooType)
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
    public void GreenScreen()
    {
        BottomColor = Color.green;
        SkyColor = Color.green;
        GameObject environmentObj = GameObject.Find("Env");
        environmentObj.SetActive(false);
        RenderSettings.skybox.SetColor(SkyColor2, SkyColor);
        RenderSettings.skybox.SetColor(SkyColor3, BottomColor);
    }

}
