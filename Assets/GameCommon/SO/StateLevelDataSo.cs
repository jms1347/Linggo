using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateLevelData
{
    public int plusLevel;
    public int plusAtt;
    public int plusAttGold;
    public int plusHp;
    public int plusHpGold;
    public float plusAppearPercent;
    public int plusAppearPercentGold;
}

[CreateAssetMenu(fileName = "StateLevelDataSo", menuName = "ScriptableObject/StateLevelDataSo")]
public class StateLevelDataSo : ScriptableObject
{
    public List<StateLevelData> stateLevelData = new List<StateLevelData>();
}