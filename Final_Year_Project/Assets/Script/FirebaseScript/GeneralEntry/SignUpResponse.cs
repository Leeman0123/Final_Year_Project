using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SignUpResponse
{
    public string localId;

    public void SetLocalId(string localId)
    {
        this.localId = localId;
    }

}
