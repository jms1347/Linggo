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
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI notMoneyText;
    bool isGuide = true;
	private void Awake()
	{
		itemIconImg = this.transform.GetChild(0).GetComponent<Image>();
		coolTimeImg = this.transform.GetChild(1).GetComponent<Image>();
        itemPriceText = this.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        timerText = coolTimeImg.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        isGuide = true;

        InitItemSlot();
	}

    #region ���� ��Ÿ�� �ʱ�ȭ
    public void InitItemSlot()
	{
		coolTimeImg.gameObject.SetActive(false);
		coolTimeImg.fillAmount = 1;
	}
	#endregion

	#region ������ ���
	public void SelectItem()
	{

		if (itemCard != null && !isCooldown)
        {
            if (GameController.Inst.gold < itemCard.itemPrice)
            {
                guideTitle.text = "";

                guideContent.text = "";

                notMoneyText.text = "��尡 �����մϴ�";
                notMoneyText.gameObject.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1f);
                notMoneyText.gameObject.SetActive(true);
                return;
            }
            if (isGuide)
            {
                //isGuide = false;
                guideTitle.text = itemCard.itemName;
                guideContent.text = itemCard.itemExp;
                itemGuideBar.SetActive(true);
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
            timerText.text = Mathf.RoundToInt(coolTimeImg.fillAmount * itemCard.itemCoolTime).ToString();

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

    #region ������ ����
    public void SettingItemSlot(ItemCard ic, Sprite icSpr)
    {
        itemCard = ic;
        itemIconImg.sprite = icSpr;
        itemPriceText.text = itemCard.itemPrice.ToString();
    }

	#endregion    
}
