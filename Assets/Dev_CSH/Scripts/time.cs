using UnityEngine;
using TMPro;


namespace CSH
{
    public class time : MonoBehaviour
    {

       [SerializeField] public TMP_Text timerText;
        [SerializeField] public float countdownTime ;
        [SerializeField] private bool timerRunning;

        public void Start()
        {
            startTimer();
        }

        public void Update()
        {
            if (timerRunning)
            {
                if (countdownTime > 0)
                {
                    countdownTime-=Time.deltaTime;
                    updateTimeDisplay();
                }

                else
                {
                    countdownTime = 0;
                    timerRunning=false;
                    TimeFinished();
                }
            }
        }

        public void startTimer()
        {
            timerRunning = true;
        }

        public void updateTimeDisplay()
        {
            int minutes = Mathf.FloorToInt(countdownTime/60);
            int seconds = Mathf.FloorToInt(countdownTime%60);
            timerText.text=string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }

        public void TimeFinished()
        {
            Debug.Log("0:00");
        }
    }

}
