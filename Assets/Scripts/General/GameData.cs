using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public static GameData current;

    public List<UnlockData> unlockDatas = new List<UnlockData>();
    public bool EverythingUnlocked;
    

    public GameData(){
        if(current == null) {
           
        }
        else { 
            unlockDatas = current.unlockDatas;   
            EverythingUnlocked = current.EverythingUnlocked;   
        }
    }

    public void SaveGame(){
        current = this;
        current.unlockDatas = UtilityManager.Instance.unlockDatas;
        SaveLoad.Save();     
    }

    public void LoadGame(){
        SaveLoad.Load();
    }
}