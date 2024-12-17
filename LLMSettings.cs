using UnityEngine;

[CreateAssetMenu(fileName = "LLMSettings", menuName = "LLM Tools/Settings")]
public class LLMSettings : ScriptableObject
{
    public string apiKey = "API key here";
    public string selectedModel = "gpt-4o";
}
