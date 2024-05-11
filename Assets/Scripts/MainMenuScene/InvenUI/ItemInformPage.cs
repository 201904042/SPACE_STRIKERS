using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInformPage : MonoBehaviour
{

    public GameObject upgradePageobj;
    public GameObject sellPageobj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackBtn()
    {
        gameObject.SetActive(false);
    }
    public void UpgradeBtn()
    {
        upgradePageobj.SetActive(true);
    }
    public void SellBtn()
    {
        sellPageobj.SetActive(true);
    }

}
