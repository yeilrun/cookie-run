using System.Collections;
using System.Collections.Generic;
using LHA;
using SHJ;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
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

    [SerializeField] AudioClip basicjellyAudio;
    [SerializeField] AudioClip bigpotionAudio;
    [SerializeField] AudioClip clashAudio;
    [SerializeField] AudioClip buttonAudio;
    [SerializeField] private AudioSource audioSourceM;
    
    private AudioSource audioSource;


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
        audioSource = GetComponent<AudioSource>();
        pauseButton.onClick.AddListener(OnClickpauseButton);
        keepGoingButton.gameObject.SetActive(false);
        stopGameButton.gameObject.SetActive(false);
        pauseIMG.gameObject.SetActive(false);

        if (SingletonManager.Instance.rankDatas != null && SingletonManager.Instance.rankDatas.Count > 0)
        {
            nextScoreIdx = SingletonManager.Instance.rankDatas.Count - 1;
            string nextscore = string.Format("{0:#,###}", SingletonManager.Instance.rankDatas[nextScoreIdx].score);
            nextScoreText.text = nextscore;
            nextUserText.text = SingletonManager.Instance.rankDatas[nextScoreIdx].username;
            nextRankText.text = (nextScoreIdx + 1).ToString() + "등";
        }
        else
        {
            nextScoreText.text = "0";
            nextUserText.text = SingletonManager.Instance.username;
            nextRankText.text = "1등";
        }
    }

    private void CustomCookieIsClashCallback(GameObject cookie, GameObject target)
    {
        if (target.CompareTag("wall"))
        {
            hpbar.ReduceHP(40);
            audioSourceM.PlayOneShot(clashAudio);
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
            audioSourceM.PlayOneShot(bigpotionAudio);
        }

        if (target.CompareTag("SmallPotion"))
        {
            hpbar.InduceHP(10);
        }

        if (target.CompareTag("Dead"))
        {
            StartCoroutine(cookieCon.CameraShake(0.5f, 0.1f));
            hpbar.ReduceHP(500);
            CustomOnFillAmountIsZeroCallback(false);
        }

        if (target.CompareTag("Jelly"))
        {
            audioSourceM.PlayOneShot(basicjellyAudio);
            target.SetActive(false);
            myScore += SingletonManager.Instance.userInfo.upgrades.GetValueOrDefault("SelectJelly", 0);
            myScoreText.text = string.Format("{0:#,###}", myScore);

            if (SingletonManager.Instance.rankDatas != null && 
                SingletonManager.Instance.rankDatas.Count > 0 && 
                SingletonManager.Instance.rankDatas[nextScoreIdx].score < myScore)
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

    private void CustomOnFillAmountIsZeroCallback(bool isStop)
    {
        cookieCon.Die();
        audioSource.Stop();
        CreateMapObject.isActive = false;
        RollingRoop.isActive = false;
        StartCoroutine(SingletonManager.Instance.SendScore(myScore));
        HealthGageBar.onFillAmountIsZeroCallback -= CustomOnFillAmountIsZeroCallback;
        StartCoroutine(DieMoveEndLoadScene(isStop));
    }

    private IEnumerator DieMoveEndLoadScene(bool isStop)
    {
        if (!isStop)
        {
            yield return new WaitForSeconds(2f);
        }

        SceneManager.LoadScene("ReleaseGameResultScene");
    }

    public void OnClickpauseButton()
    {
        audioSource.Pause();
        Time.timeScale = 0f;
        audioSourceM.PlayOneShot(buttonAudio);
        keepGoingButton.gameObject.SetActive(true);
        stopGameButton.gameObject.SetActive(true);
        pauseIMG.gameObject.SetActive(true);
    }

    public void OnClickKeepGoingButton()
    {
        audioSourceM.PlayOneShot(buttonAudio);
        audioSource.UnPause();
        keepGoingButton.gameObject.SetActive(false);
        stopGameButton.gameObject.SetActive(false);
        pauseIMG.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnClickStopGameButton()
    {
        audioSourceM.PlayOneShot(buttonAudio);
        keepGoingButton.gameObject.SetActive(false);
        stopGameButton.gameObject.SetActive(false);
        pauseIMG.gameObject.SetActive(false);
        CustomOnFillAmountIsZeroCallback(true);
        Time.timeScale = 1f;
    }
}
