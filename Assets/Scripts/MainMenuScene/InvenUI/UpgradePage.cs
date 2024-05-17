using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePage : MonoBehaviour
{
    public Image upgradImage;
    public TextMeshProUGUI upgradeIngred;
    public TextMeshProUGUI upgradeExplain;

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
