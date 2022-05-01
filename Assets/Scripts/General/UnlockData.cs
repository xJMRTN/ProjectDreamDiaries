using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnlockData
{
    public string effectName;
    public bool unlocked; 

    public UnlockData(string _name, bool _unlocked){
        effectName = _name;
        unlocked = _unlocked;
    }
}