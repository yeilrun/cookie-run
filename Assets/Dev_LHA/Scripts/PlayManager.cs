using LHA;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    [SerializeField] private HealthGageBar hpbar;
    [SerializeField] private CookieController cookieCon;

    private void OnEnable()
    {
        CookieController.onCookieIsClashCallback += CustomCookieIsClashCallback;
        HealthGageBar.onFillAmountIsZeroCallback += CustomonFillAmountIsZeroCallback;
    }

    private void OnDisable()
    {
        CookieController.onCookieIsClashCallback -= CustomCookieIsClashCallback;
        HealthGageBar.onFillAmountIsZeroCallback -= CustomonFillAmountIsZeroCallback;
    }

    private void CustomCookieIsClashCallback(GameObject cookie, GameObject target)
    {
        if (target.CompareTag("wall"))
        {
            hpbar.ReduceHP(40);
        }

        if (target.CompareTag("BigPotion"))
        {
            hpbar.InduceHP(40);
        }

        if (target.CompareTag("SmallPotion"))
        {
            hpbar.InduceHP(10);
        }
    }

    private void CustomonFillAmountIsZeroCallback()
    {
        cookieCon.Die();
    }
}
