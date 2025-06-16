using System.Collections;
using System.Collections.Generic;
using LHA;
using SHJ;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayManager : MonoBehaviour
{
    [SerializeField] private HealthGageBar hpbar;
    [SerializeField] private CookieController cookieCon;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button keepGoingButton;
    [SerializeField] private Button stopGameButton;
    [SerializeField] private Image pauseIMG;

    [SerializeField] private TextMeshProUGUI myScoreText;
    [SerializeField] private TextMeshProUGUI nextScoreText;
    [SerializeField] private TextMeshProUGUI nextRankText;
    [SerializeField] private TextMeshProUGUI nextUserText;
    
    private int myScore = 0;
    private int nextScoreIdx = 0;
    
    private void OnEnable()
    {
        CookieController.onCookieIsClashCallback += CustomCookieIsClashCallback;
        HealthGageBar.onFillAmountIsZeroCallback += CustomOnFillAmountIsZeroCallback;
    }

    private void OnDisable()
    {
        CookieController.onCookieIsClashCallback -= CustomCookieIsClashCallback;
        HealthGageBar.onFillAmountIsZeroCallback -= CustomOnFillAmountIsZeroCallback;
    }

    private void Start()
    {
        CreateMapObject.isActive = true;
        RollingRoop.isActive = true;
        pauseButton.onClick.AddListener(OnClickpauseButton);
        keepGoingButton.gameObject.SetActive(false);
        stopGameButton.gameObject.SetActive(false);
        pauseIMG.gameObject.SetActive(false);

        nextScoreIdx = SingletonManager.Instance.rankDatas != null ? SingletonManager.Instance.rankDatas.Count - 1 : 0;
        if (nextScoreIdx != 0)
        {
            string nextscore = string.Format("{0:#,###}", SingletonManager.Instance.rankDatas?[nextScoreIdx].score);
            nextScoreText.text = nextscore;
            nextUserText.text = SingletonManager.Instance.rankDatas?[nextScoreIdx].username;
            nextRankText.text = (nextScoreIdx + 1).ToString() + "등";
        }
    }

    private void CustomCookieIsClashCallback(GameObject cookie, GameObject target)
    {
        if (target.CompareTag("wall"))
        {
            hpbar.ReduceHP(40);
            //if (hpbar.IMG.fillAmount <= 0)
            //{
            //    cookieCon.Die();
            //    // Time.timeScale = 0f; 원상 복구 값 1
            //    StartCoroutine(SingletonManager.Instance.SendScore(myScore));
            //}
        }

        if (target.CompareTag("BigPotion"))
        {
            target.SetActive(false);
            hpbar.InduceHP(40);
        }

        if (target.CompareTag("SmallPotion"))
        {
            hpbar.InduceHP(10);
        }

        if (target.CompareTag("Dead"))
        {
            StartCoroutine(cookieCon.CameraShake(0.5f, 0.1f));
            hpbar.ReduceHP(500);
            CustomOnFillAmountIsZeroCallback();
        }

        if (target.CompareTag("Jelly"))
        {
            target.SetActive(false);
            myScore += SingletonManager.Instance.userInfo.upgrades.GetValueOrDefault("SelectJelly", 0);
            myScoreText.text = string.Format("{0:#,###}", myScore);

            if (SingletonManager.Instance.rankDatas?[nextScoreIdx].score < myScore)
            {
                if (nextScoreIdx != 0)
                {
                    nextScoreIdx -= 1;
                    string nextscore = string.Format("{0:#,###}", SingletonManager.Instance.rankDatas?[nextScoreIdx].score);
                    nextScoreText.text = nextscore;
                    nextUserText.text = SingletonManager.Instance.rankDatas?[nextScoreIdx].username;
                    nextRankText.text = (nextScoreIdx + 1).ToString() + "등";
                }
                else
                {
                    nextScoreText.text = string.Format("{0:#,###}", myScore);
                }
            }
        }
    }

    private void CustomOnFillAmountIsZeroCallback()
    {
        cookieCon.Die();
        CreateMapObject.isActive = false;
        RollingRoop.isActive = false;
        StartCoroutine(SingletonManager.Instance.SendScore(myScore));
        //SceneManager.LoadScene("ReleaseGameResultScene");
        StartCoroutine(DieMoveEndLoadScene());
    }

    private IEnumerator DieMoveEndLoadScene()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("ReleaseGameResultScene");
    }

    public void OnClickpauseButton()
    {
        Time.timeScale = 0f;
        keepGoingButton.gameObject.SetActive(true);
        stopGameButton.gameObject.SetActive(true);
        pauseIMG.gameObject.SetActive(true);
    }

    public void OnClickKeepGoingButton()
    {
        keepGoingButton.gameObject.SetActive(false);
        stopGameButton.gameObject.SetActive(false);
        pauseIMG.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnClickStopGameButton()
    {
        keepGoingButton.gameObject.SetActive(false);
        stopGameButton.gameObject.SetActive(false);
        pauseIMG.gameObject.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("ReleaseGameResultScene");
    }
}
