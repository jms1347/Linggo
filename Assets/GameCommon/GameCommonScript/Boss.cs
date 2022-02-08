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
            if (currentTarget != null)
            {

                float disF = Vector2.Distance(this.transform.position, currentTarget.transform.position);
                if (attackDistance >= disF)
                {


                    yield return new WaitUntil(() => monsterState != MonsterState.stun);
                    
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
                else
                {
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

    #region HP ���� �Լ�

    #endregion
    #region Ÿ�� Ž�� ���� �Լ�


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
