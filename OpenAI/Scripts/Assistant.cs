using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Assistant : MonoBehaviour
{



    public Dropdown dropdown;
    public InputField messageInput;
    public Button sendButton;
    public Button clearButton;
    public Text responseText;
    public ScrollRect scrollRect;
    public GameObject thingk;
    [Header("默认大模型")]
    public int index = 0;
    [Header("文本显示速度")]
    public float displaySpeed = 0.1f;
    [Header("大模型扮演身份")]
    public NPCCharacter npcCharacter;
    string inputMessage;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        thingk.SetActive(false);
        sendButton.onClick.AddListener(SendRequest);
        clearButton.onClick.AddListener(ClearContext);
        dropdown.ClearOptions();
        foreach (var item in OpenAI.Instance.GetAIConfigs())
        {
            dropdown.AddOptions(new List<string>() { item.name });
        }
        dropdown.onValueChanged.AddListener(DropdownValueChanged);
        dropdown.value = index;
        OpenAI.Instance.SetCharacter(npcCharacter);
    }


    void DropdownValueChanged(int _index)
    {
        index = _index;
    }

    public void SendRequest()
    {
        thingk.SetActive(true);
        inputMessage = messageInput.text;
        messageInput.text = "";
        OpenAI.Instance.SendRequest(inputMessage, index, HandleResponse);
    }

    void HandleResponse(bool isSuccess, string response)
    {
        thingk.SetActive(false);
        StopAllCoroutines();

        StartCoroutine(TextDisplayEffect("\n" + npcCharacter.name + ": \n" + response));

    }


    IEnumerator TextDisplayEffect(string response)
    {
        foreach (char c in response)
        {
            responseText.text += c;
            yield return new WaitForSeconds(displaySpeed);
            ScrollRectToBottom();
        }
    }
    void ScrollRectToBottom()
    {
        //滚动条滚动到底部
        scrollRect.content.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
        scrollRect.content.GetComponent<ContentSizeFitter>().SetLayoutVertical();
        scrollRect.verticalNormalizedPosition = 0;
    }

    /// <summary>
    /// 清除上下文
    /// </summary>
    public void ClearContext()
    {
        responseText.text = "";
        ScrollRectToBottom();
        OpenAI.Instance.Clear();
    }
}


