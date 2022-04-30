using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public sealed class UtilityManager : MonoBehaviour
{
    public static UtilityManager instance = new UtilityManager();
    List<ThoughtBubble> CurrentEffects = new List<ThoughtBubble>();
    public Color SelectedColour;
    public Color NormalColour;
    [SerializeField] float moveTextSpeed;

    public RawImage blackBox;

    public void Awake(){
        if(instance == null) {
            instance = this;        
        }
    }

    static UtilityManager(){
    }

    private UtilityManager(){

    }

    public static UtilityManager Instance{
        get{return instance;}   
    }
    
    public void CloseText(GameObject text, float speed){
        LeanTween.scale(text, new Vector3(0.0f, 0.0f, 0.0f), .2f/ speed).setIgnoreTimeScale(true);
    }

    public void OpenText(GameObject text, float speed){
        LeanTween.scale(text, new Vector3(1.0f, 1.0f, 1.0f), .2f / speed).setIgnoreTimeScale(true);
    }

    public void MoveText(GameObject text, Vector3 movePos){
        LeanTween.moveLocal(text, new Vector3(movePos.x, movePos.y, movePos.z), moveTextSpeed ).setIgnoreTimeScale(true).setEase(LeanTweenType.easeOutElastic);
    }

    public void AddToEffectList(ThoughtBubble effect){
        CurrentEffects.Add(effect);
    }

    public void RemoveFromEffectList(ThoughtBubble effect){
        CurrentEffects.Remove(effect);
    }

    public void SetText(GameObject _text, string value){
        _text.GetComponent<TextMeshProUGUI>().text = value;
    }

     public IEnumerator ScreenFade(float FadeRate, bool fadeIn, float delay){
        float targetAlpha;
        if(fadeIn) targetAlpha = 1.0f;
        else targetAlpha = 0.0f;

        yield return new WaitForSeconds(delay);
          
        Color curColor = blackBox.color;
        while(Mathf.Abs(curColor.a - targetAlpha) > 0.0001f) {
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, FadeRate * Time.deltaTime);
            blackBox.color = curColor;
            yield return null;
        }
    }



    ///EVENTS
    ///

    public event Action onDoorAoeTriggerEnter;

    public void DoorwayAoeTriggerEnter()
    {
        if (onDoorAoeTriggerEnter != null)
        { 
        
        }
    }
}