using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerjsonReader;

public class specialBomber : MonoBehaviour
{
    public GameObject Explosion_range;

    private GameObject player;
    private Player_specialSkill specialScript;
    private float speed;
    private float damage;
    private int level;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        player = GameObject.Find("Player");
        specialScript = player.GetComponent<Player_specialSkill>();

        damage = specialScript.special_Damage;
        level = specialScript.power_level;
        speed = 5;
    }
    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (!IsVisibleFrom(mainCamera)) //화면밖으로 나갈시 미사일 폭발
        {
            missile_explosion();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            missile_explosion(); //적에게 닿을시 폭발
        }
        
    }

    void missile_explosion()
    {
        specialBomber_explosionRange range = Instantiate(Explosion_range, transform.position, transform.rotation).GetComponent<specialBomber_explosionRange>();
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
