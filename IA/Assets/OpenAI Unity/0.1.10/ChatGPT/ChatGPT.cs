using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Crosstales.RTVoice;
using System.Linq;


namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {

        public Animator anim;
        public Animator animUI;
        public float speedFillImage;
        public SkinnedMeshRenderer skin;
        public List<string> noticias= new List<string>();
        public List<string> listChatResponse = new List<string>();
        public AudioSource aud;
        public string VoiceName;
      

        private OpenAIApi openai = new OpenAIApi();

       
        public Dictionary<string,string> listChatInteraction = new Dictionary<string, string>();

        public List<string> promps;
        
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

        int numChatInteraction = 0;
        private void Start()
        {

            SendReply();
         
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
            skin.SetBlendShapeWeight(1, weightEyes);

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
            skin.SetBlendShapeWeight(3, weightMouth);
        }
        private void Update()
        {
          
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
                    speak(listChatResponse.ElementAt(0));
                }
                else if (noticias.Count > 0)
                {
                    speak(noticias.ElementAt(0));
                }
            }

        }
        public void speak(string message)
        {

            starMouth = true;
            anim.SetBool("Start", true);

          spk.Speak(message, aud, Speaker.Instance.VoiceForName("Microsoft Sabina Desktop"));
            if (noticias.Contains(message)) noticias.Remove(message);
            if (listChatResponse.Contains(message)) { 
                noticias.Remove(message); numChatInteraction += -1;
                    }
        }

        public async void SendReply()
        {
            replyIsWorking = true;
            bool isChat = false;
            timeNextNotice = (noticias.Count+1) * MaxtimeNextNotice;
            List<ChatMessage> messages = new List<ChatMessage>();
            var newMessage = new ChatMessage();
            newMessage.Role = "user";
            if (listChatInteraction.Count > 0) {

                ChatMessage newmsg;
                newmsg.Role = "user";
                newmsg.Content = "Eres una presentadora llamada Carmen trabajas en un show de twitch llamado TELE IA, actua como una presentadora.";
                messages.Add(newmsg);
                newMessage.Content = "Actua como si leyeras un comentario de el chat, esta persona llamada " + listChatInteraction.ElementAt(0).Key + ",dice siguiente: " +
                 listChatInteraction.ElementAt(0).Value + ". Responde en maximo 3 renglones con humor negro e interactua. No digas que eres una inteligencia artificial.";
                isChat = true;
            
                listChatInteraction.Remove(listChatInteraction.ElementAt(0).Key);
               
               
            }
            else
            {
                newMessage.Content = promps[0];
            }
 
         
            Debug.Log("Solicitud: " +newMessage.Content);
           

            messages.Add(newMessage);

          
           
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = messages
            });
        
            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();

                if (isChat) { noticias.Insert(0, message.Content); numChatInteraction++;  }
                else
                    noticias.Add(message.Content);


                replyIsWorking = false;

            }


          

        }





            
            

        
    }
}

