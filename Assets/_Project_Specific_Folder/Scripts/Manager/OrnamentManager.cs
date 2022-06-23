using System;
using System.Collections.Generic;
using Singleton;
using UnityEngine;

public class OrnamentManager : Singleton<OrnamentManager>
{
    [Serializable]
    public struct OrnamentSpriteGroup
    {
        public int groupId;
        public List<Sprite> ringSprites;
        public List<Sprite> braceletSprites;
    }

    [Header("Ornament Sprite Section")]
    public List<OrnamentSpriteGroup> ornamentSpriteGroups;
}
