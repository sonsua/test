using UnityEngine;

public class ManjuuImpactEffect : MonoBehaviour
{
    [Header("이펙트 설정")]
    [SerializeField] private GameObject impactEffectPrefab; // 🌟 소환할 이펙트 프리팹

    [Header("충돌 세기 기준")]
    [SerializeField] private float impactThreshold = 5f; // 🌟 이 속도보다 빠르게 부딪혀야 이펙트가 터짐

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. 상대방과 부딪힌 순간의 상대 속도(Relative Velocity) 크기를 계산합니다.
        float hitSpeed = collision.relativeVelocity.magnitude;

        // 2. 설정한 기준치(Threshold)보다 강하게 부딪혔는지 체크합니다.
        if (hitSpeed >= impactThreshold)
        {
            // 3. 부딪힌 정확한 지점(첫 번째 충돌 포인트)의 위치를 가져옵니다.
            Vector2 contactPoint = collision.contacts[0].point;

            // 4. 해당 위치에 이펙트를 소환합니다!
            if (impactEffectPrefab != null)
            {
                // 부딪힌 자리에 이펙트 생성
                GameObject effect = Instantiate(impactEffectPrefab, contactPoint, Quaternion.identity);

                // 💡 [팁] 이펙트가 스스로 파괴되는 스크립트가 없다면, 1초 뒤 자동 삭제되도록 안전장치를 둡니다.
                Destroy(effect, 1.0f);
            }
        }
    }
}