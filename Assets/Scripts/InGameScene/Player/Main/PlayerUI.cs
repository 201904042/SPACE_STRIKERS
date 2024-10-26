using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private Slider hpSlider;
    private int MaxHp;
    private int curHp;
    private Slider expSlider;
    private int MaxExp;
    private int curExp;
    private Slider powSlider;
    private int MaxPow;
    private int curPow;
    private TextMeshProUGUI powLvText;

    private TextMeshProUGUI specialCountText;
    private Image specialImage;

    private void Awake()
    {
        Transform Canvas = FindObjectOfType<Canvas>().transform;
        Transform playerUI = Canvas.Find("PlayerUI").transform;
        hpSlider = transform.GetChild(0).GetComponent<Slider>();
        expSlider = transform.GetChild(1).GetComponent<Slider>();
        powSlider = transform.GetChild(2).GetComponent<Slider>();
        powLvText = powSlider.GetComponentInChildren<TextMeshProUGUI>();

        specialImage = transform.GetChild(3).GetChild(1).GetComponent<Image>();
        specialCountText = transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
    }

}
