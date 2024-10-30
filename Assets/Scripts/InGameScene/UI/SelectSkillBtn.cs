using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkillBtn : MonoBehaviour
{
    public Image skillImage;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI descripText;
    public InGameSkill skillData;

    private void Awake()
    {
        Transform SkillImage = transform.GetChild(0);
        Transform Description = transform.GetChild(1);
        skillImage = SkillImage.GetChild(1).GetComponent<Image>();
        levelText = SkillImage.GetComponentInChildren<TextMeshProUGUI>();
        descripText = Description.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetSkillData(InGameSkill skill)
    {
        ResetData();
        skillData = skill;
        skillImage.sprite = Resources.Load<Sprite>("Sprite/default"); //todo db�� �̹������
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
