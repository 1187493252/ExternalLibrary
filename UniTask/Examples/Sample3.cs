/*
* FileName:          Sample3
* CompanyName:       
* Author:            
* Description:       
*/

using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UniTaskSample
{
    public class Sample3 : MonoBehaviour
    {

        public Text text1;
        public Text text2;
        public Button btn1;
        public Button btn2;
        private void Start()
        {
            btn1.onClick.AddListener(() => { OnClick("https://www.baidu.com", text1); });
            btn2.onClick.AddListener(() => { OnClick("https://store.steampowered.com", text2); });

        }
        async UniTask<string> OpenURL(string url, float timeout)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfterSlim(TimeSpan.FromSeconds(timeout));
            var (failed, result) = await UnityWebRequest.Get(url).SendWebRequest().WithCancellation(cts.Token).SuppressCancellationThrow();
            if (!failed)
            {
                return result.downloadHandler.text.Substring(0, 20);
            }
            return "超时";
        }
        async void OnClick(string url, Text text)
        {
            var res = await OpenURL(url, 2);
            text.text = res;
        }
    }
}
