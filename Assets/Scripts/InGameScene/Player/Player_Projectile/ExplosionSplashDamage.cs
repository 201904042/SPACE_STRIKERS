using UnityEngine;


public class ExplosionSplashDamage : MonoBehaviour
{
    [Header("PlayerMissile의 스텟")]
    public float missileDamage;
    public float explosionRange;

    [Header("범위데미지 스텟")]
    [SerializeField]
    private float missileSplashDamage;
    [SerializeField]
    private float splashDamageRate;
    [SerializeField]
    private float splashTime;
    
    private bool firstSet;
    private void Awake()
    {
        firstSet = false;
        splashDamageRate = 0.5f;
        explosionRange = 1f;
        splashTime = 1f;
    }

    private void Update()
    {
        if (!firstSet)
        {
            splashDamageRate = missileDamage * splashDamageRate;
            transform.localScale = transform.localScale * explosionRange;
            firstSet = true;
        }
        
        splashTime -= Time.deltaTime;
        if (splashTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyObject>() != null)
        {
            collision.GetComponent<EnemyObject>().EnemyDamaged(splashDamageRate, gameObject);
        }
    }
}
