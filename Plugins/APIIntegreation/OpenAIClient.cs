using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEditor;

public class OpenAIClient : MonoBehaviour
{
    //private string apiKey = ""; // OpenAI API 키

    private static string assetPath = "Assets/Plugins/Settings/LLMSettings.asset";


    private LLMSettings settings;
    private void InitializeSettings()
    {
        settings = AssetDatabase.LoadAssetAtPath<LLMSettings>(assetPath);
        if (settings == null)
        {
            Debug.LogError("LLMSettings asset not found. Please configure it in LLM Tools > Model Configurator.");
        }
    }

    
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

        InitializeSettings();
        if (settings == null)
        {
            Debug.LogError("Settings is null! Make sure LLMSettings.asset exists and is loaded properly.");
            yield break;
        }

        if (string.IsNullOrEmpty(settings.apiKey))
        {
            Debug.LogError("API Key is missing! Configure it in the LLMSettings asset.");
            yield break;
        }

        if (string.IsNullOrEmpty(prompt))
        {
            Debug.LogError("User prompt is null or empty!");
            yield break;
        }

        string apiKey = settings.apiKey; // Asset에서 API Key 가져오기
        string model = settings.selectedModel; // Asset에서 모델 정보 가져오기
        // JSON 데이터 생성
        RequestData requestData = new RequestData
        {
            model = model,
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
