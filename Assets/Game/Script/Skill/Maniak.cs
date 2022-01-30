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
        public int criticalObjCnt;
        public int criticalCoefficient;
        public float skillCastingTime;

    }
    public LevelUpData[] levelUpData = new LevelUpData[10];
    int targetCnt = 0;

    BoxCollider2D boxColl;
    IEnumerator skillEffectCour;
    public List<GameObject> eyes = new List<GameObject>();
    public List<GameObject> criticalEffect = new List<GameObject>();
    public List<GameObject> basicEffect = new List<GameObject>();
    public List<GameObject> colls = new List<GameObject>();

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

        for (int i = 0; i < 5; i++) yield return time;
        boxColl.enabled = false;
        for (int i = 0; i < levelUpData[skillLevel - 1].skillCastingTime * 10; i++) yield return time;
        //눈 없애기
        for (int i = 0; i < eyes.Count; i++)
            eyes[i].SetActive(false);
        
        for (int i = 0; i < colls.Count; i++)
        {
            if (i < levelUpData[skillLevel - 1].criticalObjCnt)
            {
                criticalEffect[i].transform.position = colls[i].transform.position;
                criticalEffect[i].SetActive(true);
                //크리티컬 데미지
                int damage = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].attackCoefficient * levelUpData[skillLevel - 1].criticalCoefficient);
                colls[i].GetComponent<Monster>().CriticalDecreaseHP(damage);


            }
            else
            {
                basicEffect[i].transform.position = colls[i].transform.position;
                basicEffect[i].SetActive(true);
                //일반 데미지
                int damage = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].attackCoefficient);
                colls[i].GetComponent<Monster>().DecreaseHP(damage);
            }
        }
        for (int i = 0; i < 15; i++) yield return time;
        colls.Clear();
        for (int i = 0; i < criticalEffect.Count; i++)
        {
            basicEffect[i].SetActive(false);
            criticalEffect[i].SetActive(false);
        }
        boxColl.enabled = true;
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
                eyes[targetCnt - 1].transform.position = coll.transform.position; /*+ new Vector3(0, 0.5f, 0);*/
                eyes[targetCnt-1].SetActive(true);
                targetCnt--;
            }
        }
    }
}
