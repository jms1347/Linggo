using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDonggoos : Skill
{
    [System.Serializable]
    public class LevelUpData
    {
        public int level;
        public float attackCoefficient;
        public float objectCnt;
        public float slowTime;
        public float slowPercent;
    }
    public float ddongLifeTime;
    public LevelUpData[] levelUpData = new LevelUpData[10];
    public Transform ddonggoosRange;
    public Transform ddongPool;
    public GameObject[] ddongs;
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

        ddongPool.position = ddonggoosRange.position;

        for (int i = 0; i < levelUpData[skillLevel-1].objectCnt; i++)
        {
            ddongs[i].SetActive(true);
        }
        for (int i = 0; i < ddongLifeTime * 10; i++) yield return time;
        for (int i = 0; i < levelUpData[skillLevel - 1].objectCnt; i++)
        {
            ddongs[i].SetActive(false);
        }
        this.gameObject.SetActive(false);
    }
}
