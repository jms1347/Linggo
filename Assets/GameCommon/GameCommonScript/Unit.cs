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

    [Header("����")]
    public GameObject target;
    public bool isAttacking = false;

    void Awake()
    {
        SettingHpBar();
    }

	#region HP �Լ�
	public virtual void SettingHpBar()
    {
        hpBar = this.transform.GetChild(0).GetChild(0);
        if (maxHp == 0) hpBar.transform.parent.gameObject.SetActive(false);

        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }
    //����HP ����
    public virtual void DecreaseHP(int decreaseHp)
    {
        currentHp -= decreaseHp;
        if (currentHp < 0) currentHp = 0;
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }

    //����HP ����
    public virtual void IncreaseHP(int increaseHp)
    {
        currentHp += increaseHp;
        if (currentHp > maxHp) currentHp = maxHp;
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }

    //����HP ����
    public virtual void SetCurrentHp(float changeValue)
    {
        currentHp = changeValue;
        if (currentHp > maxHp) currentHp = maxHp;
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }

    //MAXHP ����
    public virtual void SetMaxHp(float changeValue)
    {
        maxHp += changeValue;
        currentHp += changeValue;
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1); ;
    }
    #endregion
    #region ����� �� ã�� �Լ�
    public virtual GameObject FindNearestObjectByTag(string tag)
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
    #region ����

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
    //        //�̵�
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
            //����
        }
        isAttacking = false;
    }
    #endregion
}
