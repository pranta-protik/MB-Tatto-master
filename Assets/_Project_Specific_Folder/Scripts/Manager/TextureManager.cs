using System;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class TextureManager : Singleton<TextureManager>
{
    [Serializable]
    public struct TattooGroup
    {
        public int groupId;
        public List<Texture2D> defaultTattoos;
        public List<Texture2D> expensiveTattoos;
        public List<Texture2D> expensiveBlueTattoos;
        public List<Texture2D> expensiveYellowTattoos;
        public List<Texture2D> cheapTattoos;
        public List<Texture2D> cheapBlueTattoos;
        public List<Texture2D> cheapYellowTattoos;
        public List<int> expensiveColorTattooIdSequences;
        public List<int> cheapColorTattooIdSequences;
    }
    
    public Texture2D handBurntTexture;

    [Space] [Header("Tattoo Section")] 
    public List<TattooGroup> tattooGroups;
}
