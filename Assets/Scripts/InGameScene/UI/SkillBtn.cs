using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour
{
    public Sprite btnImage;
    public bool is_imageSet;
    public TextMeshProUGUI LvText;
    public TextMeshProUGUI explainText;
    public Transform imageObj;

    public int skillId;
    public string skillType;
    public bool is_buttonSelected;

    private void Awake()
    {
        imageObj = transform.GetChild(0);
        LvText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        explainText = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        is_buttonSelected = false;
    }
    private void Update()
    {
        if (!is_imageSet)
        {
            imageObj.GetComponent<Image>().sprite = btnImage;
            is_imageSet = true;
        }
        
    }

    public void ChosenSkill()
    {
        transform.GetComponentInParent<LevelUP_UI>().chosenSkillId  = skillId;
        transform.GetComponentInParent<LevelUP_UI>().chosenSkillType = skillType;

        transform.parent.parent.GetChild(2).GetComponent<Button>().interactable = true;
    }
}
