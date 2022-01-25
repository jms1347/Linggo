using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMining : Skill
{
    [System.Serializable]
    public class LevelUpData
    {
        public int level;
        public float attackCoefficient;
        public float criticalPercent;
        public float criticalCoefficient;
        public int targetCnt;

    }
    public LevelUpData[] levelUpData = new LevelUpData[10];

    BoxCollider2D boxColl;
    IEnumerator skillEffectCour;

    bool isCriticalMode = false;
    int targetCount;
    void Start()
    {
        boxColl = this.GetComponent<BoxCollider2D>();
    }

    [System.Obsolete]
    private void OnEnable()
    {
        targetCount = levelUpData[skillLevel - 1].targetCnt;

        isCriticalMode = false;
        int ran = Random.Range(1, 101);
        if (ran <= levelUpData[skillLevel-1].criticalPercent)
        {
            hitEffect.SetActive(true);
            isCriticalMode = true;
        }
        OffTimeCount();
    }


    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            if(targetCount > 0)
            {
                targetCount--;
                if (isCriticalMode)
                {
                    int damage = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].attackCoefficient);
                    coll.GetComponent<Monster>().CriticalDecreaseHP(damage);
                }
                else
                {
                    int damage = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].attackCoefficient * levelUpData[skillLevel-1].criticalCoefficient);
                    coll.GetComponent<Monster>().DecreaseHP(damage);
                }
            }
            
        }
    }
}
