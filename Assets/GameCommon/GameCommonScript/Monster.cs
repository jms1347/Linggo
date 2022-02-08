using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Monster : MonoBehaviour
{
    public bool isBoss;
     public enum AttackType
	{
        CloseExplosion = 0,  //����
        CloseAttribute = 1,  //�����Ӽ�
        FastAttack = 2,            //������
        BigAttack = 3,             //Ŀ��
        FarAttribute = 4,     //���Ÿ��Ӽ�
        OnlyPlayerTarget = 5,  //�÷��̾ Ÿ��
        OnlyPlayerTargetNBigAttack = 6,
        Boss = 7
	}

    public enum MonsterState
    {
        move = 0,   //�Ϲ�
        stun = 1,   //����
        sheild = 2, //����
        attack = 4 //����(���?)
    }
    public MonsterState monsterState = MonsterState.move;
    public AttackType attackType = AttackType.CloseExplosion;
    public new string name;
    public int att;
    public int maxHp;
    public int currentHp;
    public float moveSpeed;

    [HideInInspector]
    public float saveSpeed;
    public float attackDistance;
    [HideInInspector]
    public Transform hpBar;
    [HideInInspector]
    public GameObject maxHpBar;
    public GameObject currentTarget;

    public GameObject linggo;
    public SpriteRenderer monsterImg;
    protected IEnumerator changeColorCour;
    protected IEnumerator slowCour;
    protected IEnumerator stunCour;
    protected IEnumerator dotCour;
    protected IEnumerator levelUpCour;
    protected IEnumerator shieldCour;
    protected IEnumerator bossBuffCour;
    protected Animator monsterAni;

    [Header("����Ʈ ������Ʈ")]
    [HideInInspector]
    public GameObject hitEffect;
    [HideInInspector]
    public GameObject slowEffect;
    [HideInInspector]
    public GameObject stunEffect;
    [HideInInspector]
    public GameObject dotEffect;
    [HideInInspector]
    public GameObject levelUpEffect;
    [HideInInspector]
    public GameObject shieldEffect;
    [HideInInspector]
    public GameObject bossBuffEffect;
     

    [Header("������")]
    public GameObject deathPrefab;
    public GameObject damageTPrefab;
    public GameObject dotDamageTPrefab;
    public GameObject healTPrefab;
    public GameObject criticalDamageTPrefab;
    public GameObject goldPrefab;


    public int CurrentHp { 
        get => currentHp; 
        set
        {
            if(currentHp > 0)
			{
                currentHp = value;
                hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
            }
			else
			{
                currentHp = 0;
                
			}
        }
    }

	void Awake()
    {
        saveSpeed = moveSpeed;
           monsterAni = this.GetComponent<Animator>();
           monsterImg = this.GetComponent<SpriteRenderer>();
        linggo = GameObject.Find("Linggo");
        SettingHpMpBar();
        SettingEffect();
    }

    public virtual void SettingEffect()
    {
        stunEffect = this.transform.GetChild(1).gameObject;
        slowEffect = this.transform.GetChild(2).gameObject;
        dotEffect = this.transform.GetChild(3).gameObject;
        levelUpEffect = this.transform.GetChild(4).gameObject;
        shieldEffect = this.transform.GetChild(5).gameObject;
    }
    private void Update()
    {
        //���� ����
        if (attackType == AttackType.OnlyPlayerTarget || attackType == AttackType.OnlyPlayerTargetNBigAttack)
        {
            if(linggo != null)
                currentTarget = linggo;

        }
        else
        {
            currentTarget = FindNearestObjectByTag("Player");

        }

        if (currentTarget != null)
        {
            Vector2 dist = (currentTarget.transform.position - this.transform.position).normalized;
            this.transform.Translate(moveSpeed * Time.deltaTime * dist);

            float disF = Vector2.Distance(this.transform.position, currentTarget.transform.position);
            if(attackDistance >= disF)
            {
                ChangeState(MonsterState.attack);
                if(attackType == AttackType.BigAttack || attackType == AttackType.OnlyPlayerTargetNBigAttack)
                {                    
                    this.transform.DOScale(2.0f, 2.0f).SetEase(Ease.Flash);
                }else if(attackType  == AttackType.FastAttack)
                {
                    moveSpeed =  3;
                }
            }            
        }
        else
        {
            currentTarget = FindNearestObjectByTag("Player");
        }

    }
    //public void DeathChangePos()
    //{
    //    this.transform.position = new Vector3(-15, 0, 0);
    //}
    //public void OnDisable()
    //{
    //    Invoke(nameof(DeathChangePos), 3.0f);
    //}
    #region �ʱ�ȭ(��Ȱ��)
    public virtual void InitMonster()
    {
        this.transform.DOKill();
        StopAllCoroutines();
        this.transform.localScale = Vector3.one;
        ChangeState(MonsterState.move);
        att = GameController.Inst.monsterAppearanceLevelDataSO.monsterAppearanceLevelData[GameController.Inst.wave - 1].monsterAtt;
        SetMaxHp(GameController.Inst.monsterAppearanceLevelDataSO.monsterAppearanceLevelData[GameController.Inst.wave - 1].monsterHp);
        stunEffect.SetActive(false);
        slowEffect.SetActive(false);
        dotEffect.SetActive(false);
        levelUpEffect.SetActive(false);
        shieldEffect.SetActive(false);
    }
    #endregion

    #region HP ���� �Լ�
    public void PlusMaxHp(int changeValue)
    {
        maxHp += changeValue;
        currentHp += changeValue;
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }
    public void SetMaxHp(int changeValue)
    {
        maxHpBar.SetActive(false);
        maxHp = changeValue;
        currentHp = maxHp;
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }

    public void SettingHpMpBar()
	{
        maxHpBar = this.transform.GetChild(0).gameObject;
        hpBar = this.transform.GetChild(0).GetChild(0);
        if (maxHp == 0) hpBar.transform.parent.gameObject.SetActive(false);

        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }
    public void CriticalDecreaseHP(int decreaseHp)
    {
        if (monsterState == MonsterState.sheild) return;
        ChangeColorEffect(Color.red);

        maxHpBar.SetActive(true);
        currentHp -= decreaseHp;
        GameObject damageT = Instantiate(criticalDamageTPrefab, this.transform.position, Quaternion.identity);
        damageT.GetComponent<DamageText>().SetDecreaseText(decreaseHp.ToString());
        if (currentHp < 0)
        {
            currentHp = 0;
            Instantiate(deathPrefab, this.transform.position, Quaternion.identity);
            GameObject gold = Instantiate(goldPrefab, this.transform.position, Quaternion.identity);
            gold.GetComponent<Gold>().MoveGoalPos(GameController.Inst.goldText.transform.parent);
            this.gameObject.SetActive(false);

            GameController.Inst.PlusKillCnt();


        }
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }
    public void DecreaseHP(int decreaseHp)
	{
        if (monsterState == MonsterState.sheild) return;
        ChangeColorEffect(Color.red);

        maxHpBar.SetActive(true);
        currentHp -= decreaseHp;
        GameObject damageT = Instantiate(damageTPrefab, this.transform.position, Quaternion.identity);
        damageT.GetComponent<DamageText>().SetDecreaseText(decreaseHp.ToString());
        if (currentHp < 0)
        {
            currentHp = 0;
            Instantiate(deathPrefab, this.transform.position, Quaternion.identity);
            GameObject gold = Instantiate(goldPrefab, this.transform.position, Quaternion.identity);
            gold.GetComponent<Gold>().MoveGoalPos(GameController.Inst.goldText.transform.parent);
            this.gameObject.SetActive(false);

            GameController.Inst.PlusKillCnt();
        }
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }
    public void DecreasePeachmonHP(int decreaseHp, int goldAcquisitionAmount)
    {
        if (monsterState == MonsterState.sheild) return;
        ChangeColorEffect(Color.red);

        maxHpBar.SetActive(true);
        currentHp -= decreaseHp;
        GameObject damageT = Instantiate(damageTPrefab, this.transform.position, Quaternion.identity);
        damageT.GetComponent<DamageText>().SetDecreaseText(decreaseHp.ToString());
        if (currentHp < 0)
        {
            currentHp = 0;
            Instantiate(deathPrefab, this.transform.position, Quaternion.identity);
            for (int i = 0; i < goldAcquisitionAmount; i++)
            {
                GameObject gold = Instantiate(goldPrefab, this.transform.position, Quaternion.identity);
                gold.GetComponent<Gold>().MoveGoalPos(GameController.Inst.goldText.transform.parent);
                this.gameObject.SetActive(false);
            }
           

            GameController.Inst.PlusKillCnt();
        }
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }

    public void DotDecreaseHP(int decreaseHp)
    {
        if (monsterState == MonsterState.sheild) return;
        ChangeColorEffect(Color.red);

        maxHpBar.SetActive(true);
        currentHp -= decreaseHp;
        GameObject damageT = Instantiate(dotDamageTPrefab, this.transform.position, Quaternion.identity);
        damageT.GetComponent<DamageText>().SetDecreaseText(decreaseHp.ToString());
        if (currentHp < 0)
        {
            currentHp = 0;
            Instantiate(deathPrefab, this.transform.position, Quaternion.identity);
            GameObject gold = Instantiate(goldPrefab, this.transform.position, Quaternion.identity);
            gold.GetComponent<Gold>().MoveGoalPos(GameController.Inst.goldText.transform.parent);
            this.gameObject.SetActive(false);

            GameController.Inst.PlusKillCnt();
        }
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }

    public void DeathSkill(GameObject deathEffect)
    {
        if (monsterState == MonsterState.sheild) return;

        GameObject damageT = Instantiate(criticalDamageTPrefab, this.transform.position, Quaternion.identity);
        damageT.GetComponent<DamageText>().SetDecreaseText(currentHp.ToString());
        currentHp = 0;
        Instantiate(deathEffect, this.transform.position, Quaternion.identity);
        GameObject gold = Instantiate(goldPrefab, this.transform.position, Quaternion.identity);
        gold.GetComponent<Gold>().MoveGoalPos(GameController.Inst.goldText.transform.parent);
        this.gameObject.SetActive(false);

        GameController.Inst.PlusKillCnt();
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }

    public void IncreaseHP(int increaseHp)
	{
        currentHp += increaseHp; 
        GameObject damageT = Instantiate(healTPrefab, this.transform.position, Quaternion.identity);
        damageT.GetComponent<DamageText>().SetIncreaseText(increaseHp.ToString());

        if (currentHp > maxHp) currentHp = maxHp;
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }
    #endregion
    #region Ÿ�� Ž�� ���� �Լ�
    public GameObject FindNearestObjectByTag(string tag)
    {
        // Ž���� ������Ʈ ����� List �� �����մϴ�.
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();

        // LINQ �޼ҵ带 �̿��� ���� ����� ���� ã���ϴ�.
        var neareastObject = objects
            .OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject;
    }

    #endregion

    #region ���� ���� ���� �Լ�
    public virtual void ChangeState(MonsterState state)
    {
         monsterState = state;
        monsterImg.color = Color.white;

        switch (monsterState)
        {
            case MonsterState.stun:
                monsterAni.speed = 0;
                monsterAni.SetBool("Attack", false);
                break;
            case MonsterState.move:
                monsterAni.speed = 1;
                monsterAni.SetBool("Attack", false);
                moveSpeed = saveSpeed;
                break;
            case MonsterState.attack:
                monsterAni.SetBool("Attack",true);
                break;
            case MonsterState.sheild:

                break;
        }
    }
    #endregion

    #region ���� �Լ�
    public void StunEffect(float time)
    {
        if (monsterState == MonsterState.sheild) return;
        if (!this.gameObject.activeSelf) return;
        if (stunCour != null)
            StopCoroutine(stunCour);
        stunCour = StunEffectCour(time);
        StartCoroutine(stunCour);
    }

    public IEnumerator StunEffectCour(float time)
    {
        var t = new WaitForSeconds(0.1f);
        ChangeState(MonsterState.stun);
        stunEffect.SetActive(true);
        float tempMove = moveSpeed;
        moveSpeed = 0;
        for (int i = 0; i < time * 10; i++) yield return t;
        ChangeState(MonsterState.move);

        stunEffect.SetActive(false);
        moveSpeed = tempMove;
    }
    #endregion
    #region ���ο� �Լ�
    public void SlowEffect(float time, float moveDecreasePercent)
    {
        if (!this.gameObject.activeSelf) return;

        if (slowCour != null)
            StopCoroutine(slowCour);
        slowCour = SlowEffectCour(time, moveDecreasePercent);
        StartCoroutine(slowCour);
    }

    public IEnumerator SlowEffectCour(float time, float moveDecreasePercent = 0.5f)
    {
        var t = new WaitForSeconds(0.1f);
        slowEffect.SetActive(true);
        float tempMove = saveSpeed;

        if(monsterState != MonsterState.stun)
            monsterAni.speed = moveDecreasePercent;

        moveSpeed *= moveDecreasePercent;
        for (int i = 0; i < time * 10; i++) yield return t;
        slowEffect.SetActive(false);
        moveSpeed = tempMove;
    }
    #endregion
    #region ��Ʈ ������ �Լ�
    public void DotEffect(int time, float damage)
    {
        if (monsterState == MonsterState.sheild) return;
        if (!this.gameObject.activeSelf) return;
        if (dotCour != null)
            StopCoroutine(dotCour);
        dotCour = DotEffectCour(time, damage);
        StartCoroutine(dotCour);

    }
    public IEnumerator DotEffectCour(int time, float damage)
    {
        var t = new WaitForSeconds(1.0f);
        dotEffect.SetActive(true);
        for (int i = 0; i < time; i++)
        {
            int dotDam = (int)(damage / time);
            DotDecreaseHP(dotDam);
            //print("��Ʈ�� : " + dotDam + " / " + damage);

            yield return t;
        }
        dotEffect.SetActive(false);
    }
    #endregion
    #region �������� �ڷ�ƾ
    public void BossBuffEffect(float time)
    {
        if (!this.gameObject.activeSelf) return;

        if (bossBuffCour != null)
            StopCoroutine(bossBuffCour);
        bossBuffCour = BossBuffEffectCour(time);
        StartCoroutine(bossBuffCour);
    }
    public IEnumerator BossBuffEffectCour(float time)
    {
        var t = new WaitForSeconds(0.1f);

        bossBuffEffect.SetActive(true);

        for (int i = 0; i < time*10; i++) yield return t;
        bossBuffEffect.SetActive(false);

    }
    #endregion
    #region ���� ���� �ڷ�ƾ
    public void ChangeColorEffect(Color color)
    {
        if (!this.gameObject.activeSelf) return;

        if (changeColorCour != null)
            StopCoroutine(changeColorCour);
        changeColorCour = ColorCour(color);
        StartCoroutine(changeColorCour);
    }
    public IEnumerator ColorCour(Color ChangeColor)
    {
        var time = new WaitForSeconds(0.1f);
        
        monsterImg.color = Color.white;
        for (int i = 0; i < 3; i++) yield return time;
        monsterImg.color = ChangeColor;
        for (int i = 0; i < 3; i++) yield return time;
        monsterImg.color = Color.white;

    }
    #endregion

    #region ����(������) �Լ�
    public void LevelUpEffect()
    {
        if (!this.gameObject.activeSelf) return;

        if (levelUpCour != null)
            StopCoroutine(levelUpCour);
        levelUpCour = LevelUpEffectCour();
        StartCoroutine(levelUpCour);
    }

    public IEnumerator LevelUpEffectCour()
    {
        var t = new WaitForSeconds(0.1f);
        levelUpEffect.SetActive(true);
        //���� ���Ѿߵ�(����)
        LevelUp();
        for (int i = 0; i < 10; i++) yield return t;
        levelUpEffect.SetActive(false);       

    }

    public void LevelUp()
    {
        att = GameController.Inst.monsterAppearanceLevelDataSO.monsterAppearanceLevelData[GameController.Inst.wave - 1].monsterAtt;
        PlusMaxHp(GameController.Inst.monsterAppearanceLevelDataSO.monsterAppearanceLevelData[GameController.Inst.wave - 1].monsterHp);

    }
    #endregion

    #region ���� �Լ�
    public void ShieldEffect(float time)
    {
        if (!this.gameObject.activeSelf) return;
        if (shieldCour != null)
            StopCoroutine(shieldCour);
        shieldCour = ShieldEffectCour(time);
        StartCoroutine(shieldCour);
    }

    public IEnumerator ShieldEffectCour(float time)
    {
        var t = new WaitForSeconds(0.1f);
        shieldEffect.SetActive(true);
        ChangeState(MonsterState.sheild);

        for (int i = 0; i < time * 10; i++) yield return t;
        shieldEffect.SetActive(false);
        ChangeState(MonsterState.move);
    }
    #endregion

    #region ���̾� ����
    public void ChangeLayer(int layerIndex)
    {
        this.GetComponent<SpriteRenderer>().sortingOrder = layerIndex;
    }
    #endregion
}
