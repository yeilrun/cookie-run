using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SingletonManager : MonoBehaviour
{
    private static SingletonManager instance = null;

    public static SingletonManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    
    private string token = string.Empty;

    public string Token
    {
        get
        {
            return token;
        }
        set
        {
            token = value;
        }
    }
    
    public class RankDataType
    {
        public string username;
        public int score;
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (userInfo == null)
        {
            userInfo = new UserInfo();
        }
    }

    public class UserInfo
    {
        public int level = 0;
        public int exp = 0;
        public int coin = 0;
        public int crystal_cnt = 0;
        public int heart_cnt = 0;
        public int key_cnt = 0;

        // 보물 클래스 따로 만들면
        // treasure_1 = 
        // treasure_2 = 
        // treasure_3 = 

        // 현재 보유중인 쿠키 또는 선택쿠키
        public Dictionary<string, string> cookies = new Dictionary<string, string>();
        
        // 보유중인 펫 또는 선택된 펫
        public Dictionary<string, string> pets = new Dictionary<string, string>();
        
        // 게임 플레이 통계
        public Dictionary<string, string> plays = new Dictionary<string, string>();
        
        // 업그레이드 현황
        public Dictionary<string, int> upgrades = new Dictionary<string, int>()
        {
            {"SelectJelly", 875},
        };
        
        // 부스트 보유 현황
        public Dictionary<string, string> items = new Dictionary<string, string>();
        
        // 설정 저장
        public Dictionary<string, string> options = new Dictionary<string, string>();

    }

    public UserInfo userInfo = null;
    public int saveMyScore = 0;

    public IEnumerator SendScore(int score)
    {
        saveMyScore = score;
        Time.timeScale = 1f;
        WWWForm form = new WWWForm();
        form.AddField("score", score);
        UnityWebRequest res = UnityWebRequest.Post("http://localhost:8000/rank/add/", form);
        res.SetRequestHeader("Authorization", $"Token {token}");
        yield return res.SendWebRequest();

        if (res.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(res.downloadHandler.text);
        }
        
        SceneManager.LoadScene("ReleaseGameMainScene");
    }
    
    private string rankGetURL = "http://127.0.0.1:8000/rank/list/";
    public List<RankDataType> rankDatas = null;

    public IEnumerator GetData(GameObject rankData, GameObject contentView)
    {
        UnityWebRequest res = UnityWebRequest.Get(rankGetURL);
        res.SetRequestHeader("Authorization", $"Token {token}");
        yield return res.SendWebRequest();
        if (res.result == UnityWebRequest.Result.Success)
        {
            rankDatas = JsonConvert.DeserializeObject<List<RankDataType>>(res.downloadHandler.text);
            foreach (RankDataType data in rankDatas)
            {
                GameObject d = Instantiate(rankData, contentView.transform);
                TextMeshProUGUI[] tmps = d.GetComponentsInChildren<TextMeshProUGUI>();
                tmps[0].text = (rankDatas.IndexOf(data) + 1).ToString();
                tmps[1].text = data.username;
                tmps[2].text = string.Format("{0:#,###}", data.score);
            }
        }
    }
}
