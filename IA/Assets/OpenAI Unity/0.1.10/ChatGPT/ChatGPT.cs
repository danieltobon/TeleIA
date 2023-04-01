using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Crosstales.RTVoice;
using System.Linq;
using TMPro;
using System.IO;
namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {

        

        public Image imageBar;
        public TextMeshProUGUI textChatUI;
        public float speedBar;
        bool barShowBool;

        public Animator anim;
        public Animator animUI;
        public float speedFillImage;
        public SkinnedMeshRenderer skin;
        public List<string> noticias= new List<string>();
        public Dictionary<string, string> listChatResponse = new Dictionary<string, string>();
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
        float timeToAnswer = 7;
        float timeNextAnswer = 3;
       
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


            if (barShowBool) {

                imageBar.fillAmount += Time.deltaTime * speedBar;
            }
            else imageBar.fillAmount += -Time.deltaTime * speedBar;

            if (timeToAnswer > 0) timeToAnswer += -Time.deltaTime;
        }
        private void Update()
        {
            Debug.Log("CHATINTERAICON COUNT: " + listChatInteraction.Count);
            if (listChatInteraction.Count > 0 && timeNextAnswer<0 )
            {
                SendReply();
                
            }
            if (timeNextAnswer > 0)
            {
                timeNextAnswer -= Time.deltaTime;

            }
            if (timeNextNotice > 0 ) {
                timeNextNotice -= Time.deltaTime;

            }
            else if(noticias.Count < 4)
            {
                SendReply();
            }
            updateBlendShapes();


            if (!spk.isBusy)
            {
                if (listChatResponse.Count > 0 && timeToAnswer < 0)
                {
                    
                    speak(listChatResponse.ElementAt(0).Value);
                    
                }
                else if (noticias.Count > 0)
                {
                   
                    speak(noticias.ElementAt(0));
                }
            }

        }
        public void barShow(string text) {
            textChatUI.text = text;
            barShowBool = true;
        }
        public void speak(string message)
        {
           
            starMouth = true;
            anim.SetBool("Start", true);

          spk.Speak(message, aud, Speaker.Instance.VoiceForName("Microsoft Sabina Desktop"));
            if (noticias.Contains(message))
            {
                barShowBool = false;
                textChatUI.text = "";
                noticias.Remove(message);
                
            }
            if (listChatResponse.ContainsValue(message)) {

                barShow(listChatResponse.ElementAt(0).Key);
                var item = listChatResponse.FirstOrDefault(kvp => kvp.Value == message);
                listChatResponse.Remove(item.Key);
               }
        }

        public async void SendReply()
        {
            List<ChatMessage> messages = new List<ChatMessage>();
            replyIsWorking = true;
            bool isChat = false;
            string name = "";
            string content = "";
            timeNextNotice = (noticias.Count+1) * MaxtimeNextNotice;
         
            var newMessage = new ChatMessage();
            newMessage.Role = "user";
            ChatMessage newmsg;
            newmsg.Role = "system";
            newmsg.Content = "Responde todo en maximo 8 renglones, genera opiniones chistosas sobre cada noticia, escribe con un toque de humor negro," +
                " Eres una presentadora llamada Carmen trabajas en un show humoristico de twitch llamado TELE IA, actua como una presentadora. No digas que eres una inteligencia artificial.no saludes al inicio";
            messages.Add(newmsg);
            ChatMessage newmsg2;
            newmsg.Role = "assistant";
            //newmsg.Content = "Esto es insolito, en espa�a un tio se lanzo desde un paracaidas";
            messages.Add(newmsg);
            if (listChatInteraction.Count > 0) {               
                newMessage.Content = "Actua como si leyeras un comentario de el chat, esta persona llamada " + listChatInteraction.ElementAt(0).Key + ",dice siguiente: " +
                 listChatInteraction.ElementAt(0).Value + ".  Responde de forma creativa chistosamente o sarcasticamente o ironicamente. Dale un toque de humor negro. ";
                name = listChatInteraction.ElementAt(0).Key;
                content = listChatInteraction.ElementAt(0).Value;
                listChatInteraction.Remove(listChatInteraction.ElementAt(0).Key);
              
                isChat = true;
                timeNextAnswer = 5;
            }
            else
            {
                newMessage.Content = " Narra 1 noticia creativa chistosa responde de forma chistosa con un toque de humor negro.";
            }
 
         
            Debug.Log("Solicitud: " +newMessage.Content);
           

            messages.Add(newMessage);

        
           
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = messages,
               
                
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();

                if (isChat)
                {
                    listChatResponse.Add(name + ": " + content, message.Content);
                    noticias.Add(message.Content);
                    string path = "C:/Users/danie/OneDrive/Escritorio/IA.txt";
                    StreamWriter writer = new StreamWriter(path, true);
                    writer.WriteLine(name + ": " + content+" Respuesta " + message.Content);
                    writer.Close();

                    timeToAnswer = 3;
                    isChat = false;
                }
                else
                {
                    noticias.Add(message.Content);
                    string path = "C:/Users/danie/OneDrive/Escritorio/IA.txt";
                    StreamWriter writer = new StreamWriter(path, true);
                    writer.WriteLine("Noticia: "+ message.Content);
                    writer.Close();
                }
               // message.Role = "assistant";
               // messages.Add(message);
              
                replyIsWorking = false;

                
            }


          

        }





            
            

        
    }
}

