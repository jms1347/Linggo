using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//유닛 참조
public class Linggo : MonoBehaviour
{
    public enum UnitState
    {
        move = 0,   //일반
        stun = 1,   //스턴
        sheild = 2, //무적
        bossDbuff =3,
        attack = 4 //공격(모션?)
    }
    public UnitState unitState = UnitState.move;
    public float attackSpeed;

    Animator ani;
    public  List<Missile> linggoMissiles = new List<Missile>();

    [Header("공격")]
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
            //이동
            unitState = UnitState.move;
        }
    }

	#region 미사일 세팅
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


    #region 가까운 적 찾는 함수
    public GameObject FindNearestObjectByTag(string tag)
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


}
