using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitCollBox : MonoBehaviour
{
	[System.Obsolete]
	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.tag == "Enemy")
		{
			coll.GetComponent<Monster>().DecreaseHP(10);
		}
	}

	[System.Obsolete]
	private void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.tag == "Enemy")
		{
		}
	}
}
