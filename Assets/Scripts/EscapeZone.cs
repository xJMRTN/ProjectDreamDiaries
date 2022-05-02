using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeZone : MonoBehaviour
{
    [SerializeField] AudioSource idleSound;
    [SerializeField] AudioSource activateSound;
    bool open = false;
    // Start is called before the first frame update
    void Start()
    {
        UtilityManager.instance.onWinConditionMet += openEscape;

    }


    void openEscape()
    {
        open = true;
        idleSound.Play();
        GetComponent<ParticleSystem>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void endgame()
    {
        StartCoroutine("startEscape");
    }
    private void OnDestroy()
    {
        UtilityManager.instance.onWinConditionMet -= openEscape;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (open)
            {
                endgame();
            }

        }
    }
    IEnumerator startEscape()
    {
        activateSound.Play();
        yield return new WaitForSeconds(1);
        UtilityManager.instance.GameEnd();
    }
}
