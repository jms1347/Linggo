using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurePoisonDeerHitCollBox : MonoBehaviour
{
    public PurePoisonDeer purePoisonDeer;
    float time = 0.5f;

    private void Start()
    {
        time = 0.5f;
    }
    [System.Obsolete]
    private void OnTriggerStay2D(Collider2D coll)
    {
        
        if (coll.tag == "Enemy")
        {
            if(time <= 0)
            {
                //print("µô");
                int damage = (int)(GameController.Inst.att * purePoisonDeer.levelUpData[purePoisonDeer.skillLevel - 1].attackCoefficient);
                coll.gameObject.GetComponent<Monster>().DecreaseHP(damage);
                time = 0.5f;
            }
            else
            {
                time -= Time.deltaTime;
            }
        }
    }
}
