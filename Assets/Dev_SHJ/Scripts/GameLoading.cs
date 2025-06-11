using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace SHJ
{
    public class GameLoading : MonoBehaviour
    {
        public delegate void LoginCallback();
        public static event LoginCallback loginCallback;
        
        [SerializeField] private TMP_InputField username;
        [SerializeField] private TMP_InputField password;

        private string loginURL = "http://127.0.0.1:8000/api-token-auth/";
        
        private struct Token
        {
            public string token;
            public Token(string token)
            {
                this.token = token;
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
                loginCallback?.Invoke();
            }
        }
    }
}

