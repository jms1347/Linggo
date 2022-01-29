using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGabibon : Skill
{
    [System.Serializable]
    public class LevelUpData
    {
        public int level;
        public float attackCoefficient;
        public int dotTime;
        public float addAttackCoefficient;
        public int targetNumber;
        public float stunTime;
    }
    public LevelUpData[] levelUpData = new LevelUpData[10];
    BoxCollider2D boxColl;
    int targetCnt = 0;

    IEnumerator skillEffectCour;
    public List<GameObject> colls = new List<GameObject>();
    public List<GameObject> stunEffect = new List<GameObject>();
    void Awake()
    {
        boxColl = this.GetComponent<BoxCollider2D>();
    }

    [System.Obsolete]
    private void OnEnable()
    {
        targetCnt = levelUpData[skillLevel - 1].targetNumber;
        if (skillEffectCour != null)
            StopCoroutine(skillEffectCour);
        skillEffectCour = SkillEffect();
        StartCoroutine(skillEffectCour);
    }

    [System.Obsolete]
    IEnumerator SkillEffect()
    {
        var time = new WaitForSeconds(0.1f);

        for (int i = 0; i < 40; i++) yield return time;
        for (int i = 0; i < stunEffect.Count; i++)
        {
            stunEffect[i].SetActive(false); 
        }
        colls.Clear();
        this.gameObject.SetActive(false);

    }

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            if(targetCnt > 0)
            {
                colls.Add(coll.gameObject);
                stunEffect[targetCnt].transform.position = coll.transform.position;
                stunEffect[targetCnt].SetActive(true);
                coll.gameObject.GetComponent<Monster>().StunEffect(levelUpData[skillLevel - 1].stunTime);

                GameObject hit = Instantiate(hitEffect, coll.transform.position, Quaternion.identity);
                int dotDam = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].addAttackCoefficient);
                coll.gameObject.GetComponent<Monster>().DotEffect(levelUpData[skillLevel - 1].dotTime, dotDam);

                targetCnt--;
            }
        }
    }
}
