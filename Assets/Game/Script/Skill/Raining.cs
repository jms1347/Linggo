using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Raining : Skill
{
    [System.Serializable]
    public class LevelUpData
    {
        public int level;
        public float attackCoefficient;
        public float xRangeAdd;
        public float yRangeAdd;
        public float delayTime;
    }
    public LevelUpData[] levelUpData = new LevelUpData[10];

    BoxCollider2D boxColl;
    IEnumerator skillEffectCour;

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
    IEnumerator SkillEffect()
    {
        var time = new WaitForSeconds(0.1f);
        this.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(1.0f);

        //this.transform.DOScale(10, levelUpData[skillLevel - 1].skillCastingTime).SetEase(Ease.Flash)
        //    .OnComplete(() =>
        //    {
        //        boxColl.enabled = true;
        //        hitEffect.transform.position = this.transform.position;
        //        hitEffect.SetActive(true);
        //    });
        //for (int i = 0; i < levelUpData[skillLevel - 1].skillCastingTime * 10; i++) yield return time;
        boxColl.enabled = false;
        this.gameObject.SetActive(false);
    }

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            int damage = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].attackCoefficient);
            coll.gameObject.GetComponent<Monster>().DecreaseHP(damage);
           // coll.gameObject.GetComponent<Monster>().StunEffect(levelUpData[skillLevel - 1].stunTime);

            if (skillLevel >= 5)
            {
             //   int dotDam = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].addAttackCoefficient);
            //    coll.gameObject.GetComponent<Monster>().DotEffect(levelUpData[skillLevel - 1].dotTime, dotDam);
            }
        }
    }
}
