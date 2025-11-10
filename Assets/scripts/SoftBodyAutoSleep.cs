using UnityEngine;
using UnityEngine.U2D.Animation;

public class SoftBodyAutoSleep : MonoBehaviour
{
    public float sleepDelay = 0.5f;      // 멈춘 상태가 지속되는 시간
    public float velocityThreshold = 0.05f; // "거의 멈춤"으로 판단할 속도 기준

    private float sleepTimer = 0f;
    private bool isSleeping = false;

    private Rigidbody2D[] boneRigidbodies;
    private SpriteSkin spriteSkin;

    void Start()
    {
        boneRigidbodies = GetComponentsInChildren<Rigidbody2D>();
        spriteSkin = GetComponent<SpriteSkin>();
    }

    void Update()
    {
        if (isSleeping) return;

        // 전체 본들의 평균 속도 계산
        float avgVelocity = 0f;
        foreach (var rb in boneRigidbodies)
        {
            avgVelocity += rb.linearVelocity.magnitude;
        }
        avgVelocity /= boneRigidbodies.Length;

        // 충분히 느려졌다면 타이머 증가
        if (avgVelocity < velocityThreshold)
        {
            sleepTimer += Time.deltaTime;
            if (sleepTimer >= sleepDelay)
                DisablePhysics();
        }
        else
        {
            sleepTimer = 0f; // 움직임 있으면 타이머 리셋
        }
    }

    void DisablePhysics()
    {
        foreach (var rb in boneRigidbodies)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true;
        }

        if (spriteSkin != null)
            spriteSkin.alwaysUpdate = false;

        isSleeping = true;
    }

    public void WakeUp()
    {
        foreach (var rb in boneRigidbodies)
            rb.isKinematic = false;

        if (spriteSkin != null)
            spriteSkin.alwaysUpdate = true;

        isSleeping = false;
        sleepTimer = 0f;
    }
}
