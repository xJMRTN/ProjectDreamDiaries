using UnityEngine;
using UnityEngine.UI;

public class ThoughtBubble : MonoBehaviour
{
    [SerializeField] enum ThoughtEffect{
        one,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine,
        ten
    }
    [SerializeField] ThoughtEffect effect;
    bool selected = false;
    [SerializeField] bool canBeUsed = false;

    Image buttonColour;

    void Start(){
        buttonColour = this.GetComponent<Image>();
    }

    public void ThoughtSelected(){
        if(!canBeUsed) return;

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