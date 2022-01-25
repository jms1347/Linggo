using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetManager : MonoBehaviour
{
    const string monsterCardDbURL = "https://docs.google.com/spreadsheets/d/1ENe27vwzfxg0sBiW0qL-V6JQHZHutXhlHEYyHq0eRB8/export?format=tsv&gid=0&range=A2:M";
    const string itemDbURL = "https://docs.google.com/spreadsheets/d/1ENe27vwzfxg0sBiW0qL-V6JQHZHutXhlHEYyHq0eRB8/export?format=tsv&gid=1165571065&range=A2:I";
    const string skillCardDbURL = "https://docs.google.com/spreadsheets/d/1ENe27vwzfxg0sBiW0qL-V6JQHZHutXhlHEYyHq0eRB8/export?format=tsv&gid=626817391&range=A2:Q";
    const string linggoDbURL = "https://docs.google.com/spreadsheets/d/1ENe27vwzfxg0sBiW0qL-V6JQHZHutXhlHEYyHq0eRB8/export?format=tsv&gid=457742873&range=A2:J";
    public MonsterAppearanceLevelDataSo monsterAppearanceLevelDataSO;
    public SkillCardSo skillCardSO;
    public LevelDataSo linggoLevelDataSO;
    public ItemSo itemSO;
    void Awake()
    {
        StartCoroutine(SettingMonserCardData());
        StartCoroutine(SettingSkillCardData());
        StartCoroutine(SettingItemData());
        StartCoroutine(SettingLinggoLevelData());

    }
    #region 링고 레벨 데이터 넣기
    IEnumerator SettingLinggoLevelData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(linggoDbURL))
        {
            yield return www.SendWebRequest();
            string data = www.downloadHandler.text;

            SetLinggoLevelData(data);
        }
    }

    void SetLinggoLevelData(string data)
    {
        if (linggoLevelDataSO.levelData != null || linggoLevelDataSO.levelData.Count > 0) linggoLevelDataSO.levelData.Clear();

        int lineSize;
        string[] line = data.Split('\n');
        lineSize = line.Length;
        for (int i = 0; i < lineSize; i++)
        {
            LevelData linggoLevelData = new LevelData();
            string[] row = line[i].Split('\t');

            linggoLevelData.level = int.Parse(row[0]);
            linggoLevelData.upHp = int.Parse(row[1]);
            linggoLevelData.upAtt = int.Parse(row[2]);
            linggoLevelData.attSpeed = float.Parse(row[3]);
            linggoLevelData.upKillExp = int.Parse(row[4]);
            linggoLevelData.accumulateKillExp = int.Parse(row[5]);
            linggoLevelData.waveCreateEnemyCnt = int.Parse(row[6]);
            linggoLevelData.waveGoalEnmyCnt = int.Parse(row[7]);
            linggoLevelData.waveTime = float.Parse(row[8]);
            linggoLevelData.killRewardGold = int.Parse(row[9]);

            linggoLevelDataSO.levelData.Add(linggoLevelData);

        }
    }
    #endregion
    #region 스킬카드 데이터 넣기
    IEnumerator SettingSkillCardData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(skillCardDbURL))
        {
            yield return www.SendWebRequest();
            string data = www.downloadHandler.text;

            SetSkillCardData(data);
        }
    }

    void SetSkillCardData(string data)
    {
        if (skillCardSO.skillCards != null || skillCardSO.skillCards.Count > 0) skillCardSO.skillCards.Clear();

        int lineSize;
        string[] line = data.Split('\n');
        lineSize = line.Length;
        for (int i = 0; i < lineSize; i++)
        {
            SkillCard skillCard = new SkillCard();
            string[] row = line[i].Split('\t');

            skillCard.cardIndex = int.Parse(row[0]);
            skillCard.cardName = row[1];
            skillCard.cardExp = row[2];
            skillCard.cardEffectExp = row[3];
            skillCard.cardLvlUpEffectExp = row[4];
            skillCard.cardGrade = (SkillCard.Grade)int.Parse(row[5]);
            skillCard.cardAppearPercent = float.Parse(row[6]);

            skillCard.cardCoolTime = float.Parse(row[7]);
            skillCard.cardCoolTimeCoefficient = float.Parse(row[8]);
            skillCard.cardDecreaseCoolTime = float.Parse(row[9]);

            skillCard.isUse = int.Parse(row[10]) == 1 ? true :false ;
            if(skillCard.isUse)
                skillCardSO.skillCards.Add(skillCard);
        }
    }
    #endregion
    #region 아이템 데이터 넣기
    IEnumerator SettingItemData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(itemDbURL))
        {
            yield return www.SendWebRequest();
            string data = www.downloadHandler.text;

            SetItemData(data);
        }
    }

    void SetItemData(string data)
    {
        if (itemSO.items != null || itemSO.items.Count > 0) itemSO.items.Clear();

        int lineSize;
        string[] line = data.Split('\n');
        lineSize = line.Length;
        for (int i = 0; i < lineSize; i++)
        {
            ItemCard item = new ItemCard();
            string[] row = line[i].Split('\t');
            item.itemIndex = int.Parse(row[0]);
            item.itemType = (ItemCard.ItemType)int.Parse(row[1]);
            item.itemName = row[2];
            item.itemExp = row[3];
            item.itemPrice = int.Parse(row[4]);
            item.itemValue = float.Parse(row[5]);
            item.itemDuration = float.Parse(row[6]);
            item.itemCoolTime = float.Parse(row[7]);

            itemSO.items.Add(item);
        }
    }
    #endregion

    
    #region 몬스터 카드 데이터 넣기
    IEnumerator SettingMonserCardData()
	{
        using (UnityWebRequest www = UnityWebRequest.Get(monsterCardDbURL))
        {
            yield return www.SendWebRequest();
            string data = www.downloadHandler.text;

            SetMonsterCardData(data);
        }
    }    

    void SetMonsterCardData(string data)
	{
        if (monsterAppearanceLevelDataSO.monsterAppearanceLevelData != null || monsterAppearanceLevelDataSO.monsterAppearanceLevelData.Count > 0) monsterAppearanceLevelDataSO.monsterAppearanceLevelData.Clear();

        int lineSize;
        string[] line = data.Split('\n');
        lineSize = line.Length;
        for (int i = 0; i < lineSize; i++)
        {
            MonsterAppearanceLevelData monsterAppearanceLevelData = new MonsterAppearanceLevelData();
            string[] row = line[i].Split('\t');

            monsterAppearanceLevelData.wave = int.Parse(row[0]);
            monsterAppearanceLevelData.monsterHp = int.Parse(row[1]);
            monsterAppearanceLevelData.monsterAtt = int.Parse(row[2]);
            monsterAppearanceLevelData.nofe1 = int.Parse(row[3]);
            monsterAppearanceLevelData.nofe2 = int.Parse(row[4]);
            monsterAppearanceLevelData.nofe3 = int.Parse(row[5]);
            monsterAppearanceLevelData.nofe4 = int.Parse(row[6]);
            monsterAppearanceLevelData.nofe5 = int.Parse(row[7]);
            monsterAppearanceLevelData.nofe6 = int.Parse(row[8]);
            monsterAppearanceLevelData.nofe7 = int.Parse(row[9]);
            monsterAppearanceLevelData.nofe8 = int.Parse(row[10]);
            monsterAppearanceLevelData.nofe9 = int.Parse(row[11]);
            monsterAppearanceLevelData.nofe10 = int.Parse(row[12]);

            monsterAppearanceLevelDataSO.monsterAppearanceLevelData.Add(monsterAppearanceLevelData);
        }
    }
	#endregion
}
