using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterCard
{
	public enum Tag
	{
		foxSheep = 0,
		friend = 1,
		enemy=2
	}
	public enum AttackType
	{
		close = 0,
		tank = 1,
		far = 2
	}
	public string monsterCardCode;
	public string monsterCardName;
	public Tag monsterCardTag;
	public AttackType monsterCardAttackType;
	public int monsterCardCost;
	public int monsterCardHp;
	public int monsterCardAtt;
	public float monsterCardAttRange;
	public float monsterCardSpeed;
	public string monsterCardExp;
	public string SprURL;
	public string addProperties;

}

[CreateAssetMenu(fileName ="MonsterCardSO", menuName = "ScriptableObject/MonsterCardSO")]
public class MonsterCardSo : ScriptableObject
{
	public List<MonsterCard> monsterCards = new List<MonsterCard>();
}
