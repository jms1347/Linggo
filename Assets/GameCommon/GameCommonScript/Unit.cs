using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Unit : MonoBehaviour
{
    public enum UnitState
    {
        idle = 0,
        move = 1,
        attack = 2,
        death = 3
    }
    public UnitState unitState = UnitState.idle;
    public int att;
    public float maxHp;
    public float currentHp;
    public float moveSpeed;
    public float attackSpeed;
    public Transform hpBar;

    [Header("공격")]
    public GameObject target;
    public bool isAttacking = false;

    void Awake()
    {
        SettingHpBar();
    }

	#region HP 함수
	public virtual void SettingHpBar()
    {
        hpBar = this.transform.GetChild(0).GetChild(0);
        if (maxHp == 0) hpBar.transform.parent.gameObject.SetActive(false);

        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }
    //현재HP 감소
    public virtual void DecreaseHP(int decreaseHp)
    {
        currentHp -= decreaseHp;
        if (currentHp < 0) currentHp = 0;
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }

    //현재HP 증가
    public virtual void IncreaseHP(int increaseHp)
    {
        currentHp += increaseHp;
        if (currentHp > maxHp) currentHp = maxHp;
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }

    //현재HP 변경
    public virtual void SetCurrentHp(float changeValue)
    {
        currentHp = changeValue;
        if (currentHp > maxHp) currentHp = maxHp;
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }

    //MAXHP 변경
    public virtual void SetMaxHp(float changeValue)
    {
        maxHp += changeValue;
        currentHp += changeValue;
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1); ;
    }
    #endregion
    #region 가까운 적 찾는 함수
    public virtual GameObject FindNearestObjectByTag(string tag)
    {
        // 탐색할 오브젝트 목록을 List 로 저장합니다.
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();

        // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
        var neareastObject = objects
            .OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject;
    }
    #endregion
    #region 공격

    //private void Update()
    //{
    //    e = FindNearestObjectByTag("Enemy");

    //    if (e != null)
    //    {
    //        if (!isAttacking)
    //        {
    //            isAttacking = true;
    //            StartCoroutine(ATTACK(e));
    //        }
    //    }
    //    else
    //    {
    //        //이동
    //        unitState = UnitState.idle;
    //    }
    //}
    public virtual IEnumerator ATTACK(GameObject e)
    {
        GameObject target = e;
        unitState = UnitState.attack;
        while (target != null)
        {
            yield return new WaitForSeconds(attackSpeed);
            //공격
        }
        isAttacking = false;
    }
    #endregion
}
