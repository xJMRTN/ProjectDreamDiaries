using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffects : MonoBehaviour
{   
    private static GameEffects instance = new GameEffects();

    [SerializeField] enum GameEffect{
        Normal,
        Flipped
    };

    [SerializeField] GameEffect effect;
    [SerializeField] GameObject world;

    public void Awake(){
        if(instance == null) {
            instance = this;        
        }
    }

    static GameEffects(){
    }

    private GameEffects(){

    }

    public static GameEffects Instance{
        get{return instance;}   
    }

    void Start(){
         UtilityManager.instance.onWinConditionMet += UnlockNewEffect;
    }

    void UnlockNewEffect(){
        PlayerPrefs.SetInt("Win", 1);
        if(GameData.current.EverythingUnlocked){
            return;
        }
        bool ready = false;
        int attempt = 0;
        while(!ready){
            int randomValue = Random.Range(0, GameData.current.unlockDatas.Count);
            if(GameData.current.unlockDatas[randomValue].unlocked == false){
                GameData.current.unlockDatas[randomValue].unlocked = true;
                ready = true;
            }
            attempt++;
            if(attempt > 100) ready = true;
        }
        UtilityManager.Instance.SaveGame();
         StartCoroutine(UtilityManager.Instance.ChangeScene(0, 1f));
    }

    public void StartEffects(){
        if(effect == GameEffect.Flipped){
            CreateNewLand();
        }
    }

    void CreateNewLand(){
        GameObject newWorld = Instantiate(world);
        newWorld.GetComponent<CreateWorld>().enabled = false;
        newWorld.GetComponent<MeshCollider>().enabled = false;

        newWorld.transform.position = new Vector3(newWorld.transform.position.x,newWorld.transform.position.y + 40f,newWorld.transform.position.z + 200);
        newWorld.transform.Rotate(180, 0, 0);
    }
}
