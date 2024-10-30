using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public Transform[] sprites; //백그라운드의 스프라이트 배열
    private int highest;
    private int lowest;
    private float viewHeight;
    private float speed;


    private void Awake()
    {
        viewHeight = Camera.main.orthographicSize*4;
        speed = 3;
        highest = sprites.Length-1;
        lowest = 0;
    }

    private void Update()
    {
        Vector2 curPos = transform.position;
        Vector2 nextPos = Vector2.down*speed*Time.deltaTime;
        transform.position = curPos+nextPos;

        if (sprites[lowest].position.y < -viewHeight)
        {
            Vector2 lowSpritePos = sprites[lowest].localPosition;
            Vector2 highSpritePos = sprites[highest].localPosition;

            sprites[lowest].transform.localPosition = highSpritePos + Vector2.up*20;

            int index = highest;
            highest = lowest;
            lowest = index-1 == -1 ? sprites.Length-1 : index - 1;
        }
    }
}
