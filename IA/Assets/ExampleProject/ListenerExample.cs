using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lexone.UnityTwitchChat;

public class ListenerExample : MonoBehaviour
{
    public Chatter chatterObject; // Latest chatter object
    public BoxController boxPrefab;
    List<BoxController> boxes = new List<BoxController>();
    public int maxBoxes = 100;    
    public int spawnCount = 0;

    private void Start()
    {
        IRC.Instance.OnChatMessage += OnChatMessage;
    }

    private void OnChatMessage(Chatter chatter)
    {
       
        if (boxes.Count>0) { 
        if (chatter.message.Contains("!move"))
            {

                foreach (var item in boxes)
                {
                    if (item.ch.login == chatter.login)
                    {
                        item.move(chatter.message);
                       
                    }
                }

            }
            
            
    }
      
            BoxController box = Instantiate(boxPrefab, gameObject.transform.position, Quaternion.identity);

            // Initialize the box with the chatter details
            box.Initialize(chatter);
            boxes.Add(box);
           
            // This is just to show the latest chatter object in the inspector
          

            spawnCount++;

        
        chatterObject = chatter;
        // Spawn a new box


    }
}
