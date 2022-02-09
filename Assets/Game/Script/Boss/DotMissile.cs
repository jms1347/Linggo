using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotMissile : MonoBehaviour
{
    public int dotDamageTime;
    public int masterAtt;
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            GameController.Inst.linggo.FireDotEffect(dotDamageTime, masterAtt);

        }
    }
}
