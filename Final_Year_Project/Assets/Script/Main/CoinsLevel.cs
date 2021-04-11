using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsLevel : MonoBehaviour
{
    public int coins;
    public int attempt;
    public int refreshRankCoins;
    public string description;

    public void InitializeValue(int coins, int attempt, int refreshRankCoins, string dec)
    {
        this.coins = coins;
        this.attempt = attempt;
        this.refreshRankCoins = refreshRankCoins;
        description = dec;
    }
}
