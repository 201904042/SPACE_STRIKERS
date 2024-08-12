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
    public float chargingTime;
    public float laserTime;
    [SerializeField]
    private float chargingTimer;
    [SerializeField]
    private float laserTimer;
    [SerializeField]
    private float LaserWidthRate;
    private float defaultLaserWidth;


    private bool isFirstSet;

    protected override void Awake()
    {
        base.Awake();
        dangerMark = transform.GetChild(0).GetComponent<LineRenderer>();
        coreLaser = transform.GetChild(1).GetComponent<LineRenderer>();
        outLineLaser = transform.GetChild(2).GetComponent<LineRenderer>();
        chargingTime = 1f;
        laserTime = 3f;
        chargingTimer = 0;
        laserTimer = 0;
        LaserWidthRate = 1;
        defaultLaserWidth = 0.3f;
        dangerMark.gameObject.SetActive(true);
        coreLaser.gameObject.SetActive(false);
        outLineLaser.gameObject.SetActive(false);

        damage = 30f;

        isFirstSet = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

    }


    private void Update()
    {
        if(!isFirstSet)
        {
            startPoint = startPointObj.transform.position;
            if(endPointObj != null)
            {
                endPoint = endPointObj.transform.position;
            }
            isFirstSet = true;
        }
        else
        {
            LaserAttack();
        }
        
    }
    
    private void LaserAttack()
    {
        if (chargingTimer < chargingTime)
        {
            chargingTimer += Time.deltaTime;
            dangerMark.startWidth = defaultLaserWidth * LaserWidthRate * (chargingTime - chargingTimer);
            
        }
        else
        {
            if (dangerMark.gameObject.activeSelf)
            {
                dangerMark.gameObject.SetActive(false);
                coreLaser.gameObject.SetActive(true);
                outLineLaser.gameObject.SetActive(true);
            }   
            
            if(laserTimer < laserTime)
            {
                laserTimer += Time.deltaTime;
            }
            else
            {
                PoolManager.poolInstance.ReleasePool(gameObject);
            }
        }
    }
}
