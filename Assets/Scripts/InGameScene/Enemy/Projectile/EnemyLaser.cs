using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        dangerMark.gameObject.SetActive(true);
        coreLaser.gameObject.SetActive(false);
        outLineLaser.gameObject.SetActive(false);

        startPointObj = null;
        endPointObj = null;
        startPoint = Vector2.zero;
        endPoint = Vector2.zero; 
    }

    private void OnDisable()
    {
        if (laserCoroutine != null) 
        {
            StopCoroutine(laserCoroutine);
        }
    }

    public void LaserActive(GameObject AttackObj, float LaserTime = 3f, float ChargingTime = 1f, float LaserWidthRate = 1, GameObject EndObj = null)
    {
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

        laserCoroutine = StartCoroutine(LaserAttackCoroutine());
    }
    

    private IEnumerator LaserAttackCoroutine()
    {
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

        // Release laser
        PoolManager.poolInstance.ReleasePool(gameObject);
    }
}
