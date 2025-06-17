using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // Silder class ����ϱ� ���� �߰��մϴ�.

public class GameManager : MonoBehaviour
{

    // �ð��� ǥ���ϴ� text UI�� ����Ƽ���� �����´�.
    public Text gameTimeUI;

    // ��ü ���� �ð��� �������ش�. ���⼭�� 300��.
     float setTime = 300;

    // �д����� �ʴ����� ����� ������ ������ش�.
    [SerializeField] int min;
    [SerializeField] float sec;

    void Update()
    {
        // ���� �ð��� ���ҽ����ش�.
        setTime -= Time.deltaTime;

        // ��ü �ð��� 60�� ���� Ŭ ��
        if (setTime >= 60f)
        {
            // 60���� ������ ����� ���� �д����� ����
            min = (int)setTime / 60;
            // 60���� ������ ����� �������� �ʴ����� ����
            sec = setTime % 60;
            // UI�� ǥ�����ش�
            gameTimeUI.text = min + ":" + (int)sec;
        }

        // ��ü�ð��� 60�� �̸��� ��
        if (setTime < 60f)
        {
            // �� ������ �ʿ�������Ƿ� �ʴ����� ������ ����
            gameTimeUI.text = "0:"+(int)sec;
        }

        // ���� �ð��� 0���� �۾��� ��
        if (setTime <= 0)
        {
            // UI �ؽ�Ʈ�� 0�ʷ� ������Ŵ.
            gameTimeUI.text = "0:00";
        }
    }
}