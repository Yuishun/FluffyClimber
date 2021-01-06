using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HataBullet : MonoBehaviour
{
    Vector3 disPos;
    float delay, nTime = 0;
    float speed;
    Rigidbody rb;
    public void Init(Vector3 dis, float delay, float speed)
    {
        disPos = dis;
        this.delay = delay;
        this.speed = speed;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        nTime += Time.deltaTime;
        if (nTime <= delay)
            return;

        var vpos = Vector3.MoveTowards(transform.position, disPos, speed * Time.deltaTime);
        rb.MovePosition(vpos);

        if (nTime > delay + 6)
            Destroy(gameObject);
    }
}
