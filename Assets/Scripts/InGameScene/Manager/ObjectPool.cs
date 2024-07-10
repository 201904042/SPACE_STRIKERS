using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.MaterialProperty;

public enum PoolType
{
    None,
    Proj,
    Enemy
}
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool poolInstance;
    public List<PrefabTypeAsset> prefabAsset;

    // Dictionary to hold lists of pooled objects by enemy type
    public Dictionary<EnemyPoolType, List<GameObject>> enemyPoolList = new Dictionary<EnemyPoolType, List<GameObject>>();
    public Dictionary<ProjPoolType, List<GameObject>> projPoolList = new Dictionary<ProjPoolType, List<GameObject>>();
    public GameObject sandBag;
    public GameObject enemy_Common;
    public GameObject enemy_Elite;
    public GameObject enemy_MiddleBoss;
    public GameObject enemy_Boss;

    public int basicPoolSize = 1;

    private void Awake()
    {
        if (poolInstance == null)
        {
            poolInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Init();
        BasicPoolObj();
    }

    private void Init()
    {
        //Dictionary�� �� List ����
        foreach (EnemyPoolType type in System.Enum.GetValues(typeof(EnemyPoolType)))
        {
            enemyPoolList[type] = new List<GameObject>();
        }
        foreach (ProjPoolType type in System.Enum.GetValues(typeof(ProjPoolType)))
        {
            projPoolList[type] = new List<GameObject>();
        }
    }

    public void BasicPoolObj()
    {
        for (int i = 0; i < basicPoolSize * 5; i++)
        {
            //CreatePooledObj(EnemyPoolType.Common, enemy_Common);
        }
        for (int i = 0; i < basicPoolSize * 3; i++)
        {
            //CreatePooledObj(EnemyPoolType.Elite, enemy_Elite);
        }
        //CreatePooledObj(EnemyPoolType.MidBoss, enemy_MiddleBoss);
        //CreatePooledObj(EnemyPoolType.Boss, enemy_Boss);
    }

    private void CreatePooledObj(EnemyPoolType EnemyPoolType, GameObject gameObject)
    {
        //������Ʈ�� �����ϰ� ��Ȱ��ȭ �Ѵ��� �ش� Ÿ���� ����Ʈ�� �߰�
        GameObject newObject = Instantiate(gameObject);
        newObject.SetActive(false);
        enemyPoolList[EnemyPoolType].Add(newObject);
    }

    public GameObject GetProjPool(ProjPoolType projType, Vector2 position, Quaternion rotation)
    {
        List<GameObject> objectList = projPoolList[projType]; //�ش� Ÿ���� ����Ʈ�� ����
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
        foreach(PrefabTypeAsset prefabType in prefabAsset)
        {
            if(projType == prefabType.projPoolType)
            {
                GameObject newObject = Instantiate(prefabType.prefab);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                newObject.SetActive(true);
                projPoolList[projType].Add(newObject);
                return newObject;
            }
            else
            {
                Debug.LogError("�ش� Ÿ���� projTypeAsset�� �������� �ʽ��ϴ�");
            }
        }
        return null;
    }

    public GameObject GetEnemyPool(int enemyId, Vector2 position, Quaternion rotation)
    {
        EnemyPoolType EnemyPoolType;
        if (CheckExistInDictionary(enemyId, out EnemyPoolType))
        {
            return ActivePooledEnemy(EnemyPoolType,enemyId, position, rotation);
        }

        Debug.LogError("�־��� id�� �ش��ϴ� ���� �����ϴ�.");
        return null;
    }

    private bool CheckExistInDictionary(int enemyId, out EnemyPoolType EnemyPoolType)
    {
        if (enemyId == 0)
        {
            EnemyPoolType = EnemyPoolType.SandBag;
            return true;
        }
        else if (enemyId > 0 && enemyId < 100)
        {
            EnemyPoolType = EnemyPoolType.Common;
            return true;
        }
        else if (enemyId >= 100 && enemyId < 200)
        {
            EnemyPoolType = EnemyPoolType.Elite;
            return true;
        }
        else if (enemyId >= 200 && enemyId < 300)
        {
            EnemyPoolType = EnemyPoolType.MidBoss;
            return true;
        }
        else if (enemyId >= 300 && enemyId < 400)
        {
            EnemyPoolType = EnemyPoolType.Boss;
            return true;
        }

        EnemyPoolType = EnemyPoolType.None;
        Debug.LogWarning("Id�� �ش��ϴ� Ÿ���� �����ϴ�");
        return false;
    }

    public GameObject ActivePooledEnemy(EnemyPoolType EnemyPoolType, int enemyId, Vector2 position, Quaternion rotation)
    {
        List<GameObject> objectList = enemyPoolList[EnemyPoolType]; //�ش� Ÿ���� ����Ʈ�� ����
        foreach (GameObject obj in objectList) //����Ʈ�� ������Ʈ �˻�
        {
            if (!obj.activeInHierarchy) //��Ȱ��ȭ������ ������Ʈ�� �ִٸ� �ش� ������Ʈ ��ȯ
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.GetComponent<EnemyObject>().enemyStat.enemyId = enemyId;
                obj.SetActive(true);
                return obj;
            }
        }

        // ��밡���� ������Ʈ�� ���ٸ� ����Ʈ�� �ش� ������Ʈ �߰�
        foreach (PrefabTypeAsset prefabType in prefabAsset)
        {
            if (EnemyPoolType == prefabType.enemyPoolType)
            {
                GameObject newObject = Instantiate(prefabType.prefab);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                newObject.GetComponent<EnemyObject>().curEnemyId = enemyId;
                newObject.SetActive(true);
                enemyPoolList[EnemyPoolType].Add(newObject);
                return newObject;
            }
            else
            {
                Debug.Log("�ش� Ÿ���� projTypeAsset�� �������� �ʽ��ϴ�");
            }
        }
        return null;
        
        
    }

    public void ReleasePool(GameObject gameObject)
    {
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
    }

    //�ʿ���� ������Ʈ ����
    public void DestroyPool(GameObject gameObject)
    {
        Destroy(gameObject);
    }

}
