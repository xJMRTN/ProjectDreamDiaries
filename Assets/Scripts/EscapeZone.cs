using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UtilityManager.instance.onWinConditionMet += openEscape;
    }


    void openEscape()
    {
        GetComponent<ParticleSystem>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
