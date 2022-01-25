using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterAppearanceLevelData
{
    public int wave;
    public int monsterHp;
    public int monsterAtt;
    public int nofe1;
    public int nofe2;
    public int nofe3;
    public int nofe4;
    public int nofe5;
    public int nofe6;
    public int nofe7;
    public int nofe8;
    public int nofe9;
    public int nofe10;
}

[CreateAssetMenu(fileName = "MonsterAppearanceLevelDataSO", menuName = "ScriptableObject/MonsterAppearanceLevelDataSO")]
public class MonsterAppearanceLevelDataSo : ScriptableObject
{
	public List<MonsterAppearanceLevelData> monsterAppearanceLevelData = new List<MonsterAppearanceLevelData>();
}
