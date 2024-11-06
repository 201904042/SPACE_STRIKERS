using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected int defaultDmgRate; //페이즈별 증가량 => 120% : 본 데미지가 10이라면 12의 데미지
    [SerializeField] protected int damageRate; //페이즈별 증가량 => 120% : 본 데미지가 10이라면 12의 데미지
    [SerializeField] protected int speed;
    [SerializeField] protected float liveTime;
    [SerializeField] protected float range;
    protected int finalDamage;

    [SerializeField] protected bool isParameterSet; //파라미터가 설정됨? 설정되어야 움직임 : default =false4

    protected Vector3 projScaleInstance; //발사체의 원래크기
    protected Coroutine activated;

    protected abstract void ResetProj();
}
