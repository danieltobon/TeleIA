using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lexone.UnityTwitchChat;
using OpenAI;
using System.Linq;
public class ChatBotCampamento : MonoBehaviour
{
    // Start is called before the first frame update


    // Update is called once per frame
    public ChatGPTCampamento gpt;
    public IRC chat;
    float currentTimeMsg=10;
    private void Start()
    {
        IRC.Instance.OnChatMessage += OnChatMessage;
       
    }
    private void Update()
    {
        currentTimeMsg -= Time.deltaTime;
        if (currentTimeMsg<=0) {
            chat.SendChatMessage("Para interactuar con la IA un tema !tema y luego el mensaje EJEMPLO: !tema habla sobre los asesinatos ");
            chat.SendChatMessage("!redes ");
            currentTimeMsg = 500;
        }
        
    }
    private void OnChatMessage(Chatter chatter )
    {
     

        if (chatter.message.Contains("!tema") && !chatter.message.Contains("comando"))
        {


         gpt.ListTopicsInteraction.Add(chatter.message.Replace("!tema",""));
           

        }
    }
}
