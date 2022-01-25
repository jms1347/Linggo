using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
	public float att;
    public Sprite missileSpr;
	private IEnumerator shotCour;
	private Vector2 targetDir;
	private Rigidbody2D rigid;

	public float moveSpeed; 
	public float lifeTime = 5f;

	private void Awake()
	{
		rigid = this.GetComponent<Rigidbody2D>();
		if (missileSpr != null)
			this.GetComponent<SpriteRenderer>().sprite = missileSpr;
	}

	private void Update()
	{
		Shot();
	}

	public virtual void Shot()
	{
		if (targetDir != null && this.enabled)
		{
			rigid.velocity = targetDir * moveSpeed;
		}
	}
	private void OnEnable()
	{
		Invoke(nameof(DisableMissile), lifeTime);
	}
	void DisableMissile() => this.gameObject.SetActive(false);
	private void OnDisable()
	{
	
		CancelInvoke();
	}
	public void SettingTarget(Vector2 dir)
	{
		targetDir = dir;
	}


	private void OnTriggerEnter2D(Collider2D coll)
	{
        if (coll.tag == "Enemy")
        {
			coll.GetComponent<Monster>().DecreaseHP(GameController.Inst.att);

            this.gameObject.SetActive(false);
        }
    }
}
