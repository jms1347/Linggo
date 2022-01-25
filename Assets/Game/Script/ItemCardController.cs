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
	#region ������ ���
	public void UseItemCard(int slotIndex)
	{
		itemSlots[slotIndex].OpenItem(); //��ų���� ��Ÿ��

		//�ߺ��Ǵ� ������ �ִ��� üũ �ϱ� ���°� ã�ƾ��ҵ�?
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

		//�ߺ��Ǵ°� ������ ���ο� ������ ��������
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
	#region ������ ����Ʈ ����
	public void SettingItemCardList()
	{
		for (int i = 0; i < itemCardSO.items.Count; i++)
		{
			ItemCard item = itemCardSO.items[i];
			itemCardList.Add(item);
		}
		
		//�ӽü���
		//     for (int i = 0; i < itemSlots.Length; i++)
		//     {
		//itemSlots[i].SettingItemSlot(itemSprs[i], itemCardList[i]);
		//     }
	}
    #endregion

    
}
