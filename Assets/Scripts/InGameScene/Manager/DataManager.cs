using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager dataInstance;

    private void Awake()
    {
        if (dataInstance == null)
        {
            dataInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(dataInstance);
    }
}
