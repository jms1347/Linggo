using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopController : MonoBehaviour
{
    [System.Serializable]
    public class ItemListBox
    {
        public GameObject itemBox;
        public Image itemPic;
        public TextMeshProUGUI itemNameT;
        public TextMeshProUGUI itemCntT;
        public TextMeshProUGUI itemPriceT;
    }

    public ItemListBox[] itemListBox = new ItemListBox[8];

    [Header("BuyPanel")]
    public GameObject buyPanel;
    public Image buyItemPic;
    public TextMeshProUGUI buyItemNameText;
    public TextMeshProUGUI buyItemExpText;
    public TextMeshProUGUI buyItemCntText;
    public int buyItemCnt = 1;
    public TextMeshProUGUI buyITemPriceText;
    

    [Header("Page��Ʈ��")]
    int maxPage, multiple, currentPage = 1, buyIndex=0;
    public Button previousBtn, nextBtn;

    public void OpenBuyPanel(int i)
    {
        buyIndex = multiple + i;
        buyItemPic.sprite = ItemCardController.Inst.itemSprs[ItemCardController.Inst.itemCardList[multiple + i].itemIndex];
        buyItemNameText.text = ItemCardController.Inst.itemCardList[multiple + i].itemName;
        buyItemExpText.text = ItemCardController.Inst.itemCardList[multiple + i].itemExp;
        buyItemCnt = 1;
        buyItemCntText.text = "1��";
        buyITemPriceText.text = ItemCardController.Inst.itemCardList[multiple + i].itemPrice.ToString();
        buyPanel.SetActive(true);
    }

    public void PlusCntBtn(int index)
    {
        if(index == 100)
        {
            //�ִ�ϱ� ���� ������ index�� ����� ���� �ʿ�
            buyItemCnt = (GameController.Inst.gold/int.Parse(buyITemPriceText.text));
            buyItemCntText.text = buyItemCnt.ToString() + "��";
        }
        else
        {
            int butCnt = buyItemCnt + index;
            if(butCnt * int.Parse(buyITemPriceText.text) > GameController.Inst.gold )
            {
                GameController.Inst.OpenGuidePop("��尡 �����մϴ�.");
            }
            else{
                buyItemCnt += index;
                buyItemCntText.text = buyItemCnt.ToString() + "��";
            }
            
        }
    }
    public void BuyItem()
    {
        if(GameController.Inst.gold < (int.Parse(buyITemPriceText.text)*buyItemCnt))
        {
            GameController.Inst.OpenGuidePop("��尡 �����մϴ�.");

            return;
        }
        
        bool isDupli = false;
        for (int i = 0; i < ItemCardController.Inst.itemSlots.Length; i++)
        {
            if (ItemCardController.Inst.itemSlots[i].itemCard == ItemCardController.Inst.itemCardList[buyIndex])
            {
                ItemCardController.Inst.itemSlots[i].SettingItemSlot(
                    ItemCardController.Inst.itemSprs[buyIndex], ItemCardController.Inst.itemCardList[buyIndex]
                    , buyItemCnt);
                
                isDupli = true;
                GameController.Inst.DecreaseGold((int.Parse(buyITemPriceText.text) * buyItemCnt));
                break;
            }
        }
        if (!isDupli)
        {
            for (int i = 0; i < ItemCardController.Inst.itemSlots.Length; i++)
            {
                if (ItemCardController.Inst.itemSlots[i].isNull || ItemCardController.Inst.itemSlots[i].itemCnt <= 0)
                {
                    ItemCardController.Inst.itemSlots[i].SettingItemSlot(
                        ItemCardController.Inst.itemSprs[buyIndex], ItemCardController.Inst.itemCardList[buyIndex]
                        , buyItemCnt);
                    isDupli = true;
                    GameController.Inst.DecreaseGold((int.Parse(buyITemPriceText.text) * buyItemCnt));
                    break;
                }
            }
        }

        //��� �޽��� ������â�� ������ �ֹ� ���Ѵٴ� �˾� �޽��� ���
        if (!isDupli)
        {
            GameController.Inst.OpenGuidePop("�������� ��ġ�� ������ ��� ������ �� �����ϴ�.");
        }
        else
        {
            buyPanel.SetActive(false);
            this.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

    }

    public void Btn(int index)
    {
        if(index == -1)
        {
            currentPage--;
        }else if(index == 0) //������
        {
            Time.timeScale = 0;

            this.gameObject.SetActive(true);
        }else if(index == -10)
        {
            //��Ŭ����
            Time.timeScale = 1;

            this.gameObject.SetActive(false);
        }
        else if(index == 1)
        {
            currentPage++;
        }
        ItemListRenewal();
    }



    #region ������ ����Ʈ ����
    public void ItemListRenewal()
    {
        // �ִ�������
        maxPage = (ItemCardController.Inst.itemCardList.Count % itemListBox.Length == 0) ? ItemCardController.Inst.itemCardList.Count / itemListBox.Length : ItemCardController.Inst.itemCardList.Count / itemListBox.Length + 1;

        // ����, ������ư
        previousBtn.interactable = (currentPage <= 1) ? false : true;
        nextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // �������� �´� ����Ʈ ����
        multiple = (currentPage - 1) * itemListBox.Length;
        for (int i = 0; i < itemListBox.Length; i++)
        {
            itemListBox[i].itemBox.SetActive((multiple + i < ItemCardController.Inst.itemCardList.Count) ? true : false);
            itemListBox[i].itemNameT.text = (multiple + i < ItemCardController.Inst.itemCardList.Count) ? ItemCardController.Inst.itemCardList[multiple + i].itemName : "";
            itemListBox[i].itemPriceT.text = (multiple + i < ItemCardController.Inst.itemCardList.Count) ? ItemCardController.Inst.itemCardList[multiple + i].itemPrice.ToString() : "";

            itemListBox[i].itemPic.gameObject.SetActive((multiple + i < ItemCardController.Inst.itemCardList.Count) ? true : false);
            itemListBox[i].itemPic.sprite = (multiple + i < ItemCardController.Inst.itemCardList.Count) ? ItemCardController.Inst.itemSprs[ItemCardController.Inst.itemCardList[multiple + i].itemIndex] : ItemCardController.Inst.itemSprs[0];
        }
    }
    #endregion
}
