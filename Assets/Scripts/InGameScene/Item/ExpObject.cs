using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class Exp_object : MonoBehaviour
{
    private int expAmount = 1;
    private float expSpeed = 5f;

    public void OnExp()
    {
        StartCoroutine(ExpBehavior());
    }

    private IEnumerator ExpBehavior()
    {
        // 0~1 사이의 랜덤한 거리만큼 이동
        float randomDistance = Random.Range(0f, 1f);
        Vector2 initialDirection = Random.insideUnitCircle.normalized; // 임의의 방향 생성
        Vector2 targetPosition = (Vector2)transform.position + initialDirection * randomDistance;


        while (Vector2.Distance(transform.position, targetPosition) > 0.01f) // 타겟 위치에 가까워질 때까지 반복
        {
            float step = expSpeed * Time.deltaTime; // 이동 거리 계산
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
            yield return null; 
        }

        // 1초 대기
        yield return new WaitForSeconds(1f);

        // 플레이어를 향해 이동
        while (PlayerMain.Instance != null)
        {
            Vector3 direction = (PlayerMain.Instance.transform.position - transform.position).normalized;
            transform.up = direction;
            transform.position += transform.up * expSpeed * Time.deltaTime;
            yield return null; 
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerStat pstat = collision.GetComponent<PlayerStat>();
            pstat.CurExp = pstat.CurExp + expAmount;
            GameManager.Game.Pool.ReleasePool(gameObject);
        }
    }
}
