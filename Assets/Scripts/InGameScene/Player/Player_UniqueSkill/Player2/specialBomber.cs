using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialBomber : MonoBehaviour
{
    //public GameObject Explosion_range;
    //private PlayerSpecialSkill specialScript;
    //private float speed;
    //private float IG_Dmg;
    //private int IG_Level;

    //private Camera mainCamera;

    //private void Awake()
    //{
    //    mainCamera = Camera.main;
    //    specialScript = GameManager.Instance.myPlayer.GetComponent<PlayerSpecialSkill>();

    //    IG_Dmg = specialScript.specialDamage;
    //    IG_Level = specialScript.IG_curPowerLevel;
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
    //    range.IG_Dmg = IG_Dmg;
    //    range.IG_Level = IG_Level;
    //    GameManager.Instance.Pool.ReleasePool(gameObject);
    //}

    //bool IsVisibleFrom(Camera camera) //��ü�� ȭ�� �ȿ� �ִ°�?
    //{
    //    Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
    //    return GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider2D>().bounds);
    //}
}
