using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace LHA
{ 
    public class HealthGageBar : MonoBehaviour
    {
        public delegate void OnFillAmountIsZeroCallback(bool isStop);
        public static OnFillAmountIsZeroCallback onFillAmountIsZeroCallback;

        public Image IMG;
        
        [SerializeField] private int maxHP = 500;
        [SerializeField] private int persecondHP = 4;
        private float ratio = 0f;

        private void Start()
        {
            ratio = (float)persecondHP / maxHP;
            IMG = GetComponent<Image>();
            StartCoroutine(ReduceHPOverTime());
        }

        private IEnumerator ReduceHPOverTime()
        {
            while (true)
            {
                IMG.fillAmount -= ratio;
                if (IMG.fillAmount <= 0)
                {
                    StopAllCoroutines();
                    onFillAmountIsZeroCallback?.Invoke(false);
                }

                yield return new WaitForSeconds(1f);
            }
        }

        public void ReduceHP(int damage)
        {
            float hp = (float)damage / maxHP;
            IMG.fillAmount -= hp;
        }

        public void InduceHP(int heal)
        {
            float hp = (float)heal / maxHP;
            IMG.fillAmount += hp;
        }
    }
}