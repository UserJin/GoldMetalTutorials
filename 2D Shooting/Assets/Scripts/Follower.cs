using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float maxShotDelay;
    public float curShotDelay;

    public ObjectManager objectManager;

    public Vector3 followPos;
    public int followDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;

    private void Awake()
    {
        parentPos = new Queue<Vector3>();
    }

    void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    private void Watch()
    {
        // Input Pos
        if (!parentPos.Contains(parent.position)) // parentPos의 위치가 변경될 때만 저장
        {
            parentPos.Enqueue(parent.position);
        }

        // Output Pos
        if(parentPos.Count >= followDelay) // parentPos의 수가 followDelay보다 커질 경우에만 큐에서 위치를 꺼내옴
        {
            followPos = parentPos.Dequeue();
        }
        else if(parentPos.Count < followDelay) // 그 전까지는 붙어서 따라다니기
        {
            followPos = parent.position;
        }

    }

    void Follow()
    {
        transform.position = followPos;
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1"))
        {
            return;
        }
        if (curShotDelay < maxShotDelay)
            return;

        GameObject bullet = objectManager.MakeObj("BulletFollower");
        bullet.transform.position = transform.position;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        curShotDelay = 0;
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
}
