using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace SHJ
{
    public class GameLoading : MonoBehaviour
    {
        [FormerlySerializedAs("LoginFormGo")]
        [Header("Login Area")] 
        [SerializeField] private GameObject loginFormGo;
        [SerializeField] private TMP_InputField loginUsername;
        [SerializeField] private TMP_InputField loginPassword;
        [SerializeField] private GameObject joinGo;
        
        [Header("Result Message")]
        [SerializeField] private TextMeshProUGUI loadingMessage;
        
        [Header("Join Area")]
        [SerializeField] private TMP_InputField joinUsername;
        [SerializeField] private TMP_InputField joinPassword;
        
        // login click btn
        public void Login()
        {
            if (loginUsername.text != string.Empty && loginPassword.text != string.Empty)
            {
                StartCoroutine(LoginRequest(loginUsername.text, loginPassword.text));
            }
        }
        
        // join open btn
        public void OnJoin()
        {
            joinUsername.text = string.Empty;
            joinPassword.text = string.Empty;
            joinGo.SetActive(true);
        }

        // join close btn
        public void OnJoinClose()
        {
            joinGo.SetActive(false);
        }
        
        // join apply btn
        public void Join()
        {
            if (joinUsername.text != string.Empty && joinUsername.text != string.Empty)
            {
                StartCoroutine(JoinRequest(joinUsername.text, joinPassword.text));
            }
        }
        
        /// <summary>
        /// private zone
        /// </summary>
        
        // res serialize
        private struct TokenType
        {
            // variable name dont change
            public readonly string token;
            
            public TokenType(string token)
            {
                this.token = token;
            }
        }

        // send login request
        private IEnumerator LoginRequest(string username, string password)
        {
            WWWForm form = new WWWForm();
            form.AddField("username", username);
            form.AddField("password", password);
            UnityWebRequest res = UnityWebRequest.Post(SingletonManager.Instance.loginURL, form);
            yield return res.SendWebRequest();

            loginFormGo.SetActive(false);
            loadingMessage.gameObject.SetActive(true);
            
            if (res.result == UnityWebRequest.Result.Success)
            {
                TokenType t = JsonConvert.DeserializeObject<TokenType>(res.downloadHandler.text);
                SingletonManager.Instance.token = t.token;
                SingletonManager.Instance.username = username;
                // SingletonManager.Instance.userInfo.upgrades.Add("SelectJelly");
                loadingMessage.text = "login success";
                yield return new WaitForSeconds(1f);
                
                loadingMessage.text = "data loading.";
                yield return new WaitForSeconds(1f);
                loadingMessage.text = "data loading..";
                yield return new WaitForSeconds(1f);
                loadingMessage.text = "data loading...";
                yield return new WaitForSeconds(1f);
                
                loadingMessage.text = "Success !!";
                yield return new WaitForSeconds(1f);
                
                SceneManager.LoadScene("ReleaseGameMainScene");
            }
            else
            {
                loadingMessage.text = "login fail";
                yield return new WaitForSeconds(1f);
                loginFormGo.SetActive(true);
                loadingMessage.gameObject.SetActive(false);
            }
        }

        // send join request
        private IEnumerator JoinRequest(string username, string password)
        {
            WWWForm form = new WWWForm();
            form.AddField("username", username);
            form.AddField("password", password);
            UnityWebRequest res = UnityWebRequest.Post(SingletonManager.Instance.joinURL, form);
            yield return res.SendWebRequest();

            if (res.result == UnityWebRequest.Result.Success)
            {
                joinGo.SetActive(false);
            }
        }
    }
}

