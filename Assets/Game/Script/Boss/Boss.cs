using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Boss : Monster
{
    public List<Missile> bossMissiles = new List<Missile>();
    [Header("���ݰ��� ����")]
    [HideInInspector]
    public bool isAttacking = false;
    protected IEnumerator attackCour;
    public float attSpeed;

    public int continuousMissileCnt;
    public float continuousMissileTime;

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
    private void Start()
    {
        if (attackCour != null)
            StopCoroutine(attackCour);
        attackCour = ATTACK();
        StartCoroutine(attackCour);
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
                print("�����ϳ�");

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
    
    #region �̻��� ���� �� �̻��� ����(����)
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
        if (attackType == AttackType.OnlyPlayerTarget)
        {
            if (linggo != null)
                currentTarget = linggo;

        }
        else
        {
            currentTarget = FindNearestObjectByTag("Player");

        }

        while (true)
        {
            if (currentTarget != null || currentTarget.tag != "Player" || currentTarget.activeSelf)
            {
                print("����Ÿ���̻����");
                float disF = Vector2.Distance(this.transform.position, currentTarget.transform.position);
                if (attackDistance >= disF)
                {
                    print("�������ݽ���");

                    yield return new WaitUntil(() => monsterState != MonsterState.stun);
                    if(attackType == AttackType.BossContinuousMissile)
                    {
                        int missileCnt = continuousMissileCnt;
                        for (int i = 0; i < bossMissiles.Count; i++)
                        {
                            if (!bossMissiles[i].gameObject.activeSelf)
                            {
                                moveSpeed = 0;
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
                                moveSpeed = 0;
                                monsterAni.SetTrigger("Attack");
                                bossMissiles[i].SettingTarget(currentTarget);
                                break;
                            }
                        }
                    }
                    
                }
                else
                {
                    print("����Ÿ�� �缼��");

                    moveSpeed = saveSpeed;
                    Vector2 dist = (currentTarget.transform.position - this.transform.position).normalized;
                    this.transform.Translate(moveSpeed * Time.deltaTime * dist);

                }
            }
            else
            {
                currentTarget = FindNearestObjectByTag("Player");
            }
            yield return new WaitForSeconds(attSpeed);

        }
    }
    #endregion
    #region �ʱ�ȭ(��Ȱ��)
    public override void InitMonster()
    {
        base.InitMonster();
    }
    #endregion
    #region ���� ���� ���� �Լ�
    public override void ChangeState(MonsterState state)
    {
        monsterState = state;
        monsterImg.color = Color.white;

        switch (monsterState)
        {
            case MonsterState.stun:
                monsterAni.speed = 0;
                //monsterAni.SetBool("Attack", false);
                break;
            case MonsterState.move:
                monsterAni.speed = 1;
                //monsterAni.SetBool("Attack", false);
                moveSpeed = saveSpeed;
                break;
            case MonsterState.attack:
                //monsterAni.SetBool("Attack", true);
                break;
            case MonsterState.sheild:

                break;
        }
    }

    #endregion

    
}
