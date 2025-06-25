using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // Silder class ����ϱ� ���� �߰��մϴ�.

namespace CSH
{
    public class GameManager : MonoBehaviour
    {

        // �ð��� ǥ���ϴ� text UI�� ����Ƽ���� �����´�.
        public Input gameTimeUI;
        [SerializeField] public Text gameTime;

        float setTime = 300;// ��ü ���� �ð��� �������ش�. ���⼭�� 300��.

        // �д����� �ʴ����� ����� ������ ������ش�.
        [SerializeField] int min;
        [SerializeField] float sec;

        public void Update()
        {
            // ���� �ð��� ���ҽ����ش�.
            setTime -= Time.deltaTime;

            // ��ü �ð��� 60�� ���� Ŭ ��
            if (setTime >= 0f)
            {
                // 60���� ������ ����� ���� �д����� ����
                min = (int)setTime / 60;
                // 60���� ������ ����� �������� �ʴ����� ����
                sec = setTime % 60;
                // UI�� ǥ�����ش�
                gameTime.text = min + ":" + (int)sec;
            }

            // ���� �ð��� 0���� �۾��� ��
            if (setTime < 0)
            {
                // UI �ؽ�Ʈ�� 0�ʷ� ������Ŵ.
                gameTime.text = "0:00";
            }

        }
    }
}