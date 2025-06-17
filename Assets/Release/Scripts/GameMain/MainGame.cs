using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SHJ
{
    public class MainGame : MonoBehaviour
    {
        [Header("Main 1")]
        [SerializeField] private GameObject main1;
        [SerializeField] private GameObject rankListGo;
        [SerializeField] private GameObject contentView;
        [SerializeField] private GameObject rankData;

        [Header("Main 2")]
        [SerializeField] private GameObject main2;
        [SerializeField] private GameObject itemListGo;
        [SerializeField] private GameObject targetItemInsetGo;

        [Header("Top")]
        [SerializeField] private GameObject topMain;
        [SerializeField] private GameObject optionModalGo;
        [SerializeField] private GameObject heartGo;
        
        [Header("Play Loading Display")]
        [SerializeField] private GameObject playLoadingGo;

        [Header("Play loading")] 
        [SerializeField] private Sprite loadingImg1;
        [SerializeField] private Vector2 messagePosToImg1;
        [SerializeField] private Vector2 cookiePosToImg1;
        [SerializeField] private Sprite loadingImg2;
        [SerializeField] private Vector2 messagePosToImg2;
        [SerializeField] private Vector2 cookiePosToImg2;
        [SerializeField] private Sprite loadingImg3;
        [SerializeField] private Vector2 messagePosToImg3;
        [SerializeField] private Vector2 cookiePosToImg3;

        [Header("Audio Clip")] 
        [SerializeField] private List<AudioClip> audioClips = null;
        [SerializeField] private AudioSource audioPlayCom;

        private AudioSource myBGAudio;
        
        private delegate void HeartMove();
        private HeartMove heartMove;
        
        private string[] playCookieSound = new[]
        {
            "한번 달려 볼까!!",
            "백수탈출 한번 해보자!!",
            "끝까지 포기하지 않아!!",
        };

        private void Start()
        {
            myBGAudio = GetComponent<AudioSource>();
            heartMove = null;
            // server get data score
            StartCoroutine(SingletonManager.Instance.GetData(rankData, contentView));
        }

        private void Update()
        {
            heartMove?.Invoke();
        }

        // click open option btn
        public void OnClickOptionModal()
        {
            optionModalGo.SetActive(true);
        }
        
        // click close option btn
        public void OnOptionClose()
        {
            optionModalGo.SetActive(false);
        }
        
        // main1 -> main2 event btn
        public void OnClickRotateItemList(bool b)
        {
            audioPlayCom.PlayOneShot(audioClips[0]);
            GameObject targetGo = b ? itemListGo : rankListGo; // disable target
            GameObject activeGo = !b ? itemListGo : rankListGo; // enable target
            StartCoroutine(LoopScaleRotate(targetGo, activeGo));
        }
        
        // game play scene go btn
        public void OnClickGamePlayScene(GameObject startBtn)
        {
            StartCoroutine(CustomPlayLoading(startBtn));
        }
        
        // main2 item detail click event
        public void ClickTargetItem(GameObject targetGo)
        {
            Transform[] trs = targetItemInsetGo.GetComponentsInChildren<Transform>();
            for (int i = 0; i < trs.Length; ++i)
            {
                if (i != 0)
                {
                    Destroy(trs[i].gameObject);
                }
            }

            GameObject go = new GameObject();
            go.transform.SetParent(targetItemInsetGo.transform);
            go.transform.localPosition = Vector3.zero;
            Image[] targetImgs = targetGo.GetComponentsInChildren<Image>();
            for (int i = 0; i < 2; ++i)
            {
                GameObject g = new GameObject();
                Image addImg = g.AddComponent<Image>();
                g.transform.localScale = new Vector3(2, 2, 2);
                addImg.sprite = targetImgs[i].sprite;
                addImg.color = targetImgs[i].color;
                g.transform.SetParent(go.transform);
                g.transform.localPosition = Vector3.zero;
                go = g;
            }
        }
        
        /// <summary>
        /// private zone
        /// </summary>
        
        // game play before random display
        private IEnumerator CustomPlayLoading(GameObject startBtn)
        {
            myBGAudio.mute = true;
            heartMove = () =>
            {
                heartGo.transform.position = Vector2.Lerp(
                    heartGo.transform.position, 
                    startBtn.transform.position, 
                    Time.deltaTime * 2f);
            };
            yield return new WaitForSeconds(2f);
            
            audioPlayCom.PlayOneShot(audioClips[3]);
            
            int randomIdx = Random.Range(1, 4);
            playLoadingGo.SetActive(true);
            FieldInfo sp = GetType().GetField(
                $"loadingImg{randomIdx}", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo cookiePos = GetType().GetField(
                $"cookiePosToImg{randomIdx}", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo mesPos = GetType().GetField(
                $"messagePosToImg{randomIdx}", BindingFlags.NonPublic | BindingFlags.Instance);

            RectTransform[] playloadingGoMessageCookie = playLoadingGo.GetComponentsInChildren<RectTransform>();
            Image img = playloadingGoMessageCookie[0].GetComponent<Image>();
            TextMeshProUGUI tmp = playloadingGoMessageCookie[1].GetComponent<TextMeshProUGUI>();

            if (sp != null && mesPos != null)
            {
                img.sprite = (Sprite)sp.GetValue(this);
                playloadingGoMessageCookie[1].anchoredPosition = (Vector2)mesPos.GetValue(this);
                tmp.text = playCookieSound[randomIdx - 1];
            }

            if (cookiePos != null)
            {
                playloadingGoMessageCookie[2].anchoredPosition = (Vector2)cookiePos.GetValue(this);
            }
            
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("ReleaseGamePlayScene");
        }
        
        // main1 -> main2 event func
        private IEnumerator LoopScaleRotate(GameObject targetGo, GameObject activeGo)
        {
            Vector3 originS = Vector3.one;
            Quaternion originRot = Quaternion.identity;
            WaitForSeconds wait = new WaitForSeconds(0.01f);

            float s_sub = 0.06f;
            float r_plus = 8.5f;
            for (int i = 0; i < 10; ++i)
            {
                Vector3 s = targetGo.transform.localScale;
                s.x = (s.x - s_sub);
                s.y = (s.y - s_sub);
                s.z = (s.z - s_sub);
                targetGo.transform.localScale = s;
                yield return wait;
            }
            
            for (int i = 0; i < 10; ++i)
            {
                Quaternion r = targetGo.transform.localRotation;
                r.y = (r.y + r_plus);
                targetGo.transform.Rotate(new Vector3(r.x, r.y, r.z));
                yield return wait;
            }

            activeGo.transform.localScale = targetGo.transform.localScale;
            activeGo.transform.localRotation = targetGo.transform.localRotation;
            yield return wait;

            targetGo.transform.localScale = originS;
            targetGo.transform.localRotation = originRot;
            targetGo.transform.parent.gameObject.SetActive(false);
            activeGo.transform.parent.gameObject.SetActive(true);
            yield return wait;

            for (int i = 0; i < 10; ++i)
            {
                Quaternion r = activeGo.transform.localRotation;
                r.y = (r.y - r_plus);
                activeGo.transform.Rotate(new Vector3(r.x, r.y, r.z));
                yield return wait;
            }

            for (int i = 0; i < 10; ++i)
            {
                Vector3 s = activeGo.transform.localScale;
                s.x = (s.x + s_sub);
                s.y = (s.y + s_sub);
                s.z = (s.z + s_sub);
                activeGo.transform.localScale = s;
                yield return wait;
            }

            activeGo.transform.localScale = originS;
            activeGo.transform.localRotation = originRot;
        }
        
        public void OnBGMSoundChange(Slider slider)
        {
            myBGAudio.volume = slider.value;
        }
        
        public void OnEffectSoundChange(Slider slider)
        {
            audioPlayCom.volume = slider.value;
        }
        
        public void OnBGMSoundMute(Toggle togle)
        {
            myBGAudio.mute = togle.isOn;
        }
        
        public void OnEffectSoundMute(Toggle togle)
        {
            audioPlayCom.mute = togle.isOn;
        }
    }
}
