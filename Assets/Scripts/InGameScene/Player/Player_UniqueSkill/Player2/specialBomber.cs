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
    //    if (!IsVisibleFrom(mainCamera)) //화면밖으로 나갈시 미사일 폭발
    //    {
    //        MissileExplosion();
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        MissileExplosion(); //적에게 닿을시 폭발
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

    //bool IsVisibleFrom(Camera camera) //객체가 화면 안에 있는가?
    //{
    //    Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
    //    return GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider2D>().bounds);
    //}
}
