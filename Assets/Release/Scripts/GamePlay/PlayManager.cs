using System.Collections.Generic;
using LHA;
using TMPro;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    [SerializeField] private HealthGageBar hpbar;
    [SerializeField] private CookieController cookieCon;

    [SerializeField] private TextMeshProUGUI myScoreText;
    private int myScore = 0;
    
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

    private void CustomCookieIsClashCallback(GameObject cookie, GameObject target)
    {
        if (target.CompareTag("wall"))
        {
            hpbar.ReduceHP(40);
            if (hpbar.IMG.fillAmount <= 0)
            {
                cookieCon.Die();
                Time.timeScale = 0f;
                StartCoroutine(SingletonManager.Instance.SendScore(myScore));
            }
        }

        if (target.CompareTag("BigPotion"))
        {
            hpbar.InduceHP(40);
        }

        if (target.CompareTag("SmallPotion"))
        {
            hpbar.InduceHP(10);
        }

        if (target.CompareTag("Jelly"))
        {
            target.SetActive(false);
            myScore += SingletonManager.Instance.userInfo.upgrades.GetValueOrDefault("SelectJelly", 0);
            myScoreText.text = string.Format("{0:#,###}", myScore);
        }
    }

    private void CustomOnFillAmountIsZeroCallback()
    {
        cookieCon.Die();
    }
}
