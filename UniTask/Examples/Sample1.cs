/*
* FileName:          Sample1
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
using UnityEngine.UI;

namespace UniTaskSample
{
    public class Sample1 : MonoBehaviour
    {
        public Text text_Normal;
        public Text text_UniTask;
        public Button btn_Normal;
        public Button btn_UniTask;
        private void Start()
        {
            btn_Normal.onClick.AddListener(() => StartCoroutine(GetResourceNormal()));
            btn_UniTask.onClick.AddListener(GetResourceUniTask);

        }

        IEnumerator GetResourceNormal()
        {
            ResourceRequest resourceRequest = Resources.LoadAsync<TextAsset>("normal");
            while (!resourceRequest.isDone)
            {
                yield return 0;

            }
            if (resourceRequest.asset != null)
            {
                text_Normal.text = ((TextAsset)resourceRequest.asset).text;
            }
        }

        async void GetResourceUniTask()
        {
            ResourceRequest resourceRequest = Resources.LoadAsync<TextAsset>("uniTask");
            var resource = await resourceRequest;
            text_UniTask.text = ((TextAsset)resource).text;

        }
    }
}
