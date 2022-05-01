using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghoulTriggerAOE : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            UtilityManager.instance.PlayerTakeDamge();
        }
    }
}
