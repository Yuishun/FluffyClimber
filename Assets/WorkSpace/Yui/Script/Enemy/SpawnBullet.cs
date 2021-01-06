using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Ragdoll_enable player;
    [Header("xy 左下 wh 右上")]
    public Rect targetArea;

    public void Spawn(float speed)
    {
        var obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
        var hbullet = obj.GetComponent<HataBullet>();
        // 補正
        var p = player.cRb.position + player.rb.velocity;
        if (p.x < targetArea.x || p.x > targetArea.width)
            p.x = player.cRb.position.x;
        if (p.y < targetArea.y || p.y > targetArea.height)
            p.y = player.cRb.position.y;
        hbullet.Init(p, 1, speed);
        obj.transform.rotation = Quaternion.FromToRotation(Vector3.left, obj.transform.position - p);
    }

    private void OnDrawGizmosSelected()
    {
        float x = (targetArea.x + targetArea.width) * 0.5f,
              y = (targetArea.y + targetArea.height) * 0.5f;
        float w = (targetArea.width - targetArea.x),
              h = (targetArea.y - targetArea.height);
        Gizmos.DrawWireCube(new Vector3(x, y, 0),
                            new Vector3(w, h, 0));

    }
}
