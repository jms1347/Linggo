using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class StarFishHitCollBox : MonoBehaviour
{
    public StarFish startFish;
    IEnumerator skillEffectCour;

    [System.Obsolete]
    public void SkillEffect(Vector3 goalPos,int index)
    {
        if (skillEffectCour != null)
            StopCoroutine(skillEffectCour);
        skillEffectCour = SkillEffect2(goalPos, index);
        StartCoroutine(skillEffectCour);
    }

    [System.Obsolete]
    IEnumerator SkillEffect2(Vector3 goalPos, int index)
    {
        var time = new WaitForSeconds(0.1f);
        if (startFish.effectSound.Length > 0)
            SoundManager.Inst.SFXPlay("StarFishDrop", startFish.effectSound[0]);
        this.transform.DOMove(goalPos, startFish.levelUpData[startFish.skillLevel - 1].fallIntervalTime)
                .SetEase(Ease.InQuad).OnComplete(() =>
                {
                    if (startFish.effectSound.Length > 0)
                        SoundManager.Inst.SFXPlay("StarFishHit", startFish.effectSound[1]);
                    this.GetComponent<BoxCollider2D>().enabled = true;
                    GameObject exEffect = Instantiate(startFish.hitEffect);
                    exEffect.transform.position = this.transform.position;
                });

        for (int i = 0; i <startFish.levelUpData[startFish.skillLevel - 1].fallIntervalTime * 11; i++) yield return time;
        this.gameObject.SetActive(false);

    }

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            coll.gameObject.GetComponent<Monster>().StunEffect(startFish.levelUpData[startFish.skillLevel - 1].stunTime);
            int damage = (int)(GameController.Inst.att * startFish.levelUpData[startFish.skillLevel - 1].attackCoefficient);
            coll.gameObject.GetComponent<Monster>().DecreaseHP(damage);
        }
    }
}
