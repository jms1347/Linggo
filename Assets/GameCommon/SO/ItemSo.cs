using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemCard
{
    public enum ItemType
    {
        potion = 0,
        buff =1,
        summon = 2
    }
    public int itemIndex;
    public ItemType itemType;
    public string itemName;
    [TextArea]
    public string itemExp;
    public int itemPrice;
    public float itemValue;
    public float itemDuration;
    public float itemCoolTime;
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObject/ItemSO")]
public class ItemSo : ScriptableObject
{
    public List<ItemCard> items = new List<ItemCard>();
}