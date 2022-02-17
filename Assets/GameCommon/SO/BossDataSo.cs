using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossData
{
    public int bossIndex;
    public string bossName;
    public int bossAtt;
    public int bossHp;
    public float bossMoveSpeed;
    public float bossAttackDistance;
    public float bossAttSpeed;
    public float bossMissileSpeed;
    public int attType;
    public int bossContinuousMissileCnt;
    public float bossContinuousMissileTime;
}
[CreateAssetMenu(fileName = "BossDataSO", menuName = "ScriptableObject/BossDataSO")]
public class BossDataSo : ScriptableObject
{
    public List<BossData> bossDatas = new List<BossData>();
}
