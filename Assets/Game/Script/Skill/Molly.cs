using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molly : Skill
{
    [System.Serializable]
    public class LevelUpData
    {
        public int level;
        public float attackCoefficient;
        public float slowPercent;
        public float slowTime;
        public float durationTime;
        public float dotCoefficient;
        public int dotTime;
    }
    public LevelUpData[] levelUpData = new LevelUpData[10];
    BoxCollider2D boxColl;
    void Start()
    {
        boxColl = this.GetComponent<BoxCollider2D>();
    }

    [System.Obsolete]
    private void OnEnable()
    {
        Invoke(nameof(OffSkill), levelUpData[skillLevel-1].durationTime);
        if (effectSound.Length > 0)
            SoundManager.Inst.SFXPlay("Molly", effectSound[0]);
    }

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            coll.GetComponent<Monster>().SlowEffect(levelUpData[skillLevel-1].slowTime, levelUpData[skillLevel-1].slowPercent);

            int damage = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].attackCoefficient);
            coll.GetComponent<Monster>().DecreaseHP(damage);
            int dotDam = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].dotCoefficient);
            coll.GetComponent<Monster>().DotEffect(levelUpData[skillLevel - 1].dotTime , dotDam);
        }
    }
}
