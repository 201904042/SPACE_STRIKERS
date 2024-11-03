using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class Exp_object : MonoBehaviour
{
    private int expAmount = 1;
    private float expSpeed = 5f;
    private float amplitude = 0.5f; // ��� ����
    private float frequency = 2f; // ��� �ֱ�
    private float elapsedTime = 0f; // �ð� ����� ����

    public void OnExp()
    {
        StartCoroutine(ExpBehavior());
    }

    private IEnumerator ExpBehavior()
    {
        // 0~1 ������ ������ �Ÿ���ŭ �̵�
        float randomDistance = Random.Range(0f, 1f);
        Vector2 initialDirection = Random.insideUnitCircle.normalized; // ������ ���� ����
        Vector2 targetPosition = (Vector2)transform.position + initialDirection * randomDistance;


        while (Vector2.Distance(transform.position, targetPosition) > 0.01f) // Ÿ�� ��ġ�� ������� ������ �ݺ�
        {
            float step = expSpeed * Time.deltaTime; // �̵� �Ÿ� ���
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
            yield return null; 
        }

        // 1�� ���
        yield return new WaitForSeconds(1f);

        // �÷��̾ ���� �̵�
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
