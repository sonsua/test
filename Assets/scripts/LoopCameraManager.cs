using System.Collections;
using UnityEngine;

public class LoopCameraManager : MonoBehaviour
{
    [Header("바구니 X 좌표 (0번은 오리지널, 1번은 눈속임용 오른쪽)")]
    [SerializeField] private float basketOriginal_X = 0f;
    [SerializeField] private float basketFake_X = 12f;

    [Header("화면 이동 속도")]
    [SerializeField] private float moveDuration = 1.2f;

    [Header("리셋 및 생성을 담당할 부모 오브젝트")]
    [SerializeField] private GameObject manjuuContainer; // 만쥬 자식들이 담기는 그룹 오브젝트

    [SerializeField] private AddObject addObjectScript;

    private bool isMoving = false;
    private Vector3 cameraStartPos;

    void Start()
    {
        cameraStartPos = transform.position;
    }

    void Update()
    {
        // 💡 구버전/신버전 인풋 시스템 모두에서 마우스 왼쪽 클릭 및 화면 터치를 감지하는 코드입니다.
        bool isTouched = false;

#if ENABLE_INPUT_SYSTEM // 신버전 인풋 시스템이 켜져있을 때
        if (UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
        {
            isTouched = true;
        }
#else // 구버전 인풋 시스템일 때
        if (Input.GetMouseButtonDown(0))
        {
            isTouched = true;
        }
#endif

        // 💡 터치가 감지되었고, 이동 중이 아닐 때 작동
        if (isTouched && !isMoving)
        {
            OnNextButtonClick();
            StartCoroutine(LoopSequenceRoutine());
        }
    }

    IEnumerator LoopSequenceRoutine()
    {
        isMoving = true;

        // 1️⃣ 오른쪽 눈속임용 화면으로 스륵 이동
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(basketFake_X, startPos.y, startPos.z);

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / moveDuration);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        transform.position = targetPos; // 도착 고정

        // 2️⃣ [오른쪽 도착 직후] Add Object 스크립트를 찾아 리셋 함수 작동시키기
        if (manjuuContainer != null)
        {
            // 💡 실제 국수님의 'Add Object' 스크립트 클래스 명으로 대체해 주세요.
            var spawnScript = manjuuContainer.GetComponent<AddObject>();
            if (spawnScript != null)
            {
                spawnScript.ResetAndRestart(); // 만쥬 싹 지우고 새로 만들기 시작!
            }
        }

        // 삭제와 생성이 안정적으로 들어갈 시간을 한 프레임 줍니다.
        yield return new WaitForEndOfFrame();

        // 3️⃣ 유저 몰래 원래 왼쪽(0, 0) 위치로 순간이동
        transform.position = new Vector3(basketOriginal_X, startPos.y, startPos.z);

        isMoving = false;
    }

    public void OnNextButtonClick()
    {
        // 🌟 [핵심 추가] 화면을 넘기기 전에 현재 소환 중인 루틴이 있다면 칼같이 정지하고 비웁니다.
        if (addObjectScript != null)
        {
            addObjectScript.ForceStopAndClear();
        }
    }
}
