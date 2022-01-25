using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDongHitCollBox : MonoBehaviour
{
    public DDonggoos ddonggoos;
    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            coll.gameObject.GetComponent<Monster>().SlowEffect(ddonggoos.levelUpData[ddonggoos.skillLevel - 1].slowTime, ddonggoos.levelUpData[ddonggoos.skillLevel - 1].slowPercent);
            int damage = (int)(GameController.Inst.att * ddonggoos.levelUpData[ddonggoos.skillLevel - 1].attackCoefficient);
            coll.gameObject.GetComponent<Monster>().DecreaseHP(damage);
            this.gameObject.SetActive(false);
            Instantiate(ddonggoos.hitEffect, this.transform.position, Quaternion.identity);
        }
    }
}
