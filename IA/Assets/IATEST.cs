using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;

public class IATEST : MonoBehaviour
{
    private OpenAIApi openai = new OpenAIApi();


    public async void Create_Chat_Completion()
    {
        var req = new CreateChatCompletionRequest
        {
            Model = "gpt-3.5-turbo",
            Messages = new List<ChatMessage>()
                {
                    new ChatMessage() { Role = "user", Content = "Hello!" }
                }
        };
        var res = await openai.CreateChatCompletion(req);
        Debug.Log(res);
    }
    void Start()
    {
        Create_Chat_Completion();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
