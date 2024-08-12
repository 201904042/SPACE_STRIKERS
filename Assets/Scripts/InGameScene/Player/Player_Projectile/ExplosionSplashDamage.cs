using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class ExplosionSplashDamage : MonoBehaviour
{
    [Header("PlayerMissileÀÇ ½ºÅÝ")]
    public float missileDamage;
    public float explosionRange;

    [Header("¹üÀ§µ¥¹ÌÁö ½ºÅÝ")]
    [SerializeField]
    private float missileSplashDamage;
    [SerializeField]
    private float splashDamageRate;
    [SerializeField]
    private float splashTime;

    private bool isSet;
  
    private void Awake()
    {
        splashDamageRate = 0.5f;
        explosionRange = 1f;
        splashTime = 1f;
    }

    private void OnEnable()
    {
        isSet = false;
    }

    private void Setting()
    {
        isSet = true;
        missileDamage = missileDamage * splashDamageRate;
        transform.localScale *= explosionRange;
        StartCoroutine(activeTime(splashTime));
    }

    private void OnDisable()
    {
        transform.localScale /= explosionRange;
    }

    private void Update()
    {
        if(isSet == false)
        {
            Setting();
        }
    }

    public void SetVariable(float Range, float Damage)
    {
        explosionRange = Range;
        missileDamage = Damage;
    }

    private IEnumerator activeTime(float time)
    {
        yield return new WaitForSeconds(time);
        PoolManager.poolInstance.ReleasePool(gameObject);
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyObject>() != null)
        {
            collision.GetComponent<EnemyObject>().EnemyDamaged(splashDamageRate, gameObject);
        }
    }
}
