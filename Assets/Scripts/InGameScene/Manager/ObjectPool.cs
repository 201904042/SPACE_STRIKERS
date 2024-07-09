using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool poolInstance;

    public List<GameObject> poolObjectList;
    public GameObject poolObj;
    public int poolSize;


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
        poolObjectList = new List<GameObject>();
        GameObject newObject;

        for(int i=0;i<poolSize;i++)
        {
            newObject = Instantiate(poolObj);
            newObject.SetActive(false);
            poolObjectList.Add( newObject );
        }
    }
}
