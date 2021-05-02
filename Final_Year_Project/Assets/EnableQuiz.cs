using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableStatus : MonoBehaviour
{
    public string _enableStatus = "true";
    
    public void ChangedStatus(int val)
    {
        if (val == 0)
        {
            _enableStatus = "true";
        }
        else
        {
            _enableStatus = "false";
        }
            
    }

}
