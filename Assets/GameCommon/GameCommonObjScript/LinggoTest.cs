using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LinggoTest : MonoBehaviour
{
    public float currentHp;
    public float maxHp;
	public float moveSpeed;
    public float att;
    public float attackSpeed;
    public int level;

	Animator linggoAni;
	Rigidbody2D rigid;
	public Image hpBar;

    public GameObject missile;
    public GameObject e;
    public List<GameObject> missiles = new List<GameObject>();
    bool isAttacking = false;

    public enum UnitState
    {
        idle = 0,
        attack = 1,
        death = 2
    }
    public UnitState unitState = UnitState.idle;

    private void Awake()
	{
		rigid = this.GetComponent<Rigidbody2D>();
		linggoAni = this.GetComponent<Animator>();
		
	}
	void Start()
    {
	}

	private void Update()
    {
        if (currentHp <= 0)
        {
            Destroy(this.gameObject);
        }
        e = FindNearestObjectByTag("Enemy");

        if (e != null)
        {
            //����
            if (Vector3.Distance(transform.localPosition, e.transform.localPosition) > 1000)
                this.transform.position = Vector3.Lerp(this.transform.position, e.transform.position, Time.deltaTime * moveSpeed * 0.5f);
            else
            {
                if (!isAttacking)
                {
                    isAttacking = true;
                    StartCoroutine(ATTACK(e));
                }
            }
        }
        else
        {
            //�̵�
            unitState = UnitState.idle;
            linggoAni.SetFloat("lingomove", moveSpeed);
            this.transform.position += new Vector3(moveSpeed, 0, 0);
        }
    }
    private GameObject FindNearestObjectByTag(string tag)
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

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            if (!isAttacking)
            {
                isAttacking = true;
                StartCoroutine(ATTACK(e));
            }
        }
    }

    IEnumerator ATTACK(GameObject e)
    {
        GameObject target = e;
        Vector3 oriPos = this.transform.localPosition;
        unitState = UnitState.attack;

        while (target != null)
        {
            //Vector2 dir = target.transform.position - this.transform.position;

            yield return new WaitForSeconds(attackSpeed);
            GameObject m = Instantiate(missile);
            //m.GetComponent<LinggoMssile>().damage = att;
            m.transform.SetParent(this.transform.parent);
            m.transform.localScale = Vector3.one;
            m.transform.position = this.transform.position;
            //m.GetComponent<LinggoMssile>().Shot(target);
        }
        isAttacking = false;
    }

    #region �̵� ���� �Լ�
    public void Move()
	{
		linggoAni.SetFloat("moveYvalue", moveSpeed);
		this.transform.position += (new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime);
	}

	
	#endregion

	#region ��ų ���� �Լ�
	#endregion

	#region HP ���� �Լ�
	public void SetCurrentHp(float changeValue)
	{
        currentHp = changeValue;
		hpBar.fillAmount = currentHp / maxHp;

	}

    public void SetMaxHp(float changeValue)
	{
        maxHp += changeValue;
		hpBar.fillAmount = currentHp / maxHp;
	}
	#endregion


	#region ������ �Լ�
	public void LevelUp()
	{
        int upHp = (int)(maxHp * 0.2f);
        SetMaxHp(upHp);
        SetCurrentHp(upHp);

    }
	#endregion


}
