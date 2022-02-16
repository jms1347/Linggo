using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemCard
{
    public enum ItemType
    {
        potion = 0,
        percentPotion = 1,
        buff =2,
        summon = 3
    }
    public int itemIndex;
    public ItemType itemType;
    public string itemName;
    [TextArea]
    public string itemExp;
    public int itemPrice;
    public int itemValue;
    public float itemDuration;
    public float itemCoolTime;
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObject/ItemSO")]
public class ItemSo : ScriptableObject
{
    public List<ItemCard> items = new List<ItemCard>();
}