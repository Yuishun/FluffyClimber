using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HataBullet : MonoBehaviour
{
    Vector3 disVec;
    float delay, nTime = 0;
    float speed;
    Rigidbody rb;
    public void Init(Vector3 dis, float delay, float speed)
    {
        disVec = (dis - transform.position).normalized;
        this.delay = delay;
        this.speed = speed;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        nTime += Time.deltaTime;
        if (nTime <= delay)
            return;

        var vpos = transform.position + disVec * speed * Time.deltaTime;
        rb.MovePosition(vpos);

        if (nTime > delay + 5)
            Destroy(gameObject);
    }
}
