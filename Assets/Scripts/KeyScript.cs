using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KeyScript : MonoBehaviour
{
    [SerializeField]
    public int ID;
    // Start is called before the first frame update
    void Start()
    {
        UtilityManager.instance.onDoorAoeTriggerEnter +=useKey;
    }

    // Update is called once per frame

    private void useKey(int newID)
    {
        if (newID == ID)
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        UtilityManager.instance.onDoorAoeTriggerEnter -= useKey;
    }
}
