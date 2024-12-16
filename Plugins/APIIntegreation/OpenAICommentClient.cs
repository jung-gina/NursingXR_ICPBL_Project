using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


public class OpenAICommentClient : MonoBehaviour
{

    private string apiUrl = "https://api.openai.com/v1/chat/completions";
    private string apiKey = "";
    private LLMSettings settings;
    private void Awake()
    {
        settings = Resources.Load<LLMSettings>("LLMSettings");
        if (settings == null)
        {
            Debug.LogError("LLMSettings asset not found. Please configure it in LLM Tools > Model Configurator.");
        }
    }

    // API 요청 및 응답 데이터 구조
    [Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    [Serializable]
    public class RequestData
    {
        public string model;
        public Message[] messages;
        public int max_tokens;
    }

    [Serializable]
    public class ResponseData
    {
        public Choice[] choices;
    }

    [Serializable]
    public class Choice
    {
        public Message message;
    }

    // OpenAI API 호출
    public IEnumerator SendRequest(string userPrompt, Action<string> callback)
    {

        
        // System 프롬프트: 주석을 추가하도록 작업 명시
        string systemPrompt = "You are an assistant that adds comments to existing C# code. "
                            + "Your job is to add clear and useful comments to the code without modifying the original structure. "
                            + "Return the updated code with only the comments added.";

        // 요청 데이터 구성
        RequestData requestData = new RequestData
        {
            model = "gpt-4o", // OpenAI 모델 설정
            messages = new[]
            {
                new Message { role = "system", content = systemPrompt },
                new Message { role = "user", content = userPrompt }
            },
            max_tokens = 1000
        };

        string jsonRequest = JsonUtility.ToJson(requestData);

        // HTTP 요청 생성
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        // 요청 전송
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            ResponseData response = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);
            string resultContent = response.choices[0].message.content;
            callback?.Invoke(resultContent);
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
            callback?.Invoke($"Error: {request.error}");
        }
    }
}
