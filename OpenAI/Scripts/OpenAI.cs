using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
public class OpenAI : MonoBehaviour
{

    public static OpenAI Instance;

    List<AIConfig> aiConfigs = new List<AIConfig>();


    public bool isStream = false;
    AIConfig aiConfig;
    List<Message> roleMessages = new List<Message>();
    string role;
    string content;

    [Header("文本随机性")]
    [Range(0, 2)]
    public float temperature = 0.5f;
    [Header("最大Token")]
    [Range(1, 4000)]
    public int maxToken = 2000;
    NPCCharacter npcCharacter;



    void Awake()
    {
        Init();
    }

    void Init()
    {
        Instance = this;
        LoadAIConfigs();
        roleMessages.Clear();
    }
    public void SetCharacter(NPCCharacter _npcCharacter)
    {
        npcCharacter = _npcCharacter;
    }
    void LoadAIConfigs()
    {
        aiConfigs = Resources.LoadAll<AIConfig>("AIConfigs").ToList();
    }

    public void SendRequest(string inputMessage, int configIndex, UnityAction<bool, string> callback)
    {
        aiConfig = aiConfigs[configIndex];
        if (aiConfig == null)
        {
            Debug.LogError("aiConfig is null");
            return;
        }
        if (roleMessages.Count < 1)
        {
            //添加身份设定
            roleMessages.Add(new Message()
            {
                role = "system",
                content = npcCharacter.description
            });
        }
        else
        {
            //为提问添加上文
            roleMessages.Add(new Message()
            {
                role = this.role,
                content = this.content
            });
        }
        //当前提问
        roleMessages.Add(new Message()
        {
            role = "user",
            content = inputMessage
        });


        switch (aiConfig.AIType)
        {
            case AIType.DeepSeek:
                StartCoroutine(SendMessage(callback));
                break;
            case AIType.Qwen:
                break;
            case AIType.Volces:
                break;
        }

    }

    UnityWebRequest CreateUnityWebRequest(string jsondata)
    {
        UnityWebRequest unityWebRequest = new UnityWebRequest(aiConfig.API_URL, "POST");
        Byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsondata);
        unityWebRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");
        unityWebRequest.SetRequestHeader("Authorization", "Bearer " + aiConfig.API_KEY);
        unityWebRequest.SetRequestHeader("Accept", "application/json");
        return unityWebRequest;
    }

    bool IsUnityWebRequestError(UnityWebRequest unityWebRequest)
    {
        return unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError || unityWebRequest.result == UnityWebRequest.Result.DataProcessingError;
    }

    IEnumerator SendMessage(UnityAction<bool, string> callback)
    {

        RequestMessage request = new RequestMessage();
        request.model = aiConfig.Model;
        request.messages = roleMessages;
        request.stream = isStream;
        request.temperature = temperature;
        request.max_tokens = maxToken;
        string json = JsonConvert.SerializeObject(request);
        Debug.Log(json);
        UnityWebRequest unityWebRequest = CreateUnityWebRequest(json);

        yield return unityWebRequest.SendWebRequest();
        if (IsUnityWebRequestError(unityWebRequest))
        {
            if (unityWebRequest.responseCode == 429)
            {
                Debug.LogWarning("速率达到限制,延迟重试中...");
                yield return new WaitForSeconds(5);
                StartCoroutine(SendMessage(callback));
                yield break;
            }
            else
            {
                Debug.LogError($"{unityWebRequest.responseCode}|{unityWebRequest.downloadHandler.text}");
                callback?.Invoke(false, $"{unityWebRequest.responseCode}|{unityWebRequest.downloadHandler.text}");
                yield break;

            }
        }
        else
        {
            ParseMessage(unityWebRequest, callback);
        }
    }

    void ParseMessage(UnityWebRequest webRequest, UnityAction<bool, string> callback)
    {
        string requestcontent = webRequest.downloadHandler.text;
        Debug.Log(requestcontent);


        if (isStream)
        {


        }
        else
        {

            ResponsesData responsesData = ParseResponseMessage(requestcontent);
            if (responsesData != null && responsesData.choices.Count > 0)
            {
                string _role = responsesData.choices[0].message.role;
                string _content = responsesData.choices[0].message.content;
                content = _content;
                role = _role;
                callback?.Invoke(true, content);
            }
            else
            {
                callback?.Invoke(false, requestcontent);
            }
        }
        webRequest.Dispose();
    }

    ResponsesData ParseResponseMessage(string requestcontent)
    {
        try
        {
            ResponsesData responsesData = JsonConvert.DeserializeObject<ResponsesData>(requestcontent);
            if (responsesData == null || responsesData.choices == null || responsesData.choices.Count < 1)
            {
                Debug.LogError("数据格式错误或没有有效数据");
                return null;

            }
            return responsesData;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }

    public List<AIConfig> GetAIConfigs()
    {
        return aiConfigs;
    }

    public void Clear()
    {
        roleMessages.Clear();
    }
}

public class RequestMessage
{
    public string model;
    public List<Message> messages;
    public bool stream;
    public float temperature;
    public int max_tokens;
}

public class Message
{
    public string role;
    public string content;

}

[Serializable]
public class NPCCharacter
{
    public string name;
    public string description;
}




public class ChoicesItem
{
    /// <summary>
    /// 
    /// </summary>
    public string finish_reason { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int index { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Message message { get; set; }
}

public class Usage
{
    /// <summary>
    /// 
    /// </summary>
    public int completion_tokens { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int prompt_tokens { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int total_tokens { get; set; }
}

public class ResponsesData
{
    /// <summary>
    /// 
    /// </summary>
    public string id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<ChoicesItem> choices { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int created { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string model { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string @object { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Usage usage { get; set; }
}
