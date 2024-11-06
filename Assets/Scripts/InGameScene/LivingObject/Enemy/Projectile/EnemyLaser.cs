using System.Collections;
using UnityEngine;

public class EnemyLaser : EnemyProjectile
{
    [SerializeField]
    private LineRenderer dangerMark;
    [SerializeField]
    private LineRenderer coreLaser;
    [SerializeField]
    private LineRenderer outLineLaser;

    public GameObject startPointObj;
    private Vector2 startPoint;
    public GameObject endPointObj;
    private Vector2 endPoint;

    public float chargingTime = 1f;
    public float laserTime = 3f;
    [SerializeField]
    private float laserWidthRate = 1f;
    private float defaultLaserWidth = 0.3f;

    private Coroutine laserCoroutine;
    private bool isLaserCoroutineRunning = false;

    protected override void Awake()
    {
        base.Awake();
        dangerMark = transform.GetChild(0).GetComponent<LineRenderer>();
        coreLaser = transform.GetChild(1).GetComponent<LineRenderer>();
        outLineLaser = transform.GetChild(2).GetComponent<LineRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        if (laserCoroutine != null)
        {
            StopCoroutine(laserCoroutine);
            isLaserCoroutineRunning = false;
        }

        base.OnDisable();
    }

    protected override void ResetProj()
    {
        base.ResetProj();
        dangerMark.gameObject.SetActive(true);
        coreLaser.gameObject.SetActive(false);
        outLineLaser.gameObject.SetActive(false);

        startPointObj = null;
        endPointObj = null;
        startPoint = Vector2.zero;
        endPoint = Vector2.zero;
        laserCoroutine = null;
        isLaserCoroutineRunning = false;
    }

    public void LaserActive(GameObject AttackObj, float LaserTime = 3f, float ChargingTime = 1f, float LaserWidthRate = 1, GameObject EndObj = null)
    {
        if (AttackObj == null)
        {
            Debug.LogError("LaserActive: AttackObj is null.");
            return;
        }

        // �̹� �ڷ�ƾ�� ���� ���̸� �ߴ�
        if (laserCoroutine != null && isLaserCoroutineRunning)
        {
            StopCoroutine(laserCoroutine);
            isLaserCoroutineRunning = false;
        }

        // �ʱ�ȭ
        startPointObj = AttackObj;
        if (EndObj != null)
        {
            endPointObj = EndObj;
        }
        laserTime = LaserTime;
        chargingTime = ChargingTime;
        laserWidthRate = LaserWidthRate;

        startPoint = startPointObj.transform.position;
        if (endPointObj != null)
        {
            endPoint = endPointObj.transform.position;
        }

        // ���ο� �ڷ�ƾ ����
        laserCoroutine = StartCoroutine(LaserAttackCoroutine());
    }

    private IEnumerator LaserAttackCoroutine()
    {
        isLaserCoroutineRunning = true;

        float chargingTimer = 0f;

        dangerMark.startWidth = defaultLaserWidth;

        // Charging phase
        while (chargingTimer < chargingTime)
        {
            chargingTimer += Time.deltaTime;
            dangerMark.startWidth = defaultLaserWidth * laserWidthRate * (chargingTime - chargingTimer);
            yield return null;
        }

        // Activate laser
        dangerMark.gameObject.SetActive(false);
        coreLaser.gameObject.SetActive(true);
        outLineLaser.gameObject.SetActive(true);

        // Laser phase
        yield return new WaitForSeconds(laserTime);
        isLaserCoroutineRunning = false;
        // Release laser
        GameManager.Game.Pool.ReleasePool(gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
