using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MarbleMove : MonoBehaviour
{
    public float moveTime;
    //public float moveScale;

    public Transform goalPos;
    public Transform startPos;

    public void Awake()
    {
        startPos = this.transform.parent.transform;
    }

    private void OnEnable()
    {

        InitMarbleMoveObj();
        MoveGoalPos();
    }

    public void InitMarbleMoveObj()
    {
        this.transform.position = startPos.position;
        //this.gameObject.SetActive(true);
    }

    public void MoveGoalPos()
    {
        //this.transform.DOScale(moveScale, moveTime).SetEase(Ease.InQuart);
        this.transform.DOMove(goalPos.position, moveTime)
            .OnComplete(() =>
            {
                GameController.Inst.marbleExpEffect.SetActive(true);
                this.gameObject.SetActive(false);
            });
    }
}
