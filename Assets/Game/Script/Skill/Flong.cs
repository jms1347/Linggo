using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flong : Skill
{
	[System.Serializable]
	public class LevelUpData
	{
		public int level;
		public float attackCoefficient;
		public float bossAddDamageCoefficient;
	}
	public LevelUpData[] levelUpData = new LevelUpData[10];
	int targetCnt = 0;
	public List<GameObject> colls = new List<GameObject>();

	BoxCollider2D boxColl;
	IEnumerator skillEffectCour;
	public GameObject flongEffect;
	private void Awake()
	{
		boxColl = this.GetComponent<BoxCollider2D>();

	}

	[System.Obsolete]
	private void OnEnable()
	{
		targetCnt = 1;
		//OffTimeCount();

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
			colls.Add(coll.gameObject);
		}
	}

	[System.Obsolete]
	IEnumerator SkillEffect()
	{
		while (targetCnt > 0)
		{
			bool isBoss = false;
            for (int i = 0; i < colls.Count; i++)
            {
                if (colls[i].name.Contains("Boss"))
				{

					targetCnt--;
					print("ÇÃ·Õº¸½º : " + targetCnt);
					flongEffect.transform.position = colls[i].transform.position;
					flongEffect.SetActive(true);
					int damage = (int)(GameController.Inst.att * levelUpData[skillLevel-1].attackCoefficient * levelUpData[skillLevel-1].bossAddDamageCoefficient);
					colls[i].GetComponent<Monster>().DecreaseHP(damage);
					isBoss = true;
					break;
				}
            }
            if (!isBoss)
            {
				for (int i = 0; i < colls.Count; i++)
				{

					targetCnt--;
					print("ÇÃ·Õ³ëº¸½º : " + targetCnt);
					flongEffect.transform.position = colls[i].transform.position;
					flongEffect.SetActive(true);
					int damage = (int)(GameController.Inst.att * levelUpData[skillLevel-1].attackCoefficient);
					colls[i].GetComponent<Monster>().DecreaseHP(damage);
					break;
				}
			}
			
			yield return new WaitForSeconds(1.1f);

		}
		colls.Clear();
		this.gameObject.SetActive(false);
	}
}
