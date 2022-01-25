using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillCard
{
    public enum Grade
    {
        normal = 0,
        rare = 1,
        epic = 2
    }
    public int cardIndex;
    public string cardName;
    public string cardExp;
    public string cardEffectExp;
    public string cardLvlUpEffectExp;
    public Grade cardGrade; //10%Â÷ÀÌ
    public float cardAppearPercent;
    public float cardCoolTime;
    public float cardCoolTimeCoefficient;
    public float cardDecreaseCoolTime;
    public bool isUse;
}

[CreateAssetMenu(fileName = "SkillCardSO", menuName = "ScriptableObject/SkillCardSO")]
public class SkillCardSo : ScriptableObject
{
    public List<SkillCard> skillCards = new List<SkillCard>();
}
