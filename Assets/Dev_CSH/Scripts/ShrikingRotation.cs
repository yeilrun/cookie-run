using UnityEngine.UI;
using UnityEngine;
using System.Collections;

namespace CSH
{

    public class ShrikingRotation : MonoBehaviour
    {
        public GameObject targetObject; // 회전 및 크기가 변경될 오브젝트
        public float shrinkSpeed = 0.5f; // 크기 축소 속도
        public float rotateSpeed = 10f; // 회전 속도
        public float shrinkAmount = 0.5f; // 크기 축소 비율

        private bool isRotating = false; // 회전 상태

        public void OnButtonClick()
        {
            // 버튼 클릭 시 호출될 메서드
            if (isRotating) return;

            isRotating = true;
            StartCoroutine(ShrinkAndRotateCoroutine()); // 코루틴 시작
        }

        IEnumerator ShrinkAndRotateCoroutine()
        {

            // 원래 크기를 저장
            Vector3 originalScale = targetObject.transform.localScale;

            // 크기 축소 및 회전 코루틴
            float elapsedTime = 0;
            while (elapsedTime < 1){
                // 크기 축소 (Lerp 함수 사용)
                Vector3 targetScale = targetObject.transform.localScale * (1 - shrinkAmount);
                targetObject.transform.localScale = Vector3.Lerp(targetObject.transform.localScale, targetScale, elapsedTime);

                // 회전 (Slerp 함수 사용)
                Quaternion targetRotation = Quaternion.Euler(0, 360 * rotateSpeed * elapsedTime, 0);
                targetObject.transform.rotation = Quaternion.Slerp(targetObject.transform.rotation, targetRotation, elapsedTime);

                // 시간 업데이트
                elapsedTime += shrinkSpeed * Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }

            // 회전 완료 후 원래 크기로 복구
            targetObject.transform.localScale = originalScale;

            // 회전 상태 초기화
            isRotating = false;
        }


    }
}