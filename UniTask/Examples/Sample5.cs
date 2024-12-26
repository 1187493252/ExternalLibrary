/*
* FileName:          Sample5
* CompanyName:       
* Author:            
* Description:       
*/

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UniTaskSample
{
    public class Sample5 : MonoBehaviour
    {


        public Button btn1;
        public Button btn2;


        private void Start()
        {
            btn1.onClick.AddListener(() => Click(this.GetCancellationTokenOnDestroy()));

            //   Click(this.GetCancellationTokenOnDestroy());
            TripleClick(this.GetCancellationTokenOnDestroy());
        }
        async void TripleClick(CancellationToken token)
        {
            await btn2.OnClickAsAsyncEnumerable().Take(3).ForEachAsync((_, index) =>
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                Debug.Log(index);

            }, token);
            Debug.Log("TripleClick complete");

        }



        async void Click(CancellationToken token)
        {
            var first = btn1.OnClickAsync();
            await first;
            var second = btn1.OnClickAsync();
            int index = await UniTask.WhenAny(second, UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token));
            if (index == 0)
            {
                Debug.Log("双击");
            }
        }
    }
}
