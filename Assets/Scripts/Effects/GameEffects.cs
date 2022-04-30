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
