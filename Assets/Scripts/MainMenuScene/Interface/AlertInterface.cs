using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertInterface : MonoBehaviour
{
    public TextMeshProUGUI alertText;
    public Button cancelBtn;


    public void Awake()
    {
        alertText = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        cancelBtn = transform.GetChild(3).GetComponent<Button>();
        alertText.text = "알림입니다";

        cancelBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.AddListener(CloseInterface);
    }

    private void CloseInterface()
    {
        gameObject.SetActive(false);
    }

    public void SetAlert(string alertContent)
    {
        alertText.text = alertContent;
        gameObject.SetActive(true);
    }
}

