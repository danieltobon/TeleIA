using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lexone.UnityTwitchChat;
using OpenAI;
using System.Linq;
public class ChatBot : MonoBehaviour
{
    // Start is called before the first frame update


    // Update is called once per frame
    public ChatGPT gpt;
    public IRC chat;
    float currentTimeMsg=10;
    private void Start()
    {
        IRC.Instance.OnChatMessage += OnChatMessage;
     
    }
    private void Update()
    {

        currentTimeMsg -= Time.deltaTime;
        if (currentTimeMsg <= 0)
        {
            chat.SendChatMessage("Para interactuar con la IA un tema !tema y luego el mensaje EJEMPLO: !tema habla sobre los asesinatos ");
            chat.SendChatMessage("!redes ");
            currentTimeMsg = 500;
        }
    }
    private void OnChatMessage(Chatter chatter)
    {
        if (chatter.message.Contains("!comandosPelota")) {

            chat.SendChatMessage("Comandos para mover la tu pelota !move, !move derecha !move izquierda !move adelante !move atras ");

        }

        if (chatter.message.Contains("!carmen") && !chatter.message.Contains("comando"))
        {
           
            if(!gpt.listChatInteraction.ContainsKey(chatter.login))
            gpt.listChatInteraction.Add(chatter.login, chatter.message.Replace("!carmen", "")); 
            else chat.SendChatMessage(chatter.login +  " Solo es posible hacer una peticion, espera a que te responda" );

        }
    }
}
