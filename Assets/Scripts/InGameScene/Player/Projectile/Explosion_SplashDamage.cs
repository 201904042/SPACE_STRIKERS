using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class Explosion_SplashDamage : MonoBehaviour
{
    [Header("PlayerMissileÀÇ ½ºÅÝ")]
    public float missile_Damage;
    public float explosion_range;

    [Header("¹üÀ§µ¥¹ÌÁö ½ºÅÝ")]
    [SerializeField]
    private float missile_splash_damage;
    [SerializeField]
    private float splash_damageRate;
    [SerializeField]
    private float splashTime;
    
    private bool first_set;
    private void Awake()
    {
        first_set = false;
        splash_damageRate = 0.5f;
        explosion_range = 1f;
        splashTime = 1f;
    }

    private void Update()
    {
        if (!first_set)
        {
            missile_splash_damage = missile_Damage * splash_damageRate;
            transform.localScale = transform.localScale * explosion_range;
            first_set = true;
        }
        
        splashTime -= Time.deltaTime;
        if (splashTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().Enemydamaged(missile_splash_damage, gameObject);
        }
    }
}
