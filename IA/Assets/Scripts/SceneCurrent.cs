using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Crosstales.RTVoice;
public class SceneCurrent : MonoBehaviour
{
    public float time = 300;
    
 
    public TextMeshProUGUI textTime;

    public Speaker spk;
    public AudioSource aud;
    public Animator anim;
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2) speakCarmen(GameObject.Find("ActualScene").GetComponent<actualScene>().nextScene);
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
            time += -Time.deltaTime;
        if (time < 0)
        {
            if(GameObject.Find("ActualScene").GetComponent<actualScene>().nextScene== "Campamento")
              SceneManager.LoadScene(1);
            if (GameObject.Find("ActualScene").GetComponent<actualScene>().nextScene == "Noticiero")
                SceneManager.LoadScene(0);
            if(SceneManager.GetActiveScene().buildIndex != 2) SceneManager.LoadScene(2);


        }

        textTime.text = "Proximo show en: " + convertirSegundosHorasMinutos(time);


    }
    public string convertirSegundosHorasMinutos(float segundos)
    {
        int hor, min, seg;
        
        min = (int) (segundos / 60);
        seg = (int)(segundos % 60);
        if (seg < 10) seg = 0 + seg;
        return  min + ":" + seg ;
    }

    public void speakCarmen(string message)
    {
        anim.SetBool("Start",true);
        spk.Speak("Atencion! Interrumpimos este programa para continuar con nuestro horario de programacion. A continuacion : " + message, aud, Speaker.Instance.VoiceForName("Microsoft Sabina Desktop"), pitch: 1.4f);
      
    }
}
