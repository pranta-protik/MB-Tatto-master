public static class CurrencySystem
{
    public static string GetConvertedCurrencyString(int currency)
    {
        if (currency < 10000)
        {
            return $"{currency}";
        }

        if (currency < 1000000)
        {
            if (currency % 1000 == 0)
            {
                return $"{currency / 1000f}K";
            }
            
            return $"{currency / 1000f:0.00}K";
        }

        if (currency % 1000000 == 0)
        {
            return $"{currency / 1000000f}M";
        }
        
        return $"{currency / 1000000f:0.00}M";
    }
}