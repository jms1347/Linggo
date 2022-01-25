using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaliRaim : Skill
{
    [System.Serializable]
    public class LevelUpData
    {
        public int level;
        public float attackCoefficient;
        public int targetNumber;
        public float slowTime;
        public float slowPercent;
        public float healPercent;
    }
    public LevelUpData[] levelUpData = new LevelUpData[10];
    int targetCnt = 0;
    BoxCollider2D boxColl;
    public GameObject healObjPrefab;
    private void Awake()
    {
        boxColl = this.GetComponent<BoxCollider2D>();

    }
    [System.Obsolete]
    private void OnEnable()
    {
        targetCnt = levelUpData[skillLevel - 1].targetNumber;
        OffTimeCount();
    }
    

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            if (targetCnt > 0)
            {
                targetCnt--;

                coll.GetComponent<Monster>().SlowEffect(levelUpData[skillLevel - 1].slowTime, levelUpData[skillLevel - 1].slowPercent);
                int damage = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].attackCoefficient);
                coll.GetComponent<Monster>().DecreaseHP(damage);

                if (levelUpData[skillLevel - 1].healPercent > 0)
                {
                    GameObject healObj = Instantiate(healObjPrefab, coll.transform.position, Quaternion.identity);
                    int heal = Mathf.RoundToInt(coll.GetComponent<Monster>().currentHp * (levelUpData[skillLevel - 1].healPercent * 0.01f));
                    healObj.GetComponent<HealObj>().MoveGoalPos(GameController.Inst.linggo.transform, heal);
                }
            }
        }
    }
}
