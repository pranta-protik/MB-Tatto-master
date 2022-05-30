using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class TextureManager : Singleton<TextureManager> 
{
    public Texture DefaultFlower, DefaultSkull, DefaultPinup , DefaultCeleb , defaultCartoon , DefaultMoney;

    public Texture[] FlowerExpensiveTattos, FlowerCheapTattos;
    public Texture[] SkullExpensiveTattos, SkullCheapTattos;

    public Texture[] PinnupGirlExpensiveTattos, PinnupGirlCheapTattos;

    public Texture[] CelebExpensiveTattos, CelebCheapTattos;

    public Texture[] CartoonExpensiveTattos, CartoonCheapTattos; 
    
    public Texture[] MoneyExpensiveTattos, MoneyCheapTattos;




}
