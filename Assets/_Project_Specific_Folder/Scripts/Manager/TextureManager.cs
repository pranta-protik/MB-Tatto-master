using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class TextureManager : Singleton<TextureManager>
{
    public Texture2D flowerDefaultTattoo;
    public List<Texture2D> flowerExpensiveTattoos;
    public List<Texture2D> flowerExpensiveBlueTattoos;
    public List<Texture2D> flowerExpensiveYellowTattoos;
    public List<Texture2D> flowerCheapTattoos;
    public List<Texture2D> flowerCheapBlueTattoos;
    public List<Texture2D> flowerCheapYellowTattoos;
    public List<int> flowerExpensiveColorTattooIdSequences;
    public List<int> flowerCheapColorTattooIdSequences;

    public Texture2D handBurntTexture;
    
    public Texture DefaultFlower, DefaultSkull, DefaultPinup , DefaultCeleb , DefaultCaligraphy , DefaultMoney;

    public Texture[] FlowerExpensiveTattos, FlowerCheapTattos;
    public Texture[] FlowerBadBlue, FlowerGoodBlue, FlowerGoodYellow, FlowerBadYellow;
    public Texture[] SkullExpensiveTattos, SkullCheapTattos;
    public Texture[] SkullBadBlue, SkullGoodBlue, SkullGoodYellow, SkullBadYellow;


    public Texture[] PinnupGirlExpensiveTattos, PinnupGirlCheapTattos;
    public Texture[] PinnupGirlBadBlue, PinnupGirlGoodBlue, PinnupGirlGoodYellow, PinnupGirlBadYellow;

    public Texture[] CelebExpensiveTattos, CelebCheapTattos;
    public Texture[] CelebBadBlue, CelebGoodBlue, CelebGoodYellow, CelebBadYellow;
    
    public Texture[] CaligraphyExpensiveTattos, CaligraphyCheapTattos;
    public Texture[] CaligraphyBadBlue, CaligraphyGoodBlue, CaligraphyGoodYellow, CaligraphyBadYellow;

    public Texture[] MoneyExpensiveTattos, MoneyCheapTattos;
    public Texture[] MoneyBadBlue, MoneyGoodBlue, MoneyGoodYellow, MoneyBadYellow;

    public Sprite[] ringFlowerTextures, ringSkullTextures, braceletFlowerTextures, braceletSkullTextures;
}
