using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.MaterialProperty;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool poolInstance;
    public List<EnemyData> enemyDataList;
    public List<SkillProjData> SkillDataList;
    public List<ProjData> ProjDataList;

    // Dictionary to hold lists of pooled objects by enemy type
    public Dictionary<EnemyType, List<GameObject>> enemyDic = new Dictionary<EnemyType, List<GameObject>>();
    public Dictionary<SkillProjType, List<GameObject>> skillDic = new Dictionary<SkillProjType, List<GameObject>>();
    public Dictionary<ProjType, List<GameObject>> projDic = new Dictionary<ProjType, List<GameObject>>();

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
    }

    private void Init()
    {
        //Dictionary�� �� List ����
        foreach (EnemyType type in System.Enum.GetValues(typeof(EnemyType)))
        {
            enemyDic[type] = new List<GameObject>();
        }
        foreach (SkillProjType type in System.Enum.GetValues(typeof(SkillProjType)))
        {
            skillDic[type] = new List<GameObject>();
        }
        foreach (ProjType type in System.Enum.GetValues(typeof(ProjType)))
        {
            projDic[type] = new List<GameObject>();
        }
    }

    public GameObject GetSkill(SkillProjType skillType, Vector2 position, Quaternion rotation)
    {
        List<GameObject> objectList = skillDic[skillType]; //�ش� Ÿ���� ����Ʈ�� ����
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
        foreach(SkillProjData skillData in SkillDataList)
        {
            if(skillType == skillData.skillType)
            {
                GameObject newObject = Instantiate(skillData.prefab);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                newObject.SetActive(true);
                skillDic[skillType].Add(newObject);
                Debug.Log($"{newObject} ����");
                return newObject;
            }
        }
        return null;
    }

    public GameObject GetProj(ProjType projType, Vector2 position, Quaternion rotation)
    {
        List<GameObject> objectList = projDic[projType]; //�ش� Ÿ���� ����Ʈ�� ����
        foreach (GameObject obj in objectList) //����Ʈ�� ������Ʈ �˻�
        {
            if (!obj.activeInHierarchy) //��Ȱ��ȭ������ ������Ʈ�� �ִٸ� �ش� ������Ʈ ��ȯ
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                Debug.Log($"{obj} Ȱ��ȭ");
                return obj;
            }
        }

        // ��밡���� ������Ʈ�� ���ٸ� ����Ʈ�� �ش� ������Ʈ �߰�
        foreach (ProjData projData in ProjDataList)
        {
            if (projType == projData.projType)
            {
                GameObject newObject = Instantiate(projData.prefab);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                newObject.SetActive(true);
                projDic[projType].Add(newObject);
                Debug.Log($"{newObject} ����");
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
        if (enemyId == 0)
        {
            enemyType = EnemyType.SandBag;
            return true;
        }
        else if (enemyId > 0 && enemyId < 100)
        {
            enemyType = EnemyType.Common;
            return true;
        }
        else if (enemyId >= 100 && enemyId < 200)
        {
            enemyType = EnemyType.Elite;
            return true;
        }
        else if (enemyId >= 200 && enemyId < 300)
        {
            enemyType = EnemyType.MidBoss;
            return true;
        }
        else if (enemyId >= 300 && enemyId < 400)
        {
            enemyType = EnemyType.Boss;
            return true;
        }

        enemyType = EnemyType.None;
        Debug.LogWarning("Id�� �ش��ϴ� Ÿ���� �����ϴ�");
        return false;
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
                if(enemyType == EnemyType.Common)
                {
                    obj.GetComponent<Enemy_Common>().enemyStat.enemyId = enemyId;
                }
                /* ������ �� �ۼ��� �ݿ�
                else if (enemyType == EnemyType.Elite)
                {
                    obj.GetComponent<Enemy_Elite>().enemyStat.enemyId = enemyId;
                }
                else if (enemyType == EnemyType.Common)
                {
                    obj.GetComponent<Enemy_Common>().enemyStat.enemyId = enemyId;
                }
                else if (enemyType == EnemyType.Common)
                {
                    obj.GetComponent<Enemy_Common>().enemyStat.enemyId = enemyId;
                }*/
                Debug.Log($"{obj} Ȱ��ȭ");
                obj.SetActive(true);
                SpawnManager.spawnInstance.activeEnemyList.Add(obj);
                return obj;
            }
        }

        // ��밡���� ������Ʈ�� ���ٸ� ����Ʈ�� �ش� ������Ʈ �߰�
        foreach (EnemyData enemyData in enemyDataList)
        {
            if (enemyType == enemyData.enemyType)
            {
                GameObject newObject = Instantiate(enemyData.prefab);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                if (enemyType == EnemyType.Common)
                {
                    newObject.GetComponent<Enemy_Common>().enemyStat.enemyId = enemyId;
                }
                /* ������ �� �ۼ��� �ݿ�
                else if (enemyType == EnemyType.Elite)
                {
                    newObject.GetComponent<Enemy_Elite>().enemyStat.enemyId = enemyId;
                }
                else if (enemyType == EnemyType.Common)
                {
                    newObject.GetComponent<Enemy_Common>().enemyStat.enemyId = enemyId;
                }
                else if (enemyType == EnemyType.Common)
                {
                    newObject.GetComponent<Enemy_Common>().enemyStat.enemyId = enemyId;
                }*/
                newObject.SetActive(true);
                enemyDic[enemyType].Add(newObject);
                Debug.Log($"{newObject} ����");
                SpawnManager.spawnInstance.activeEnemyList.Add(newObject);
                return newObject;
            }
        }
        return null; 
    }

    public void ReleasePool(GameObject gameObject)
    {
        if (SpawnManager.spawnInstance.activeEnemyList.Contains(gameObject))
        {
            SpawnManager.spawnInstance.activeEnemyList.Remove(gameObject);
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

}
