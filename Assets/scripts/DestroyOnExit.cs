using UnityEngine;

public class DestroyOnExit : MonoBehaviour
{
    // 애니메이션이 끝나면 실행되거나, 안전하게 소환 직후 지정된 초 뒤에 삭제합니다.
    void Start()
    {
        // 만약 이펙트 애니메이션이 0.4초짜리라면 0.4f를 적어주면 됩니다.
        Destroy(gameObject, 0.4f);
    }
}