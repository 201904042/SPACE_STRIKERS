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
        //Dictionary에 새 List 생성
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
        List<GameObject> objectList = skillDic[skillType]; //해당 타입의 리스트를 지정
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
        foreach(SkillProjData skillData in SkillDataList)
        {
            if(skillType == skillData.skillType)
            {
                GameObject newObject = Instantiate(skillData.prefab);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                newObject.SetActive(true);
                skillDic[skillType].Add(newObject);
                //Debug.Log($"{newObject} 생성");
                return newObject;
            }
        }
        return null;
    }

    public GameObject GetProj(ProjType projType, Vector2 position, Quaternion rotation)
    {
        List<GameObject> objectList = projDic[projType]; //해당 타입의 리스트를 지정
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
        foreach (ProjData projData in ProjDataList)
        {
            if (projType == projData.projType)
            {
                GameObject newObject = Instantiate(projData.prefab);
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                newObject.SetActive(true);
                projDic[projType].Add(newObject);
                //Debug.Log($"{newObject} 생성");
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

        Debug.LogWarning("주어진 id에 해당하는 적이 없습니다.");
        return null;
    }

    private bool CheckEnemyDic(int enemyId, out EnemyType enemyType)
    {
        if (enemyId == 0)
        {
            enemyType = EnemyType.SandBag;
            return true;
        }
        else if (enemyId > 0 && enemyId < 10)
        {
            enemyType = EnemyType.Common;
            return true;
        }
        else if (enemyId >= 10 && enemyId < 20)
        {
            enemyType = EnemyType.Elite;
            return true;
        }
        else if (enemyId >= 20 && enemyId < 30)
        {
            enemyType = EnemyType.MidBoss;
            return true;
        }
        else if (enemyId >= 30 && enemyId < 40)
        {
            enemyType = EnemyType.Boss;
            return true;
        }

        enemyType = EnemyType.None;
        Debug.LogWarning("Id에 해당하는 타입이 없습니다");
        return false;
    }

    public GameObject ActivePooledEnemy(EnemyType enemyType, int enemyId, Vector2 position, Quaternion rotation)
    {
        List<GameObject> objectList = enemyDic[enemyType]; //해당 타입의 리스트를 지정
        foreach (GameObject obj in objectList) //리스트에 오브젝트 검색
        {
            if (!obj.activeInHierarchy) //비활성화상태인 오브젝트가 있다면 해당 오브젝트 반환
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
                    obj.GetComponent<Enemy_Common>().enemyStat.enemyId = enemyId;
                }
                else if (enemyType == EnemyType.Common)
                {
                    obj.GetComponent<Enemy_Common>().enemyStat.enemyId = enemyId;
                }*/
                //Debug.Log($"{obj} 활성화");
                obj.SetActive(true);
                SpawnManager.spawnInstance.activeEnemyList.Add(obj);
                return obj;
            }
        }

        // 사용가능한 오브젝트가 없다면 리스트에 해당 오브젝트 추가
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
                else if (enemyType == EnemyType.Elite)
                {
                    newObject.GetComponent<Enemy_Elite>().enemyStat.enemyId = enemyId;
                }
                /*
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
                //Debug.Log($"{newObject} 생성");
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

    //필요없는 오브젝트 제거
    public void DestroyPool(GameObject gameObject)
    {
        
        Destroy(gameObject);
    }

}
