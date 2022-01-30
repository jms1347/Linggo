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
        public float skillDurationTime;
    }
    public LevelUpData[] levelUpData = new LevelUpData[10];
    public GameObject rainingRange;
    BoxCollider2D boxColl;
    IEnumerator skillEffectCour;
    float time = 1.0f;
    void Awake()
    {
        boxColl = this.GetComponent<BoxCollider2D>();
        time = 1.0f;

    }

    [System.Obsolete]
    private void OnEnable()
    {
        boxColl.size = new Vector2(levelUpData[skillLevel - 1].xRangeAdd, levelUpData[skillLevel - 1].yRangeAdd);
        rainingRange.transform.localScale = new Vector2(levelUpData[skillLevel - 1].xRangeAdd, levelUpData[skillLevel - 1].yRangeAdd);
        this.transform.localScale = new Vector3(levelUpData[skillLevel - 1].xRangeAdd == 2 ? 2 : 3
            , levelUpData[skillLevel - 1].yRangeAdd == 1 ? 1 : 2
            , levelUpData[skillLevel - 1].xRangeAdd == 2 ? 2 : 3);

        if (skillEffectCour != null)
            StopCoroutine(skillEffectCour);
        skillEffectCour = SkillEffect();
        StartCoroutine(skillEffectCour);
    }
    [System.Obsolete]
    IEnumerator SkillEffect()
    {
        var time = new WaitForSeconds(0.1f);
        for (int i = 0; i < levelUpData[skillLevel - 1].skillDurationTime * 10; i++) yield return time;
        this.gameObject.SetActive(false);
    }


    [System.Obsolete]
    private void OnTriggerStay2D(Collider2D coll)
    {

        if (coll.tag == "Enemy")
        {
            if (time <= 0)
            {
                print("µô");
                int damage = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].attackCoefficient);
                coll.gameObject.GetComponent<Monster>().DecreaseHP(damage);
                time = 1.0f;
            }
            else
            {
                time -= Time.deltaTime;
            }
        }
    }
}
