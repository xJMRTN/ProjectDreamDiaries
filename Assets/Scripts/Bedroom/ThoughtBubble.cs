using UnityEngine;
using UnityEngine.UI;

public class ThoughtBubble : MonoBehaviour
{
    [SerializeField] Sprite NormalImage;
    [SerializeField] Sprite GlowImage;
    [SerializeField] Sprite QuestionMark;
    [SerializeField] enum ThoughtEffect{
        Noir,
        Fried,
        Flood,
        Flipped,
        High,
        Distort,
        Survive,
        Sound,
        Key,
        Find,
        Normal
    }

    public enum ThoughtCategory{
           Camera,
           Objective,
           Modifier,
    }

    [SerializeField] ThoughtEffect effect;
    public ThoughtCategory cat;

    public bool random = false;

    public float dreamChance;
    bool selected = false;
    [SerializeField] AudioSource ButtonSelect;
    [SerializeField] AudioClip Select;

    [SerializeField] GameObject ButtonText;

    Image buttonColour;

    public string effectName;
    public bool unlocked = false;

    void Awake(){
        ButtonSelect.clip = Select;
        buttonColour = this.GetComponent<Image>();      
    }

    public void Setup(){
        if(random) return;
        Debug.Log("Setting up: " + name);
        if(unlocked){
            buttonColour.sprite = NormalImage;
        }
         else{
             Debug.Log(buttonColour);
             Debug.Log(QuestionMark);
            buttonColour.sprite = QuestionMark;
         }       
    }

    public void ThoughtSelected(){
        if(!unlocked) return;

        selected = !selected;
        if(selected){
            ButtonSelect.Play();
             UtilityManager.Instance.AddToEffectList(this);
             //buttonColour.color = UtilityManager.Instance.SelectedColour;
             buttonColour.sprite = GlowImage;
        }
        else {
            ButtonSelect.Play();
            UtilityManager.Instance.RemoveFromEffectList(this);
            //buttonColour.color = UtilityManager.Instance.NormalColour;
             buttonColour.sprite = NormalImage;
        }
    }

    public void Deselect(){
        //buttonColour.color = UtilityManager.Instance.NormalColour;
        buttonColour.sprite = NormalImage;
        selected = false;
    }
}