using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGabibon : MonoBehaviour
{
    [System.Serializable]
    public class LevelUpData
    {
        public int level;
        public float attackCoefficient;
        public float xRangeAdd;
        public float yRangeAdd;
        public float nukbackX;
    }
    public LevelUpData[] levelUpData = new LevelUpData[10];

    IEnumerator skillEffectCour;
    public List<GameObject> colls = new List<GameObject>();

    [System.Obsolete]
    private void OnEnable()
    {
        //Invoke(nameof(OffSkill), 1.25f);
        if (skillEffectCour != null)
            StopCoroutine(skillEffectCour);
        skillEffectCour = SkillEffect();
        StartCoroutine(skillEffectCour);
    }

    [System.Obsolete]
    IEnumerator SkillEffect()
    {
        var time = new WaitForSeconds(0.1f);

        for (int i = 0; i < 10; i++) yield return time;
        colls.Clear();
    }
}
