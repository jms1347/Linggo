using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ApooApoo : Skill
{
	[System.Serializable]
	public class LevelUpData
	{
		public int level;
		public float attackCoefficient;
		public float xRangeAdd;
		public float yRangeAdd;
		public float nukbackX;
	}
	public LevelUpData[] levelUpData = new LevelUpData[10];

	BoxCollider2D boxColl;
	IEnumerator skillEffectCour;

	public Transform startPos;
    public List<GameObject> colls = new List<GameObject>();
    
        void Awake()
    {
		boxColl = this.GetComponent<BoxCollider2D>();

	}

	[System.Obsolete]
	private void OnEnable()
	{
		//Invoke(nameof(OffSkill), 1.25f);
		if (skillEffectCour != null)
			StopCoroutine(skillEffectCour);
		skillEffectCour = SkillEffect();
		StartCoroutine(skillEffectCour);
	}

	[System.Obsolete]
	IEnumerator SkillEffect()
	{
		var time = new WaitForSeconds(0.1f);
		boxColl.enabled = false;
		boxColl.size = new Vector2(levelUpData[skillLevel - 1].xRangeAdd, levelUpData[skillLevel - 1].yRangeAdd * 4);
		this.transform.localScale = new Vector2(levelUpData[skillLevel - 1].xRangeAdd, levelUpData[skillLevel - 1].yRangeAdd);
		for (int i = 0; i < 10; i++) yield return time;
		boxColl.enabled = true;

        this.transform.DOMoveX(this.transform.position.x - levelUpData[skillLevel].nukbackX, 1.0f);
  //          .OnComplete(()=>
		//{
  //          for (int i = 0; i < colls.Count; i++)
  //          {
  //              colls[i].transform.DOMoveX(colls[i].transform.position.x - levelUpData[skillLevel].nukbackX, 1.0f);
  //          }

  //      });

        for (int i = 0; i < 10; i++) yield return time;
        colls.Clear();
        this.gameObject.SetActive(false);

    }

    private void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Enemy")
		{
            colls.Add(coll.gameObject);
            //coll.transform.DOMoveX(coll.transform.position.x - levelUpData[skillLevel].nukbackX, 1.5f).SetEase(Ease.Linear);

            int damage = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].attackCoefficient);
			coll.gameObject.GetComponent<Monster>().DecreaseHP(damage);
		}
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            colls.Add(coll.gameObject);
            coll.transform.DOMoveX(coll.transform.position.x - levelUpData[skillLevel-1].nukbackX, 1.5f).SetEase(Ease.Linear);

            int damage = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].attackCoefficient);
            coll.GetComponent<Monster>().DecreaseHP(damage);
        }
    }
}
