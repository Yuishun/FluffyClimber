using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player_Root") || other.gameObject.layer == LayerMask.NameToLayer("Player_Bone"))
        {
            PlayerMovement_y p_ = other.transform.root.GetComponent<PlayerMovement_y>();
            if(p_)
            {
                if(!p_.bDead)
                GameManager_y.ClearGame();
            }
        }

        //SceneManager.LoadScene("Clear");
    }

}
