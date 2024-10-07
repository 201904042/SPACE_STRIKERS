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
    public List<EnemyInfo> enemyDataList;
    public List<SkillProjData> skillDataList;
    public List<ProjData> projDataList;

    public Transform projPool;
    public Transform enemyPool;
    public Transform skillPool;

    public Dictionary<EnemyType, List<GameObject>> enemyDic = new Dictionary<EnemyType, List<GameObject>>();
    public Dictionary<SkillProjType, List<GameObject>> skillDic = new Dictionary<SkillProjType, List<GameObject>>();
    public Dictionary<ProjType, List<GameObject>> projDic = new Dictionary<ProjType, List<GameObject>>();

    public void Init()
    {
        enemyDataList = new List<EnemyInfo>();
        skillDataList = new List<SkillProjData>();
        projDataList = new List<ProjData>();
        enemyDic = new Dictionary<EnemyType, List<GameObject>>();
        skillDic = new Dictionary<SkillProjType, List<GameObject>>();
        projDic = new Dictionary<ProjType, List<GameObject>>();

        FindDataObject("Prefab/Scriptable/EnemyData", enemyDataList);
        FindDataObject("Prefab/Scriptable/SkillData", skillDataList);
        FindDataObject("Prefab/Scriptable/ProjData", projDataList);
        ClearAllDict();
        InitializeDictionary(enemyDic);
        InitializeDictionary(skillDic);
        InitializeDictionary(projDic);

        projPool = GameObject.Find("ProjPool").transform;
        enemyPool = GameObject.Find("EnemyPool").transform;
        skillPool = GameObject.Find("SkillPool").transform;
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
        foreach(SkillProjData skillData in skillDataList)
        {
            if(skillType == skillData.skillType)
            {
                GameObject newObject = Instantiate(skillData.prefab);
                newObject.transform.SetParent(skillPool);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                newObject.SetActive(true);
                skillDic[skillType].Add(newObject);
                //Debug.Log($"{newObject} ����");
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
                //Debug.Log($"{obj} Ȱ��ȭ");
                return obj;
            }
        }

        // ��밡���� ������Ʈ�� ���ٸ� ����Ʈ�� �ش� ������Ʈ �߰�
        foreach (ProjData projData in projDataList)
        {
            if (projType == projData.projType)
            {
                GameObject newObject = Instantiate(projData.prefab);
                newObject.transform.SetParent(projPool);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                newObject.SetActive(true);
                projDic[projType].Add(newObject);
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
            0 => EnemyType.SandBag,
            < 10 => EnemyType.Common,
            < 20 => EnemyType.Elite,
            < 30 => EnemyType.MidBoss,
            < 40 => EnemyType.Boss,
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
                if(enemyType == EnemyType.Common)
                {
                    obj.GetComponent<Enemy_Common>().enemyStat.enemyId = enemyId;
                }
                else if (enemyType == EnemyType.Elite)
                {
                    obj.GetComponent<Enemy_Elite>().enemyStat.enemyId = enemyId;
                }
                /*
                else if (enemyType == EnemyType.Common)
                {
                    obj.GetComponent<Enemy_Common>().enemyStat.enemyCode = enemyCode;
                }*/
                else if (enemyType == EnemyType.Boss)
                {
                    obj.GetComponent<Enemy_Boss>().enemyStat.enemyId = enemyId;
                }
                //Debug.Log($"{obj} Ȱ��ȭ");
                obj.SetActive(true);
                Managers.Instance.Spawn.activeEnemyList.Add(obj);
                return obj;
            }
        }

        // ��밡���� ������Ʈ�� ���ٸ� ����Ʈ�� �ش� ������Ʈ �߰�
        foreach (EnemyInfo enemyData in enemyDataList)
        {
            if (enemyType == enemyData.enemyType)
            {
                GameObject newObject = Instantiate(enemyData.prefab);
                newObject.transform.SetParent(enemyPool);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                if (enemyType == EnemyType.Common)
                {
                    newObject.GetComponent<Enemy_Common>().enemyStat.enemyId = enemyId;
                }
                else if (enemyType == EnemyType.Elite)
                {
                    newObject.GetComponent<Enemy_Elite>().enemyStat.enemyId = enemyId;
                }
                /*
                else if (enemyType == EnemyType.Common)
                {
                    newObject.GetComponent<Enemy_Common>().enemyStat.enemyCode = enemyCode;
                }*/
                else if (enemyType == EnemyType.Boss)
                {
                    newObject.GetComponent<Enemy_Boss>().enemyStat.enemyId = enemyId;
                }
                newObject.SetActive(true);
                enemyDic[enemyType].Add(newObject);
                //Debug.Log($"{newObject} ����");
                Managers.Instance.Spawn.activeEnemyList.Add(newObject);
                return newObject;
            }
        }
        return null; 
    }

    public void ReleasePool(GameObject gameObject)
    {
        if (Managers.Instance.Spawn.activeEnemyList.Contains(gameObject))
        {
            Managers.Instance.Spawn.activeEnemyList.Remove(gameObject);
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
        skillDic.Clear();
        projDic.Clear();
    }
}
