using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialBomber : MonoBehaviour
{
    //public GameObject Explosion_range;
    //private PlayerSpecialSkill specialScript;
    //private float speed;
    //private float damage;
    //private int level;

    //private Camera mainCamera;

    //private void Awake()
    //{
    //    mainCamera = Camera.main;
    //    specialScript = GameManager.Instance.myPlayer.GetComponent<PlayerSpecialSkill>();

    //    damage = specialScript.specialDamage;
    //    level = specialScript.powerLevel;
    //    speed = 5;
    //}
    //private void Update()
    //{
    //    transform.Translate(Vector3.up * speed * Time.deltaTime);
    //    if (!IsVisibleFrom(mainCamera)) //ȭ������� ������ �̻��� ����
    //    {
    //        MissileExplosion();
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        MissileExplosion(); //������ ������ ����
    //    }
        
    //}

    //void MissileExplosion()
    //{
    //    ExplosionRangeOfSpecialBomber range = Instantiate(Explosion_range, transform.position, transform.rotation)
    //        .GetComponent<ExplosionRangeOfSpecialBomber>();
    //    range.damage = damage;
    //    range.level = level;
    //    GameManager.Instance.Pool.ReleasePool(gameObject);
    //}

    //bool IsVisibleFrom(Camera camera) //��ü�� ȭ�� �ȿ� �ִ°�?
    //{
    //    Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
    //    return GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider2D>().bounds);
    //}
}
