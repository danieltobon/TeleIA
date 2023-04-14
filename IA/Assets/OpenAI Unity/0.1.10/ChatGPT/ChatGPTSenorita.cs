using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Crosstales.RTVoice;
using System.Linq;
using TMPro;
using System.IO;
namespace OpenAI
{
    public class ChatGPTSenorita : MonoBehaviour
    {
      
        public int countAnswer = 0;
        public float timeSpeakingCarmen=10;
        public float timeSpeakingChela=0;
        
        public Image imageBar;
     
        public float speedBar;
        bool barShowBool;
        public TextMeshProUGUI textInteraction;

        bool SpeakingChela;
        bool SpeakingCarmen;

        public Animator animChela;
        public Animator animCarmen;
        public float speedFillImage;
        public SkinnedMeshRenderer skin;
        public SkinnedMeshRenderer skinChela;
        public List<string> noticias= new List<string>();
        public Dictionary<string, string> listChatResponse = new Dictionary<string, string>();
        public AudioSource aud;
        public AudioSource audMen;
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

        string msgForChela;
        public string answerForChela="";
        private void Start()
        {
           


            SendReplyChela("");
         
        }
        private void Update()
        {
        
            if (listChatInteraction.Count > 0 && timeNextAnswer < 0)
            {
                SendReply("");

            }
            if (timeNextAnswer > 0)
            {
                timeNextAnswer -= Time.deltaTime;

            }
            if (timeNextNotice > 0)
            {
                timeNextNotice -= Time.deltaTime;

            }
            else if (noticias.Count < 4)
            {
                
            }
            updateBlendShapes();


            if (!spk.isBusy && countAnswer==0)
            {
                if (listChatResponse.Count > 0)
                {

                    speakCarmen(listChatResponse.ElementAt(0).Value);

                }
                else if (noticias.Count > 0)
                {

                    speakCarmen(noticias.ElementAt(0));
                }
            }
            if (countAnswer == 1)
            {
                
               
                timeSpeakingCarmen -= Time.deltaTime;
                if (timeSpeakingCarmen <= 0) {  countAnswer = 2; }
            }
            if (countAnswer == 2)
            {
               
            
             
                timeSpeakingChela -= Time.deltaTime;
                if (timeSpeakingChela <= -1) countAnswer = 0;
            }


            if (timeSpeakingCarmen <= 0) {
                speakChela(answerForChela);
                SendReply(answerForChela);
                timeSpeakingCarmen = 10;
            }

            if (barShowBool)
            {

                imageBar.fillAmount += Time.deltaTime * speedBar;
            }
            else imageBar.fillAmount += -Time.deltaTime * speedBar;
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
            skinChela.SetBlendShapeWeight(0, weightEyes);
            skinChela.SetBlendShapeWeight(1, weightEyes);

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
            if(SpeakingCarmen)
                skin.SetBlendShapeWeight(3, weightMouth);
            else
            {
                skin.SetBlendShapeWeight(3, 0);
            }
            if (SpeakingChela)
                skinChela.SetBlendShapeWeight(4, weightMouth);
            else
            {
                skinChela.SetBlendShapeWeight(4, 0);
            }

          
        }
       
 
        public void speakCarmen(string message)
        {
           
            starMouth = true;
            SpeakingCarmen = true;
            SpeakingChela = false;
            animCarmen.SetBool("Start", true);
            animChela.SetBool("Start", false);
            try
            {
                spk.Speak(message, aud, Speaker.Instance.VoiceForName("Microsoft Sabina Desktop"), pitch: 1.4f);
            }
            catch (System.Exception)
            {
                spk.Speak("Hola", aud, Speaker.Instance.VoiceForName("Microsoft Sabina Desktop"), pitch: 1.4f);

            }
          
            if (noticias.Contains(message))
            {
                barShowBool = false;
                textInteraction.text = "";
                noticias.Remove(message);
                
            }
            if (listChatResponse.ContainsValue(message)) {

                textInteraction.text = listChatResponse.ElementAt(0).Key;
                barShowBool = true;
                 var item = listChatResponse.FirstOrDefault(kvp => kvp.Value == message);
                listChatResponse.Remove(item.Key);
               }
            countAnswer = 1;
            timeSpeakingCarmen = spk.ApproximateSpeechLength(message);
            SendReplyChela(message);
        }
        
