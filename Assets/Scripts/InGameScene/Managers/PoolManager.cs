using System;
using System.Collections.Generic;
using UnityEngine;


public class PoolManager 
{
    private const string EnemyDataPath = "Prefab/Scriptable/EnemyData";
    private const string PlayerDataPath = "Prefab/Scriptable/PlayerProjData";
    private const string OtherDataPath = "Prefab/Scriptable/OtherProjData";

    private const string PoolObjName = "Pools";
    private const string EnemyPoolName = "EnemyPool";
    private const string PlayerPoolName = "PlayerProjs";
    private const string OtherPoolName = "OtherProjs";

    //등록할 데이터, 타입과 프리팹을 연결
    public List<EnemyInfo> EnemyDataList;
    public List<PlayerProjData> playerProjDataList;
    public List<OtherProjData> OtherProjDataList;

    public Transform otherProjPool;
    public Transform enemyPool;
    public Transform playerProjPool;

    public Dictionary<EnemyType, List<GameObject>> enemyDic = new Dictionary<EnemyType, List<GameObject>>();
    public Dictionary<PlayerProjType, List<GameObject>> playerProjDic = new Dictionary<PlayerProjType, List<GameObject>>();
    public Dictionary<OtherProjType, List<GameObject>> otherProjDic = new Dictionary<OtherProjType, List<GameObject>>();

    public void Init()
    {
        EnemyDataList = new List<EnemyInfo>();
        playerProjDataList = new List<PlayerProjData>();
        OtherProjDataList = new List<OtherProjData>();

        FindDataObject(EnemyDataPath, EnemyDataList);
        FindDataObject(PlayerDataPath, playerProjDataList);
        FindDataObject(OtherDataPath, OtherProjDataList);

        InitializeDictionary(enemyDic);
        InitializeDictionary(playerProjDic);
        InitializeDictionary(otherProjDic);

        //풀로 생성된 객체들의 부모 위치
        Transform Pools = GameObject.Find(PoolObjName).transform;
        otherProjPool = Pools.Find(OtherPoolName);
        enemyPool = Pools.Find(EnemyPoolName);
        playerProjPool = Pools.Find(PlayerPoolName);

        if(otherProjPool == null || enemyPool == null || playerProjPool == null)
        {
            Debug.LogError("풀 지정안됨");
        }

        Debug.Log("풀링 초기화 완료");
    }

    private void FindDataObject<T>(string path, List<T> targetList) where T : ScriptableObject
    {
        
        T[] datas = Resources.LoadAll<T>(path);
        foreach (T data in datas)
        {
            targetList.Add(data);
        }

        Debug.Log($"{typeof(T)} 스크리터블 오브젝트 {targetList.Count}개 확인");
    }

    private void InitializeDictionary<T>(Dictionary<T, List<GameObject>> targetDict)
    {
        foreach (T type in Enum.GetValues(typeof(T)))
        {
            targetDict[type] = new List<GameObject>();
        }
    }

    /// <summary>
    /// 플레이어 발사체를 생성
    /// </summary>
    public GameObject GetPlayerProj(PlayerProjType projType, Vector2 position, Quaternion rotation)
    {
        List<GameObject> objectList = playerProjDic[projType]; //해당 타입의 리스트를 지정
        foreach (GameObject obj in objectList) //리스트에 오브젝트 검색
        {
            if (!obj.activeInHierarchy) //비활성화상태인 오브젝트가 있다면 해당 오브젝트 반환
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
        }

        // 사용가능한 오브젝트가 없다면 리스트에 해당 오브젝트 추가
        foreach(PlayerProjData proj in playerProjDataList)
        {
            if(projType == proj.projType)
            {
                GameObject newObject = GameManager.InstantObject(proj.prefab, playerProjPool);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                newObject.SetActive(true);
                playerProjDic[projType].Add(newObject);
                //Debug.Log($"{newObject} 생성");
                return newObject;
            }
        }
        return null;
    }

    /// <summary>
    /// 아이템 혹은 적의 발사체 등을 생성
    /// </summary>
    public GameObject GetOtherProj(OtherProjType projType, Vector2 position, Quaternion rotation)
    {
        List<GameObject> objectList = otherProjDic[projType]; //해당 타입의 리스트를 지정
        foreach (GameObject obj in objectList) //리스트에 오브젝트 검색
        {
            if (!obj.activeInHierarchy) //비활성화상태인 오브젝트가 있다면 해당 오브젝트 반환
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                //Debug.Log($"{obj} 활성화");
                return obj;
            }
        }

        // 사용가능한 오브젝트가 없다면 리스트에 해당 오브젝트 추가
        foreach (OtherProjData projData in OtherProjDataList)
        {
            if (projType == projData.projType)
            {
                GameObject newObject = GameManager.InstantObject(projData.prefab, otherProjPool);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                newObject.SetActive(true);
                otherProjDic[projType].Add(newObject);
                //Debug.Log($"{newObject} 생성");
                return newObject;
            }
        }
        return null;
    }

    /// <summary>
    /// 적 오브젝트를 생성
    /// </summary>
    public GameObject GetEnemy(EnemyType type, Vector2 position, Quaternion rotation)
    {
        List<GameObject> objectList = enemyDic[type]; //해당 타입의 리스트를 지정
        foreach (GameObject obj in objectList) //리스트에 오브젝트 검색
        {
            if (!obj.activeInHierarchy) //비활성화상태인 오브젝트가 있다면 해당 오브젝트 반환
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                GameManager.Game.Spawn.activeEnemyList.Add(obj);
                return obj;
            }
        }

        // 사용가능한 오브젝트가 없다면 리스트에 해당 오브젝트 추가
        foreach (EnemyInfo enemyData in EnemyDataList)
        {
            if (type == enemyData.enemyType)
            {
                GameObject newObject = GameManager.InstantObject(enemyData.prefab, enemyPool);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                newObject.SetActive(true);
                enemyDic[type].Add(newObject);
                GameManager.Game.Spawn.activeEnemyList.Add(newObject);
                return newObject;
            }
        }
        return null; 
    }


    public void ReleasePool(GameObject gameObject)
    {
        if (GameManager.Game.Spawn.activeEnemyList.Contains(gameObject)) 
        {
            GameManager.Game.Spawn.activeEnemyList.Remove(gameObject);
        }
        
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
    }

    //필요없는 오브젝트 제거
    public void DestroyPool(GameObject gameObject)
    {
        GameManager.DestroyObject(gameObject);
    }


    public void ClearAllDict()
    {
        enemyDic.Clear();
        playerProjDic.Clear();
        otherProjDic.Clear();
    }
}
