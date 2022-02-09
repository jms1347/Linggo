using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStunMissile : MonoBehaviour
{
    public float stunTime;
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            GameController.Inst.linggo.LightningStunEffect(stunTime);

        }
    }
}
