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
    [DebuggableField("Level/Level No", CustomName = "Level No" )]
    public int LevelNo;
    [DebuggableField("Tattoo", CustomName = "Tattto")]
    public TattooType eTattooType;
    public bool IsEndReached;
    [DebuggableField("Road Color", CustomName = "Road Color")]
    public Color RoadColor;

    [DebuggableField("Gate Color", CustomName = "Gate Color")]
    public Color GetaColor;
    [DebuggableField("Border Color", CustomName = "Border Color")]
    public Color PathBorder;
    public GameObject water;

    GameObject g_Water;
    public List<GameObject> m_Roads = new List<GameObject>();
    public List<GameObject> PathBorders= new List<GameObject>();
    public GameObject[] Gates;
    public GameObject priceTag;
    Material[] mat;
    public GameObject _groundFog, _pillar;
    public bool EnableUA;
    public override void Start()
    {
        if (EnableUA)
        {
            // Setting Default
            UAManager.Instance.SkyColor = new Color32(170, 177, 255, 255);
            UAManager.Instance.BottomColor = new Color32(0, 51, 255, 255);
            _groundFog = GameObject.Find("Env");
          
            _groundFog.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_HeightFogColor", new Color(121, 132, 255, 255));
                // _groundFog.transform.GetChild(01).GetComponent<Renderer>().material.SetColor("_Color", new Color(145, 190, 255, 255));
        
        }
    }

    private void Update()
    {
        if (EnableUA)

        {
            RenderSettings.skybox.SetColor("_SkyColor2", UAManager.Instance.SkyColor);
            RenderSettings.skybox.SetColor("_SkyColor3", UAManager.Instance.BottomColor);
            GameObject hand = GameManager.Instance.handGroups[PlayerPrefs.GetInt("SelectedHandCardId")].mainHand.GetComponentInChildren<SkinnedMeshRenderer>().gameObject;
            hand.GetComponent<Renderer>().material.SetColor("_Color", UAManager.Instance.hnadColor);
            GameObject.Find("Env").gameObject.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_HeightFogColor", HeightFogColor);


            UASwitchTattoo();
            foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (gameObj.name == "Road Mesh Holder")
                {
                    if (!m_Roads.Contains(gameObj))
                        m_Roads.Add(gameObj);
                }
            }
            foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (gameObj.name == "PathBorder")
                {
                    if (!PathBorders.Contains(gameObj))
                        PathBorders.Add(gameObj);
                }
            }
            PathBorders[0].GetComponent<MeshRenderer>().material.SetColor("_Color", PathBorder);
            PathBorders[01].GetComponent<MeshRenderer>().material.SetColor("_Color", PathBorder);
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
                mat = m_Roads[0].GetComponent<MeshRenderer>().materials;
                mat[0].color = RoadColor;

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
            GameManager.Instance.PlaySpecificLevel(LevelNo - 1);
            SceneManager.LoadScene("Main");
        }
        [DebuggableAction]
        public void EnableWaterSurface()
        {
            g_Water = Instantiate(water);
        }
        [DebuggableAction]
        public void DisableWaterSurface()
        {
            GameObject _LevelWater;
            if (g_Water != null)
                Destroy(g_Water);
            _LevelWater = GameObject.Find("water");

            if (_LevelWater != null)
                _LevelWater.SetActive(false);
        }


        public void UASwitchTattoo()
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
            RenderSettings.skybox.SetColor("_SkyColor2", SkyColor);
            RenderSettings.skybox.SetColor("_SkyColor3", BottomColor);
        }
    
}
