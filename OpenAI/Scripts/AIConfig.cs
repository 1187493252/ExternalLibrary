using UnityEngine;

[CreateAssetMenu(fileName = "AIConfig", menuName = "AIConfig")]
public class AIConfig : ScriptableObject
{
    public AIType AIType;
    public string API_URL;
    public string API_KEY;
    public string Model;
}
public enum AIType
{
    DeepSeek,
    Qwen,
    Volces
}