using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSmogBuff : MonoBehaviour
{
    private IEnumerator buffCoolCour;
    private IEnumerator buffEfectCour;
    public GameObject buffObj;
    public float buffDurationTime;
    public float buffCoolTime;
    [Header("»ç¿îµå")]
    public AudioClip skillCastingSound;
    private void OnEnable()
    {
        BuffEffect();

    }

    private void OnDisable()
    {
        if (buffObj.activeSelf)
        {
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

        if (skillCastingSound != null)
            SoundManager.Inst.SFXPlay("bossSmogSkill", skillCastingSound);
        buffObj.SetActive(true);
        buffObj.transform.localPosition = Vector2.zero;

        for (int i = 0; i < buffDurationTime * 10; i++) yield return t;

        buffObj.SetActive(false);

    }
}
