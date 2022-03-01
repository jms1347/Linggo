using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DoubleSkillCardUI : MonoBehaviour
{
    //public GameObject farmingCardUI;
    public FarmingCardLevelExpDataSo farmingCardLevelExpDataSo;
    [System.Serializable]
    public class SelectBox
    {
        public SkillCard skillCard;
        public Image cardImg;

        public TextMeshProUGUI gradeText;
        public TextMeshProUGUI skillNameText;
        public TextMeshProUGUI skillExpText;
        public TextMeshProUGUI skillLvExpText;
    }
    [System.Serializable]
    public class SkillLvExpData
    {
        public int skillIndex;
        public int skillLevel;
        public string skillLvExp;
    }

    public SelectBox[] selectBoxes;
    public SelectBox selectFarmingSkillCard;
    public GameObject[] selectRedBoxes;
    public GameObject oBtn;
    //public List<SkillLvExpData> skillLvExpDataes = new List<SkillLvExpData>();
    public void Awake()
    {
        
        //for (int i = 0; i < farmingCardLevelExpDataSo.farmingCardLevelExpData.Count; i++)
        //{
        //    SkillLvExpData skillLvExpData = new SkillLvExpData();
        //    skillLvExpData.skillIndex = farmingCardLevelExpDataSo.farmingCardLevelExpData[i].farmingCardIndex;
        //    skillLvExpData.skillLevel = farmingCardLevelExpDataSo.farmingCardLevelExpData[i].farmingCardLevel;
        //    skillLvExpData.skillLvExp = farmingCardLevelExpDataSo.farmingCardLevelExpData[i].farmingCardLevelExpStr;

        //    skillLvExpDataes.Add(skillLvExpData);
        //}
    }

    #region 파밍 카드 세팅(2개)
    public void SettingCards(Sprite[] sprs, SkillCard[] cardInfos)
    {
        for (int i = 0; i < selectBoxes.Length; i++)
        {
            selectBoxes[i].skillCard = cardInfos[i];
            selectBoxes[i].cardImg.sprite = sprs[i];
            switch (cardInfos[i].cardGrade)
            {
                case SkillCard.Grade.epic:
                    selectBoxes[i].gradeText.color = new Color32(255, 0, 219, 255);
                    selectBoxes[i].gradeText.text = "에픽등급";
                    selectBoxes[i].skillNameText.color = new Color32(255, 0, 219, 255);

                    break;
                case SkillCard.Grade.rare:
                    selectBoxes[i].gradeText.color = new Color32(39, 165, 255, 255);
                    selectBoxes[i].skillNameText.color = new Color32(39, 165, 255, 255);
                    selectBoxes[i].gradeText.text = "레어등급";

                    break;
                default: //노멀
                    selectBoxes[i].gradeText.color = new Color32(100, 255, 87, 255);
                    selectBoxes[i].skillNameText.color = new Color32(100, 255, 87, 255);
                    selectBoxes[i].gradeText.text = "일반등급";

                    break;
            }

            int dupliIndex = SkillCardController.Inst.CheckDupliSkillCard(cardInfos[i].skillIndex);
            if(dupliIndex != -1)
            {
                selectBoxes[i].skillNameText.text = "Lv." + (SkillCardController.Inst.skillSlots[dupliIndex].level+1) +" " +cardInfos[i].skillName;
                //selectBoxes[i].skillLvExpText.text = skillLvExpDataes[cardInfos[i].skillIndex * 10 + (SkillCardController.Inst.skillSlots[dupliIndex].level + 1)].skillLvExp;
                selectBoxes[i].skillLvExpText.text = farmingCardLevelExpDataSo.farmingCardLevelExpData[cardInfos[i].skillIndex * 10 + (SkillCardController.Inst.skillSlots[dupliIndex].level + 1)].farmingCardLevelExpStr;

            }
            else
            {
                selectBoxes[i].skillNameText.text = "Lv.1 "+ cardInfos[i].skillName;
                //selectBoxes[i].skillLvExpText.text = skillLvExpDataes[cardInfos[i].skillIndex * 10].skillLvExp;
                selectBoxes[i].skillLvExpText.text = farmingCardLevelExpDataSo.farmingCardLevelExpData[cardInfos[i].skillIndex * 10].farmingCardLevelExpStr;  
            }

            selectBoxes[i].skillExpText.text = cardInfos[i].skillExp;

        }                
    }
    #endregion
    #region 파밍카드 선택
    int index;

    public void SelectFarmingCard()
    {
        Time.timeScale = 1f;
        SkillCardController.Inst.GetSkillCardUI.SetActive(false);
        if (selectRedBoxes[0].activeSelf)
        {
            index = 0;
        }
        else if (selectRedBoxes[1].activeSelf)
        {
            index = 1;
        }
        int dupliIndex = SkillCardController.Inst.CheckDupliSkillCard(selectBoxes[index].skillCard.skillIndex);
        selectFarmingSkillCard = selectBoxes[index];
        if (index == 0)
        {
            //0번 슬롯
            if (dupliIndex != -1)
            {
                //중복일때
                SkillCardController.Inst.skillSlots[dupliIndex].LevelUp();
            }
            else
            {
                //중복 아닐때 스킬 슬롯 선택
                //farmingCardUI.SetActive(false);
                for (int i = 0; i < SkillCardController.Inst.skillSlots.Length; i++)
                {
                    SkillCardController.Inst.skillSlots[i].selectBtn.SetActive(true);
                }
            }
        }
        else
        {
            //1번 슬롯
            if (dupliIndex != -1)
            {
                //중복일때
                SkillCardController.Inst.skillSlots[dupliIndex].LevelUp();
            }
            else
            {
                //중복 아닐때 스킬 슬롯 선택
                //farmingCardUI.SetActive(false);
                for (int i = 0; i < SkillCardController.Inst.skillSlots.Length; i++)
                {
                    SkillCardController.Inst.skillSlots[i].selectBtn.SetActive(true);
                }
            }
        }
        selectRedBoxes[0].SetActive(false);
        selectRedBoxes[1].SetActive(false);
    }

    public void SelectSkillSlot(int index)
    {
        SkillCardController.Inst.skillSlots[index].SettingSkillSlot(selectFarmingSkillCard.cardImg.sprite, selectFarmingSkillCard.skillCard);

        SkillCardController.Inst.OffFarmingSKillCardSystem();

    }
    #endregion

    public void RefuseFarmingCard()
    {
        Time.timeScale = 1f;
        selectRedBoxes[0].SetActive(false);
        selectRedBoxes[1].SetActive(false);
        this.gameObject.SetActive(false);
        oBtn.SetActive(false);
    }
}
