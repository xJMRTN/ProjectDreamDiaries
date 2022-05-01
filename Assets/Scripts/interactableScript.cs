using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableScript : MonoBehaviour
{
    [SerializeField]
    Light light;
    void Start()
    {
        StartCoroutine("ping");
    }

    IEnumerator ping()
    {
        while (true)
        {
            LeanTween.value(gameObject, 0, 1, 1).setOnUpdate(updateLight);
            yield return new WaitForSeconds(1);
            LeanTween.value(gameObject, 1, 0, 1).setOnUpdate(updateLight);
            yield return new WaitForSeconds(1);

        }
    }

    void updateLight(float newNumber)
    {
        light.intensity = newNumber;
    }





}
