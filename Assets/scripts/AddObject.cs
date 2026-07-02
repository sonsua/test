using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObject : MonoBehaviour
{
    public float spawnTime = 1f;
    public List<GameObject> objList = new List<GameObject>();

    [Header("소환 오브젝트 크기 설정")]
    [SerializeField] private Vector3 spawnScale = Vector3.one;

    // 🌟 [추가] 현재 만쥬를 소환 중인지 체크하는 상태 변수
    public bool isSpawning { get; private set; } = false;

    // 🌟 현재 실행 중인 코루틴을 기억하고 제어하기 위한 변수
    private Coroutine spawnCoroutine;

    void Start()
    {
        Application.targetFrameRate = 60;

        ShuffleList(objList);
        // 처음 시작할 때 코루틴을 변수에 담아 실행합니다.
        spawnCoroutine = StartCoroutine(SpawnObj());
    }

    IEnumerator SpawnObj()
    {
        isSpawning = true;
        for (int i = 0; i < objList.Count; i++)
        {
            GameObject prefab = objList[i];
            GameObject newObj = Instantiate(prefab, new Vector3(0, 7, 0), Quaternion.identity);
            newObj.transform.parent = this.transform;

            newObj.transform.localScale = spawnScale;

            yield return new WaitForSeconds(spawnTime);
        }

        // 생성이 모두 끝나면 변수를 비워줍니다.
        spawnCoroutine = null;
        isSpawning = false;
    }

    public void ResetAndRestart()
    {
        // 1. 만약 이미 생성 중인 코루틴이 있다면 강제로 멈춥니다.
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        // 2. 이 스크립트가 붙은 오브젝트(GameObject)의 모든 자식(만쥬)을 삭제
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        ShuffleList(objList);
        // 3. 🌟 [수정] 즉시 생성하지 않고, 1초 대기 후 소환하는 별도의 코루틴을 실행합니다.
        isSpawning = true;
        spawnCoroutine = StartCoroutine(ResetDelayRoutine());
    }

    // 🌟 1초 쉬었다가 생성을 시작하게 만들어주는 새로운 코루틴입니다.
    IEnumerator ResetDelayRoutine()
    {
        // 화면 이동 직후 원하는 대기 시간 (1초)만큼 멈춥니다.
        yield return new WaitForSeconds(0.5f);

        // 대기가 끝나면 기존의 생성 루프(SpawnObj)를 이어서 실행합니다.
        yield return StartCoroutine(SpawnObj());
    }

    private void ShuffleList(List<GameObject> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);

            // i번째와 rnd(랜덤)번째의 위치를 서로 맞바꿉니다.
            GameObject temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }
    public void ForceStopAndClear()
    {
        // 1. 현재 돌고 있는 모든 소환 루틴을 칼같이 중단시킵니다.
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        // 2. 소환 중이라는 상태도 확실하게 꺼줍니다.
        isSpawning = false;

        // 3. 생성되어 바구니에 담겨있던 만쥬들을 흔적도 없이 전부 파괴합니다.
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        Debug.Log("만쥬 소환이 강제 중단되었고 목록이 초기화되었습니다!");
    }
}
