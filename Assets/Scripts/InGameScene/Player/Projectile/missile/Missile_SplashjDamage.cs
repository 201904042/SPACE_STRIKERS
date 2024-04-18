using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class Missile_SplashDamage : MonoBehaviour
{
    GameObject player;
    private float missile_splash_damage;
    private float player_statDamage;
    [SerializeField]
    private float missile_splash_damageRate;

    private float explodeRadius;

    private float splashTime;
    private float timeDelay = 0.2f;

    private void Awake()
    {
        missile_splash_damageRate = 0.5f;
        explodeRadius = 2.5f;
        player = GameObject.Find("Player");
        player_statDamage = player.GetComponent<PlayerStat>().damage;
        missile_splash_damage = player_statDamage * missile_splash_damageRate;
        splashTime = 0;
    }

    private void Update()
    {
        splashTime += 0.01f;
        if (splashTime > timeDelay)
        {
            Destroy(gameObject);
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        Vector2 explosionPos = transform.position;

        if (collision.CompareTag("Enemy"))
        {
            Vector2 enemyPos = collision.transform.position;

            Vector2 dir = enemyPos - explosionPos;
            
            float dist = dir.magnitude;
            if (dist <= explodeRadius)
            {
                
                if (collision.GetComponent<Enemy>() != null)
                {
                    collision.GetComponent<Enemy>().Enemydamaged(missile_splash_damage, gameObject);
                }
               
               
            }
        }
    }

    
}
