using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Parent : MonoBehaviour
{
    public AccountJsonReader data;
    [HideInInspector]
    public Transform UIs;
    [HideInInspector]
    public GameObject Main;
    [HideInInspector]
    public GameObject Planet;
    [HideInInspector]
    public GameObject Stage;
    [HideInInspector]
    public GameObject Ready;
    [HideInInspector]
    public GameObject Store;
    [HideInInspector]
    public GameObject Inven;

    protected virtual void Awake()
    {
        UIs = GameObject.Find("UIs").transform;
        data = GameObject.Find("UIManager").GetComponent<AccountJsonReader>();
        Main = UIs.GetChild(0).gameObject;
        Planet = UIs.GetChild(1).gameObject;
        Stage = UIs.GetChild(2).gameObject;
        Ready = UIs.GetChild(3).gameObject;
        Store = UIs.GetChild(4).gameObject;
        Inven = UIs.GetChild(5).gameObject;
    }

}
