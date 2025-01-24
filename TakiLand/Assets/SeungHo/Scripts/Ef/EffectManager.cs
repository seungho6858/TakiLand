using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; } // 싱글턴 패턴

    // 프리팹 이름별로 풀을 관리하는 딕셔너리
    private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        // 싱글턴 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 특정 프리팹 이름으로 효과를 소환합니다.
    /// </summary>
    public GameObject SpawnEffect(string prefabName, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(prefabName))
        {
            // 해당 이름의 풀이 없으면 새로 생성
            pools[prefabName] = new Queue<GameObject>();
        }

        GameObject effect;

        if (pools[prefabName].Count > 0)
        {
            // 풀에서 재사용 가능한 객체 가져오기
            effect = pools[prefabName].Dequeue();
        }
        else
        {
            // 풀이 비어 있으면 Resources에서 로드
            GameObject prefab = Resources.Load<GameObject>(prefabName);
            if (prefab == null)
            {
                Debug.LogError($"Prefab '{prefabName}' not found in Resources.");
                return null; // 프리팹이 없으면 null 반환
            }
            effect = Instantiate(prefab);
        }

        // 위치, 회전 설정 및 활성화
        effect.transform.position = position;
        effect.transform.rotation = rotation;
        effect.SetActive(true);

        return effect;
    }

    /// <summary>
    /// 특정 프리팹 이름의 효과를 풀로 반환합니다.
    /// </summary>
    public void ReturnEffect(string prefabName, GameObject effect)
    {
        if (!pools.ContainsKey(prefabName))
        {
            // 만약 프리팹 풀을 아직 생성하지 않았다면 생성
            pools[prefabName] = new Queue<GameObject>();
        }

        effect.SetActive(false); // 비활성화
        pools[prefabName].Enqueue(effect); // 풀에 추가
    }
}
