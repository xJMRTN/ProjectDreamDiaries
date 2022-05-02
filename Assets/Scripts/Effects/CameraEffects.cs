using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public sealed class CameraEffects : MonoBehaviour
{
    private static CameraEffects instance = new CameraEffects();
    [SerializeField] Volume CameraVolume;
    [SerializeField] VolumeProfile Normal;
    [SerializeField] VolumeProfile Noir;
    [SerializeField] VolumeProfile Fried;
    [SerializeField] VolumeProfile High;

    [SerializeField] enum CameraEffect{
        Normal, 
        Noir,
        Fried,
        High
    };

    [SerializeField] CameraEffect effect;

    [SerializeField] bool CameraDistortion;
    [SerializeField] bool Flooded;
    [SerializeField] GameObject waterObj;

    LensDistortion ld;
    PaniniProjection pp;
    LiftGammaGain LGG;
    DepthOfField dof;

    [SerializeField] Vector2 xMRange;
    [SerializeField] Vector2 yMRange;
    [SerializeField] Vector2 scaleRange;

    float scale = 1f;
    float xM = 0.2f;
    float yM = 0.2f;

    Vector4 liftValues;
    Vector4 gammaValues;
    Vector4 gainValues;

    bool busy = false;
    bool colourBusy = false;
    bool inWater;
    [SerializeField] Transform water;
    public Transform HeadPoint;

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

    void Start(){
        string gameEffect = PlayerPrefs.GetString("ModifierChoice");
         switch(gameEffect){
             case "Flood":
                Flooded = true;
                break;
             case "Distort":
                CameraDistortion = true;
                break;
         }

        string cameraEffect = PlayerPrefs.GetString("CameraChoice");
         switch(cameraEffect){
             case "Fried":
                effect = CameraEffect.Fried;
                break;
             case "Noir":
                effect = CameraEffect.Noir;
                break;
            case "Normal":
                effect = CameraEffect.Normal;
                break;
            case "High":
                effect = CameraEffect.High;
                break;
         }




        if(Flooded) waterObj.SetActive(true);
        switch(effect){
            case CameraEffect.Normal:
                CameraVolume.profile = Normal;
                break;
            case CameraEffect.Noir:
                CameraVolume.profile = Noir;
             break;
            case CameraEffect.Fried:
                CameraVolume.profile = Fried;
             break;     
             case CameraEffect.High:
                CameraVolume.profile = High;
                LiftGammaGain temp;
                if(CameraVolume.profile.TryGet(out temp)){
                    LGG = temp;
                    liftValues = ((Vector4)LGG.lift);
                    gammaValues = ((Vector4)LGG.gamma);
                    gainValues = ((Vector4)LGG.gain);
                }
             break;       
        }    

        DepthOfField dofTemp;
        if(CameraVolume.profile.TryGet(out dofTemp)){
            dof = dofTemp;
        }
        dof.focalLength.value = 50f;
            
       

        if(CameraDistortion){
            SetupDistortion();
        } 
    }

    void SetupDistortion(){
        CameraVolume.profile.Add<LensDistortion>();
        CameraVolume.profile.Add<PaniniProjection>();

        LensDistortion temp;
        PaniniProjection pan;

        if(CameraVolume.profile.TryGet(out temp))
            ld = temp;

        if(CameraVolume.profile.TryGet(out pan))
            pp = pan;


        ld.intensity.Override(1f);
        pp.distance.Override(1f);
        pp.cropToFit.Override(1f);


        ld.scale.Override(scale);
        ld.xMultiplier.Override(xM);
        ld.yMultiplier.Override(yM);
    }

    void Update(){
        if(CameraDistortion){
            if(!busy) StartCoroutine(CalculateWave());
            MoveCamera();
        }

        // if(effect == CameraEffect.High){
        //     if(!colourBusy) StartCoroutine(CalculateColours());
        //     ChangeColours();
        // }

        if(Flooded)CheckIfInWater();

       
    }

    IEnumerator CalculateColours(){
       colourBusy = true;
        liftValues = new Vector4(liftValues.x + Random.Range(-0.03f, 0.03f), liftValues.y + Random.Range(-0.03f, 0.03f), liftValues.z + Random.Range(-0.03f, 0.03f), liftValues.w);
        gammaValues = new Vector4(gammaValues.x + Random.Range(-0.03f, 0.03f), gammaValues.y + Random.Range(-0.03f, 0.03f), gammaValues.z + Random.Range(-0.03f, 0.03f), gammaValues.w);
        gainValues = new Vector4(gainValues.x + Random.Range(-0.03f, 0.03f), gainValues.y + Random.Range(-0.03f, 0.03f), gainValues.z + Random.Range(-0.03f, 0.03f), gainValues.w);

       

        

        yield return new WaitForSeconds(0.04f);
        colourBusy = false;
    }

    void ChangeColours(){
        Vector4 newLift = new Vector4(
        Mathf.Lerp(((Vector4)LGG.lift).x, liftValues.x, Time.deltaTime),
        Mathf.Lerp(((Vector4)LGG.lift).y, liftValues.y, Time.deltaTime),
        Mathf.Lerp(((Vector4)LGG.lift).z, liftValues.z, Time.deltaTime),
        liftValues.w);

        Vector4 newGamma = new Vector4(
        Mathf.Lerp(((Vector4)LGG.gamma).x, gammaValues.x, Time.deltaTime),
        Mathf.Lerp(((Vector4)LGG.gamma).y, gammaValues.y, Time.deltaTime),
        Mathf.Lerp(((Vector4)LGG.gamma).z, gammaValues.z, Time.deltaTime),
        gammaValues.w);

        Vector4 newGain = new Vector4(
        Mathf.Lerp(((Vector4)LGG.gain).x, gainValues.x, Time.deltaTime),
        Mathf.Lerp(((Vector4)LGG.gain).y, gainValues.y, Time.deltaTime),
        Mathf.Lerp(((Vector4)LGG.gain).z, gainValues.z, Time.deltaTime),
        gainValues.w);
        
        if(newLift.x < 1f) newLift.y = 1f;
        if(newLift.y < 1f) newLift.z = 1f;
        if(newLift.z < 1f) newLift.x = 1f;
        if(newLift.x > 0.5f) newLift.x = 0.5f;
        if(newLift.y > 0.5f) newLift.y = 0.5f;
        if(newLift.z > 0.5f) newLift.z = 0.5f;

        if(newGamma.x < 1f) newGamma.y = 1f;
        if(newGamma.y < 1f) newGamma.z = 1f;
        if(newGamma.z < 1f) newGamma.x = 1f;
        if(newGamma.x > 0.5f) newGamma.x = 0.5f;
        if(newGamma.y > 0.5f) newGamma.y = 0.5f;
        if(newGamma.z > 0.5f) newGamma.z = 0.5f;

        if(newGain.x < 1f) newGain.y = 1f;
        if(newGain.y < 1f) newGain.z = 1f;
        if(newGain.z < 1f) newGain.x = 1f;
        if(newGain.x > 0.5f) newGain.x = 0.5f;
        if(newGain.y > 0.5f) newGain.y = 0.5f;
        if(newGain.z > 0.5f) newGain.z = 0.5f;


        LGG.lift.Override(newLift);
        LGG.gamma.Override(newGamma);
        LGG.gain.Override(newGain);
    }

    IEnumerator CalculateWave(){
        busy = true;
        scale += Random.Range(-0.02f, 0.02f);
        xM += Random.Range(-0.02f, 0.02f);
        yM += Random.Range(-0.02f, 0.02f);

        if(scale > scaleRange.y) scale = scaleRange.y;
        if(scale < scaleRange.x) scale = scaleRange.x;
        if(xM > xMRange.y) xM = xMRange.y;
        if(xM < xMRange.x) xM = xMRange.x;
        if(yM > yMRange.y) yM = yMRange.y;
        if(yM < yMRange.x) yM = yMRange.x;

        
        yield return new WaitForSeconds(0.05f);
        busy = false;
    }

    void MoveCamera(){
        float newScale = Mathf.Lerp(((float)ld.scale), scale, Time.deltaTime);
        float newX = Mathf.Lerp(((float)ld.xMultiplier), xM, Time.deltaTime);
        float newY = Mathf.Lerp(((float)ld.yMultiplier), yM, Time.deltaTime);


        ld.scale.Override(newScale);
        ld.xMultiplier.Override(newX);
        ld.yMultiplier.Override(newY);
    }

    void CheckIfInWater(){
        if(HeadPoint.position.y < water.position.y && !inWater){
            inWater = true;
            dof.focalLength.value = 220f;
        }

        if(HeadPoint.position.y > water.position.y && inWater){
            inWater = false;
            dof.focalLength.value = 50f;
        }
    }
}