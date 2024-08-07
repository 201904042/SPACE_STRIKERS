using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour
{
    public SkillData SkillData
    {
        get => skillData;
        set
        {
            Debug.Log("스킬데이터 인계");
            skillData = value;
            FindSkillInPlayerSkill();
        }
    }

    private SkillData skillData;
    public TextMeshProUGUI LvText;
    public TextMeshProUGUI explainText;
    public Image imageObj;

    public bool isButtonSelected;

    private void Awake()
    {
        imageObj = transform.GetChild(0).GetComponent<Image>();
        LvText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        explainText = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        isButtonSelected = false;
        Debug.Log("awake");
    }

    private void FindSkillInPlayerSkill()
    {
        Transform playerSkillSlot = GameObject.Find("Player").transform.GetChild(1);
        SkillInterface skillInterface;
        for (int i = 0; i < playerSkillSlot.childCount; i++)
        {
            if (playerSkillSlot.GetChild(i).GetComponent<SkillInterface>().skillId == skillData.skillID)
            {
                skillInterface = playerSkillSlot.GetChild(i).GetComponent<SkillInterface>();
                Debug.Log(imageObj);
                imageObj.sprite = skillData.skillIcon;
                LvText.text = skillInterface.level.ToString();
                explainText.text = skillInterface.skillIntro.ToString();

                break;
            }
        }
    }

    public void ChosenSkill()
    {
        transform.GetComponentInParent<LevelUP_UI>().ChosenSkillData  = skillData;

        transform.parent.parent.GetChild(2).GetComponent<Button>().interactable = true;
    }
}
