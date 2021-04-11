using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Teachers
{
    public string email;
    public string name;
    public string uid;

    public void SetName(string name)
    {
        this.name = name;
    }

    public void SetUid(string uid)
    {
        this.uid = uid;
    }

    public void SetEmail(string email)
    {
        this.email = email;
    }
}
