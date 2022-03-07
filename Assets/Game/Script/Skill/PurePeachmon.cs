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
        public int goldAcquisitionAmount;
        public float pullingForceCoefficient;
        public bool isDebuffTreatment;
        public float healPercent;
    }
    public LevelUpData[] levelUpData = new LevelUpData[10];

    public Transform startPos;

    public BoxCollider2D coll;
    IEnumerator skillEffectCour;

    public bool isStartEffect = false;
    public List<GameObject> colls = new List<GameObject>();
    public GameObject healObjPrefab;

    void Awake()
    {
        coll = this.GetComponent<BoxCollider2D>();

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
        isStartEffect = true;
        coll.size = new Vector2(3, 3);
        for (int j = 0; j < 20; j++) yield return time;
        isStartEffect = false;
        coll.size = new Vector2(5, 5);
        if (effectSound.Length > 0)
            SoundManager.Inst.SFXPlay("PurePeachmon", effectSound[0]);
        for (int i = 0; i < colls.Count; i++)
        {
            //print("복숭아 스킬 발동");
            if (levelUpData[skillLevel - 1].healPercent > 0)
            {
                GameObject healObj = Instantiate(healObjPrefab, colls[i].transform.position, Quaternion.identity);
                int heal = Mathf.RoundToInt(colls[i].GetComponent<Monster>().currentHp * (levelUpData[skillLevel - 1].healPercent * 0.01f));
                healObj.GetComponent<HealObj>().MoveGoalPos(GameController.Inst.linggo.transform, heal);
                print("heal : " + heal);
            }
            int damage = (int)(GameController.Inst.att * levelUpData[skillLevel - 1].attackCoefficient);
            colls[i].GetComponent<Monster>().DecreasePeachmonHP(damage, levelUpData[skillLevel - 1].goldAcquisitionAmount);

        }

        //무적
        if(levelUpData[skillLevel - 1].isDebuffTreatment)
        {
            GameController.Inst.linggo.GetComponent<Linggo>().ShieldEffect(1.0f);
        }
        for (int j = 0; j < 10; j++) yield return time;
        isStartEffect = false;
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
                colls.Add(coll.gameObject);

            }
        }
    }

    [System.Obsolete]
    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            if (isStartEffect)
            {
                //print("글래이스캣 첫번째 스킬 발동");
                //coll.GetComponent<Monster>().currentTarget = startPos.gameObject;
                coll.transform.position = Vector2.Lerp(coll.transform.position, startPos.position, 0.2f / levelUpData[skillLevel-1].pullingForceCoefficient * Time.deltaTime);
            }
        }
    }
}
