using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace SHJ
{
    public class GameLoading : MonoBehaviour
    {
        [SerializeField] private TMP_InputField username;
        [SerializeField] private TMP_InputField password;

        private string loginURL = "http://hjsondev.iptime.org:8080/api/token/";
        
        private struct Token
        {
            public string access;
            public string refresh;
            public Token(string access, string refresh)
            {
                this.access = access;
                this.refresh = refresh;
            }
        }
        
        private Token sToken = new Token();
        
        public void Login()
        {
            if (username.text != string.Empty && password.text != string.Empty)
            {
                StartCoroutine(LoginRequest(username.text, password.text));
            }
        }

        private IEnumerator LoginRequest(string username, string password)
        {
            WWWForm form = new WWWForm();
            form.AddField("username", username);
            form.AddField("password", password);
            UnityWebRequest res = UnityWebRequest.Post(loginURL, form);
            yield return res.SendWebRequest();

            if (res.result == UnityWebRequest.Result.Success)
            {
                sToken = JsonConvert.DeserializeObject<Token>(res.downloadHandler.text);
            }
        }
    }
}

