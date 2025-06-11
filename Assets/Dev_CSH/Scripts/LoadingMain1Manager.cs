using UnityEngine;
using System.Collections;

namespace CSH
{
    public class LoadingMain1Manager : MonoBehaviour
    {
        // public GameObject targetObject; // ȸ�� �� ũ�Ⱑ ����� ������Ʈ
        // public float shrinkSpeed = 0.5f; // ũ�� ��� �ӵ�
        // public float rotateSpeed = 10f; // ȸ�� �ӵ�
        // public float shrinkAmount = 0.5f; // ũ�� ��� ����
        //
        // private bool isRotating = false; // ȸ�� ����
        //
        // public void OnButtonClick()
        // {
        //     // ��ư Ŭ�� �� ȣ��� �޼���
        //     if (isRotating) return;
        //
        //     isRotating = true;
        //     StartCoroutine(ShrinkAndRotateCoroutine()); // �ڷ�ƾ ����
        // }
        //
        // IEnumerator ShrinkAndRotateCoroutine()
        // {
        //
        //     // ���� ũ�⸦ ����
        //     Vector3 originalScale = targetObject.transform.localScale;
        //
        //     // ũ�� ��� �� ȸ�� �ڷ�ƾ
        //     float elapsedTime = 0;
        //     while (elapsedTime < 1){
        //         // ũ�� ��� (Lerp �Լ� ���)
        //         Vector3 targetScale = targetObject.transform.localScale * (1 - shrinkAmount);
        //         targetObject.transform.localScale = Vector3.Lerp(targetObject.transform.localScale, targetScale, elapsedTime);
        //
        //         // ȸ�� (Slerp �Լ� ���)
        //         Quaternion targetRotation = Quaternion.Euler(0, 360 * rotateSpeed * elapsedTime, 0);
        //         targetObject.transform.rotation = Quaternion.Slerp(targetObject.transform.rotation, targetRotation, elapsedTime);
        //
        //         // �ð� ������Ʈ
        //         elapsedTime += shrinkSpeed * Time.deltaTime;
        //         yield return null; // ���� �����ӱ��� ���
        //     }
        //
        //     // ȸ�� �Ϸ� �� ���� ũ��� ����
        //     targetObject.transform.localScale = originalScale;
        //
        //     // ȸ�� ���� �ʱ�ȭ
        //     isRotating = false;
        // }
    }
}