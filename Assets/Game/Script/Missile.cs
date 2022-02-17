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
    public bool attackItem = true;

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
                GameObject ex = Instantiate(exPrefab);
                ex.transform.position = coll.transform.position;

                coll.GetComponent<Monster>().DecreaseHP(GameController.Inst.att);

                this.gameObject.SetActive(false);
            }
        }
        else
        {
            if (coll.tag == "Player")
            {
                if (coll.name.Contains("Linggo"))
                {

                    GameController.Inst.DecreaseHP(master.GetComponent<Monster>().att);
                }
                else if (coll.name.Contains("Item") && attackItem)
                {
                    coll.gameObject.GetComponent<GhostItem>().DecreaseHP(master.GetComponent<Monster>().att);

                }
                else
                {
                    coll.gameObject.GetComponent<Monster>().DecreaseHP(master.GetComponent<Monster>().att);
                }

                if (exPrefab != null)
                {
                    GameObject ex = Instantiate(exPrefab);
                    ex.transform.position = coll.transform.position;
                }
                this.gameObject.SetActive(false);
            }

        }


        if(coll.tag == "Wall")
        {
            this.gameObject.SetActive(false);
        }
    }
}
