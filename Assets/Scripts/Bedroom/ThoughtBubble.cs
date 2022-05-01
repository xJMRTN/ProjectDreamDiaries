using UnityEngine;
using UnityEngine.UI;

public class ThoughtBubble : MonoBehaviour
{
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
           Modifier
    }

    [SerializeField] ThoughtEffect effect;
    public ThoughtCategory cat;

    public float dreamChance;
    bool selected = false;
    [SerializeField] AudioSource ButtonSelect;
    [SerializeField] AudioClip Select;

    [SerializeField] GameObject ButtonText;

    Image buttonColour;

    public string effectName;
    public bool unlocked = false;

    void Start(){
        ButtonSelect.clip = Select;
        buttonColour = this.GetComponent<Image>();      
    }

    public void Setup(){
        if(unlocked)
            UtilityManager.Instance.SetText(ButtonText, effectName);
         else
            UtilityManager.Instance.SetText(ButtonText, "?");
    }

    public void ThoughtSelected(){
        if(!unlocked) return;

        selected = !selected;
        if(selected){
            ButtonSelect.Play();
             UtilityManager.Instance.AddToEffectList(this);
             buttonColour.color = UtilityManager.Instance.SelectedColour;
        }
        else {
            ButtonSelect.Play();
            UtilityManager.Instance.RemoveFromEffectList(this);
            buttonColour.color = UtilityManager.Instance.NormalColour;
        }
    }

    public void Deselect(){
        buttonColour.color = UtilityManager.Instance.NormalColour;
        selected = false;
    }
}