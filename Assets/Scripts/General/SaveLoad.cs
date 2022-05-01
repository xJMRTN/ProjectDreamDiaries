using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public sealed class SaveLoad : MonoBehaviour
{
    public static List<GameData> savedGames = new List<GameData>();
    private static SaveLoad instance = new SaveLoad();    

    void Awake(){
        if(instance == null) {
            instance = this;         
        }
        
    }

    static SaveLoad(){
        
    }

    private SaveLoad(){
        
    }

    public static SaveLoad Instance{
        get{return instance;}
    }

    public static int GetCurrentScene(){  
        int sceneID =  UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        return sceneID;
    }

    public static void Save(){              
        savedGames.Add(GameData.current);
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log("here");
        FileStream file = File.Create(Application.persistentDataPath + "/DreamDesigner.dd");
        Debug.Log(Application.persistentDataPath + "/DreamDesigner.dd");
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
        savedGames.Remove(GameData.current);
        Load();
    }

    public static void Load(){
        if(File.Exists(Application.persistentDataPath + "/DreamDesigner.dd")){
            savedGames.Clear();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/DreamDesigner.dd", FileMode.Open);
            SaveLoad.savedGames = (List<GameData>)bf.Deserialize(file);
            GameData.current = SaveLoad.savedGames[0];
            file.Close();
        }
    }
}   