using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurePeachmon : Skill
{
    [System.Serializable]
    public class LevelUpData
    {
        public int level;
        public float attackCoefficient;
        public float skillCastTime;
        
    }
    public LevelUpData[] levelUpData = new LevelUpData[10];
    int targetCnt = 0;

    public Transform startPos;

    public BoxCollider2D coll;
    IEnumerator skillEffectCour;

    public GameObject startEffectPool;
    public GameObject lastEffectPool;

    public bool isStartEffect = false;
    public bool isLastEffect = false;
    public List<GameObject> colls = new List<GameObject>();

    void Awake()
    {
        coll = this.GetComponent<BoxCollider2D>();

    }

    [System.Obsolete]
    private void OnEnable()
    {
        //Invoke(nameof(OffSkill), 5.5f);

        if (skillEffectCour != null)
            StopCoroutine(skillEffectCour);
        skillEffectCour = SkillEffect();
        StartCoroutine(skillEffectCour);
    }

    [System.Obsolete]
    IEnumerator SkillEffect()
    {
        var time = new WaitForSeconds(0.1f);
        startEffectPool.SetActive(true);
        lastEffectPool.SetActive(false);
        isStartEffect = true;
        isLastEffect = false;
        coll.enabled = true;
        for (int j = 0; j < levelUpData[skillLevel - 1].skillCastTime * 10; j++) yield return time;
        startEffectPool.SetActive(false);
        lastEffectPool.SetActive(true);
        isStartEffect = false;
        isLastEffect = true;
        for (int i = 0; i < colls.Count; i++)
        {
            int damage = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].attackCoefficient);
            colls[i].GetComponent<Monster>().DecreaseHP(damage);
            print("글래이스캣 두번째 스킬 발동");
        }
        for (int j = 0; j < 22; j++) yield return time;
        isStartEffect = false;
        isLastEffect = false;
        colls.Clear();
        this.gameObject.SetActive(false);
    }

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            if (isStartEffect)
            {
                if (targetCnt > 0)
                {
                    targetCnt--;
                    colls.Add(coll.gameObject);
                }
            }
        }
    }
}
