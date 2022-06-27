using UnityEngine;
using HomaGames.HomaConsole.Core.Attributes;
using Singleton;

public class UAManager : Singleton<UAManager>
{
    [DebuggableField("General/SkyBox", Order = 10)]
    public Color SkyColor;
    [DebuggableField("General/SkyBox", Order = 10)]
    public Color HeightFogColor;
    [DebuggableField("Character Change", CustomName = "Hand Change")]
    public int HandId;
    
    public bool IsEndReached;
}
