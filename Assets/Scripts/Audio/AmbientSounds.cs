using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSounds : MonoBehaviour
{
    [SerializeField] AudioClip[] AmbientMushroomSounds;
    [SerializeField] AudioSource source;

    bool ready = true;

    void Update(){
        if(ready){
            StartCoroutine(PlaySound());
        }
    }

    IEnumerator PlaySound(){
        ready = false;
        int randomTrack = Random.Range(0, AmbientMushroomSounds.Length);
        source.clip = AmbientMushroomSounds[randomTrack];
        source.Play();
        yield return new WaitForSeconds(4f);
        ready = true;
    }
}
