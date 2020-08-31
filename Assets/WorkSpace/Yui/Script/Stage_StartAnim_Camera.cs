using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_StartAnim_Camera : MonoBehaviour
{
    [SerializeField]
    Ragdoll_enable player;
    PlayerMovement_y P;

    [SerializeField]
    Vector3 vec;

    [SerializeField]
    float time = 1f;

    [SerializeField]
    Transform maincamera;

    bool islook = false;
    Transform bone;

    // Start is called before the first frame update
    void Start()
    {
        P = player.GetComponent<PlayerMovement_y>();
        P.enabled = false;
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
        {
            P.enabled = true;
            StartCoroutine(MoveCamera());
        }
    }

    IEnumerator MoveCamera()
    {
        float time = 0;
        Vector3 pos = transform.position;
        Quaternion rot = transform.localRotation;
        while (time <= 1)
        {
            transform.position =
                Vector3.Slerp(pos, maincamera.transform.position, time);
            transform.localRotation =
                Quaternion.Slerp(rot, maincamera.transform.localRotation, time);

            time += Time.deltaTime * 2;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
