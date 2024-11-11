using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.MaterialProperty;


public class PoolManager 
{
    private const string PlayerDataPath = "Scriptable/PlayerProjData";
    private const string OtherDataPath = "Scriptable/OtherProjData";

    private const string PoolObjName = "Pools";
    private const string EnemyPoolName = "EnemyPool";
    private const string PlayerPoolName = "PlayerProjs";
    private const string OtherPoolName = "OtherProjs";

    //����� ������, Ÿ�԰� �������� ����
    public Dictionary<int, int> useEnemyList => GameManager.Game.Stage.enemyCodeAmountFair;
    public List<PlayerProjData> playerProjDataList;
    public List<OtherProjData> OtherProjDataList;

    public Transform otherProjPool;
    public Transform enemyPool;
    public Transform playerProjPool;

    public Dictionary<int , List<GameObject>> enemyDic = new Dictionary<int, List<GameObject>>();
    public Dictionary<PlayerProjType, List<GameObject>> playerProjDic = new Dictionary<PlayerProjType, List<GameObject>>();
    public Dictionary<OtherProjType, List<GameObject>> otherProjDic = new Dictionary<OtherProjType, List<GameObject>>();

    public void Init()
    {
        playerProjDataList = new List<PlayerProjData>();
        OtherProjDataList = new List<OtherProjData>();

        //�����غ��� �̸� �Ҵ��� �ʿ���� �ʿ��Ҷ� ��ųʸ��� ������ ������. üũ�Ұ�
        FindDataObject(PlayerDataPath, playerProjDataList);
        FindDataObject(OtherDataPath, OtherProjDataList);
        foreach (int id in useEnemyList.Keys)
        {
            enemyDic[id] = new List<GameObject>();
        }

        //InitializeDictionary(playerProjDic);
        //InitializeDictionary(otherProjDic);

        //Ǯ�� ������ ��ü���� �θ� ��ġ
        Transform Pools = GameObject.Find(PoolObjName).transform;
        otherProjPool = Pools.Find(OtherPoolName);
        enemyPool = Pools.Find(EnemyPoolName);
        playerProjPool = Pools.Find(PlayerPoolName);

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

    /// <summary>
    /// �÷��̾� �߻�ü�� ����
    /// </summary>
    public GameObject GetPlayerProj(PlayerProjType projType, Vector2 position, Quaternion rotation)
    {
        if (!playerProjDic.ContainsKey(projType))
        {
            //���ο� Ǯ ����
            playerProjDic.Add(projType, new List<GameObject>());
        }

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
                GameObject newObject = GameManager.InstantiateObject(proj.prefab, playerProjPool);
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

    /// <summary>
    /// ������ Ȥ�� ���� �߻�ü ���� ����
    /// </summary>
    public GameObject GetOtherProj(OtherProjType projType, Vector2 position, Quaternion rotation)
    {
        if (!otherProjDic.ContainsKey(projType))
        {
            //���ο� Ǯ ����
            otherProjDic.Add(projType, new List<GameObject>());
        }

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
                GameObject newObject = GameManager.InstantiateObject(projData.prefab, otherProjPool);
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

    /// <summary>
    /// �� ������Ʈ�� ����
    /// </summary>
    public GameObject GetEnemy(int id, Vector2 position, Quaternion rotation)
    {
        if (!enemyDic.ContainsKey(id))
        {
            //���ο� Ǯ ����
            enemyDic.Add(id, new List<GameObject>());
        }


        List<GameObject> objectList = enemyDic[id]; //�ش� Ÿ���� ����Ʈ�� ����
        foreach (GameObject obj in objectList) //����Ʈ�� ������Ʈ �˻�
        {
            if (!obj.activeInHierarchy) //��Ȱ��ȭ������ ������Ʈ�� �ִٸ� �ش� ������Ʈ ��ȯ
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                GameManager.Game.Spawn.activeEnemyList.Add(obj);
                return obj;
            }
        }

        // ��밡���� ������Ʈ�� ���ٸ� ����Ʈ�� �ش� ������Ʈ �߰�
        foreach (int enemyId in useEnemyList.Keys)
        {
            if (id == enemyId)
            {
                GameObject newObject = GameManager.InstantiateObject(GameManager.LoadFromResources<GameObject>(DataManager.enemy.GetData(id).path), enemyPool);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                newObject.SetActive(true);
                enemyDic[id].Add(newObject);
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

    //�ʿ���� ������Ʈ ����
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
