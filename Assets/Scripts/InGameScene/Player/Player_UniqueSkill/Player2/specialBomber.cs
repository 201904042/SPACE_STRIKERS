using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialBomber : MonoBehaviour
{
    public GameObject Explosion_range;

    private GameObject player;
    private PlayerSpecialSkill specialScript;
    private float speed;
    private float damage;
    private int level;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        player = GameObject.Find("Player");
        specialScript = player.GetComponent<PlayerSpecialSkill>();

        damage = specialScript.specialDamage;
        level = specialScript.powerLevel;
        speed = 5;
    }
    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (!IsVisibleFrom(mainCamera)) //화면밖으로 나갈시 미사일 폭발
        {
            MissileExplosion();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            MissileExplosion(); //적에게 닿을시 폭발
        }
        
    }

    void MissileExplosion()
    {
        ExplosionRangeOfSpecialBomber range = Instantiate(Explosion_range, transform.position, transform.rotation)
            .GetComponent<ExplosionRangeOfSpecialBomber>();
        range.damage = damage;
        range.level = level;
        Destroy(gameObject);
    }

    bool IsVisibleFrom(Camera camera) //객체가 화면 안에 있는가?
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider2D>().bounds);
    }
}
