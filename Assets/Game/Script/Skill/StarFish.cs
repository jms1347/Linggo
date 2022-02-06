using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StarFish : Skill
{
    [System.Serializable]
    public class LevelUpData
    {
        public int level;
        public float attackCoefficient;
        public float objectCnt;
        public float targetCnt;
        public float skillCastingTime;
        public float fallIntervalTime;
        public float stunTime;
    }
    public LevelUpData[] levelUpData = new LevelUpData[10];

    IEnumerator skillEffectCour;

    public GameObject starFishRange;
    public GameObject[] objects;
    public GameObject[] effects;
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
        for (int i = 0; i < levelUpData[skillLevel - 1].objectCnt; i++)
        {
            objects[i].transform.position = new Vector3(starFishRange.transform.position.x + (1f * i), 4.5f, 0) + new Vector3(1, 0, 0) * 4.5f;
            objects[i].SetActive(true);
            objects[i].GetComponent<BoxCollider2D>().enabled = false;

        }

        for (int i = 0; i < levelUpData[skillLevel-1].objectCnt; i++)
        {
            for (int j = 0; j < levelUpData[skillLevel - 1].skillCastingTime * 10; j++) yield return time;
            objects[i].GetComponent<StarFishHitCollBox>().SkillEffect(new Vector3(starFishRange.transform.position.x + (1f * i), starFishRange.transform.position.y, 0), i);
        }
        for (int i = 0; i < levelUpData[skillLevel - 1].skillCastingTime*10; i++) yield return time;
        
        this.gameObject.SetActive(false);
    }
}
