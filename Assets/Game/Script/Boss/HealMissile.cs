using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMissile : MonoBehaviour
{
    public Monster master;
    public int healPercent;
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            int heal = Mathf.RoundToInt(GameController.Inst.att * healPercent * 0.01f);
            master.IncreaseHP(heal);
        }
    }
}
