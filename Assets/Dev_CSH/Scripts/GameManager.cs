using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // Silder class 사용하기 위해 추가합니다.

namespace CSH
{
    public class GameManager : MonoBehaviour
    {

        // 시간을 표시하는 text UI를 유니티에서 가져온다.
        public Input gameTimeUI;
        [SerializeField] public Text gameTime;

        float setTime = 300;// 전체 제한 시간을 설정해준다. 여기서는 300초.

        // 분단위와 초단위를 담당할 변수를 만들어준다.
        [SerializeField] int min;
        [SerializeField] float sec;

        public void Update()
        {
            // 남은 시간을 감소시켜준다.
            setTime -= Time.deltaTime;

            // 전체 시간이 60초 보다 클 때
            if (setTime >= 0f)
            {
                // 60으로 나눠서 생기는 몫을 분단위로 변경
                min = (int)setTime / 60;
                // 60으로 나눠서 생기는 나머지를 초단위로 설정
                sec = setTime % 60;
                // UI를 표현해준다
                gameTime.text = min + ":" + (int)sec;
            }

            // 남은 시간이 0보다 작아질 때
            if (setTime < 0)
            {
                // UI 텍스트를 0초로 고정시킴.
                gameTime.text = "0:00";
            }

        }
    }
}