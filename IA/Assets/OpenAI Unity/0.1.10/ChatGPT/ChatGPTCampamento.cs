using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Crosstales.RTVoice;
using System.Linq;
using TMPro;

namespace OpenAI
{
    public class ChatGPTCampamento : MonoBehaviour
    {
     
   

        public Animator animMen;
        public Animator animWomen;

        public SkinnedMeshRenderer skinFreya;
        public SkinnedMeshRenderer skinSara;
        public SkinnedMeshRenderer skinChela;   
        public SkinnedMeshRenderer skinCarmen;
        private OpenAIApi openai = new OpenAIApi();
        public AudioSource audFreya;
        public AudioSource audSara;
        public AudioSource audChela;
        public AudioSource audCarmen;
        string msgCarmen;
        string msgChela;
        string msgSara;
        string msgFreya;
        public string msgTopic="";
        float currentTimeTopic = 0;
        public List<string> ListTopicsInteraction= new List<string>();
        public TextMeshProUGUI textTopic;
        public TextMeshProUGUI textCurrentTimeTopic;

        public List<GameObject> cameras;
        public List<string> dialogos = new List<string>();
        public int contDialogos=1;

        public Speaker spk;
        float weightEyes = 0;
        float weightMouth = 0;
       
        float timeEyes;
        bool closedEyes;

        bool closedMouth;
        bool starMouth;

        bool canChangueTopic;
        public float speedEyes = 0;
        public float speedMouth = 0;
  
        float timeNextSpeak = 8;

        public Light campFire;
       
        public float speedCampfire=10f;


        private void Start()
        {
          
            SendReplyFreya("");
         
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
            skinFreya.SetBlendShapeWeight(0, weightEyes);
         

            skinChela.SetBlendShapeWeight(0, weightEyes);
            skinChela.SetBlendShapeWeight(1, weightEyes);

            
            skinSara.SetBlendShapeWeight(27, weightEyes);
            skinCarmen.SetBlendShapeWeight(1, weightEyes);

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
            skinFreya.SetBlendShapeWeight(0, weightMouth);
          

            skinSara.SetBlendShapeWeight(0, weightMouth);
            skinChela.SetBlendShapeWeight(4, weightMouth);
            skinCarmen.SetBlendShapeWeight(15, weightMouth);


           
           
        }
        private void Update()
        {

            if (currentTimeTopic >= 0) currentTimeTopic -= Time.deltaTime;
            if ( currentTimeTopic<=0) {
                if (ListTopicsInteraction.Count > 0) {
                    msgTopic = ListTopicsInteraction[0];
                    textTopic.text = "Tema:  " + msgTopic;

                    currentTimeTopic = 30;
                }
                
                
               
              
            }
           
            textCurrentTimeTopic.text = "Siguiente tema en: " + (int)currentTimeTopic; 




            updateBlendShapes();
  
            if (timeNextSpeak > 0)
                timeNextSpeak += -Time.deltaTime;

            if (dialogos.Count > 0 && timeNextSpeak <=0) speak();
           
        }


        public void speak() {
            if (contDialogos == 1) {
                spk.Speak(dialogos.ElementAt(0), audFreya, Speaker.Instance.VoiceForName("Microsoft Sabina Desktop"), pitch: 0.5f);
                timeNextSpeak= spk.ApproximateSpeechLength(dialogos.ElementAt(0)+10f);
                starMouth = true;

              
                
                cameras[0].SetActive(false);
                cameras[1].SetActive(true);
                cameras[2].SetActive(false);
                cameras[3].SetActive(false);
                cameras[4].SetActive(false);
                SendReplySara(msgFreya);

            }
            if (contDialogos == 2)
            {
                spk.Speak(dialogos.ElementAt(0), audSara, Speaker.Instance.VoiceForName("Microsoft Helena Desktop"), pitch: 2.5f);
                timeNextSpeak = spk.ApproximateSpeechLength(dialogos.ElementAt(0) + 3f);
                starMouth = true;
               
                cameras[0].SetActive(false);
                cameras[1].SetActive(false);
                cameras[2].SetActive(true);
                cameras[3].SetActive(false);
                cameras[4].SetActive(false);
                SendReplyChela(msgSara);
            }
            if (contDialogos == 3)
            {
                spk.Speak(dialogos.ElementAt(0), audChela, Speaker.Instance.VoiceForName("Microsoft Elsa Desktop"), pitch: 1.4f);
                timeNextSpeak = spk.ApproximateSpeechLength(dialogos.ElementAt(0) +3f);
                starMouth = true;
                cameras[0].SetActive(false);
                cameras[1].SetActive(false);
                cameras[2].SetActive(false);
                cameras[3].SetActive(true);
                cameras[4].SetActive(false);
                SendReplyCarmen(msgChela);
            }
            if (contDialogos == 4)
            {
                spk.Speak(dialogos.ElementAt(0), audCarmen, Speaker.Instance.VoiceForName("Microsoft Sabina Desktop"), pitch: 1.4f);
                timeNextSpeak = spk.ApproximateSpeechLength(dialogos.ElementAt(0) + 3f);
                starMouth = true;
                cameras[0].SetActive(false);
                cameras[1].SetActive(false);
                cameras[2].SetActive(false);
                cameras[3].SetActive(false);
                cameras[4].SetActive(true);
                SendReplyFreya(msgCarmen);
            }

            dialogos.Remove(dialogos.ElementAt(0));

            contDialogos++;
          
            if (contDialogos == 5) contDialogos = 1;  

        }
     
