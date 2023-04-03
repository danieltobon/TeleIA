using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public float time = 300;
    public int scene;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
            time += -Time.deltaTime;
        if (time < 0)
        {

            SceneManager.LoadScene(scene);
         
     
        
        }
            
    }
}
