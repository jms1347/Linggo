using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurePoisonDeer : Skill
{
    [System.Serializable]
    public class LevelUpData
    {
        public int level;
        public float attackCoefficient;
        public int objectCnt;
        public float xRangeAdd;
        public float yRangeAdd;
        public float skillCastingTime;

    }
    public LevelUpData[] levelUpData = new LevelUpData[10];
    public GameObject[] lights;
    public Transform lightPool;
    public Transform purePoisonDeerRange;
    IEnumerator skillEffectCour;

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
        //for (int i = 0; i < levelUpData[skillLevel - 1].skillCastingTime * 10; i++) yield return time;

        lightPool.localScale = new Vector3(levelUpData[skillLevel - 1].xRangeAdd, levelUpData[skillLevel - 1].yRangeAdd, levelUpData[skillLevel - 1].xRangeAdd);
        for (int i = 0; i < levelUpData[skillLevel - 1].objectCnt; i++)
        {
            if (i == 0)
            {
                lights[i].transform.position = purePoisonDeerRange.position;
            }
            else
            {
                lights[i].transform.position = new Vector3(Random.Range(purePoisonDeerRange.transform.position.x - 9.0f, purePoisonDeerRange.transform.position.x + 9.1f), Random.Range(purePoisonDeerRange.transform.position.y - 4.5f, purePoisonDeerRange.transform.position.y + 2.1f), 0);
            }
            lights[i].SetActive(true);
            
        }
        for (int i = 0; i < levelUpData[skillLevel - 1].skillCastingTime * 10; i++) yield return time;
        for (int i = 0; i < levelUpData[skillLevel - 1].objectCnt; i++)
        {
            lights[i].SetActive(false);
        }
        this.gameObject.SetActive(false);
    }
}
