using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Crosstales.RTVoice;
using System.Linq;


namespace OpenAI
{
    public class ChatGPTMillonario : MonoBehaviour
    {

        public Animator animMen;
        public Animator animWomen;

        public SkinnedMeshRenderer skinWoman;
        private OpenAIApi openai = new OpenAIApi();

        public List<string> msgWomen= new List<string>();
        public List<string> msgMen = new List<string>();



        public List<string> listChatResponse = new List<string>();
        public Dictionary<string, string> listChatInteraction = new Dictionary<string, string>();
        public List<string> promps;

        public AudioSource audWoman;
        public AudioSource audMen;
        public string VoiceName;



        List<ChatMessage> messagesMen = new List<ChatMessage>();
        List<ChatMessage> messagesWoman = new List<ChatMessage>();


        public Speaker spk;
        float weightEyes = 0;
        float weightMouth = 0;
       
        float timeEyes;
        bool closedEyes;

        bool closedMouth;
        bool starMouth;

        public float speedEyes = 0;
        public float speedMouth = 0;
        public float MaxtimeNextNotice = 0;
        float timeNextNotice = 10;

        bool replyIsWorking;
        private void Start()
        {

            SendReplyMen();
         
        }
        void updateBlendShapes() {
            if (weightEyes<100 && closedEyes==false ) {
                timeEyes += Time.deltaTime;
                if (timeEyes > 3)
                {
                    weightEyes += speedEyes * Time.deltaTime;

                    
                }

            }
            else  closedEyes = true;
            
            if (closedEyes)
            {
                timeEyes = 0;
                weightEyes -= speedEyes * Time.deltaTime;
                if (weightEyes < 0) closedEyes = false;
            }
            skinWoman.SetBlendShapeWeight(1, weightEyes);

            if (starMouth) {
                if (weightMouth < 100 && closedMouth == false)
                {
                   
                  
                        weightMouth += speedMouth * Time.deltaTime;


                }
                else closedMouth = true;

                if (closedMouth)
                {

                    weightMouth -= speedMouth * Time.deltaTime;
                    if (weightMouth < 0) closedMouth = false;
                }



            }
            else
            {
                weightMouth = 0;
            }
            skinWoman.SetBlendShapeWeight(15, weightMouth);
        }
        private void Update()
        {
        /*  
            if (listChatInteraction.Count > 0 && !replyIsWorking)
                SendReply();
            if (timeNextNotice > 0) {
                timeNextNotice -= Time.deltaTime;

            }
            else
            {
                SendReply();
            }
            updateBlendShapes();


            if (!spk.isBusy)
            {
                if (listChatResponse.Count > 0)
                {
                   speakMen (listChatResponse.ElementAt(0));
                }
                else if (noticias.Count > 0)
                {
                    speakMen(noticias.ElementAt(0));
                }
            }
        */
        }
        public async void SendReplyMen()
        {




            var newMessage = new ChatMessage();
            newMessage.Role = "user";

           

                ChatMessage newmsg;
                newmsg.Role = "user";
                newmsg.Content = "Eres pablo laserna un presentador de un programa llamado IA Millonaria, el programa es estilo quien quiere ser millonario y estas charlando con un concursante";
            //messages.Add(newmsg);
                 newMessage.Content = "Realiza una pregunta se selccion multiple, dame 4 posiblesa respuestas y 1 de esas respuestas es verdadera ";
                 //listChatInteraction.ElementAt(0).Value + ",responde con humor negro e interactua. Resume tu respuesta en Maximo 100 palabras";


               // listChatInteraction.Remove(listChatInteraction.ElementAt(0).Key);

            
           

            Debug.Log("Solicitud: " + newMessage.Content);


            messagesMen.Add(newMessage);



            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = messagesMen
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                //message.Content = message.Content.Substring(0, 50);


                msgMen.Add(message.Content);
                Debug.Log("RespuestaCHARLY: " + message.Content);
                string[] text = message.Content.Split('.');

                foreach (var item in text)
                {
                    Debug.Log(item.Substring(item.IndexOf("Clara:"), item.Length).Trim(' '));
                }
            }
            


         }
    /*    public async void SendReplyWoman()
        {




            var newMessage = new ChatMessage();
            newMessage.Role = "user";

            if (listChatInteraction.Count > 0)
            {

                ChatMessage newmsg;
                newmsg.Role = "user";
                newmsg.Content = "Eres una presentadora llamada Carmen trabajas en un show de twitch llamado TELE IA, actua como una presentadora. Responde sobre temas ilegales de una forma humoristica ";
                //messages.Add(newmsg);
                newMessage.Content = "Actua como si leyeras un comentario de el chat, esta persona llamada " + listChatInteraction.ElementAt(0).Key + ",dice siguiente: " +
                 listChatInteraction.ElementAt(0).Value + ",responde con humor negro e interactua. Resume tu respuesta en Maximo 100 palabras";


                listChatInteraction.Remove(listChatInteraction.ElementAt(0).Key);

            }
            else
            {
                
                 newMessage.Content = "Actua como un personaje llamada Clara, Desenvuelve el momento de lo que dice Charly, " +
                    ", no incluyas acciones o actos en presente, Charly dice: " + msgMen[msgMen.Count-1];
            }


            Debug.Log("Solicitud: " + newMessage.Content);


            messagesMen.Add(newMessage);



            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = messagesMen
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
              //  message.Content = message.Content.Substring(0, 50);


                msgWomen.Add(message.Content);
                Debug.Log("RespuestaCLARA: " + message.Content);
            }

            

        } */




        /*    public void speakWomen(string message)
            {

                starMouth = true;
                animWomen.SetBool("Start", true);

              spk.Speak(message, audWoman, Speaker.Instance.VoiceForName("Microsoft Sabina Desktop"));
                if (noticias.Contains(message)) noticias.Remove(message);
                if (listChatResponse.Contains(message)) noticias.Remove(message);
            }
            public void speakMen(string message)
            {

                starMouth = true;
                anim.SetBool("Start", true);

                spk.Speak(message, audMen, Speaker.Instance.VoiceForName("Microsoft Sabina Desktop"));
                if (noticias.Contains(message)) noticias.Remove(message);
                if (listChatResponse.Contains(message)) noticias.Remove(message);
            }        

            */
    }
    
}

