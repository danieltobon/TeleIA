using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class actualScene : MonoBehaviour
{
    public string nextScene;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            nextScene = "Campamento";
        if (SceneManager.GetActiveScene().buildIndex == 1)
            nextScene = "Noticiero";
    }
}
