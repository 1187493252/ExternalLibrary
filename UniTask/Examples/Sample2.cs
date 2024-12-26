/*
* FileName:          Sample2
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
using UnityEngine.UI;

namespace UniTaskSample
{
    public class Sample2 : MonoBehaviour
    {
        public Transform ball;

        public Button btn_Move;
        public Button btn_CancelMove;
        CancellationTokenSource cts;
        private void Start()
        {
            btn_Move.onClick.AddListener(() => { Btn1(); });
            btn_CancelMove.onClick.AddListener(() => { Btn2(); });


        }


        async Task<int> Move(Transform transform, CancellationToken token)
        {
            float totaltime = 5;
            float currenttime = 0;
            while (currenttime < totaltime)
            {
                currenttime += Time.deltaTime;
                transform.position += Vector3.right * Time.deltaTime;
                await UniTask.NextFrame(token);
            }
            return 0;
        }

        public async void Btn1()
        {
            try
            {
                cts = new CancellationTokenSource();

                await Move(ball, cts.Token);

            }
            catch (OperationCanceledException)
            {

                Debug.LogError("捕获异常");
            }
        }
        public async void Btn2()
        {
            cts.Cancel();
            cts.Dispose();
        }
    }
}
