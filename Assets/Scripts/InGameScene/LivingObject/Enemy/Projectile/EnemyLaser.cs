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

    private GameObject startObj;

    public float chargingTime = 1f;
    private float laserWidthRate = 1f;
    private float defaultLaserWidth = 0.3f;
    private float laserTime = 1f;

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
        defaultDmgRate = LaserDmgRate;
    }

    protected override void ResetProj()
    {
        base.ResetProj();
        laserCoroutine = null;
        startObj = null;
        isLaserCoroutineRunning = false;
    }

    public override void SetLaser(GameObject _startObj, bool isAim, float angle = 0, float _laserTime = 1, float chargingTime = 1f, float laserWidthRate = 1)
    {
        if (_startObj == null)
        {
            Debug.LogError("SetLaser: startObj is null.");
            return;
        }

        this.startObj = _startObj;
        this.laserTime = _laserTime;
        this.chargingTime = chargingTime;
        this.laserWidthRate = laserWidthRate;

        coreLaser.startWidth = defaultLaserWidth * laserWidthRate;
        outLineLaser.startWidth = defaultLaserWidth * laserWidthRate * 1.2f;


        StartLaserCoroutine();
    }

    private void StartLaserCoroutine()
    {
        if (isLaserCoroutineRunning)
        {
            StopCoroutine(laserCoroutine);
            isLaserCoroutineRunning = false;
        }
        laserCoroutine = StartCoroutine(LaserBehavior());
        isLaserCoroutineRunning = true;
    }

    private Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radian);
        float sin = Mathf.Sin(radian);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }

    private IEnumerator LaserBehavior()
    {
        dangerMark.gameObject.SetActive(true);
        coreLaser.gameObject.SetActive(false);
        outLineLaser.gameObject.SetActive(false);

        yield return StartCoroutine(LaserCharging());

        dangerMark.gameObject.SetActive(false);
        coreLaser.gameObject.SetActive(true);
        outLineLaser.gameObject.SetActive(true);

        yield return StartCoroutine(LiveTimer(laserTime));
        isLaserCoroutineRunning = false;
    }

    private IEnumerator LaserCharging()
    {
        float chargingTimer = 0f;
        while (chargingTimer < chargingTime)
        {
            if (startObj.activeSelf == false || startObj == null)
            {
                StopLaser();
                yield break;
            }

            chargingTimer += Time.deltaTime;
            float width = defaultLaserWidth * laserWidthRate * (chargingTime - chargingTimer);
            dangerMark.startWidth = width;
            dangerMark.endWidth = width;
            yield return null;
        }
    }

    private void StopLaser()
    {
        if (laserCoroutine != null)
        {
            StopCoroutine(laserCoroutine);
            laserCoroutine = null;
        }
        isLaserCoroutineRunning = false;
        GameManager.Game.Pool.ReleasePool(gameObject);
    }
}
