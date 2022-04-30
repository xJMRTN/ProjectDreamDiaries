using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WinConditionSign : MonoBehaviour
{
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        UtilityManager.instance.onWinConditionModified  += changeText;
    }


    void changeText(int newNumber)
    {
        text.text = newNumber + "/" +1;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
