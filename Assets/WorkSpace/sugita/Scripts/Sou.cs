using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sou : MonoBehaviour
{
    bool m_flag;
    public bool Flag{set{ m_flag = value; }}

    // Start is called before the first frame update
    void Start()
    {
        m_flag = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool UpCamera()
    {
        return m_flag;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (m_flag)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player_Root")||
            collision.gameObject.layer == LayerMask.NameToLayer("Player_Bone"))
        {
                m_flag = true;
        }
    }
}
