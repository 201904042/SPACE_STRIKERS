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

    // Update is called once per frame
    void Update()
    {
        if (curPlayer==0||curPlayer != ChosingCharPageObj.GetComponent<SelectCharPage>().curPlayer)
        {
            if (curPlayer==0)
            {
                curPlayer = 1;
            }
            else
            {
                curPlayer = ChosingCharPageObj.GetComponent<SelectCharPage>().curPlayer;
            }

            switch (curPlayer)
            {
                case 1: playerImage.GetComponent<Image>().sprite = playerImage1; break;
                case 2: playerImage.GetComponent<Image>().sprite = playerImage2; break;
                case 3: playerImage.GetComponent<Image>().sprite = playerImage3; break;
                case 4: playerImage.GetComponent<Image>().sprite = playerImage4; break;
            }
            playerInformationSet();
        }
    }

    private void playerStat() {
        foreach (var player in playerstat.PlayerList.player)
        {
            if (player.id == curPlayer)
            {
                playerLv = player.level;
                playerHp = player.hp;
                playerDmg = player.damage;
                playerDef = player.defence;
                playerMSpd = player.move_speed;
                playerASpd = player.attack_speed;
            }
        }
    }
    private void playerInformationSet() {
        playerStat();
        playerInformText.text =
            "Lv : " + playerLv + "\n" +
            "Hp : " + playerHp + "\n" +
            "Dmg : " + playerDmg + "\n" +
            "Def : " + playerDef + "\n" +
            "MoveSpeed : " + playerMSpd + "\n" +
            "AtkSpeed : " + playerASpd + "\n";
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
