using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Crosstales.RTVoice;
using System.Linq;
using TMPro;

namespace OpenAI
{
    public class ChatGPTMillonario : MonoBehaviour
    {
     
        public GameObject panelSubtitles;
        public TextMeshProUGUI subtitles;

        public Animator animMen;
        public Animator animWomen;

        public SkinnedMeshRenderer skinCharly;
        public SkinnedMeshRenderer skinSara;
        public SkinnedMeshRenderer skinChela;   
        public SkinnedMeshRenderer skinCarmen;
        private OpenAIApi openai = new OpenAIApi();
        public AudioSource audCharly;
        public AudioSource audSara;
        public AudioSource audChela;
        public AudioSource audCarmen;



        List<ChatMessage> messagesCharly = new List<ChatMessage>();
        List<ChatMessage> messagesSara = new List<ChatMessage>();
        List<ChatMessage> messagesChela = new List<ChatMessage>();
        List<ChatMessage> messagesCarmen = new List<ChatMessage>();

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

        public float speedEyes = 0;
        public float speedMouth = 0;
  
        float timeNextSpeak = 8;

        public Light campFire;
        bool campMaxIntensi;
        bool replyIsWorking;
        public float speedCampfire=10f;
        private void Start()
        {
            panelSubtitles.SetActive(false);
            SendReplyCharly("");
         
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
            skinCharly.SetBlendShapeWeight(16, weightEyes);
            skinCharly.SetBlendShapeWeight(17, weightEyes);

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
            skinCharly.SetBlendShapeWeight(9, weightMouth);
          

            skinSara.SetBlendShapeWeight(0, weightMouth);
            skinChela.SetBlendShapeWeight(4, weightMouth);
            skinCarmen.SetBlendShapeWeight(15, weightMouth);


           
           
        }
        private void Update()
        {

            campFire.intensity += speedCampfire * Time.deltaTime;
            campFire.range += speedCampfire * Time.deltaTime;
            if (campFire.intensity > 2.5f) {
                speedCampfire = -speedCampfire;
                

            }
            if(campFire.intensity < 2f) speedCampfire = Mathf.Abs( speedCampfire);


            updateBlendShapes();
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
            if (timeNextSpeak > 0)
                timeNextSpeak += -Time.deltaTime;

            if (dialogos.Count > 0 && timeNextSpeak <=0) speak();

        }


        public void speak() {
            if (contDialogos == 1) {
                spk.Speak(dialogos.ElementAt(0), audCharly, Speaker.Instance.VoiceForName("Microsoft David Desktop"), pitch: 1.4f);
                timeNextSpeak= spk.ApproximateSpeechLength(dialogos.ElementAt(0));
                starMouth = true;

                panelSubtitles.SetActive(true);
                subtitles.text = dialogos.ElementAt(0);
                cameras[0].SetActive(false);
                cameras[1].SetActive(true);
                cameras[2].SetActive(false);
                cameras[3].SetActive(false);
                cameras[4].SetActive(false);
               
    
    }
            if (contDialogos == 2)
            {
                spk.Speak(dialogos.ElementAt(0), audSara, Speaker.Instance.VoiceForName("Microsoft Helena Desktop"), pitch: 1.4f);
                timeNextSpeak = spk.ApproximateSpeechLength(dialogos.ElementAt(0));
                starMouth = true;
                panelSubtitles.SetActive(false);
                cameras[0].SetActive(false);
                cameras[1].SetActive(false);
                cameras[2].SetActive(true);
                cameras[3].SetActive(false);
                cameras[4].SetActive(false);
            }
            if (contDialogos == 3)
            {
                spk.Speak(dialogos.ElementAt(0), audChela, Speaker.Instance.VoiceForName("Microsoft Elsa Desktop"), pitch: 1.4f);
                timeNextSpeak = spk.ApproximateSpeechLength(dialogos.ElementAt(0));
                starMouth = true;
                cameras[0].SetActive(false);
                cameras[1].SetActive(false);
                cameras[2].SetActive(false);
                cameras[3].SetActive(true);
                cameras[4].SetActive(false);
            }
            if (contDialogos == 4)
            {
                spk.Speak(dialogos.ElementAt(0), audCarmen, Speaker.Instance.VoiceForName("Microsoft Sabina Desktop"), pitch: 1.4f);
                timeNextSpeak = spk.ApproximateSpeechLength(dialogos.ElementAt(0));
                starMouth = true;
                cameras[0].SetActive(false);
                cameras[1].SetActive(false);
                cameras[2].SetActive(false);
                cameras[3].SetActive(false);
                cameras[4].SetActive(true);
            }

            dialogos.Remove(dialogos.ElementAt(0));

            contDialogos++;
            if (contDialogos == 5) contDialogos = 1;

        }
     
