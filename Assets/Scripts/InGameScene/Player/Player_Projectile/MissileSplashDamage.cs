using UnityEngine;


public class MissileSplashDamage : MonoBehaviour
{
    private float missileSplashDamage;
    private float playerStatDamage;
    private float missileSplashDamageRate;
    private float explodeRadius;
    private float splashTime;
    private float timeDelay = 0.2f;

    private void Awake()
    {
        missileSplashDamageRate = 0.5f;
        explodeRadius = 2.5f;
        playerStatDamage = GameManager.game.myPlayer.GetComponent<PlayerStat>().damage;
        missileSplashDamage = playerStatDamage * missileSplashDamageRate;
        splashTime = 0;
    }

    private void Update()
    {
        splashTime += 0.01f;
        if (splashTime > timeDelay)
        {
            Managers.Instance.Pool.ReleasePool(gameObject);
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
                if (collision.GetComponent<EnemyObject>() != null)
                {
                    collision.GetComponent<EnemyObject>().EnemyDamaged(missileSplashDamage, gameObject);
                }
            }
        }
    }
}
