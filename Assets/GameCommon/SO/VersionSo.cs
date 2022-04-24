using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VersionDB
{
    public int majorNum;
    public int minorNum;
    public int patchNum;
}

[CreateAssetMenu(fileName = "VersionSO", menuName = "ScriptableObject/VersionSO")]
public class VersionSo : ScriptableObject
{
    public VersionDB versionData;
}
