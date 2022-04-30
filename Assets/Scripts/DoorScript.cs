using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField]
    private int ID;
    // Start is called before the first frame update
    void Start()
    {
        UtilityManager.instance.onDoorAoeTriggerEnter += openDoor;
    }


    void openDoor(int newID)
    {
        if (newID == ID)
        {
            LeanTween.rotateY(this.gameObject, -90f, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
