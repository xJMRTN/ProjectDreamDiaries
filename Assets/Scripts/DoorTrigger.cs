using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    int ID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<KeyScript>())
        {
            if (other.GetComponent<KeyScript>().ID == ID)
            {
                Debug.Log("event triggered");
                UtilityManager.instance.DoorAoeTriggerEnter(ID);
            }
        }
    }
}
