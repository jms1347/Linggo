using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardController : MonoBehaviour
{
    public static ItemCardController Inst { get; private set; }
    private void Awake() => Inst = this;

    [SerializeField] ItemSo itemCardSO;
    public List<ItemCard> itemCardList = new List<ItemCard>();  //DB 아이템 리스트

	public ItemSlot[] itemSlots = new ItemSlot[4];          //우측 하단 아이템 슬롯
    public List<ItemIcon> itemIcons = new List<ItemIcon>(); //좌측 상단 버프 아이콘
	public List<Sprite> itemSprs = new List<Sprite>();      //아이템 아이콘(이미지)

    public List<GhostItem> ghostItems = new List<GhostItem>();      //유닛 20마리 미리 생성
    public List<Transform> ghostItemPos = new List<Transform>();    //유닛 생성 포스 20개
    public List<GameObject> purificationEffects = new List<GameObject>();   //정화 이펙트
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
        
        //링고에 버프 이미지 생성(사용시)
        GameController.Inst.linggo.ItemEffect(itemSprs[itemSlots[slotIndex].index]);

        switch (itemSlots[slotIndex].itemCard.itemType)
        {
            case ItemCard.ItemType.potion :
                GameController.Inst.IncreaseHP(itemSlots[slotIndex].itemCard.itemValue);
                break;
            case ItemCard.ItemType.percentPotion:
                int plusPotion = Mathf.RoundToInt(GameController.Inst.maxHP * itemSlots[slotIndex].itemCard.itemValue * 0.01f);
                GameController.Inst.IncreaseHP(plusPotion);
                break;
            case ItemCard.ItemType.buff:
                if(itemSlots[slotIndex].itemCard.itemIndex == 3)
                {
                    GameController.Inst.linggo.ShieldEffect(itemSlots[slotIndex].itemCard.itemValue);
                }
                else if (itemSlots[slotIndex].itemCard.itemIndex == 1)
                {
                    for (int i = 0; i < itemSlots[slotIndex].itemCard.itemValue; i++)
                    {
                        GameObject mon = GameController.Inst.linggo.FindNearestObjectByTag("Enemy");
                        if (mon != null && !mon.name.Contains("Boss"))
                        {
                            mon.GetComponent<Monster>().Betrayal();
                            for (int j = 0; j < GameController.Inst.fieldMonsters.Count; j++)
                            {
                                if (GameController.Inst.fieldMonsters[j].gameObject.Equals(mon.gameObject))
                                {
                                    GameController.Inst.fieldMonsters.RemoveAt(j);
                                    break;
                                }
                            }
                            //이펙트
                            for (int j = 0; j < purificationEffects.Count; j++)
                            {
                                if (!purificationEffects[j].activeSelf)
                                {
                                    purificationEffects[j].transform.position = mon.transform.position + (Vector3.up * 0.5f);
                                    purificationEffects[j].SetActive(true);
                                    break;
                                }
                            }
                        }

                    }
                }
                //else if(itemSlots[slotIndex].itemCard.itemIndex == 6)
                //{
                //    int plusAtt = Mathf.RoundToInt(GameController.Inst.att * itemSlots[slotIndex].itemCard.itemValue * 0.01f);
                //    GameController.Inst.ChangeAtt(plusAtt, 60);
                //}else if(itemSlots[slotIndex].itemCard.itemIndex == 7)
                //{
                //    GameController.Inst.linggo.ResistanceEffect(itemSlots[slotIndex].itemCard.itemValue);
                //}
                //else if(itemSlots[slotIndex].itemCard.itemIndex == 9)
                //{
                //    int potion = Mathf.RoundToInt(GameController.Inst.maxHP * itemSlots[slotIndex].itemCard.itemValue * 0.01f);
                //    GameController.Inst.ChangeAtt(GameController.Inst.att, 15);
                //    GameController.Inst.IncreaseHP(potion);
                //}
                break;
            case ItemCard.ItemType.summon:
                for (int i = 0; i < ghostItems.Count; i++)
                {
                    if (!ghostItems[i].gameObject.activeSelf)
                    {
                        int hp = Mathf.RoundToInt(GameController.Inst.currentHP * (itemSlots[slotIndex].itemCard.itemValue * 0.01f));
                        ghostItems[i].InitUnit(hp);
                        ghostItems[i].transform.position = ghostItemPos[i].transform.position;
                        ghostItems[i].transform.localRotation = Quaternion.Euler(0, 0, 0);
                        break;
                    }
                }
                break;
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

        //세팅
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].SettingItemSlot(itemCardList[i]);
        }
    }
    #endregion

    
}
