using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected int defaultDmgRate; //����� ������ => 120% : �� �������� 10�̶�� 12�� ������
    [SerializeField] protected int damageRate; //����� ������ => 120% : �� �������� 10�̶�� 12�� ������
    [SerializeField] protected int speed;
    [SerializeField] protected float liveTime;
    [SerializeField] protected float range;
    protected int finalDamage;

    [SerializeField] protected bool isParameterSet; //�Ķ���Ͱ� ������? �����Ǿ�� ������ : default =false4

    protected Vector3 projScaleInstance; //�߻�ü�� ����ũ��
    protected Coroutine activated;

    protected abstract void ResetProj();
}
