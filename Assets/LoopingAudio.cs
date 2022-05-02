using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingAudio : MonoBehaviour
{
    [SerializeField] AudioSource noise;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("loopAudio");
    }

    IEnumerator loopAudio()
    {
        while (true)
        {
            noise.Play();
            yield return new WaitForSeconds(5 + Random.Range(-1, 2));
            noise.Stop();
        }
    }

}
