using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class ItemSlot : MonoBehaviour, IDragHandler, IEndDragHandler
{
	public Image[] itemSlots;
	public bool isNull = true;
	public Image itemIconImg;
	public TextMeshProUGUI itemCntText;
	public int index;
	public int itemCnt;
	public ItemCard itemCard;
	public Image coolTimeImg;
	public bool isCooldown = false;
	IEnumerator ItemCour;

	public Sprite noneSpr;
	private void Awake()
	{
		itemIconImg = this.transform.GetChild(0).GetComponent<Image>();
		coolTimeImg = this.transform.GetChild(1).GetComponent<Image>();
		itemCntText = itemIconImg.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

		InitItemSlot();
	}

    #region 슬롯 초기화
    public void InitItemSlot()
	{
		this.GetComponent<Button>().interactable = false;
		isNull = true;
		itemCard = null;
		itemIconImg.sprite = null;
		itemIconImg.gameObject.SetActive(false);
		itemCnt = 0;
		itemCntText.text = itemCnt.ToString();
		coolTimeImg.gameObject.SetActive(false);
		coolTimeImg.fillAmount = 1;
	}
	#endregion

	#region 아이템 사용
	public void SelectItem()
	{
		if (itemCard != null && !isCooldown && itemCnt > 0)
        {
			ItemCardController.Inst.UseItemCard(index);
			itemCnt--;
			itemCntText.text = itemCnt.ToString();
			
		}

	}    

    public void OpenItem()
	{
		if (!isCooldown)
		{
			isCooldown = true;
			coolTimeImg.gameObject.SetActive(true);

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
			}
			yield return null;
		}
	}
    #endregion

    #region 아이템 세팅
    public void SettingItemSlot(Sprite cardImg, ItemCard ic,int cnt)
    {
        if (itemCard == ic) PlusCount();
        else
        {
            isNull = false;
            if (cardImg != null)
            {
                itemIconImg.sprite = cardImg;
                coolTimeImg.sprite = cardImg;
            }
            else
            {
				itemIconImg.sprite = noneSpr;
			}
			itemIconImg.gameObject.SetActive(true);
            itemCnt = cnt;
            itemCntText.text = itemCnt.ToString();
            itemCard = ic;
            this.GetComponent<Button>().interactable = true;
        }
    }

	//아이템 수량 증가
    public void PlusCount()
	{
		itemCnt++;
		itemCntText.text = itemCnt.ToString();
	}
	#endregion

	public void OnDrag(PointerEventData eventData)
    {
        if (itemCard != null)
        {
            itemIconImg.transform.SetParent(this.transform.parent);
            itemIconImg.transform.SetAsLastSibling();
            itemIconImg.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -Camera.main.transform.position.z);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemCard != null)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(itemSlots[i].rectTransform, itemIconImg.transform.position))
                {
                    ItemSlot collSlot = itemSlots[i].GetComponent<ItemSlot>();

                    if (collSlot.isNull)
                    {
                        Vector2 tempPos = collSlot.transform.position;
                        collSlot.transform.position = this.transform.position;
                        this.transform.position = tempPos;
                    }
                    else
                    {
                        Vector2 tempPos = this.transform.position;
                        this.transform.position = collSlot.transform.position;
                        collSlot.transform.SetSiblingIndex(collSlot.index);
                        collSlot.transform.position = tempPos;
                    }
                    break;
                }
            }
            itemIconImg.transform.SetParent(this.transform);
            itemIconImg.transform.SetAsFirstSibling();
            itemIconImg.transform.localPosition = Vector2.zero;
        }
    }
}
