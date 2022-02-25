using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FarmingCardLevelExpData
{
    public int farmingCardIndex;
    public int farmingCardLevel;
    public string farmingCardLevelExpStr;
}


[CreateAssetMenu(fileName = "FarmingCardLevelExpDataSO", menuName = "ScriptableObject/FarmingCardLevelExpDataSO")]
public class FarmingCardLevelExpDataSo : ScriptableObject
{
    public List<FarmingCardLevelExpData> farmingCardLevelExpData = new List<FarmingCardLevelExpData>();
}