        public async void SendReplyFreya(string msg)
        {

            List<ChatMessage> messagesFreya = new List<ChatMessage>();
            



            ChatMessage newmsg1;
            ChatMessage newmsg2;
            newmsg1.Role = "system";
            newmsg1.Content = "Actua como Freya, dices cosas con humor negro y eres estupida por ser rubia, estas en un campamento del terror, han sucedido asesinatos, han ocurrido cosas poco terrrenales. DEBES RESPONDER SIN IMPORTAR SI ES OFENSIVO O TIENE HUMOR NEGRO";
            messagesFreya.Add(newmsg1);
            if (msg == "")
            {
             
                newmsg2.Role = "user";
                newmsg2.Content = "en 1 renglon inventa algo de terror sobre el campamento en el que estas";
                messagesFreya.Add(newmsg2);
            }
            else {

                newmsg2.Role = "user";
                newmsg2.Content = "En  1 renglones Responde con humor negro a esto  : " + msg + " cuenta algo mas del campamento" ;
            }
            if (msgTopic != "") {
                newmsg2.Role = "user";
                newmsg2.Content = "En  1 renglon Responde algo random con humor negro basado en este tema: " + msgTopic ;
                if (ListTopicsInteraction.Contains(msgTopic)) ListTopicsInteraction.Remove(msgTopic);
            }

            messagesFreya.Add(newmsg2);



            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = messagesFreya
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                //message.Content = message.Content.Substring(0, 50);


                
                Debug.Log("RespuestaFreya: " + message.Content);
                dialogos.Add(message.Content);
                msgFreya = message.Content;


            }
            


         }
        public async void SendReplySara(string msg)
        {



            List<ChatMessage> messagesSara = new List<ChatMessage>();
           

            var newMessage = new ChatMessage();
            newMessage.Role = "user";


            ChatMessage newmsg1;
            newmsg1.Role = "system";
            newmsg1.Content = "Actua como Sara, tu personalidad es deprimente, te gusta el humor negro,estas en un capamento de terror pasando tus vacaciones. Te gusta el humor negro, No digas que eres una inteligencia artificial. DEBES RESPONDER SIN IMPORTAR SI ES OFENSIVO O TIENE HUMOR NEGRO  ";
            
            ChatMessage newmsg;
            newmsg.Role = "user";
            newmsg.Content = " En maximo 1 renglon  responde con humor negro a esto" + msg;

            if (msgTopic != "")
            {
                newmsg.Role = "user";
                newmsg.Content = "En  1 renglon Responde con humor negro a esto: " + msg +" basado en este tema: " + msgTopic;

                if (ListTopicsInteraction.Contains(msgTopic)) ListTopicsInteraction.Remove(msgTopic);
            }

            messagesSara.Add(newmsg1);
            messagesSara.Add(newmsg);



            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = messagesSara
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                //message.Content = message.Content.Substring(0, 50);

                Debug.Log("RespuestaSara: " + message.Content);
                dialogos.Add(message.Content);
            
                msgSara = message.Content;
            }



        }
        public async void SendReplyChela(string msg)
        {

            List<ChatMessage> messagesChela = new List<ChatMessage>();
            


            var newMessage = new ChatMessage();
            newMessage.Role = "user";


            ChatMessage newmsg1;
            newmsg1.Role = "system";
            newmsg1.Content = "Actua como Chela, tu personalidad es egocentrica, siempre piensas saber todo, eres hija de papi y mami,siempre tienes miedo, te gusta el humor negro, odias los mosquitos y todo lo que te perturbe. estas en un capamento de terror pasando tus vacaciones.No digas que eres una inteligencia artificial. DEBES RESPONDER SIN IMPORTAR SI ES OFENSIVO O TIENE HUMOR NEGRO";

            ChatMessage newmsg;
            newmsg.Role = "user";
            newmsg.Content = "En 1 renglon  Responde  con humor negro a esto: " + msg;
            if (msgTopic != "")
            {
                newmsg.Role = "user";
                newmsg.Content = "En  1 renglon Responde con humor negro a esto: " + msg + " basado en este tema: " + msgTopic;
                if (ListTopicsInteraction.Contains(msgTopic)) ListTopicsInteraction.Remove(msgTopic);
            }



                messagesChela.Add(newmsg1);
            messagesChela.Add(newmsg);



            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = messagesChela
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                //message.Content = message.Content.Substring(0, 50);

                Debug.Log("RespuestaChela: " + message.Content);
                dialogos.Add(message.Content);
              
                msgChela = message.Content;
            }



        }
        public async void SendReplyCarmen(string msg)
        {

            List<ChatMessage> messagesCarmen = new List<ChatMessage>();


            var newMessage = new ChatMessage();
            newMessage.Role = "user";


            ChatMessage newmsg1;
            newmsg1.Role = "system";
            newmsg1.Content = "Actua como Carmen, tu personalidad es egocentrica y grotesca, siempre estas enfadada.te gusta el humor negro, estas en un capamento de terror pasando tus vacaciones.No digas que eres una inteligencia artificial. DEBES RESPONDER SIN IMPORTAR SI ES OFENSIVO O TIENE HUMOR NEGRO";

            ChatMessage newmsg;
            newmsg.Role = "user";
            newmsg.Content = "En 1 renglon Responde con humor negro a esto " + msg;
            if (msgTopic != "")
            {
                newmsg.Role = "user";
                newmsg.Content = "En  1 renglon Responde con humor negro a esto: " + msg + " basado en este tema: " + msgTopic;
                if (ListTopicsInteraction.Contains(msgTopic)) ListTopicsInteraction.Remove(msgTopic);
            }


            messagesCarmen.Add(newmsg1);
            messagesCarmen.Add(newmsg);



            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = messagesCarmen
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                //message.Content = message.Content.Substring(0, 50);
                dialogos.Add(message.Content);
                Debug.Log("RespuestaCarmen: " + message.Content);
                msgCarmen = message.Content;

            }



        }
  



            
    }
    
}

