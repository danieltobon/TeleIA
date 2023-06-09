using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangueCamera : MonoBehaviour
{
    public List<GameObject> cameras;
    
    public  float currentTimePrincip;
    public float currentTimeSecund;
    public GameObject camActive;
    public float randTimpePrinMax;
    public float randTimpeSecunMax;
    public GameObject player;
    public GameObject player2;
    public float speedPlayer;
    void Start()
    {
        currentTimePrincip = Random.Range(20, 40);
        currentTimePrincip = Random.Range(6, 10);
        camActive = cameras[0];
    }

    // Update is called once per frame
    void Update()
    {


        if (cameras[0].active)
        {

            currentTimePrincip -= Time.deltaTime;
            if (currentTimePrincip < 0)
            {

                activeCamara(cameras[Random.Range(1, cameras.Count)]);

                
            }
        }
        else
        {
            currentTimeSecund -= Time.deltaTime;
            if (currentTimeSecund < 0)
            {


                activeCamara(cameras[0]);

              


            }

        }

       
        var rotation = Quaternion.LookRotation(camActive.transform.position - player.transform.position);
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotation, Time.deltaTime * speedPlayer);
        player.transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y, 0); // lock x and z axis to
       
        var rotation2 = Quaternion.LookRotation(camActive.transform.position - player2.transform.position);
        player2.transform.rotation = Quaternion.Slerp(player2.transform.rotation, rotation2, Time.deltaTime * speedPlayer);
        player2.transform.eulerAngles = new Vector3(0, player2.transform.eulerAngles.y, 0); // lock x and z axis to
    }

    void activeCamara(GameObject cam)
    {
        
        cam.SetActive(true);
        camActive = cam;
        for (int i = 0; i < cameras.Count; i++)
        {
            if (cameras[i] != cam)
                cameras[i].SetActive(false);
            else cam.SetActive(true);
        }
        currentTimePrincip = Random.Range(5, 30);
        currentTimeSecund = Random.Range(3, 15);
    }
}
