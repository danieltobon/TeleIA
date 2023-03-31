using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Crosstales.RTVoice;
using System.Linq;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Threading;

namespace OpenAI
{
    public class ChatGPTChatGPTWithDalle : MonoBehaviour
    {

        public Animator anim;
        public Animator animUI;
        public float speedFillImage;
        public SkinnedMeshRenderer skin;
        Dictionary<string, Sprite[]> noticias = new Dictionary<string, Sprite[]>();
        public AudioSource aud;
        public string VoiceName;
      

        private OpenAIApi openai = new OpenAIApi();

        private List<ChatMessage> messages = new List<ChatMessage>();
        private List<ChatMessage> messagesDalle = new List<ChatMessage>();

        public List<string> promps;
          


        bool start;

        public Speaker spk;

        public Sprite[] Sp = new Sprite[3];
        public string noticiaTemp = "";

        public Image image;
        public Image imageBackGround;
        public Sprite spriteDefault;
        float currenTimeImage = 0;
        public float maxTimeImage = 0;
        int i = 0;
        

        public Sprite[] listSp = new Sprite[3];
        string messageNotice;
        int j = 0;
        bool fillImageNotice;


        float weightEyes = 0;
        float weightMouth = 0;
       
        float timeEyes;
        bool closedEyes;

        bool closedMouth;
        bool starMouth;

        public float speedEyes = 0;
        public float speedMouth = 0;
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

            updateBlendShapes();
                if (!start && noticias.Count > 0)
            {

                speak();
                start = true;
            }

            if (Sp[2] !=null)
            {

                currenTimeImage += Time.deltaTime;
                if (i < Sp.Length && currenTimeImage > maxTimeImage)
                {
                    image.sprite = Sp[i];
                 
                    i++;
                    currenTimeImage = 0;
                   
                }
                if (i == 2 && currenTimeImage >= maxTimeImage - 5)
                {
                    /*animUI.SetBool("Open", false);
                    animUI.SetBool("Closed", true); */
                    fillImageNotice = false;

                }

            }

            if (listSp[2]) {
                noticias.Add(messageNotice, listSp.ToArray());
                messageNotice = "";
              
                if (noticias.Count < 10) SendReply();
                listSp[0] = null;
                listSp[1] = null;
                listSp[2] = null;
            }

            if (fillImageNotice) {
                imageBackGround.fillAmount += speedFillImage * Time.deltaTime;
                image.fillAmount += speedFillImage * Time.deltaTime;
            }
            else
            {
                imageBackGround.fillAmount -= speedFillImage * Time.deltaTime;
                image.fillAmount -= speedFillImage * Time.deltaTime;
            }

          
        }
        public void speak()
        {

            starMouth = true;
            anim.SetBool("Start", true);
            fillImageNotice = true;
            int random = Random.Range(0, noticias.Count);
            if (noticias.ElementAt(random).Key != null)
            {
                noticiaTemp = noticias.ElementAt(random).Key;

                Sp[0] = noticias.FirstOrDefault(t => t.Key == noticiaTemp).Value[0];
                Sp[1] = noticias.FirstOrDefault(t => t.Key == noticiaTemp).Value[1];
                Sp[2] = noticias.FirstOrDefault(t => t.Key == noticiaTemp).Value[2];
               
                noticias.Remove(noticiaTemp);

                spk.Speak(noticiaTemp, aud, Speaker.Instance.VoiceForName("Microsoft Sabina Desktop"));
                maxTimeImage = spk.ApproximateSpeechLength(noticiaTemp) / 3;
                image.sprite = Sp[0];
                i = 1;
                j++;
                currenTimeImage = 0;
                
            }
            else
            {
                speak();
            }
            

        }
      

        public async void SendReply()
        {

            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = promps[Random.Range(0,promps.Count)]
                
            };
            Debug.Log("Solicitud: " +newMessage.Content);
           

            messages.Add(newMessage);


            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = messages
            });
            
            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();


              
                Debug.Log("Respuesta   " + message.Content);
                messageNotice = message.Content;

                listSp[0] = spriteDefault;
                listSp[1] = spriteDefault;
                listSp[2] = spriteDefault;


                // SendImageRequest(message);

            }

           
           

        }




        public async void SendImageRequest(ChatMessage prompt)
        {
            var newMessageDalle = new ChatMessage()
            {
                Role = "user",
                Content = "Dame 3 ideas un poco similares de imagenes sobre el siguiente texto para generar una imagen Dalle, enumera de la siguiente forma 1. 2. 3. TEXTO:  " + prompt.Content
            };
          

            messagesDalle.Add(newMessageDalle);

            var completionResponse2 = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = messagesDalle
            });
            var messageDal = completionResponse2.Choices[0].Message;
            
          
            

            string temp0 = messageDal.Content.Substring(messageDal.Content.IndexOf("1.")+2, 
                (messageDal.Content.IndexOf("2.")-2)-( messageDal.Content.IndexOf("1.") + 2));

            string temp1 = messageDal.Content.Substring(messageDal.Content.IndexOf("2.") + 2,
                (messageDal.Content.IndexOf("3.") - 2) - (messageDal.Content.IndexOf("2.") + 2));

            string temp2 = messageDal.Content.Substring(messageDal.Content.IndexOf("3."),
                (messageDal.Content.Length)-(messageDal.Content.IndexOf("3.") ) );
          

            string[] stringTemp = { temp0, temp1, temp2 };

            for (int i = 0; i < 3; i++)
            {
               
                var response = await openai.CreateImage(new CreateImageRequest
                {
                    Prompt = stringTemp[i],
                Size = ImageSize.Size256
                });
                if (response.Data != null && response.Data.Count > 0)
                {
                    using (var request = new UnityWebRequest(response.Data[0].Url))
                    {

                        request.downloadHandler = new DownloadHandlerBuffer();
                        request.SetRequestHeader("Access-Control-Allow-Origin", "*");
                        request.SendWebRequest();

                        while (!request.isDone) await Task.Yield();

                        Texture2D texture = new Texture2D(2, 2);
                        texture.LoadImage(request.downloadHandler.data);
                        var sprite = Sprite.Create(texture, new Rect(0, 0, 256, 256), Vector2.zero, 1f);
                        messagesDalle.Clear();
                        sprite.name = "sprite" + i;
                        listSp[i] = sprite;
                       
                    }

                }
                else
                {
                    listSp[i] = spriteDefault;
                    Debug.LogWarning("No image was created from this prompt.");
                }

            }
           

            
            

        }
    }
}

