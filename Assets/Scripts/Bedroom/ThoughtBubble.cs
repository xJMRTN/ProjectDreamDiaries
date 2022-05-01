using UnityEngine;
using UnityEngine.UI;

public class ThoughtBubble : MonoBehaviour
{
    [SerializeField] enum ThoughtEffect{
        Noir,
        Fried,
        Flood,
        Normal,
        High,
        Distort,
        Survive,
        Sound,
        Key,
        Find
    }

    [SerializeField] ThoughtEffect effect;

    public bool bad;
    bool selected = false;

    [SerializeField] GameObject ButtonText;

    Image buttonColour;

    public string effectName;
    public bool unlocked = false;

    [SerializeField] UIManager uIManager;

    void Start(){
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
             UtilityManager.Instance.AddToEffectList(this);
             buttonColour.color = UtilityManager.Instance.SelectedColour;
        }
        else {
            UtilityManager.Instance.RemoveFromEffectList(this);
            buttonColour.color = UtilityManager.Instance.NormalColour;
        }
    }
}