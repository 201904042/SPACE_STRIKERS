using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour
{
    public Transform skillImages;
    public Image skillImage;
    public TextMeshProUGUI levelText;
    public Transform description;
    public TextMeshProUGUI descripText;

    public InGameSkill skillData;


    private void Awake()
    {
        skillImages = transform.GetChild(0);
        levelText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        description = transform.GetChild(2);

        skillImage = skillImages.GetChild(1).GetComponent<Image>();
        descripText = description.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetSkillData(InGameSkill skill)
    {
        ResetData();
        skillData = skill;
        skillImage.sprite = Resources.Load<Sprite>("Sprite/default"); //todo db의 이미지대로
        levelText.text = $"lv . {skill.curSkillLevel}";
        //descripText.text = skill.SkillLevels[skill.curSkillLevel-1].Description;
    }

    public void ResetData()
    {
        skillData = null;
        skillImage.sprite = null;
        levelText.text = "";
        descripText.text = "";
    }

    public InGameSkill GetSkillData()
    {
        return skillData;
    }

    
}
