using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SHJ
{
    public class MainGame : MonoBehaviour
    {
        [SerializeField] private GameObject main1;
        [SerializeField] private GameObject rankListGo;
        [SerializeField] private GameObject contentView;
        [SerializeField] private GameObject rankData;

        [SerializeField] private GameObject main2;
        [SerializeField] private GameObject itemListGo;

        [SerializeField] private GameObject topMain;
        [SerializeField] private GameObject optionModalGo;

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

        private void Start()
        {
            StartCoroutine(SingletonManager.Instance.GetData(rankData, contentView));
        }
        
        // On public
        public void OnClickOptionModal()
        {
            optionModalGo.SetActive(true);
        }
        
        public void OnOptionClose()
        {
            optionModalGo.SetActive(false);
        }
        
        public void OnClickRotateItemList(bool b)
        {
            GameObject targetGo = b ? itemListGo : rankListGo; // disable target
            GameObject activeGo = !b ? itemListGo : rankListGo; // enable target
            StartCoroutine(LoopScaleRotate(targetGo, activeGo));
        }
        
        public void OnClickGamePlayScene()
        {
            StartCoroutine(CustomPlayLoading());
        }
        
        // private IEnumerator
        
        private IEnumerator CustomPlayLoading()
        {
            int randomIdx = Random.Range(1, 4);
            playLoadingGo.SetActive(true);
            FieldInfo sp = GetType().GetField($"loadingImg{randomIdx}", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo cookiePos = GetType()
                .GetField($"cookiePosToImg{randomIdx}", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo mesPos = GetType().GetField($"messagePosToImg{randomIdx}",
                BindingFlags.NonPublic | BindingFlags.Instance);

            RectTransform[] playloadingGo_message_cookie = playLoadingGo.GetComponentsInChildren<RectTransform>();
            Image img = playloadingGo_message_cookie[0].GetComponent<Image>();
            TextMeshProUGUI tmp = playloadingGo_message_cookie[1].GetComponent<TextMeshProUGUI>();

            if (sp != null && mesPos != null)
            {
                img.sprite = (Sprite)sp.GetValue(this);
                playloadingGo_message_cookie[1].anchoredPosition = (Vector2)mesPos.GetValue(this);
                tmp.text = "한번 달려 볼까!!";
            }

            if (cookiePos != null)
            {
                playloadingGo_message_cookie[2].anchoredPosition = (Vector2)cookiePos.GetValue(this);
            }

            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("ReleaseGamePlayScene");
        }
        
        private IEnumerator LoopScaleRotate(GameObject targetGo, GameObject activeGo)
        {
            Vector3 originS = Vector3.one;
            Quaternion originRot = Quaternion.identity;
            WaitForSeconds wait = new WaitForSeconds(0.01f);
            // 작아진다
            float s_sub = 0.06f;
            float r_plus = 6f;
            for (int i = 0; i < 10; ++i)
            {
                Vector3 s = targetGo.transform.localScale;
                s.x = (s.x - s_sub);
                s.y = (s.y - s_sub);
                s.z = (s.z - s_sub);
                targetGo.transform.localScale = s;
                yield return wait;
            }

            // 돌린다
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

            // 돌린다
            for (int i = 0; i < 10; ++i)
            {
                Quaternion r = activeGo.transform.localRotation;
                r.y = (r.y - r_plus);
                activeGo.transform.Rotate(new Vector3(r.x, r.y, r.z));
                yield return wait;
            }

            // 커진다
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
        
        
        
    }
}
