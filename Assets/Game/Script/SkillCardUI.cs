using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillCardUI : MonoBehaviour
{
    public Image cardImg;
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardExpText;
    public TextMeshProUGUI cardEffectText;
    public TextMeshProUGUI cardLvlUpGText;
    
    public void SettingCard(Sprite spr, SkillCard cardInfo)
	{
        cardImg.sprite = spr;
		switch (cardInfo.cardGrade)
		{
            case SkillCard.Grade.epic:
                gradeText.color = new Color32(255, 0, 219, 255);
                gradeText.text = "에픽 등급";
                cardNameText.color = new Color32(255, 0, 219, 255);
                break;
            case SkillCard.Grade.rare:
                gradeText.color = new Color32(39, 165, 255, 255);
                cardNameText.color = new Color32(39, 165, 255, 255);
                gradeText.text = "레어 등급";
                break;
            default: //노멀
                gradeText.color = new Color32(100, 255, 87, 255);
                cardNameText.color = new Color32(100, 255, 87, 255);
                gradeText.text = "일반 등급";
                break;
		}
        cardNameText.text = cardInfo.cardName;
        cardExpText.text = cardInfo.cardExp;
        cardEffectText.text = cardInfo.cardEffectExp;
        cardLvlUpGText.text = cardInfo.cardLvlUpEffectExp;
	}
    
}
