using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_StartAnim_Camera : MonoBehaviour
{
    [SerializeField]
    Ragdoll_enable player;

    [SerializeField]
    Vector3 vec;

    [SerializeField]
    float time = 1f;

    bool islook = false;
    Transform bone;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayAnim(time));
    }

    IEnumerator PlayAnim(float time)
    {
        bone = player.transform.GetChild(0);
        player.canGetup = false;
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        player.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        player.AllRagdollChangeVelocity(vec);
        player.canGetup = true;
        islook = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!islook)
            return;

        transform.LookAt(bone);
        if (!player.IsRagdoll)
            gameObject.SetActive(false);
    }
}
