using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowUp : ItemBasic
{
    Player_specialSkill p_skill;
    protected override void Awake()
    {
        base.Awake();
        p_skill = player.GetComponent<Player_specialSkill>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            PowUP();
            Destroy(gameObject);
        }
    }
    void PowUP()
    {
        switch (p_skill.power_level)
        {
            case 0:
                p_skill.power_level = 1; p_skill.power_increase = 5; break;
            case 1:
                p_skill.power_level = 2; p_skill.power_increase = 15; break;
            case 2:
                p_skill.power_level = 3; p_skill.power_increase = 30; break;
        }
    }
}
