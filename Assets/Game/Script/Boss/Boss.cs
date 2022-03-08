using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Boss : Monster
{
    public List<Missile> bossMissiles = new List<Missile>();
    [Header("공격관련 변수")]
    [HideInInspector]
    public bool isAttacking = false;
    protected IEnumerator attackCour;
    public float attSpeed;
    public int continuousMissileCnt;
    public float continuousMissileTime;

    [Header("사운드")]
    public AudioClip missileSound;
    public AudioClip moveSound;

    void Awake()
    {
        saveSpeed = moveSpeed;
           monsterAni = this.GetComponent<Animator>();
           monsterImg = this.GetComponent<SpriteRenderer>();
        linggo = GameObject.Find("Linggo");
        SettingHpMpBar();
        SettingMissile();
        SettingEffect();
    }

    public override void Update()
    {
        if (attackType == AttackType.OnlyPlayerTarget || isOnlyPlayer)
        {
            if (linggo != null)
                currentTarget = linggo;

        }
        else
        {
            currentTarget = FindNearestObjectByTag("Player");
        }

        if (currentTarget != null || currentTarget.tag != "Player" || currentTarget.activeSelf)
        {
            float disF = Vector2.Distance(this.transform.position, currentTarget.transform.position);
            if (attackDistance >= disF)
            {
                if (!isAttacking)
                {

                    if (attackCour != null)
                        StopCoroutine(attackCour);
                    attackCour = ATTACK();
                    StartCoroutine(attackCour);
                }
            }
            else
            {
                if (moveSound != null)
                    SoundManager.Inst.SFXPlay("bossMove", moveSound);
                ChangeState(MonsterState.move);
                moveSpeed = saveSpeed;
                Vector2 dist = (currentTarget.transform.position - this.transform.position).normalized;
                this.transform.Translate(moveSpeed * Time.deltaTime * dist);
            }
        }
        else
        {
            currentTarget = FindNearestObjectByTag("Player");
        }
    }

    public void SettingBossData(BossData data)
    {
        this.name = data.bossName;
        this.att = data.bossAtt;
        this.maxHp = data.bossHp;
        this.currentHp = maxHp;
        this.moveSpeed = data.bossMoveSpeed;
        this.attackDistance = data.bossAttackDistance;
        this.attSpeed = data.bossAttSpeed;
        for (int i = 0; i < bossMissiles.Count; i++)
        {
            bossMissiles[i].missileSpeed = data.bossMissileSpeed;
        }

        switch (data.attType)
        {
            case 0:
                this.attackType = AttackType.Boss;
                break;
            case 1:
                this.attackType = AttackType.OnlyPlayerTarget;
                break;
            case 2:
                this.attackType = AttackType.BossContinuousMissile;
                break;
            default:
                this.attackType = AttackType.Boss;
                break;
        }
        this.continuousMissileCnt = data.bossContinuousMissileCnt;
        this.continuousMissileTime = data.bossContinuousMissileTime;
    }
    
    #region 미사일 세팅 및 미사일 생성(공격)
    public void SettingMissile()
    {
        Transform mPool = this.transform.GetChild(6);
        for (int i = 0; i < mPool.childCount; i++)
        {
            bossMissiles.Add(mPool.GetChild(i).GetComponent<Missile>());
        }
    }

    public IEnumerator ATTACK(/*GameObject e*/)
    {
        isAttacking = true;
        ChangeState(MonsterState.attack);

        yield return new WaitUntil(() => monsterState != MonsterState.stun);

        if (attackType == AttackType.BossContinuousMissile)
        {
            int missileCnt = continuousMissileCnt;
            for (int i = 0; i < bossMissiles.Count; i++)
            {
                if (!bossMissiles[i].gameObject.activeSelf)
                {
                    if (missileSound != null)
                        SoundManager.Inst.SFXPlay("bossMissile", missileSound);
                    monsterAni.SetTrigger("Attack");
                    bossMissiles[i].SettingTarget(currentTarget);
                    missileCnt--;

                    if (missileCnt == 0) break;
                    yield return new WaitForSeconds(continuousMissileTime);

                }
            }
        }
        else
        {
            for (int i = 0; i < bossMissiles.Count; i++)
            {
                if (!bossMissiles[i].gameObject.activeSelf)
                {
                    if (missileSound != null)
                        SoundManager.Inst.SFXPlay("bossMissileSound", missileSound);
                    monsterAni.SetTrigger("Attack");
                    bossMissiles[i].SettingTarget(currentTarget);
                    break;
                }
            }
        }
        yield return new WaitForSeconds(attSpeed);
        isAttacking = false;
    }
    #endregion
    #region 초기화(재활용)
    public override void InitMonster()
    {
        base.InitMonster();
    }
    #endregion
    #region 몬스터 상태 관련 함수
    public override void ChangeState(MonsterState state)
    {
        monsterState = state;
        monsterImg.color = Color.white;

        switch (monsterState)
        {
            case MonsterState.stun:
                monsterAni.speed = 0;
                break;
            case MonsterState.move:
                monsterAni.speed = 1;
                moveSpeed = saveSpeed;
                break;
            case MonsterState.attack:
                monsterAni.speed = 1;
                moveSpeed = 0;
                break;
            case MonsterState.sheild:

                break;
        }
    }

    #endregion

    
}
