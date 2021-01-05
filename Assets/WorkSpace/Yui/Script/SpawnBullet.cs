using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform player;

    public void Spawn()
    {
        var obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;

    }
}
