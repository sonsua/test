using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObject : MonoBehaviour
{
    public float spawnTime = 1f;

    public List<GameObject> objList = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnObj());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator SpawnObj()
    {
        for (int i = 0; i < objList.Count; i++)
        {
            GameObject prefab = objList[i];
            GameObject newObj = Instantiate(prefab, new Vector3(0, 7, 0), Quaternion.identity);

            // 생성된 오브젝트의 부모를 이 오브젝트로 설정
            newObj.transform.parent = this.transform;

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
