using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class HealObj : MonoBehaviour
{
    public float moveTime;
    public float delayTime;

    public GameObject healEffect;

    public void MoveGoalPos(Transform goalPos,int healHP)
    {
        this.transform.DOMove(goalPos.position, Random.Range(moveTime-0.5f, moveTime+0.6f))
            .SetEase(Ease.InQuart).SetDelay(delayTime)
            .OnComplete(() =>
            {
                GameController.Inst.IncreaseHP(healHP);
                GameObject heal = Instantiate(healEffect, this.transform.position+ new Vector3(Random.Range(-0.2f, 0.5f), Random.Range(0.8f, 1.5f), 0), Quaternion.identity);
                Destroy(this.gameObject);
            });
    }
}
