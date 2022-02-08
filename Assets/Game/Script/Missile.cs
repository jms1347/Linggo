using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public bool isPlayer;
	private IEnumerator shotCour;
    private Vector2 targetDir;
	private Rigidbody2D rigid;
    public GameObject master;
    public GameObject missileStartPos;
    public GameObject exPrefab;
    public float missileSpeed;
	private void Awake()
	{
		rigid = this.GetComponent<Rigidbody2D>();

        if(isPlayer)
            missileSpeed = GameController.Inst.missileSpeed;
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
			rigid.velocity = targetDir * missileSpeed;
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
        if (isPlayer)
            this.transform.position = master.transform.position;
        else this.transform.position = missileStartPos.transform.position;
        Vector2 dir;
        if (isPlayer)
            dir = target.transform.position - master.transform.position;
        else dir = target.transform.position - missileStartPos.transform.position;
        dir.Normalize();
        targetDir = dir;
        this.gameObject.SetActive(true);
	}


	private void OnTriggerEnter2D(Collider2D coll)
	{
        if (isPlayer)
        {
            if (coll.tag == "Enemy")
            {
                Instantiate(exPrefab, coll.transform.position, Quaternion.identity);
                coll.GetComponent<Monster>().DecreaseHP(GameController.Inst.att);

                this.gameObject.SetActive(false);
            }
        }
        else
        {
            if (coll.tag == "Player")
            {
                if(exPrefab != null)
                    Instantiate(exPrefab, coll.transform.position, Quaternion.identity);
                GameController.Inst.DecreaseHP(master.GetComponent<Monster>().att);

                this.gameObject.SetActive(false);
            }

        }


        if(coll.tag == "Wall")
        {
            this.gameObject.SetActive(false);
        }
    }
}
