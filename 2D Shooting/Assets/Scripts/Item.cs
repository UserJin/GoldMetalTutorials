using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string type;
    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() // 활성화 시 특정 방향으로 이동
    {
        rigid.velocity = Vector2.down * 1.5f;
    }

}
