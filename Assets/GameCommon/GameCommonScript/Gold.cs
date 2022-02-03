using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gold : MonoBehaviour
{
    public float moveTime;
    public float delayTime;
    public float moveScale;
    
    public void MoveGoalPos(Transform goalPos)
    {
        float ran = Random.Range(moveTime-0.5f, moveTime + 0.5f);
        this.transform.DOScale(moveScale, ran).SetDelay(delayTime).SetEase(Ease.InQuart);
        this.transform.DOMove(goalPos.position, ran)
            .SetEase(Ease.InQuart).SetDelay(delayTime)
            .OnComplete(() =>
            {
                GameController.Inst.IncreaseGold(GameController.Inst.linggoLevelDataSO.levelData[GameController.Inst.wave - 1].killRewardGold);
                Destroy(this.gameObject);
            });
    }

}
