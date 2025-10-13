using UnityEngine;
using UnityEngine.U2D.Animation;

public class SoftBodyAutoSleep : MonoBehaviour
{
    public float sleepDelay = 0.5f; // �浹 �� ���� ������� ��� �ð�
    private float sleepTimer = 0f;
    private bool isSleeping = false;
    private int collisionCount = 0;

    private Rigidbody2D[] boneRigidbodies;
    private SpriteSkin spriteSkin;

    void Start()
    {
        boneRigidbodies = GetComponentsInChildren<Rigidbody2D>();
        spriteSkin = GetComponent<SpriteSkin>();
    }

    void Update()
    {
        // �浹 ���̸� Ÿ�̸� �۵�
        if (collisionCount > 0 && !isSleeping)
        {
            sleepTimer += Time.deltaTime;

            if (sleepTimer >= sleepDelay)
                DisablePhysics();
        }
    }

    // BoneCollisionReporter���� ȣ���
    public void NotifyCollisionEnter()
    {
        collisionCount++;
        sleepTimer = 0f;

        // �̹� ��� ���¶�� �ٽ� ���� Ȱ��ȭ
        if (isSleeping)
            EnablePhysics();
    }

    // BoneCollisionReporter���� ȣ���
    public void NotifyCollisionExit()
    {
        collisionCount = Mathf.Max(0, collisionCount - 1);
        sleepTimer = 0f;
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

    void EnablePhysics()
    {
        foreach (var rb in boneRigidbodies)
            rb.isKinematic = false;

        if (spriteSkin != null)
            spriteSkin.alwaysUpdate = true;

        isSleeping = false;
    }
}
