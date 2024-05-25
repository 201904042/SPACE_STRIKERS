using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPowBar : MonoBehaviour
{
    GameObject player;
    PlayerSpecialSkill playerSpecialSkill;
    Slider hp_bar;
    Transform fill;
    Image fillImage;
    TextMeshProUGUI pow_text;

    private int powerLevel;
    private float maxPlayerTime;
    private float curPlayerTime;
    private void Awake()
    {
        player = GameObject.Find("Player");
        playerSpecialSkill = player.GetComponent<PlayerSpecialSkill>();
        fill = transform.GetChild(1).GetChild(0);
        fillImage = fill.GetComponent<Image>();
        pow_text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        powerLevel = playerSpecialSkill.powerLevel;
        maxPlayerTime = playerSpecialSkill.powerIncreaseMax;
        curPlayerTime = playerSpecialSkill.powerIncrease;
        hp_bar = transform.GetComponent<Slider>();

    }

    private void Update()
    {
        powerLevel = playerSpecialSkill.powerLevel;
        maxPlayerTime = playerSpecialSkill.powerIncreaseMax;
        curPlayerTime = playerSpecialSkill.powerIncrease;

        hp_bar.value = curPlayerTime / maxPlayerTime;
        if(powerLevel == 0)
        {
            fillImage.color = Color.white;
            pow_text.text = "POW Lv 0";
        }
        else if(powerLevel == 1)
        {
            fillImage.color = Color.green;
            pow_text.text = "POW Lv 1";
        }
        else if (powerLevel == 2)
        {
            fillImage.color = Color.yellow;
            pow_text.text = "POW Lv 2";
        }
        else if (powerLevel == 3)
        {
            fillImage.color = Color.red;
            pow_text.text = "POW Lv MAX";
        }

    }
}
