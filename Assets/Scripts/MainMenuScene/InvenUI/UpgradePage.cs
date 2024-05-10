using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void cancelBtn()
    {
        gameObject.SetActive(false);
    }

    public void acceptBtn() {
        //강화 진행

        gameObject.SetActive(false);
    }
}
