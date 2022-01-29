using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maniak : Skill
{
    [System.Serializable]
    public class LevelUpData
    {
        public int level;
        public float attackCoefficient;

        public int targetNumber;
        public int objCnt;
        public int criticalObjCnt;
        public float skillCastingTime;

    }
    public LevelUpData[] levelUpData = new LevelUpData[10];
    int targetCnt = 0;

    BoxCollider2D boxColl;
    IEnumerator skillEffectCour;
    public List<GameObject> colls = new List<GameObject>();
    public GameObject healObjPrefab;

    private void Awake()
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

        for (int i = 0; i < levelUpData[skillLevel - 1].skillCastingTime * 10; i++) yield return time;
        for (int i = 0; i < colls.Count; i++)
        {
            if (levelUpData[skillLevel - 1].healPercent > 0)
            {
                GameObject healObj = Instantiate(healObjPrefab, colls[i].transform.position, Quaternion.identity);
                int heal = Mathf.RoundToInt(colls[i].GetComponent<Monster>().currentHp * (levelUpData[skillLevel - 1].healPercent * 0.01f));
                healObj.GetComponent<HealObj>().MoveGoalPos(GameController.Inst.linggo.transform, heal);
            }

            colls[i].GetComponent<Monster>().DeathSkill(hitEffect);
        }
        yield return null;
        colls.Clear();
        this.gameObject.SetActive(false);
    }
    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            if (targetCnt > 0)
            {
                colls.Add(coll.gameObject);
                targetCnt--;
            }
        }
    }
}
