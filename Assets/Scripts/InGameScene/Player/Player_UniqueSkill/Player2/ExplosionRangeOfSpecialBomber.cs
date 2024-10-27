using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;


public class ExplosionRangeOfSpecialBomber : MonoBehaviour
{
    //private PlayerSpecialSkill specialScript;
    //public float IG_Dmg; // specialBomber에서 받아옴
    //private float damageTik;//데미지 사이의 간격
    //private float liveTime;
    //private bool isDamaging;
    //public int IG_Level; //specialBomber에서 받아옴

    //private bool time_has_set;
    //private void Awake()
    //{
    //    specialScript = GameManager.Instance.myPlayer.GetComponent<PlayerSpecialSkill>();
    //    damageTik = 0.1f;
    //    isDamaging = false;
    //    time_has_set = false;


    //}
    //void Update()
    //{
    //    if (IG_Level == 1)
    //    {
    //        transform.localScale = new Vector3(2f, 2f, 2f);
    //        if (!time_has_set)
    //        {
    //            liveTime = 3;
    //            specialScript.specialFireTime = liveTime;
    //        }

    //    }
    //    else if (IG_Level == 2)
    //    {
    //        transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
    //        if (!time_has_set)
    //        {
    //            liveTime = 4;
    //            specialScript.specialFireTime = liveTime;
    //        }
    //    }
    //    else if (IG_Level == 3)
    //    {
    //        transform.localScale = new Vector3(5, 5f, 5f);
    //        if (!time_has_set)
    //        {
    //            liveTime = 5;
    //            specialScript.specialFireTime = liveTime;
    //        }
    //    }

    //    if (!time_has_set) {
    //        time_has_set = true;
    //    }

    //    liveTime -= Time.deltaTime;
    //    if (liveTime <= 0)
    //    {
    //        specialScript.isSkillActivating = false;
    //        GameManager.Instance.Pool.ReleasePool(gameObject);
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Enemy")) // 적 객체와 충돌한 경우
    //    {
    //        isDamaging = true; // 데미지를 줄 준비가 되었음을 표시
    //        StartCoroutine(DealDamage(collision)); // 데미지 주기 시작
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Enemy")) // 적 객체와 충돌이 끝난 경우
    //    {
    //        isDamaging = false; // 데미지 중단
    //    }
    //}

    //private IEnumerator DealDamage(Collider2D enemy)
    //{
    //    while (enemy != null && enemy.gameObject.activeSelf && isDamaging) // 데미지를 주는 동안
    //    {
    //        if (enemy.gameObject.tag == "Enemy")
    //        {
    //            if (enemy.gameObject.GetComponent<EnemyObject>() != null)
    //            {
    //                enemy.gameObject.GetComponent<EnemyObject>().EnemyDamaged(IG_Dmg, gameObject);
    //            }
    //        }
    //        yield return new WaitForSeconds(damageTik); // 데미지 간격만큼 대기
    //    }
    //}

 
    
}
