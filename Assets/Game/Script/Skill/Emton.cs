using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emton : Skill
{
	[System.Serializable]
	public class LevelUpData
	{
		public int level;
		public int objectCnt;
		public float attackCoefficient;
		public float xRangeAdd;
		public float yRangeAdd;
	}
	public LevelUpData[] levelUpData = new LevelUpData[10];
	public GameObject[] emtons = new GameObject[7];
	public GameObject emtonRange;
	BoxCollider2D boxColl;
	IEnumerator skillEffectCour;

	void Awake()
    {
		boxColl = this.GetComponent<BoxCollider2D>();
	}

	[System.Obsolete]
	private void OnEnable()
	{
		//OffTimeCount();
		boxColl.size = new Vector2(levelUpData[skillLevel-1].xRangeAdd, levelUpData[skillLevel-1].yRangeAdd);
		emtonRange.transform.localScale = new Vector2(levelUpData[skillLevel-1].xRangeAdd, levelUpData[skillLevel-1].yRangeAdd);

		for (int i = 0; i < emtons.Length; i++)
        {
			emtons[i].transform.localScale = new Vector3(levelUpData[skillLevel-1].xRangeAdd*0.5f, levelUpData[skillLevel-1].yRangeAdd*0.5f, levelUpData[skillLevel-1].yRangeAdd * 0.5f);
		}
		if (skillEffectCour != null)
			StopCoroutine(skillEffectCour);
		skillEffectCour = SkillEffect();
		StartCoroutine(skillEffectCour);
	}
	[System.Obsolete]
	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.tag == "Enemy")
		{
			int damage = (int)(GameController.Inst.att * levelUpData[skillLevel-1].attackCoefficient);
			coll.GetComponent<Monster>().DecreaseHP(damage);
		}
	}


	[System.Obsolete]
	IEnumerator SkillEffect()
	{
		yield return null;
        for (int i = 0; i < levelUpData[skillLevel-1].objectCnt; i++)
        {
			emtons[i].SetActive(true);
			boxColl.enabled =true;
			yield return new WaitForSeconds(1.0f);
			emtons[i].SetActive(false);
			boxColl.enabled = false;
			yield return new WaitForSeconds(0.2f);

		}

		this.gameObject.SetActive(false);
	}
}
