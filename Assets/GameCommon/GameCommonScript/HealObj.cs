using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class HealObj : MonoBehaviour
{
    public float moveTime;
    public float delayTime;

    public void MoveGoalPos(Transform goalPos,int healHP)
    {
        this.transform.DOMove(goalPos.position, Random.Range(moveTime-0.5f, moveTime+0.6f))
            .SetEase(Ease.InQuart).SetDelay(delayTime)
            .OnComplete(() =>
            {
                GameController.Inst.IncreaseHP(healHP);
                Destroy(this.gameObject);
            });
    }
}
