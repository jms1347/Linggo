using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public int skillLevel = 1;

    public GameObject hitEffect;

    public float effectLifeTime;

    [Header("»ç¿îµå")]
    public AudioClip[] effectSound;

    [System.Obsolete]
    public void OffTimeCount()
    {
        ParticleSystem p = this.transform.GetChild(0).GetComponent<ParticleSystem>();
        effectLifeTime = p.duration;
        Invoke(nameof(OffSkill), p.duration);
    }

    public void OffSkill()
    {
        this.gameObject.SetActive(false);
    }
}
