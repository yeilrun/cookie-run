using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;

namespace CSH
{
    public class GameMainFirst : MonoBehaviour
    {
        // public delegate void LoadingCallback();
        // public static LoadingCallback loadingCallback;
        private string rankGetURL = "http://127.0.0.1:8000/rank/list/";
        
        [SerializeField] private GameObject contentView;
        [SerializeField] private GameObject rankData;
        [SerializeField] private GameObject optionModalGo;

        public class RankDataType
        {   
            public string username;
            public int score;
        }
        
        public void GetDataRequest(string token)
        {
            StartCoroutine(GetData(token));
        }

        private IEnumerator GetData(string token)
        {
            UnityWebRequest res = UnityWebRequest.Get(rankGetURL);
            res.SetRequestHeader("Authorization", $"Token {token}");
            yield return res.SendWebRequest();
            
            if (res.result == UnityWebRequest.Result.Success)
            {
                List<RankDataType> datas = JsonConvert.DeserializeObject<List<RankDataType>>(res.downloadHandler.text);
                foreach (RankDataType data in datas)
                {
                    GameObject d = Instantiate(rankData, contentView.transform);
                    TextMeshProUGUI[] tmps = d.GetComponentsInChildren<TextMeshProUGUI>();
                    tmps[0].text = (datas.IndexOf(data) + 1).ToString();
                    tmps[1].text = data.username;
                    tmps[2].text = string.Format("{0:#,###}", data.score);
                }
            }
        }

        public void OnOptionClose()
        {
            optionModalGo.SetActive(false);
        }
    }
}