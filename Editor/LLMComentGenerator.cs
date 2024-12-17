using UnityEditor;
using UnityEngine;
using System.IO;

public class LLMCommentGenerator : EditorWindow
{
    private string selectedFilePath = "";
    private string generatedCommentedCode = "";
    private Vector2 scrollPosition;

    private OpenAICommentClient apiClient; // OpenAICommentClient 사용
    private GameObject apiClientObject;

    [MenuItem("LLM Tools/Comment Generator")]
    public static void ShowWindow()
    {
        GetWindow<LLMCommentGenerator>("Comment Generator");
    }

     private void OnEnable()
    {
        // OpenAICommentClient 인스턴스화
        apiClientObject = GameObject.Find("OpenAICommentClient");
        if (apiClientObject == null)
        {
            apiClientObject = new GameObject("OpenAICommentClient");
            apiClient = apiClientObject.AddComponent<OpenAICommentClient>();
        }
        else
        {
            apiClient = apiClientObject.GetComponent<OpenAICommentClient>();
        }
    }

    

    private void OnGUI()
    {
        GUILayout.Label("Comment Generator", EditorStyles.boldLabel);

        // 파일 선택 버튼
        if (GUILayout.Button("Select File to Comment"))
        {
            selectedFilePath = EditorUtility.OpenFilePanel("Select C# File", "", "cs");
            if (!string.IsNullOrEmpty(selectedFilePath))
            {
                Debug.Log("Selected File: " + selectedFilePath);
            }
        }

        GUILayout.Space(10);
        GUILayout.Label("Selected File:");
        GUILayout.TextField(selectedFilePath);

        // 주석 생성 버튼
        if (GUILayout.Button("Generate Comments"))
        {

            generatedCommentedCode = "Requesting...";
            if (!string.IsNullOrEmpty(selectedFilePath))
            {
                string fileContent = File.ReadAllText(selectedFilePath);
                apiClient.StartCoroutine(apiClient.SendRequest(fileContent, OnAPISuccess));
            }
            else
            {
                Debug.LogWarning("No file selected!");
            }
        }

        GUILayout.Space(10);

        // 주석이 추가된 코드 표시
        GUILayout.Label("Generated Code with Comments:");
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(300));
        generatedCommentedCode = EditorGUILayout.TextArea(generatedCommentedCode, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();

        // 파일 저장 버튼
        if (GUILayout.Button("Save to GeneratedScripts Folder"))
        {
            SaveToGeneratedScripts();
        }

        // 반영하기 버튼
        if (GUILayout.Button("Apply to Original File"))
        {
            ApplyToOriginalFile();
        }
    }

    private void OnAPISuccess(string commentedCode)
    {
        generatedCommentedCode = ExtractPureCSharpCode(commentedCode);
        Debug.Log("Comments generated successfully!");
    }

    private void SaveToGeneratedScripts()
    {
        if (!string.IsNullOrEmpty(generatedCommentedCode))
        {
            string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"CommentedScript_{timestamp}.cs";
            string savePath = Path.Combine("Assets/GeneratedScripts", fileName);

            Directory.CreateDirectory("Assets/GeneratedScripts");
            File.WriteAllText(savePath, generatedCommentedCode);
            Debug.Log($"File saved to: {savePath}");
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogWarning("No generated code to save!");
        }
    }

    private void ApplyToOriginalFile()
    {
        if (!string.IsNullOrEmpty(selectedFilePath) && !string.IsNullOrEmpty(generatedCommentedCode))
        {
            File.WriteAllText(selectedFilePath, generatedCommentedCode);
            Debug.Log($"File updated: {selectedFilePath}");
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogWarning("No file selected or generated code is empty!");
        }
    }

    private string ExtractPureCSharpCode(string rawCode)
    {
        // 백틱과 "csharp" 언어 태그 제거
        if (string.IsNullOrEmpty(rawCode))
            return "";

        string result = rawCode;

        // 백틱과 언어 태그가 있는 경우 제거
        if (result.StartsWith("```"))
        {
            int start = result.IndexOf('\n') + 1; // 첫 번째 줄 스킵
            int end = result.LastIndexOf("```");

            if (start >= 0 && end > start)
            {
                result = result.Substring(start, end - start).Trim();
            }
        }

        return result;
    }
}
