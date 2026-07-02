using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation; // Sprite Library 기능을 쓰기 위해 필요해요

// 💡 각 만쥬의 face에 붙어있는 깜빡임 스크립트
public class ManjuuBlink : MonoBehaviour
{
    private SpriteResolver spriteResolver;

    void Awake()
    {
        spriteResolver = GetComponent<SpriteResolver>();
    }

    void Start()
    {
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            float randomWaitTime = Random.Range(1f, 8f);
            yield return new WaitForSeconds(randomWaitTime);

            // 평소 자연스러운 깜빡임
            spriteResolver.SetCategoryAndLabel("face", "2");
            yield return new WaitForSeconds(0.3f);
            spriteResolver.SetCategoryAndLabel("face", "1");
        }
    }

    // 🌟 [수정] 웍질할 때 외부에서 호출하면 딱 0.2초만 감았다가 뜨는 코루틴 실행 함수
    public void TriggerBlinkOnce()
    {
        StartCoroutine(BlinkOnceRoutine());
    }

    IEnumerator BlinkOnceRoutine()
    {
        // 🌟 [수정] 웍질이 시작되면 새로 만든 3번 표정(예: 웍질용 눈)으로 교체!
        spriteResolver.SetCategoryAndLabel("face", "3");

        // 0.2초 ~ 0.5초 사이 랜덤 대기
        float randomBlinkDuration = Random.Range(0.3f, 1f);
        yield return new WaitForSeconds(randomBlinkDuration);

        // 🌟 [수정] 대기가 끝나면 다시 평소 눈(1번 표정)으로 돌아옵니다.
        spriteResolver.SetCategoryAndLabel("face", "1");
    }
}