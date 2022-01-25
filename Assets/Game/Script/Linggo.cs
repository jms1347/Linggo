using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//���� ����
public class Linggo : MonoBehaviour
{
    public enum UnitState
    {
        move = 0,   //�Ϲ�
        stun = 1,   //����
        sheild = 2, //����
        bossDbuff =3,
        attack = 4 //����(���?)
    }
    public UnitState unitState = UnitState.move;
    public float attackSpeed;

    Animator ani;
    public  List<Missile> linggoMissiles = new List<Missile>();

    [Header("����")]
    public GameObject target;
    public bool isAttacking = false;

    void Awake()
    {
        SettingMissile();

        ani = this.GetComponent<Animator>();
        ani.SetBool("isMove", true);
    }

    private void Update()
    {
        target = FindNearestObjectByTag("Enemy");

        if (target != null)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                StartCoroutine(ATTACK(target));
            }
        }
        else
        {
            //�̵�
            unitState = UnitState.move;
        }
    }

	#region �̻��� ����
	public void SettingMissile()
	{
        Transform mPool = this.transform.GetChild(1);
        for (int i = 0; i < mPool.childCount; i++)
        {
            linggoMissiles.Add(mPool.GetChild(i).GetComponent<Missile>());
        }
    }
	#endregion
	private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            if (!isAttacking)
            {
                isAttacking = true;
                StartCoroutine(ATTACK(target));
            }
        }
    }
    public IEnumerator ATTACK(GameObject e)
    {
        GameObject target = e;
        unitState = UnitState.attack;
        while (target != null)
        {
            yield return new WaitForSeconds(attackSpeed);
            if (target == null) break;

            for (int i = 0; i < linggoMissiles.Count; i++)
            {
                if (!linggoMissiles[i].gameObject.activeSelf)
                {
                    linggoMissiles[i].transform.position = this.transform.position;
                    linggoMissiles[i].gameObject.SetActive(true);
                    Vector2 dir = target.transform.position - this.transform.position;
                    linggoMissiles[i].SettingTarget(dir);
                    break;
                }
            }
        }
        isAttacking = false;
    }


    #region ����� �� ã�� �Լ�
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


}
