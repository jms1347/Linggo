using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardController : MonoBehaviour
{
    public static ItemCardController Inst { get; private set; }
    private void Awake() => Inst = this;

    [SerializeField] ItemSo itemCardSO;
    public List<ItemCard> itemCardList = new List<ItemCard>();

	public ItemSlot[] itemSlots = new ItemSlot[4];
    public List<ItemIcon> itemIcons = new List<ItemIcon>();
	public List<Sprite> itemSprs = new List<Sprite>();

    void Start()
    {
		SettingItemCardList();

	}
	#region 아이템 사용
	public void UseItemCard(int slotIndex)
	{
		itemSlots[slotIndex].OpenItem(); //스킬슬롯 쿨타임

		//중복되는 버프가 있는지 체크 하구 없는거 찾아야할듯?
		bool isReduplication = false;

		for (int i = 0; i < itemIcons.Count; i++)
		{
			if (itemIcons[i].item == itemSlots[slotIndex].itemCard) 
			{
				isReduplication = true;
				itemIcons[i].ReduplicateUseBuff();
				break;
			}
		}

		//중복되는게 없으면 새로운 공간에 버프생성
        if (!isReduplication)
        {
			for (int i = 0; i < itemIcons.Count; i++)
			{
				if (!itemIcons[i].isCooldown)
				{
					itemIcons[i].UseBuff(itemSlots[slotIndex].itemCard, itemSprs[itemSlots[slotIndex].itemCard.itemIndex]);
					break;
				}
			}
		}		
	}
	#endregion
	#region 아이템 리스트 세팅
	public void SettingItemCardList()
	{
		for (int i = 0; i < itemCardSO.items.Count; i++)
		{
			ItemCard item = itemCardSO.items[i];
			itemCardList.Add(item);
		}
		
		//임시세팅
		//     for (int i = 0; i < itemSlots.Length; i++)
		//     {
		//itemSlots[i].SettingItemSlot(itemSprs[i], itemCardList[i]);
		//     }
	}
    #endregion

    
}
