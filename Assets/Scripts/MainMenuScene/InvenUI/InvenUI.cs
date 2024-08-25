using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class InvenUI : UI_Parent
{
    
    public GameObject ChosingCharPageObj;
    public int curPlayer=0;

    public Sprite playerImage1;
    public Sprite playerImage2;
    public Sprite playerImage3;
    public Sprite playerImage4;
    public GameObject playerImage;

    public PlayerJsonReader playerstat;
    public TextMeshProUGUI playerInformText;
    public int playerLv;
    private float playerHp;
    private float playerDmg;
    private float playerDef;
    private float playerMSpd;
    private float playerASpd;

    protected override void Awake()
    {
        base.Awake();
    }


    

    public void BackBtn()
    {
        gameObject.SetActive(false);
        Main.SetActive(true);
    }

    public void Player_Btn() {
        ChosingCharPageObj.SetActive(true);
    }
}
