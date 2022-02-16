using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveSpeedBuff : MonoBehaviour
{
    private IEnumerator buffCoolCour;
    private IEnumerator buffEfectCour;
    public GameObject buffObj;
    public float plusMoveSpeed;
    public float buffDurationTime;
    public float buffCoolTime;

    private void OnEnable()
    {
        BuffEffect();

    }

    private void OnDisable()
    {
        if (buffObj.activeSelf)
        {
            for (int i = 0; i < GameController.Inst.fieldMonsters.Count; i++)
            {
                GameController.Inst.fieldMonsters[i].GetComponent<Monster>().OffBuffMoveSpeed();
            }
            buffObj.SetActive(false);
        }

    }

    public void BuffEffect()
    {
        if (buffCoolCour != null)
            StopCoroutine(buffCoolCour);
        buffCoolCour = BuffCoolCour();
        StartCoroutine(buffCoolCour);
    }
    public IEnumerator BuffCoolCour()
    {
        var t = new WaitForSeconds(0.1f);
        while (this.gameObject.activeSelf)
        {
            for (int i = 0; i < buffCoolTime * 10; i++) yield return t;
            if (buffEfectCour != null)
                StopCoroutine(buffEfectCour);
            buffEfectCour = BuffEffectCour();
            StartCoroutine(buffEfectCour);
        }

    }
    public IEnumerator BuffEffectCour()
    {
        var t = new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<Boss>().monsterAni.SetTrigger("Attack");
        buffObj.SetActive(true);
        buffObj.transform.position = new Vector2(-11f, -0.5f);
        for (int i = 0; i < GameController.Inst.fieldMonsters.Count; i++)
        {
            GameController.Inst.fieldMonsters[i].GetComponent<Monster>().OnBuffMoveSpeed(plusMoveSpeed);
        }
        for (int i = 0; i < buffDurationTime * 10; i++) yield return t;
        for (int i = 0; i < GameController.Inst.fieldMonsters.Count; i++)
        {
            GameController.Inst.fieldMonsters[i].GetComponent<Monster>().OffBuffMoveSpeed();
        }
        buffObj.SetActive(false);

    }
}
