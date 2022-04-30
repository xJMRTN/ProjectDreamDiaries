using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTriggerScript : MonoBehaviour
{
    [SerializeField]
    int ID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per fram

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<WinConditionScript>())
        {
            if (other.GetComponent<WinConditionScript>().ID == ID)
            {
                UtilityManager.instance.WinConditionModify(1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<WinConditionScript>())
        {
            if (other.GetComponent<WinConditionScript>().ID == ID)
            {
                UtilityManager.instance.WinConditionModify(-1);
            }
        }
    }
}
