using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    //이건 고정값 총알 100% , 분열총알 200%, 레이저 150%
    private const int BulletDmgRate = 100;
    private const int SplitBulletDmgRate = 200;
    private const int LaserDmgRate = 150;

    //필요 파라미터 = 적오브젝트의 데미지 스텟, 속도, 발동시간, 크기
    public int enemyDmg;
    [SerializeField] protected int defaultDmgRate; //페이즈별 증가량 => 120% : 본 데미지가 10이라면 12의 데미지
    [SerializeField] protected int damageRate; //페이즈별 증가량 => 120% : 본 데미지가 10이라면 12의 데미지
    [SerializeField] protected int speed;
    [SerializeField] protected float liveTime;
    [SerializeField] protected float range;
    protected int finalDamage;

    protected Coroutine activated;

    protected Vector3 projScaleInstance; //발사체의 원래크기

    [SerializeField] protected bool isParameterSet; //파라미터가 설정됨? 설정되어야 움직임 : default =false
    [SerializeField] protected bool isHitOnce;  //해당 발사체가 한번의 타격만을 다루는지 ex) 총알은 한번 충돌하면 사라지고 레이저는 한번 충돌해도 사라지지 않음

    protected virtual void Awake()
    {
        projScaleInstance = transform.localScale; //발사체의 원래 크기를 저장

        ResetProj();
    }

    protected virtual void OnEnable()
    {

    }


    protected virtual void OnDisable()
    {
        //비활성화 시 초기화
        ResetProj();
    }

    protected virtual void ResetProj()
    {
        damageRate = 0;
        speed = 0;
        liveTime = 0;
        range = 0;
        finalDamage = 0;

        if (activated != null)
        {
            StopCoroutine(activated);
            activated = null;
        }

        transform.localScale = projScaleInstance;

        isParameterSet = false;
        isHitOnce = true;
    }
    

    protected virtual void Update()
    {
        if (!isParameterSet)
        {
            Debug.Log("파라미터가 설정되지 않음");
            return;
        }

        MoveUp();
    }

    public virtual void SetDamage(int enemyDmgStat)
    {
        enemyDmg = enemyDmgStat;
    }

    public virtual void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime = 0, float _range = 0)
    {
        speed = _projSpeed;
        damageRate = _dmgRate;
        finalDamage = (enemyDmg * defaultDmgRate / 100) + (enemyDmg * damageRate / 100);  //damageRate = 발사체별 증폭률 + 페이즈 증폭률
    
        //레이저 같이 일정 시간동안 사라지지 않는 발사체에 쓰임
        if (_liveTime != 0)
        {
            liveTime = _liveTime; 
            StartCoroutine(LiveTimer(liveTime));
        }

        //이것도 페이즈의 증폭량에 따라 조금씩 커지게 할듯, 혹은 레이저 크기를 조절하거나
        if (_range == 0)
        {
            range = 1;
        }
        else
        {
            range = _range;
            transform.localScale = projScaleInstance * range;
        }

        isParameterSet = true;
    }

    protected virtual IEnumerator LiveTimer(float activeTime)
    {
        yield return new WaitForSeconds(activeTime);

        GameManager.Game.Pool.ReleasePool(gameObject);
    }


    protected void MoveUp()
    {
        if (isParameterSet)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            PlayerMain.pStat.PlayerDamaged(finalDamage, gameObject);
            GameManager.Game.Pool.ReleasePool(gameObject);
        }

        if (collision.transform.tag == "BulletBorder")
        {
            GameManager.Game.Pool.ReleasePool(gameObject);
        }
    }
}
