using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Col_Tri_Event : MonoBehaviour
{
    public UnityEvent events;

    public LayerMask DoLayer;

    public bool once = false;
    public bool enter = true;
    public bool exit = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!enter)
            return;
        if(0 < (collision.gameObject.layer & ~DoLayer))
        {
            events.Invoke();
            if (once)
                this.enabled = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (!exit)
            return;
        if (0 < (collision.gameObject.layer & ~DoLayer))
        {
            events.Invoke();
            if (once)
                this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enter)
            return;
        if (0 < (other.gameObject.layer & ~DoLayer))
        {
            events.Invoke();
            if (once)
                this.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!exit)
            return;
        if (0 < (other.gameObject.layer & ~DoLayer))
        {
            events.Invoke();
            if (once)
                this.enabled = false;
        }
    }
}
