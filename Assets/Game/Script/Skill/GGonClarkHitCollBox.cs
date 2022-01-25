using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGonClarkHitCollBox : MonoBehaviour
{
	public GGonClark ggonclark;
	[System.Obsolete]
	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.tag == "Enemy")
		{
			coll.gameObject.GetComponent<Monster>().StunEffect(ggonclark.levelUpData[ggonclark.skillLevel - 1].stunTime);
			int damage = (int)(GameController.Inst.att * ggonclark.levelUpData[ggonclark.skillLevel - 1].attackCoefficient);
			coll.gameObject.GetComponent<Monster>().DecreaseHP(damage);
		}
	}
}
