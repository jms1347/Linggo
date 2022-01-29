using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Adrok : Skill
{
	[System.Serializable]
	public class LevelUpData
	{
		public int level;
		public float attackCoefficient;
		public float xRangeAdd;
		public float yRangeAdd;
		public float nukbackX;
		public float stunTime;

	}
	public LevelUpData[] levelUpData = new LevelUpData[10];

	BoxCollider2D boxColl;
	IEnumerator skillEffectCour;

	public Transform startPos;
	public GameObject adrokRange;

    public List<GameObject> colls = new List<GameObject>();

    void Awake()
    {
		boxColl = this.GetComponent<BoxCollider2D>();

	}

	[System.Obsolete]
	private void OnEnable()
	{
		adrokRange.transform.localScale = new Vector2(levelUpData[skillLevel-1].xRangeAdd, levelUpData[skillLevel-1].yRangeAdd);

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
		this.transform.localScale = Vector3.one;
		this.transform.rotation = Quaternion.Euler(0, 0, 45);
		this.transform.position = startPos.position + Vector3.one*10;
		this.transform.DOMove(startPos.position, 0.5f).SetEase(Ease.InQuad).OnComplete(()=>
		{
			GameObject exEffect = Instantiate(hitEffect);
			exEffect.transform.localScale = new Vector3(levelUpData[skillLevel-1].xRangeAdd, levelUpData[skillLevel-1].yRangeAdd, levelUpData[skillLevel].yRangeAdd);
			exEffect.transform.position = this.transform.position; 
		});
		for (int i = 0; i < 5; i++) yield return time;
		boxColl.transform.localScale = new Vector2(levelUpData[skillLevel - 1].xRangeAdd, levelUpData[skillLevel - 1].yRangeAdd);
		boxColl.enabled = true;
		yield return null;
		boxColl.transform.localScale = Vector3.one;

		this.transform.DOScale(levelUpData[skillLevel-1].xRangeAdd, 1.5f);
		this.transform.rotation = Quaternion.identity;

        this.transform.DOMoveX(this.transform.position.x - levelUpData[skillLevel-1].nukbackX, 1.5f).SetEase(Ease.Linear);
        //yield return null;
        //for (int i = 0; i < colls.Count; i++)
        //{
        //    colls[i].transform.DOMoveX(colls[i].transform.position.x - levelUpData[skillLevel].nukbackX, 1.5f).SetEase(Ease.Linear);
        //}
        for (int i = 0; i < 15; i++) yield return time;

        colls.Clear();
        this.gameObject.SetActive(false);

    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
		if (coll.gameObject.tag == "Enemy")
        {
            colls.Add(coll.gameObject);
            //coll.transform.DOMoveX(coll.transform.position.x - levelUpData[skillLevel].nukbackX, 1.5f).SetEase(Ease.Linear);

            if (skillLevel > 4)
            {
				coll.gameObject.GetComponent<Monster>().StunEffect(levelUpData[skillLevel-1].stunTime);

			}
			int damage = (int)(GameController.Inst.att * levelUpData[skillLevel-1].attackCoefficient);
			coll.gameObject.GetComponent<Monster>().DecreaseHP(damage);
		}
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            colls.Add(coll.gameObject);
            coll.transform.DOMoveX(coll.transform.position.x - levelUpData[skillLevel].nukbackX, 1.5f).SetEase(Ease.Linear);

            if (skillLevel > 4)
            {
                coll.GetComponent<Monster>().StunEffect(levelUpData[skillLevel - 1].stunTime);

            }
            int damage = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].attackCoefficient);
            coll.GetComponent<Monster>().DecreaseHP(damage);
        }
    }
}
