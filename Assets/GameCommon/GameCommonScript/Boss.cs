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

    public override void Update()
    {
        //링고만 공격
        if (attackType == AttackType.OnlyPlayerTarget)
        {
            if (linggo != null)
                currentTarget = linggo;

        }
        else
        {
            currentTarget = FindNearestObjectByTag("Player");

        }

        if (currentTarget != null)
        {
            Vector2 dist = (currentTarget.transform.position - this.transform.position).normalized;

            float disF = Vector2.Distance(this.transform.position, currentTarget.transform.position);
            if (attackDistance >= disF)
            {
                ChangeState(MonsterState.attack);

            }
            else
            {
                this.transform.Translate(moveSpeed * Time.deltaTime * dist);
            }
        }
        else
        {
            currentTarget = FindNearestObjectByTag("Player");
        }
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
        while (true)
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

            if (currentTarget != null)
            {
                Vector2 dist = (currentTarget.transform.position - this.transform.position).normalized;
                this.transform.Translate(moveSpeed * Time.deltaTime * dist);

                float disF = Vector2.Distance(this.transform.position, currentTarget.transform.position);
                if (attackDistance >= disF)
                {
                    for (int i = 0; i < bossMissiles.Count; i++)
                    {
                        if (!bossMissiles[i].gameObject.activeSelf)
                        {
                            bossMissiles[i].SettingTarget(currentTarget);
                            break;
                        }
                    }
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
    #region 초기화(재활용)
    public override void InitMonster()
    {
        base.InitMonster();
    }

    #endregion

    #region HP 관련 함수

    #endregion
    #region 타켓 탐색 관련 함수


    #endregion

    #region 몬스터 상태 관련 함수
    public override void ChangeState(MonsterState state)
    {
        base.ChangeState(state);
    }

    #endregion

    
}
