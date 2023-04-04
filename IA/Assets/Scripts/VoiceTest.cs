using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;

public class VoiceTest : MonoBehaviour
{
    public AudioSource aud;
    public string Text;
    public string VoiceName;
    void Start()
    {
        speak();
    }
    void speak() {
      
        Speaker.Instance.Speak(Text, aud, Speaker.Instance.VoiceForName(VoiceName));
    
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
