/*
* FileName:          Sample4
* CompanyName:       
* Author:            
* Description:       
*/

using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UniTaskSample
{
    public class Sample4 : MonoBehaviour
    {
        public Image Image_Normal;
        public Image Image_UniTask;
        public Button btn_Normal;
        public Button btn_UniTask;
        public string url1 = "https://t7.baidu.com/it/u=1595072465,3644073269&fm=193&f=GIF";
        public string url2 = "https://t7.baidu.com/it/u=4198287529,2774471735&fm=193&f=GIF";
        private void Start()
        {
            btn_Normal.onClick.AddListener(() => StartCoroutine(GetImageNormal()));
            btn_UniTask.onClick.AddListener(GetImageUniTask);

        }

        IEnumerator GetImageNormal()
        {
            UnityWebRequest resourceRequest = UnityWebRequestTexture.GetTexture(url1);
            yield return resourceRequest.SendWebRequest();
            if (resourceRequest.result == UnityWebRequest.Result.ProtocolError || resourceRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(resourceRequest.error);
            }
            else
            {
                if (resourceRequest.isDone)
                {
                    var tex = ((DownloadHandlerTexture)resourceRequest.downloadHandler).texture;
                    Sprite sprite = Sprite.Create(tex, new Rect(Vector2.zero, new Vector2(tex.width, tex.height)), new Vector2(0.5f, 0.25f));
                    Image_Normal.sprite = sprite;
                }
            }
        }

        async void GetImageUniTask()
        {
            var resourceRequest = UnityWebRequestTexture.GetTexture(url2);
            var resource = await resourceRequest.SendWebRequest();
            var tex = ((DownloadHandlerTexture)resourceRequest.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(Vector2.zero, new Vector2(tex.width, tex.height)), new Vector2(0.5f, 0.25f));
            Image_UniTask.sprite = sprite;

        }
    }
}
