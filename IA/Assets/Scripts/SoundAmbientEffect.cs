using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAmbientEffect : MonoBehaviour
{
    public AudioSource aud;
    public List<AudioClip> clips;
    float currentTime = 16;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime >= 0) currentTime-= Time.deltaTime;
        else
        {
            aud.PlayOneShot(clips[Random.Range(0, clips.Count)]);
            currentTime = 16;
        }
    }
}
