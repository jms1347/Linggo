using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pence : Skill
{
	[System.Serializable]
	public class LevelUpData
    {
		public int level;
		public float stunTime;
		public float attackCoefficient;
		public int targetNumber;
    }
	public LevelUpData[] levelUpData = new LevelUpData[10];
	int targetCnt = 0;
	BoxCollider2D boxColl;


	private void Awake()
	{
		boxColl = this.GetComponent<BoxCollider2D>();
		
	}

	[System.Obsolete]
	private void OnEnable()
	{
		targetCnt = levelUpData[skillLevel-1].targetNumber;
		OffTimeCount();

        if(effectSound.Length > 0)
            SoundManager.Inst.SFXPlay("Pence", effectSound[0]);
        
    }

	[System.Obsolete]
    private void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.tag == "Enemy")
		{
			if(targetCnt > 0)
			{
				targetCnt--;

				coll.GetComponent<Monster>().StunEffect(levelUpData[skillLevel-1].stunTime);
				int damage = (int)(GameController.Inst.att * levelUpData[skillLevel-1].attackCoefficient);
				coll.GetComponent<Monster>().DecreaseHP(damage);
			}			
		}
	}
}
