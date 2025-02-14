using SFB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FileLoadSample : MonoBehaviour, IPointerDownHandler
{
    // テキストアウトプット
    [SerializeField] private Text outputText;

    // 読み込んだテキスト
    private string _loadedText = "";

#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //

    // StandaloneFileBrowserのブラウザスクリプトプラグインから呼び出す
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    // ファイルを開く
    public void OnPointerDown(PointerEventData eventData) {
        UploadFile(gameObject.name, "OnFileUpload", ".", false);
    }

    // ファイルアップロード後の処理
    public void OnFileUpload(string url) {
        StartCoroutine(Load(url));
    }

#else
    //
    // OSビルド & Unity editor上
    //
    public void OnPointerDown(PointerEventData eventData) { }

    void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() => OpenFile());
    }

    // ファイルを開く
    public void OpenFile()
    {
        // 拡張子フィルタ
        var extensions = new[] {
            new ExtensionFilter("All Files", "*" ),
        };

        // ファイルダイアログを開く
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        if (paths.Length > 0 && paths[0].Length > 0)
        {

            StartCoroutine(Load(new System.Uri(paths[0]).AbsoluteUri));

        }
    }

#endif
    // ファイル読み込み
    private IEnumerator Load(string url)
    {
        var request = UnityWebRequest.Get(url);

        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            yield return null;
        }

        _loadedText = request.downloadHandler.text;
        Debug.Log(_loadedText);
        outputText.text = _loadedText;
    }

}