        public void speakChela(string message)
        {
            starMouth = true;
            SpeakingCarmen = false;
            SpeakingChela = true;
            animCarmen.SetBool("Start", false);
            animChela.SetBool("Start", true);

            try
            {
                spk.Speak(message, aud, Speaker.Instance.VoiceForName("Microsoft Elsa Desktop"), pitch: 2f);
            }
            catch (System.Exception)
            {

                spk.Speak("Hola", aud, Speaker.Instance.VoiceForName("Microsoft Elsa Desktop"), pitch: 1f);
            }
            



            timeSpeakingChela = spk.ApproximateSpeechLength(message);

        }
        public async void SendReplyChela(string msg)
        {
            List<ChatMessage> messages = new List<ChatMessage>();
           

            var newMessage = new ChatMessage();
            
            var newmsg = new ChatMessage();
            newmsg.Role = "system";
            newmsg.Content = "Actua como Chela, estas en un programa de television llamado señorita Laura, estas contando a la presentadora Laura tu mala experiencia con tu marido";
            messages.Add(newmsg);


            newMessage.Role = "user";
           


            if(msg!="")
                newMessage.Content = "responde a lo que te dijo Laura:" + msg + " Termina contando algo bizarro que te hizo tu marido" ;
            else
                        newMessage.Content = "inicia contando algo random que tu ex marido te hizo";


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
                
              
                Debug.Log("Respuesta Chela: " +message.Content);
                SendReply(message.Content);
            }




        }
        public async void SendReply(string msg)
        {
            int randomNotice = Random.Range(0,4);
            Debug.Log(randomNotice);
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
            newmsg.Content = "Actua como la presentadora del programa real llamado laura en america, tienes una invitada sentada en el sofa llamda chela, a chela le ha pasado muchas desgracias con su ex marido";
            messages.Add(newmsg);
            ChatMessage newmsg2;

         
            newmsg2.Role = "user";
            newmsg2.Content = "chela dice " + msg;
            messages.Add(newmsg2);
           
           // newmsg.Role = "assistant";
            //newmsg.Content = "Esto es insolito, en españa un tio se lanzo desde un paracaidas";
            messages.Add(newmsg);
         /*   if (listChatInteraction.Count > 0) {               
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
                if(randomNotice ==1)
                newMessage.Content = " Responde a lo que dijo Chela:  " +  msg + " luego Narra 1 noticia creativa chistosa responde de forma chistosa con un toque de humor negro." ;
               else
                    newMessage.Content = "Responde a lo que dijo Chela:  " + msg ;
            }
      */
         
           
           

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

                if (message.Content[0] == 'C' && message.Content[0] == 'a' && message.Content[0] == 'r')
                    message.Content.Remove(0,6);

                if (isChat)
                {
                    listChatResponse.Add(name + ": " + content, message.Content);
                    SendReplyChela(message.Content);
                  /*  string path = "C:/Users/danie/OneDrive/Escritorio/IA.txt";
                    StreamWriter writer = new StreamWriter(path, true);
                    writer.WriteLine(name + ": " + content+" Respuesta " + message.Content);
                    writer.Close();*/

                    timeToAnswer = 3;
                    isChat = false;
                }
                else
                {
                    noticias.Add(message.Content);
                    Debug.Log("Respuesta: " + newMessage.Content);
                    /*  string path = "C:/Users/danie/OneDrive/Escritorio/IA.txt";
                      StreamWriter writer = new StreamWriter(path, true);
                      writer.WriteLine("Noticia: "+ message.Content);
                      writer.Close();*/
                }
               // message.Role = "assistant";
               // messages.Add(message);
              
                replyIsWorking = false;

                
            }


          

        }





            
            

        
    }
}

