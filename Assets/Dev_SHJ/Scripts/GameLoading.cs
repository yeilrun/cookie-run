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
        
        [SerializeField] private TMP_InputField loginUsername;
        [SerializeField] private TMP_InputField loginPassword;
        
        [SerializeField] private GameObject joinGo;
        [SerializeField] private TMP_InputField joinUsername;
        [SerializeField] private TMP_InputField joinPassword;
        
        [SerializeField] private TextMeshProUGUI loadingMessage;
        
        private string loginURL = "http://127.0.0.1:8000/api-token-auth/";
        private string joinURL = "http://127.0.0.1:8000/api-auth/create/";
        
        public struct Token
        {
            public string token;
            public Token(string token)
            {
                this.token = token;
            }
        }
        
        public Token sToken = new Token();
        
        public void Login()
        {
            if (loginUsername.text != string.Empty && loginPassword.text != string.Empty)
            {
                StartCoroutine(LoginRequest(loginUsername.text, loginPassword.text));
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
                loadingMessage.text = "login success";
                yield return new WaitForSeconds(1f);
                
                loadingMessage.text = "data loading...";
                yield return new WaitForSeconds(2f);
                loginCallback?.Invoke();
            }
            else
            {
                loadingMessage.text = "login fail";
            }

        }

        public void OnJoin()
        {
            joinUsername.text = string.Empty;
            joinPassword.text = string.Empty;
            joinGo.SetActive(true);
        }

        public void OnJoinClose()
        {
            joinGo.SetActive(false);
        }
        
        public void Join()
        {
            if (joinUsername.text != string.Empty && joinUsername.text != string.Empty)
            {
                StartCoroutine(JoinRequest(joinUsername.text, joinPassword.text));
            }
        }

        private IEnumerator JoinRequest(string username, string password)
        {
            WWWForm form = new WWWForm();
            form.AddField("username", username);
            form.AddField("password", password);
            UnityWebRequest res = UnityWebRequest.Post(joinURL, form);
            yield return res.SendWebRequest();

            if (res.result == UnityWebRequest.Result.Success)
            {
                joinGo.SetActive(false);
                Debug.Log("success");
            }
        }
    }
}

