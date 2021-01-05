using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeathCommentHolder : MonoBehaviour
{
    public DeathManager.TrapType trapType;
    public int trapNumber;
    public bool bExcessUse = false;
    public bool bActiveTrigger;
    public bool bScreenSpaceComment;

    public Vector3 dispPos;

    [System.Serializable]
    public struct CommentSt
    {
        public int deathNum;    //  表示条件
        [SerializeField] public List<DeathCommentManager.CommentInfo> commentList;
    }
    [SerializeField] public List<CommentSt> comments = new List<CommentSt>();
    private int activeIndex = -1;

    private Collider triggerBox;
    private bool bActive = false;

    void Start()
    {
        triggerBox = gameObject.GetComponent<Collider>();
        if(triggerBox != null && !bActiveTrigger)
        {
            triggerBox.enabled = false;
        }

        DeathManager.RegisterTrap(trapNumber, trapType);

        CheckDispCondition();
    }

    void Update()
    {
        if(bActive && !bActiveTrigger)
        {
            if(GameManager_y.Instance.bInGame)
            {
                ShowComment();
                bActive = false;
            }
        }
    }

    //  トラップでプレイヤー死亡時に呼び出し
    public void IncreaseDeathCount()
    {
        DeathManager.UpdateDeathInfo(trapNumber);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!bActiveTrigger)
            return;

        if (!bActive)
            return;

        if (0 < (other.gameObject.layer &
            LayerMask.GetMask("Player_Root", "Player_Bone")))
        {
            var p = other.transform.root.GetComponent<PlayerMovement_y>();
            if(p != null)
            {
                if(!p.bDead)
                {
                    ShowComment();
                    bActive = false;
                }
            }
        }
    }

    private void ShowComment()
    {
        if (bScreenSpaceComment)
        {
            DeathCommentManager.ShowSSComment(comments[activeIndex].commentList);
        }
        else
        {
            DeathCommentManager.ShowWSComment(comments[activeIndex].commentList, dispPos);
        }
    }

    private void CheckDispCondition()
    {
        if(trapType != DeathManager.TrapType.None)
        {
            DeathManager.CauseOfDeath _cod = DeathManager.GetPreviousDeathInfo();
            if(trapNumber == _cod.TrapNumber)
            {
                int i = 0;
                for(i = 0; i < comments.Count; ++i)
                {
                    if(comments[i].deathNum == _cod.DeathCount)
                    {
                        activeIndex = i;
                        bActive = true;
                        return;
                    }
                }
                if(bExcessUse)
                {
                    if(i >= comments.Count && _cod.DeathCount > comments[comments.Count - 1].deathNum)
                    {
                        activeIndex = comments.Count - 1;
                        bActive = true;
                        return;
                    }
                }
            }
        }
        else
        {
            int _deathNum = DeathManager.GetTotalDeathCount();
            for (int i = 0; i < comments.Count; ++i)
            {
                if(_deathNum == comments[i].deathNum)
                {
                    activeIndex = i;
                    bActive = true;
                    break;
                }
            }
        }
    }
}
