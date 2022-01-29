using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
	private IEnumerator shotCour;
    private Vector2 targetDir;
	private Rigidbody2D rigid;

    public GameObject exPrefab;
	private void Awake()
	{
		rigid = this.GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
        if(this.gameObject.activeSelf)
		    Shot();
	}

	public virtual void Shot()
	{
		if (targetDir != null)
		{
			rigid.velocity = targetDir * GameController.Inst.missileSpeed;
		}
	}
	private void OnEnable()
	{
		Invoke(nameof(DisableMissile), 10.0f);
	}
	void DisableMissile() => this.gameObject.SetActive(false);
	private void OnDisable()
	{	
		CancelInvoke();
	}
	public void SettingTarget(GameObject target)
	{
        this.transform.position = GameController.Inst.linggo.transform.position;
        Vector2 dir = target.transform.position - GameController.Inst.linggo.transform.position;
        dir.Normalize();
        targetDir = dir;
        this.gameObject.SetActive(true);
	}


	private void OnTriggerEnter2D(Collider2D coll)
	{
        if (coll.tag == "Enemy")
        {
            Instantiate(exPrefab, coll.transform.position, Quaternion.identity);
			coll.GetComponent<Monster>().DecreaseHP(GameController.Inst.att);

            this.gameObject.SetActive(false);
        }

        if(coll.tag == "Wall")
        {
            this.gameObject.SetActive(false);
        }
    }
}
