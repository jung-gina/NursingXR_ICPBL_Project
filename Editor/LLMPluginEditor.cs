using UnityEditor;
using UnityEngine;
using System; // DateTime을 사용하기 위해 필요
using System.IO; // 파일 저장 관련 클래스
using System.Text.RegularExpressions;

public class LLMPluginEditor : EditorWindow
{
    private string prompt = "예) 간호사가 청진기를 드는 C# 코드를 작성해주세요.";
    //private string result = "LLM API 코드 생성 결과가 여기에 표시됩니다.";
    private OpenAIClient apiClient;

    [MenuItem("LLM Tools/Code Generator")]
    public static void ShowWindow()
    {
        GetWindow<LLMPluginEditor>("LLM Code Generator");
    }

    private void OnEnable()
    {
        // OpenAIClient 인스턴스 찾기
        GameObject clientObject = GameObject.Find("OpenAIClient");
        if (clientObject == null)
        {
            clientObject = new GameObject("OpenAIClient");
            clientObject.AddComponent<OpenAIClient>();
        }
        apiClient = clientObject.GetComponent<OpenAIClient>();
    }

    private Vector2 scrollPositionText; // Generated Text 스크롤 위치
    private Vector2 scrollPositionCode; // Generated Code 스크롤 위치

    private void OnGUI()
    {
        GUILayout.Label("OpenAI Code Generator", EditorStyles.boldLabel);

        // Prompt 입력 필드
        GUILayout.Label("Prompt:");
        prompt = EditorGUILayout.TextArea(prompt, GUILayout.Height(100));

        // API 요청 버튼
        if (GUILayout.Button("Generate Code"))
        {
            generatedText = "Requesting...";
            apiClient.StartCoroutine(apiClient.SendRequest(prompt, OnAPISuccess));
        }

        // 결과 표시 (Generated Text)
        GUILayout.Label("Generated Text", EditorStyles.boldLabel);
        scrollPositionText = EditorGUILayout.BeginScrollView(scrollPositionText, GUILayout.Height(150));
        generatedText = EditorGUILayout.TextArea(generatedText, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();

        // 결과 표시 (Generated Code)
        GUILayout.Label("Generated Code", EditorStyles.boldLabel);
        scrollPositionCode = EditorGUILayout.BeginScrollView(scrollPositionCode, GUILayout.Height(200));
        generatedCode = EditorGUILayout.TextArea(generatedCode, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();
    }
    

    private string generatedCode = "";
    private string generatedText = "";


     // API 응답에서 Text와 Code를 분리
    private void ExtractTextAndCode(string response)
    {
        // 정규식을 사용하여 백틱(```) 안의 코드 추출
        var codeMatch = Regex.Match(response, @"```[a-zA-Z]*\s*(.*?)```", RegexOptions.Singleline);

        if (codeMatch.Success)
        {
            generatedCode = codeMatch.Groups[1].Value; // 코드 부분 추출
            generatedText = response.Replace(codeMatch.Value, "").Trim(); // 나머지 텍스트
        }
        else
        {
            generatedText = response; // 코드가 없으면 전체를 텍스트로
            generatedCode = "// No code found in response";
        }
    }

    private void OnAPISuccess(string apiResponse)
    {
        // 응답을 JSON 형식으로 나눔
        ExtractTextAndCode(apiResponse);

        // 현재 날짜와 시간을 가져와서 파일 이름에 포함
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss"); // 형식: YYYYMMDD_HHMMSS
        string jsonFileName = $"GeneratedResponse_{timestamp}.json";
        string codeFileName = $"GeneratedCode_{timestamp}.cs";

        string folderPath = "Assets/GeneratedScripts/";
        Directory.CreateDirectory(folderPath); // 폴더가 없으면 생성

        // JSON 데이터 생성
        var jsonData = new
        {
            text = generatedText,
            code = generatedCode
        };
        // JSON 형식으로 저장
        string jsonFilePath = Path.Combine(folderPath, jsonFileName);
        File.WriteAllText(jsonFilePath, JsonUtility.ToJson(jsonData, true));

        // 코드 파일 저장
        string codeFilePath = Path.Combine(folderPath, codeFileName);
        File.WriteAllText(codeFilePath, generatedCode);

        // Unity 에디터에 갱신
        Debug.Log($"JSON Saved: {jsonFilePath}");
        Debug.Log($"Code Saved: {codeFilePath}");
        AssetDatabase.Refresh();
    }
}
