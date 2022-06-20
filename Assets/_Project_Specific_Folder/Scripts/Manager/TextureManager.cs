using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class TextureManager : Singleton<TextureManager>
{
    public enum ETattooClass
    {
        All,
        Flower,
        Skull,
        PinupGirl,
        Celebrity,
        Money,
        Calligraphy
    }
    
    public Texture2D handBurntTexture;
    
    [Space][Header("Tattoo Section")] 
    public ETattooClass tattooClass;
    
    [Space][Header("Flower Tattoo Section")]
    public Texture2D flowerDefaultTattoo;
    public List<Texture2D> flowerExpensiveTattoos;
    public List<Texture2D> flowerExpensiveBlueTattoos;
    public List<Texture2D> flowerExpensiveYellowTattoos;
    public List<Texture2D> flowerCheapTattoos;
    public List<Texture2D> flowerCheapBlueTattoos;
    public List<Texture2D> flowerCheapYellowTattoos;
    public List<int> flowerExpensiveColorTattooIdSequences;
    public List<int> flowerCheapColorTattooIdSequences;

    [Space][Header("Skull Tattoo Section")]
    public Texture2D skullDefaultTattoo;
    public List<Texture2D> skullExpensiveTattoos;
    public List<Texture2D> skullExpensiveBlueTattoos;
    public List<Texture2D> skullExpensiveYellowTattoos;
    public List<Texture2D> skullCheapTattoos;
    public List<Texture2D> skullCheapBlueTattoos;
    public List<Texture2D> skullCheapYellowTattoos;
    public List<int> skullExpensiveColorTattooIdSequences;
    public List<int> skullCheapColorTattooIdSequences;

    [Space][Header("Pinup Girl Tattoo Section")]
    public Texture2D pinupGirlDefaultTattoo;
    public List<Texture2D> pinupGirlExpensiveTattoos;
    public List<Texture2D> pinupGirlExpensiveBlueTattoos;
    public List<Texture2D> pinupGirlExpensiveYellowTattoos;
    public List<Texture2D> pinupGirlCheapTattoos;
    public List<Texture2D> pinupGirlCheapBlueTattoos;
    public List<Texture2D> pinupGirlCheapYellowTattoos;
    public List<int> pinupGirlExpensiveColorTattooIdSequences;
    public List<int> pinupGirlCheapColorTattooIdSequences;
    
    [Space][Header("Celebrity Tattoo Section")]
    public Texture2D celebrityDefaultTattoo;
    public List<Texture2D> celebrityExpensiveTattoos;
    public List<Texture2D> celebrityExpensiveBlueTattoos;
    public List<Texture2D> celebrityExpensiveYellowTattoos;
    public List<Texture2D> celebrityCheapTattoos;
    public List<Texture2D> celebrityCheapBlueTattoos;
    public List<Texture2D> celebrityCheapYellowTattoos;
    public List<int> celebrityExpensiveColorTattooIdSequences;
    public List<int> celebrityCheapColorTattooIdSequences;
    
    [Space][Header("Money Tattoo Section")]
    public Texture2D moneyDefaultTattoo;
    public List<Texture2D> moneyExpensiveTattoos;
    public List<Texture2D> moneyExpensiveBlueTattoos;
    public List<Texture2D> moneyExpensiveYellowTattoos;
    public List<Texture2D> moneyCheapTattoos;
    public List<Texture2D> moneyCheapBlueTattoos;
    public List<Texture2D> moneyCheapYellowTattoos;
    public List<int> moneyExpensiveColorTattooIdSequences;
    public List<int> moneyCheapColorTattooIdSequences;

    [Space][Header("Calligraphy Tattoo Section")]
    public Texture2D calligraphyDefaultTattoo;
    public List<Texture2D> calligraphyExpensiveTattoos;
    public List<Texture2D> calligraphyExpensiveBlueTattoos;
    public List<Texture2D> calligraphyExpensiveYellowTattoos;
    public List<Texture2D> calligraphyCheapTattoos;
    public List<Texture2D> calligraphyCheapBlueTattoos;
    public List<Texture2D> calligraphyCheapYellowTattoos;
    public List<int> calligraphyExpensiveColorTattooIdSequences;
    public List<int> calligraphyCheapColorTattooIdSequences;
}
