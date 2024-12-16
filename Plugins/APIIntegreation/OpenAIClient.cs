using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class OpenAIClient : MonoBehaviour
{
    private string apiKey = ""; // OpenAI API 키
    
    // 테스트용 mock API URL
    //private static readonly string apiUrl = "http://localhost:5000/v1/chat/completions";

    // 실제 API URL
    private string apiUrl = "https://api.openai.com/v1/chat/completions";

    // API 요청 데이터 구조
    [System.Serializable]
    public class RequestData
    {
        public string model;
        public Message[] messages;
        public int max_tokens;
    }

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class ResponseData
    {
        public Choice[] choices;
    }

    [System.Serializable]
    public class Choice
    {
        public Message message;
    }

    public IEnumerator SendRequest(string prompt, System.Action<string> callback)
    {
        // JSON 데이터 생성
        RequestData requestData = new RequestData
        {
            model = "gpt-4o",
            messages = new[]
            {
                new Message { role = "system", content = "You are an assistant that generates C# code. "
                                                + "Ensure the code is clean, well-commented, and organized into a single C# file." },
                new Message { role = "user", content = prompt }
            },
            max_tokens = 10000
        };

        string jsonRequest = JsonUtility.ToJson(requestData);
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            ResponseData responseData = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);
            string resultContent = responseData.choices[0].message.content;
            callback?.Invoke(resultContent);
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
            callback?.Invoke($"Error: {request.error}");
        }
    }
}
