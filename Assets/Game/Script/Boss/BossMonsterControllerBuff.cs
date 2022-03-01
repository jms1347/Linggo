using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BossMonsterControllerBuff : MonoBehaviour
{
    private IEnumerator buffCoolCour;
    private IEnumerator buffEfectCour;

    
    public GameObject[] buffObj;
    public float buffDurationTime;
    public float buffCoolTime;
    public List<GameObject> selectMonsters = new List<GameObject>();
    private void OnEnable()
    {
        BuffEffect();

    }
    private void OnDisable()
    {
        if (selectMonsters.Count > 0)
        {
            for (int i = 0; i < buffObj.Length; i++)
            {
                if (selectMonsters[i] != null)
                {
                    buffObj[i].transform.DOKill();
                    selectMonsters[i].transform.DOKill();
                }
                else
                {
                    buffObj[i].SetActive(false);
                }
            }
            for (int i = 0; i < buffObj.Length; i++)
            {
                buffObj[i].SetActive(false);
            }
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

            yield return new WaitUntil(() => GameController.Inst.fieldMonsters.Count >= 3);
            if (buffEfectCour != null)
                StopCoroutine(buffEfectCour);
            buffEfectCour = BuffEffectCour();
            StartCoroutine(buffEfectCour);                             
        }

    }
    public IEnumerator BuffEffectCour()
    {
        var t = new WaitForSeconds(0.1f);
        int[] ran = GameController.Inst.GetRandomInt(GameController.Inst.fieldMonsters.Count, 0, GameController.Inst.fieldMonsters.Count);
        selectMonsters.Clear();
        for (int i = 0; i < buffObj.Length; i++)
        {
            if (GameController.Inst.fieldMonsters[ran[i]] != null)
            {
                selectMonsters.Add(GameController.Inst.fieldMonsters[ran[i]].gameObject);
                buffObj[i].transform.position = GameController.Inst.fieldMonsters[ran[i]].transform.position;
                buffObj[i].SetActive(true);
            }
        }

        for (int i = 0; i < selectMonsters.Count; i++)
        {
            selectMonsters[i].GetComponent<Monster>().moveSpeed = 0;
        }
        for (int i = 0; i < buffDurationTime * 10; i++) yield return t;

        for (int i = 0; i < buffObj.Length; i++)
        {
            if (selectMonsters[i] != null)
            {
                buffObj[i].transform.DOMove(GameController.Inst.linggo.transform.position, 1.0f).SetEase(Ease.Flash);
                selectMonsters[i].transform.DOMove(GameController.Inst.linggo.transform.position, 1.0f).SetEase(Ease.Flash);
            }
            else
            {
                buffObj[i].SetActive(false);
            }
        }
        for (int i = 0; i < 10; i++) yield return t;

        for (int i = 0; i < buffObj.Length; i++)
        {
            buffObj[i].SetActive(false);
        }
        selectMonsters.Clear();

    }
}
