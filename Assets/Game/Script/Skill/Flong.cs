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
	public List<GameObject> colls = new List<GameObject>();

	BoxCollider2D boxColl;
	IEnumerator skillEffectCour;
	public GameObject flongEffect;
    public Transform flongPos;
	private void Awake()
	{
		boxColl = this.GetComponent<BoxCollider2D>();

	}

	[System.Obsolete]
	private void OnEnable()
	{
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
            for (int i = 0; i < colls.Count; i++)
            {
                if (colls[i].name.Contains("Boss"))
                {
                    int damage = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].attackCoefficient * levelUpData[skillLevel - 1].bossAddDamageCoefficient);
                    colls[i].GetComponent<Monster>().DecreaseHP(damage);
                }
                else
                {
                    int damage = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].attackCoefficient);
                    colls[i].GetComponent<Monster>().DecreaseHP(damage);
                }
            }
        }
	}

	[System.Obsolete]
	IEnumerator SkillEffect()
    {
        var time = new WaitForSeconds(0.1f);

        flongEffect.transform.position = flongPos.position;
        flongEffect.SetActive(true);
        for (int j = 0; j < 12; j++) yield return time;
        flongEffect.SetActive(false);

    }
}
