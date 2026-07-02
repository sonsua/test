using System.Collections;
using UnityEngine;

public class WokStyleManjuuMixer : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private Quaternion bgOriginalRotation;

    [Header("시간 설정 (초)")]
    [SerializeField] private float minWaitTime = 3f;
    [SerializeField] private float maxWaitTime = 6f;

    [Header("기본 믹싱 연출 설정")]
    [SerializeField] private float mixDuration = 0.8f;
    [SerializeField] private float baseJumpHeight = 2.5f;
    [SerializeField] private float baseRotateAngle = 15f;
    [SerializeField] private float baseRollDistance = 0.5f;

    [Header("배경 연출 설정")]
    // 🌟 [수정] 이름을 적는 대신, 인스펙터에서 Circle 오브젝트를 직접 드래그해서 넣도록 변경합니다.
    [SerializeField] private Transform bgTransform;
    [SerializeField] private float bgRotateMultiplier = 0.3f;

    // 🌟 [추가] 생성 스크립트를 연결할 변수
    [SerializeField] private AddObject addObjectScript;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // 🌟 [수정] 인스펙터에서 Circle을 직접 연결해 두므로, Start에서 Find로 찾을 필요가 없어졌습니다.
        if (bgTransform != null)
        {
            bgOriginalRotation = bgTransform.rotation;
        }

        StartCoroutine(AutoWokMixRoutine());
    }

    IEnumerator AutoWokMixRoutine()
    {
        while (true)
        {
            float commonWait = 4f;
            yield return new WaitForSeconds(commonWait);

            if (addObjectScript != null)
            {
                while (addObjectScript.isSpawning)
                {
                    yield return null;
                }
            }

            // 1. [웍질 시작 직전] 모든 자식 만쥬의 깜빡임을 발생시킴
            ManjuuBlink[] manjuuBlinks = GetComponentsInChildren<ManjuuBlink>();
            foreach (ManjuuBlink blink in manjuuBlinks)
            {
                blink.TriggerBlinkOnce();
            }

            float elapsedTime = 0f;

            float currentJumpHeight = baseJumpHeight * Random.Range(0.6f, 1.4f);
            float currentRotateAngle = baseRotateAngle * Random.Range(0.5f, 1.5f);
            float currentRollDistance = baseRollDistance * Random.Range(0.4f, 1.6f);

            float directionSign = Random.Range(0, 2) == 0 ? 1f : -1f;
            float randomSpinOffset = Random.Range(-15f, 15f);

            while (elapsedTime < mixDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / mixDuration;

                // 1. 웍질 포물선
                float height = Mathf.Sin(t * Mathf.PI) * currentJumpHeight;
                float rollX = Mathf.Sin(t * Mathf.PI * 1.5f) * currentRollDistance * directionSign;
                float rollY = -Mathf.Cos(t * Mathf.PI) * (currentRollDistance * 0.3f);

                // 3. 웍 기울이기
                float currentAngle = Mathf.Sin(t * Mathf.PI) * currentRotateAngle * directionSign;
                float randomSpin = Mathf.Sin(t * Mathf.PI) * randomSpinOffset;

                // 최종 적용 (만쥬 그룹)
                transform.position = originalPosition + new Vector3(rollX, height + rollY, 0f);
                transform.rotation = Quaternion.Euler(0f, 0f, originalRotation.eulerAngles.z + currentAngle + randomSpin);

                // 배경 오브젝트(Circle) 회전 연출
                if (bgTransform != null)
                {
                    float bgAngle = -currentAngle * bgRotateMultiplier;
                    bgTransform.rotation = Quaternion.Euler(0f, 0f, bgOriginalRotation.eulerAngles.z + bgAngle);
                }

                yield return null;
            }

            // 복귀 애니메이션 시작
            float returnTime = 0f;
            float returnDuration = 0.2f;
            Vector3 centerPos = transform.position;
            Quaternion centerRot = transform.rotation;

            while (returnTime < returnDuration)
            {
                returnTime += Time.deltaTime;
                float returnT = returnTime / returnDuration;

                transform.position = Vector3.Lerp(centerPos, originalPosition, returnT);
                transform.rotation = Quaternion.Lerp(centerRot, originalRotation, returnT);

                if (bgTransform != null)
                {
                    bgTransform.rotation = Quaternion.Lerp(bgTransform.rotation, bgOriginalRotation, returnT);
                }

                yield return null;
            }

            // 최종 위치 깔끔하게 고정
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            if (bgTransform != null) bgTransform.rotation = bgOriginalRotation;
        }
    }
}