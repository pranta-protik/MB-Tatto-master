using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class TextureManager : Singleton<TextureManager> 
{
    public Texture DefaultFlower, DefaultSkull, DefaultPinup , DefaultCeleb , defaultCartoon , DefaultMoney;

    public Texture[] FlowerExpensiveTattos, FlowerCheapTattos;
    public Texture[] FlowerBadBlue, FlowerGoodBlue, FlowerGoodYellow, FlowerBadYellow;
    public Texture[] SkullExpensiveTattos, SkullCheapTattos;
    public Texture[] SkullBadBlue, SkullGoodBlue, SkullGoodYellow, SkullBadYellow;


    public Texture[] PinnupGirlExpensiveTattos, PinnupGirlCheapTattos;
    public Texture[] PinnupGirlBadBlue, PinnupGirlGoodBlue, PinnupGirlGoodYellow, PinnupGirlBadYellow;

    public Texture[] CelebExpensiveTattos, CelebCheapTattos;
    public Texture[] CelebBadBlue, CelebGoodBlue, CelebGoodYellow, CelebBadYellow;


    public Texture[] CartoonExpensiveTattos, CartoonCheapTattos;
    public Texture[] CartoonBadBlue, CartoonGoodBlue, CartoonGoodYellow, CartoonBadYellow;

    public Texture[] MoneyExpensiveTattos, MoneyCheapTattos;
    public Texture[] MoneyBadBlue, MoneyGoodBlue, MoneyGoodYellow, MoneyBadYellow;




}
