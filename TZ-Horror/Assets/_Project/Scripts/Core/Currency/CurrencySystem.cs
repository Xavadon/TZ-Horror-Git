using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrencySystem 
{
    public static int Currency { get; private set; }

    public static void Increace(int value)
    {
        if (value <= 0)
        {
            return;
        }

        Currency += value;
        MoneyPopupService.ShowPopup(value);
    }
}
