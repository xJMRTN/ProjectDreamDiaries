using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KeyScript : MonoBehaviour
{
    private int ID;
    // Start is called before the first frame update
    void Start()
    {
        UtilityManager.instance.onDoorAoeTriggerEnter +=useKey;
    }

    // Update is called once per frame

    private void useKey()
    { 
    
    }

    void Update()
    {
        
    }
}
