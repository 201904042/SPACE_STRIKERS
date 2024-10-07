using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    SpriteRenderer shieldSpriteRenderer;
    Color color;
    private Color shieldColor_lv0 = new Color(1f, 1f, 1f, 0);
    private Color shieldColor_lv1 = new Color(1f, 1f, 1f, 60f/255f);
    private Color shieldColor_lv2 = new Color(1f, 153f/255f, 153f/255f, 60f/255f);
    private Color shieldColor_lv3 = new Color(1f, 56f/255f, 56f/255f, 60f/255f);

    PlayerControl playerControlScript;
    public int shieldMaxNum;
    
    public int shieldCurNum;

    public bool shieldIsActive;
    private float shieldRestoreTime;
    private float shieldTimer;

    private float playerStatDamage;
    private float shieldDamage;
    private float shieldDamageRate = 100; //���ݵ������� 4��(Ȥ�� �ظ��� �Ϲݸ� �ѹ����ϵ��� ���� �����Ұ�

    public bool shieldHasDamageable;
    private void Awake()
    {
        shieldSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        shieldMaxNum = 3; //���߿� ������ ����(2,4,6)�� ���� ���� ���� 
        shieldCurNum = shieldMaxNum;
        shieldRestoreTime = 10f; //�÷��̾��� ���ݼӵ� ��ʷ� ��ĥ��
        shieldTimer = 0;
        shieldIsActive = true;
        

        playerControlScript = GameManager.Instance.myPlayer.GetComponent<PlayerControl>();
        shieldHasDamageable = true;
        playerStatDamage = GameManager.Instance.myPlayer.GetComponent<PlayerStat>().damage ;
        shieldDamage = playerStatDamage * shieldDamageRate;

    }

    private void Update()
    {
        
        if (shieldCurNum < shieldMaxNum)
        {
            shieldTimer += Time.deltaTime;
            if(shieldRestoreTime <= shieldTimer)
            {
                shieldIsActive = true;
                shieldCurNum += 1;
                ShieldColorChange();
                shieldTimer = 0;
            }

        }
    }

    public void ShieldColorChange()
    {
        if(shieldCurNum == 1)
        {

            color = shieldColor_lv1;
        }
        else if(shieldCurNum == 2)
        {

            color = shieldColor_lv2;
        }
        else if (shieldCurNum == 3)
        {

            color = shieldColor_lv3;
        }
        else
        {
            color = shieldColor_lv0;
        }
        shieldSpriteRenderer.color = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shieldIsActive)
        {
            if (collision.CompareTag("Enemy") || collision.CompareTag("Enemy_Projectile"))
            {
                if(collision.GetComponent<EnemyObject>() != null)
                {
                    collision.GetComponent<EnemyObject>().EnemyDamaged(shieldDamage,gameObject);
                }
                playerControlScript.PlayerKnockBack(collision); //���尡 �ջ�ɰ�� �÷��̾�� �˹�ȿ��

                shieldTimer = 0; // �΋H���ٸ� ���� Ÿ�̸� �ʱ�ȭ
                shieldCurNum -= 1;
                ShieldColorChange();

                if (shieldCurNum <= 0)
                {
                    shieldIsActive = false;
                }
            }
        }
    }
}
