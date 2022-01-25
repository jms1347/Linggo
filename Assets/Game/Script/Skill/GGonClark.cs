using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GGonClark : Skill
{
	[System.Serializable]
	public class LevelUpData
	{
		public int level;
		public float attackCoefficient;
		public int objectCnt;
		public float stunTime;
	}
	public LevelUpData[] levelUpData = new LevelUpData[10];


	public Transform startPos;
	public GameObject[] moveObjs;
	public BoxCollider2D[] boxColl;
	IEnumerator skillEffectCour;

	//void Awake()
	//{

	//}

	[System.Obsolete]
	private void OnEnable()
	{
		//Invoke(nameof(OffSkill), 7.0f);

		if (skillEffectCour != null)
			StopCoroutine(skillEffectCour);
		skillEffectCour = SkillEffect();
		StartCoroutine(skillEffectCour);
	}

	[System.Obsolete]
	IEnumerator SkillEffect()
	{
		var time = new WaitForSeconds(0.1f);
		for (int i = 0; i < boxColl.Length; i++)
		{
			moveObjs[i].SetActive(false);
			boxColl[i].gameObject.SetActive(false);
		}

		for (int i = 0; i < levelUpData[skillLevel-1].objectCnt; i++)
        {
			//moveObjs[i].transform.localScale = Vector3.one;
			moveObjs[i].transform.rotation = Quaternion.Euler(0, 0, 45);
			moveObjs[i].transform.position = startPos.position + new Vector3(Random.Range(-2f, 2.1f), 0, 0) + Vector3.one * 10;
			moveObjs[i].SetActive(true);

			moveObjs[i].transform.DOMove(startPos.position + new Vector3(Random.Range(-2f, 2.1f), Random.Range(-2f, 2.1f)), 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
			{
				GameObject exEffect = Instantiate(hitEffect);
				boxColl[i].transform.position = moveObjs[i].transform.position;
				exEffect.transform.position = moveObjs[i].transform.position;
				boxColl[i].gameObject.SetActive(true);
				moveObjs[i].SetActive(false);
			});
			
			for (int j = 0; j< 10; j++) yield return time;
			boxColl[i].gameObject.SetActive(false);

		}
		for (int j = 0; j < 10; j++) yield return time;
		this.gameObject.SetActive(false);
	}
}
