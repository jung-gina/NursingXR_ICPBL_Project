using UnityEditor;
using UnityEngine;

public class ModelConfigurator : EditorWindow
{
    private static LLMSettings settings;
    private string[] models = { "gpt-3.5-turbo", "gpt-4o", "gpt-4o-mini" };
    private bool[] toggleStates;

    [MenuItem("LLM Tools/Model Configurator")]
    public static void ShowWindow()
    {
        GetWindow<ModelConfigurator>("Model Configurator");
    }

    private void OnEnable()
    {
        // 설정 파일 로드
        settings = AssetDatabase.LoadAssetAtPath<LLMSettings>("Assets/Plugins/Settings/LLMSettings.asset");

        if (settings == null)
        {
            settings = CreateInstance<LLMSettings>();
            AssetDatabase.CreateAsset(settings, "Assets/Plugins/Settings/LLMSettings.asset");
            AssetDatabase.SaveAssets();
        }

        // 토글 상태 초기화
        toggleStates = new bool[models.Length];
        for (int i = 0; i < models.Length; i++)
        {
            toggleStates[i] = models[i] == settings.selectedModel;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Model Configuration", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // API Key 설정
        GUILayout.Label("OpenAI API Key:");
        settings.apiKey = EditorGUILayout.TextField(settings.apiKey);

        GUILayout.Space(10);

        // 모델 선택 (토글 버튼)
        GUILayout.Label("Select GPT Model:");
        for (int i = 0; i < models.Length; i++)
        {
            bool newToggleState = EditorGUILayout.ToggleLeft(models[i], toggleStates[i]);
            if (newToggleState)
            {
                for (int j = 0; j < models.Length; j++)
                {
                    toggleStates[j] = (i == j);
                }
                settings.selectedModel = models[i];
            }
        }

        GUILayout.Space(10);

        // 저장 버튼
        if (GUILayout.Button("Save Configuration"))
        {
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
            Debug.Log("Configuration Saved!");
        }

        // 현재 설정 표시
        GUILayout.Space(10);
        GUILayout.Label($"Current API Key: {HideApiKey(settings.apiKey)}", EditorStyles.helpBox);
        GUILayout.Label($"Current Selected Model: {settings.selectedModel}", EditorStyles.helpBox);
    }

    private string HideApiKey(string key)
    {
        if (string.IsNullOrEmpty(key)) return "Not Set";
        return key.Substring(0, Mathf.Min(4, key.Length)) + "****";
    }
}
