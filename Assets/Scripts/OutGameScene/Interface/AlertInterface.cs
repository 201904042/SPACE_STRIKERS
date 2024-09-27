using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertInterface : UIInterface
{
    public TextMeshProUGUI alertText;
    public Button cancelBtn;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetComponent()
    {
        base.SetComponent();
        alertText = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        cancelBtn = transform.GetChild(3).GetComponent<Button>();
        alertText.text = "알림입니다";

        cancelBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.AddListener(CloseInterface);
    }

    //얘는 딱히 필요없을듯
    //public override IEnumerator GetValue()
    //{
    //    return base.GetValue();
    //}

    public void SetAlert(string alertContent)
    {
        alertText.text = alertContent;
        gameObject.SetActive(true);
    }
}

