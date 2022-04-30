using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public sealed class CameraEffects : MonoBehaviour
{
    private static CameraEffects instance = new CameraEffects();
    [SerializeField] Volume CameraVolume;
    [SerializeField] VolumeProfile Normal;
    [SerializeField] VolumeProfile Noir;
    [SerializeField] VolumeProfile Fried;

    public void Awake(){
        if(instance == null) {
            instance = this;        
        }
    }

    static CameraEffects(){
    }

    private CameraEffects(){

    }

    public static CameraEffects Instance{
        get{return instance;}   
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.J)){
            CameraVolume.profile = Normal;
        }

        if(Input.GetKeyDown(KeyCode.K)){
            CameraVolume.profile = Noir;
        }

        if(Input.GetKeyDown(KeyCode.L)){
            CameraVolume.profile = Fried;
        }
    }
}