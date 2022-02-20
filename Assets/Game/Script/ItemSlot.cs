using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class ItemSlot : MonoBehaviour
{
    public int index;
    public Image itemIconImg;
	public TextMeshProUGUI itemPriceText;
	public ItemCard itemCard;
	public Image coolTimeImg;
	public bool isCooldown = false;
	IEnumerator ItemCour;

    public GameObject itemGuideBar;
    public TextMeshProUGUI guideTitle;
    public TextMeshProUGUI guideContent;
    bool isGuide = true;
	private void Awake()
	{
		itemIconImg = this.transform.GetChild(0).GetComponent<Image>();
		coolTimeImg = this.transform.GetChild(1).GetComponent<Image>();
        itemPriceText = this.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        isGuide = true;

        InitItemSlot();
	}

    #region 슬롯 쿨타임 초기화
    public void InitItemSlot()
	{
		coolTimeImg.gameObject.SetActive(false);
		coolTimeImg.fillAmount = 1;
	}
	#endregion

	#region 아이템 사용
	public void SelectItem()
	{
        if (isGuide)
        {
            isGuide = false;
            guideTitle.text = itemCard.itemName;
            guideContent.text = itemCard.itemExp;
            itemGuideBar.SetActive(true);
        }
		if (itemCard != null && !isCooldown)
        {
            if (GameController.Inst.gold < itemCard.itemPrice)
            {
                guideTitle.text = "";

                guideContent.text = "골드가 부족합니다";
                itemGuideBar.SetActive(true);
                return;
            }

            GameController.Inst.DecreaseGold(itemCard.itemPrice);
            ItemCardController.Inst.UseItemCard(index);
		}
	}    

    public void OpenItem()
	{
		if (!isCooldown)
		{
			isCooldown = true;
			coolTimeImg.gameObject.SetActive(true);
            this.GetComponent<Button>().interactable = false;

            if (ItemCour != null)
				StopCoroutine(ItemCour);

			ItemCour = OpenItemCour();
			StartCoroutine(ItemCour);
		}

	}
	IEnumerator OpenItemCour(float time = 1)
	{
		coolTimeImg.fillAmount = time;

        while (isCooldown)
		{
			coolTimeImg.fillAmount -= 1 / itemCard.itemCoolTime * Time.deltaTime;
			if (coolTimeImg.fillAmount <= 0)
			{
				coolTimeImg.fillAmount = 0;
				isCooldown = false;
				coolTimeImg.gameObject.SetActive(false);
                this.GetComponent<Button>().interactable = true;

            }
            yield return null;
		}
	}
    #endregion

    #region 아이템 세팅
    public void SettingItemSlot(ItemCard ic)
    {
        itemCard = ic;
        itemPriceText.text = itemCard.itemPrice.ToString();
    }

	#endregion    
}
