
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    PlayerStat pStat => PlayerMain.pStat;
    GameManager gm => GameManager.Game;
    private Slider hp => gm.UI.p_hpSlider;
    private float maxHp => pStat.IG_Hp;
    private float curHp => pStat.CurHp;

    private Slider exp => gm.UI.p_expSlider;
    private float MaxExp =>  pStat.IG_MaxExp;
    private float curExp => pStat.CurExp;

    private Slider pow => gm.UI.p_powSlider;
    private float MaxPow => PlayerMain.powMax;
    private float curPow => pStat.CurPow;

   
    private Image USImage => gm.UI.p_uSkillImage;
    private TextMeshProUGUI US_CountText => gm.UI.p_uSkillCountText;
    private int USCount => pStat.USkillCount;
    private TextMeshProUGUI powText => gm.UI.p_powLvText;

    public void Init()
    {
        HpBarChange();
        PowBarChange();
        ExpBarChange();
        SetUniqueSkillImage();
        SetUniquSkillCount();
    }

    public void HpBarChange()
    {
        
        hp.value = curHp/ maxHp;
        //todo => maxHp %별로 색깔놀이
    }

    public void ExpBarChange()
    {
        exp.value = curExp / MaxExp; //계산해야됨
        Debug.Log(exp.value);
    }

    public void PowBarChange()
    {
        pow.value = curPow / MaxPow;

        Image fillImage = pow.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        int powLv  = pStat.IG_curPowerLevel;
        if (powLv == 0)
        {
            fillImage.color = Color.white;
            powText.text = "POW Lv 0";
        }
        else if (powLv == 1)
        {
            fillImage.color = Color.green;
            powText.text = "POW Lv 1";
        }
        else if (powLv == 2)
        {
            fillImage.color = Color.yellow;
            powText.text = "POW Lv 2";
        }
        else if (powLv == 3)
        {
            fillImage.color = Color.red;
            powText.text = "POW Lv MAX";
        }
    }

    public void SetUniqueSkillImage()
    {
        Sprite image = Resources.Load<Sprite>("Sprite/default");
        if(image == null)
        {
            Debug.Log("유니크 스킬의 경로명이 잘못됨");
            return;
        }
        USImage.sprite = image;
    }

    public void SetUniquSkillCount()
    {
        US_CountText.text = USCount.ToString();
    }

}