        public async void SendReplyCharly(string msg)
        {




          
           
            ChatMessage newmsg1;
            if (msg == "")
            {
               
                newmsg1.Role = "system";
                newmsg1.Content = "Eres Charly,tu personalidad es demasiado positiva, siempre estas feliz y eres un poco estupido. estas en un capamento pasando tus vacaciones, en este momento te encuentras reunido con un grupo de 4 personas llamadas Carmen,Sara, Chela y Chela,estan alrededor de una fogata y debatiando ideas y opiniones" +
                    "de drama, y a ti te toca empezar el tema, inicia el tema para que los demas puedan hablar sobre eso, el tema debe ser gracioso y dar risa,incluye un toque de humor negro esta enfocado para un publico joven.En maximo 2 renglones inicia un dialogo para que otra persona responda. Tu dialogo va dirijo a Sara. No inicies el dialogo etiquetando mi nombre";

            }
            else {

                newmsg1.Role = "user";
                newmsg1.Content = "Responde a lo que dijo Carmen: " + msg;
            }
                
            //messages.Add(newmsg);
                 //newMessage.Content = "Realiza una pregunta se selccion multiple, dame 4 posiblesa respuestas y 1 de esas respuestas es verdadera ";
                 //listChatInteraction.ElementAt(0).Value + ",responde con humor negro e interactua. Resume tu respuesta en Maximo 100 palabras";


               // listChatInteraction.Remove(listChatInteraction.ElementAt(0).Key);

      


            messagesCharly.Add(newmsg1);



            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = messagesCharly
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                //message.Content = message.Content.Substring(0, 50);


                
                Debug.Log("RespuestaCHARLY: " + message.Content);
                dialogos.Add(message.Content);
             
                SendReplySara(message.Content);
               
            }
            


         }
        public async void SendReplySara(string msg)
        {




            var newMessage = new ChatMessage();
            newMessage.Role = "user";


            ChatMessage newmsg1;
            newmsg1.Role = "system";
            newmsg1.Content = "Eres Sara,tu personalidad es deprimente, siempre estas aburrida, no generas emociones y la vida te vale verga, estas en un capamento pasando tus vacaciones, en este momento te encuentras reunido con un grupo de 4 personas llamadas Carmen,Charly Chela,estan alrededor de una fogata y debatiando ideas y opiniones" +
                "de drama, tienes que responder en maximo 2 renglones a lo que dijo la persona anterior a ti, incluye un toque de humor negro en tu respuesta. Tu dialogo va dirijo a Chela. No inicies el dialogo etiquetando mi nombre";
            
            ChatMessage newmsg;
            newmsg.Role = "user";
            newmsg.Content = " responde a Charly dijo: " + msg ;
            //messages.Add(newmsg);
            //newMessage.Content = "Realiza una pregunta se selccion multiple, dame 4 posiblesa respuestas y 1 de esas respuestas es verdadera ";
            //listChatInteraction.ElementAt(0).Value + ",responde con humor negro e interactua. Resume tu respuesta en Maximo 100 palabras";


            // listChatInteraction.Remove(listChatInteraction.ElementAt(0).Key);

     

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
                SendReplyChela(message.Content);
            }



        }
        public async void SendReplyChela(string msg)
        {




            var newMessage = new ChatMessage();
            newMessage.Role = "user";


            ChatMessage newmsg1;
            newmsg1.Role = "system";
            newmsg1.Content = "Eres Chela, tu personalidad es egocentrica, siempre piensas saber todo, eres hija de papi y mami, odias los mosquitos y todo lo que te perturbe. estas en un capamento pasando tus vacaciones, en este momento te encuentras reunido con un grupo de 3 personas llamadas Carmen,Charly Sara,estan alrededor de una fogata y debatiando ideas y opiniones" +
                "de drama, tienes que responder  en maximo 2 reglones a lo que dijo la persona anterior a ti, incluye un toque de humor negro en tu respuesta. No inicies el dialogo etiquetando mi nombre";

            ChatMessage newmsg;
            newmsg.Role = "user";
            newmsg.Content = "Sara dijo: " + msg;
            //messages.Add(newmsg);
            //newMessage.Content = "Realiza una pregunta se selccion multiple, dame 4 posiblesa respuestas y 1 de esas respuestas es verdadera ";
            //listChatInteraction.ElementAt(0).Value + ",responde con humor negro e interactua. Resume tu respuesta en Maximo 100 palabras";


            // listChatInteraction.Remove(listChatInteraction.ElementAt(0).Key);

           
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
                SendReplyCarmen(message.Content);
            }



        }
        public async void SendReplyCarmen(string msg)
        {




            var newMessage = new ChatMessage();
            newMessage.Role = "user";


            ChatMessage newmsg1;
            newmsg1.Role = "system";
            newmsg1.Content = "Eres Carmen, tu personalidad es egocentrica y grotesca, siempre estas enfadada. estas en un capamento pasando tus vacaciones, en este momento te encuentras reunido con un grupo de 3 personas llamadas Chela,Charly Sara,estan alrededor de una fogata y debatiando ideas y opiniones" +
                "de drama. en 2 renglones tienes que responder a lo que dijo la persona anterior a ti, incluye un toque de humor negro en tu respuesta. Tu dialogo va dirijo a charly. No inicies el dialogo etiquetando mi nombre";

            ChatMessage newmsg;
            newmsg.Role = "user";
            newmsg.Content = "Chela dijo: " + msg;
            //messages.Add(newmsg);
            //newMessage.Content = "Realiza una pregunta se selccion multiple, dame 4 posiblesa respuestas y 1 de esas respuestas es verdadera ";
            //listChatInteraction.ElementAt(0).Value + ",responde con humor negro e interactua. Resume tu respuesta en Maximo 100 palabras";


            // listChatInteraction.Remove(listChatInteraction.ElementAt(0).Key);


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
                SendReplyCharly(message.Content);

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




            
    }
    
}

