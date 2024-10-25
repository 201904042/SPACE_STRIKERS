using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.MaterialProperty;

public class PoolManager : MonoBehaviour
{
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
        enemyDic = new Dictionary<EnemyType, List<GameObject>>();
        playerProjDic = new Dictionary<PlayerProjType, List<GameObject>>();
        otherProjDic = new Dictionary<OtherProjType, List<GameObject>>();

        FindDataObject("Prefab/Scriptable/EnemyData", EnemyDataList);
        FindDataObject("Prefab/Scriptable/PlayerProjData", playerProjDataList);
        FindDataObject("Prefab/Scriptable/OtherProjData", OtherProjDataList);

        ClearAllDict();
        InitializeDictionary(enemyDic);
        InitializeDictionary(playerProjDic);
        InitializeDictionary(otherProjDic);

        Transform Pools = GameObject.Find("Pools").transform;
        otherProjPool = Pools.Find("OtherProjs");
        enemyPool = Pools.Find("EnemyPool");
        playerProjPool = Pools.Find("PlayerProjs");
        if(otherProjPool == null || enemyPool == null || playerProjPool == null)
        {
            Debug.LogError("Ǯ �����ȵ�");
        }

        Debug.Log("Ǯ�� �ʱ�ȭ �Ϸ�");
    }

    private void FindDataObject<T>(string path, List<T> targetList) where T : ScriptableObject
    {
        T[] datas = Resources.LoadAll<T>(path);
        foreach (T data in datas)
        {
            targetList.Add(data);
        }

        Debug.Log($"{typeof(T)} ��ũ���ͺ� ������Ʈ {targetList.Count}�� Ȯ��");
    }

    private void InitializeDictionary<T>(Dictionary<T, List<GameObject>> targetDict)
    {
        foreach (T type in Enum.GetValues(typeof(T)))
        {
            targetDict[type] = new List<GameObject>();
        }
    }

    public GameObject GetPlayerProj(PlayerProjType projType, Vector2 position, Quaternion rotation)
    {
        List<GameObject> objectList = playerProjDic[projType]; //�ش� Ÿ���� ����Ʈ�� ����
        foreach (GameObject obj in objectList) //����Ʈ�� ������Ʈ �˻�
        {
            if (!obj.activeInHierarchy) //��Ȱ��ȭ������ ������Ʈ�� �ִٸ� �ش� ������Ʈ ��ȯ
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
        }

        // ��밡���� ������Ʈ�� ���ٸ� ����Ʈ�� �ش� ������Ʈ �߰�
        foreach(PlayerProjData proj in playerProjDataList)
        {
            if(projType == proj.projType)
            {
                GameObject newObject = Instantiate(proj.prefab, playerProjPool);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                newObject.SetActive(true);
                playerProjDic[projType].Add(newObject);
                //Debug.Log($"{newObject} ����");
                return newObject;
            }
        }
        return null;
    }

    public GameObject GetOtherProj(OtherProjType projType, Vector2 position, Quaternion rotation)
    {
        List<GameObject> objectList = otherProjDic[projType]; //�ش� Ÿ���� ����Ʈ�� ����
        foreach (GameObject obj in objectList) //����Ʈ�� ������Ʈ �˻�
        {
            if (!obj.activeInHierarchy) //��Ȱ��ȭ������ ������Ʈ�� �ִٸ� �ش� ������Ʈ ��ȯ
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                //Debug.Log($"{obj} Ȱ��ȭ");
                return obj;
            }
        }

        // ��밡���� ������Ʈ�� ���ٸ� ����Ʈ�� �ش� ������Ʈ �߰�
        foreach (OtherProjData projData in OtherProjDataList)
        {
            if (projType == projData.projType)
            {
                GameObject newObject = Instantiate(projData.prefab, otherProjPool);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                newObject.SetActive(true);
                otherProjDic[projType].Add(newObject);
                //Debug.Log($"{newObject} ����");
                return newObject;
            }
        }
        return null;
    }

    public GameObject GetEnemy(int enemyId, Vector2 position, Quaternion rotation)
    {
        EnemyType enemyType;
        if (CheckEnemyDic(enemyId, out enemyType))
        {
            return ActivePooledEnemy(enemyType,enemyId, position, rotation);
        }

        Debug.LogWarning("�־��� id�� �ش��ϴ� ���� �����ϴ�.");
        return null;
    }

    private bool CheckEnemyDic(int enemyId, out EnemyType enemyType)
    {
        enemyType = enemyId switch
        {
            600 => EnemyType.SandBag,
            < 510 => EnemyType.Common,
            < 520 => EnemyType.Elite,
            < 530 => EnemyType.MidBoss,
            < 540 => EnemyType.Boss,
            _ => EnemyType.None
        };

        if (enemyType == EnemyType.None)
        {
            Debug.LogWarning("Id�� �ش��ϴ� Ÿ���� �����ϴ�");
            return false;
        }

        return true;
    }

    public GameObject ActivePooledEnemy(EnemyType enemyType, int enemyId, Vector2 position, Quaternion rotation)
    {
        List<GameObject> objectList = enemyDic[enemyType]; //�ش� Ÿ���� ����Ʈ�� ����
        foreach (GameObject obj in objectList) //����Ʈ�� ������Ʈ �˻�
        {
            if (!obj.activeInHierarchy) //��Ȱ��ȭ������ ������Ʈ�� �ִٸ� �ش� ������Ʈ ��ȯ
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.GetComponent<EnemyObject>().SetId(enemyId);
                //Debug.Log($"{obj} Ȱ��ȭ");
                obj.SetActive(true);
                GameManager.Instance.Spawn.activeEnemyList.Add(obj);
                return obj;
            }
        }

        // ��밡���� ������Ʈ�� ���ٸ� ����Ʈ�� �ش� ������Ʈ �߰�
        foreach (EnemyInfo enemyData in EnemyDataList)
        {
            if (enemyType == enemyData.enemyType)
            {
                GameObject newObject = Instantiate(enemyData.prefab, enemyPool);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                newObject.GetComponent<EnemyObject>().SetId(enemyId);
                newObject.SetActive(true);
                enemyDic[enemyType].Add(newObject);
                //Debug.Log($"{newObject} ����");
                GameManager.Instance.Spawn.activeEnemyList.Add(newObject);
                return newObject;
            }
        }
        return null; 
    }

    public void ReleasePool(GameObject gameObject)
    {
        if (GameManager.Instance.Spawn.activeEnemyList.Contains(gameObject))
        {
            GameManager.Instance.Spawn.activeEnemyList.Remove(gameObject);
        }
        
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
    }

    //�ʿ���� ������Ʈ ����
    public void DestroyPool(GameObject gameObject)
    {
        Destroy(gameObject);
    }


    public void ClearAllDict()
    {
        enemyDic.Clear();
        playerProjDic.Clear();
        otherProjDic.Clear();
    }
}
