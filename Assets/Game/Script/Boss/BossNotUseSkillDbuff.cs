using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossNotUseSkillDbuff : MonoBehaviour
{
    private IEnumerator buffCoolCour;
    private IEnumerator buffEfectCour;

    public SkillSlot[] skillSlot;
    public GameObject[] buffObj;
    public float buffDurationTime;
    public float buffCoolTime;
    private void OnEnable()
    {
        BuffEffect();

    }

    private void OnDisable()
    {
        for (int i = 0; i < skillSlot.Length; i++)
        {
            skillSlot[i].GetComponent<Button>().interactable = true;
            buffObj[i].SetActive(false);

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
        
        for (int i = 0; i < skillSlot.Length; i++)
        {
            if (!skillSlot[i].isNull)
            {
                skillSlot[i].GetComponent<Button>().interactable = false;
                buffObj[i].SetActive(true);
                buffObj[i].transform.position = skillSlot[i].transform.position;
            }
        }
        for (int i = 0; i < buffDurationTime * 10; i++) yield return t;
        for (int i = 0; i < skillSlot.Length; i++)
        {
            skillSlot[i].GetComponent<Button>().interactable = true;
            buffObj[i].SetActive(false);

        }
    }
}
