using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Students
{
    public string email;
    public string name;
    public int coins;
    public string uid;

    public void SetEmail(string email)
    {
        this.email = email;
    }

    public void SetName(string name)
    {
        this.name = name;
    }

    public void SetUid(string uid)
    {
        this.uid = uid;
    }

    public void SetCoins(int coins)
    {
        this.coins = coins;
    }


}
