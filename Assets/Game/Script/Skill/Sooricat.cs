using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sooricat : Skill
{
	[System.Serializable]
	public class LevelUpData
	{
		public int level;
		public float attackCoefficient;
		public int targetCnt;

	}
	public LevelUpData[] levelUpData = new LevelUpData[10];
	public GameObject[] sooricats = new GameObject[8];

	BoxCollider2D boxColl;
	IEnumerator skillEffectCour;

	public List<GameObject> colls = new List<GameObject>();
	void Start()
    {
		boxColl = this.GetComponent<BoxCollider2D>();
	}

	[System.Obsolete]
	private void OnEnable()
	{
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

    private void OnTriggerExit2D(Collider2D coll)
    {
		if (coll.tag == "Enemy" )
		{
            for (int i = 0; i < colls.Count; i++)
            {
				if(colls[i] == coll.gameObject)
                {
					colls.RemoveAt(i);
					break;
                }
            }
		}
	}

    [System.Obsolete]
	IEnumerator SkillEffect()
	{
		yield return new WaitForSeconds(1.0f);

		for (int i = 0; i < levelUpData[skillLevel-1].targetCnt; i++)
		{
			if (colls.Count > 0)
			{
				int ran = Random.Range(0, colls.Count);

				sooricats[i].transform.position = colls[ran].transform.position;
				//sooricats[i].transform.position = new Vector2(colls[ran].transform.position.x, colls[ran].transform.position.y - 0.5f);
				sooricats[i].SetActive(true);
				int damage = (int)(GameController.Inst.att * levelUpData[skillLevel-1].attackCoefficient);
				colls[ran].GetComponent<Monster>().DecreaseHP(damage);
				yield return new WaitForSeconds(1.0f);
				sooricats[i].SetActive(false);
				
			}
		}
		colls.Clear();
		this.gameObject.SetActive(false);
	}
}
