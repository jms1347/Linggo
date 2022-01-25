using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int level;
    public int upHp;
    public int upAtt;
    public float attSpeed;
    public int upKillExp;
    public int accumulateKillExp;
    public int waveCreateEnemyCnt;
    public int waveGoalEnmyCnt;
    public float waveTime;
    public int killRewardGold;
}

[CreateAssetMenu(fileName = "LevelDataSo", menuName = "ScriptableObject/LevelDataSo")]
public class LevelDataSo : ScriptableObject
{
    public List<LevelData> levelData = new List<LevelData>();
}
