using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class NoiseEmitter : MonoBehaviour
{

    [SerializeField]
    float frequency = 5f;

    [SerializeField]
    AudioSource audio;

    [SerializeField]
    Light light;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("ping");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ping()
    {
        while (true)
        {
            LeanTween.value(gameObject, 0,1,frequency/2).setOnUpdate(updateLight);
            audio.Play();
            yield return new WaitForSeconds(frequency/2);
            LeanTween.value(gameObject, 1,0, frequency / 2).setOnUpdate(updateLight);
            yield return new WaitForSeconds(frequency / 2);
            audio.Stop();

        }
    }

    void updateLight(float newNumber)
    {
        light.intensity = newNumber;
    }
}
