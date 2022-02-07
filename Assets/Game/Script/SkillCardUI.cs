using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillCardUI : MonoBehaviour
{
    public Image cardImg;
   
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillExpText;
    public TextMeshProUGUI[] skillItemExpText;
    public TextMeshProUGUI[] skillLvUpItemText;   
    
    public void SettingCard(Sprite spr, SkillCard cardInfo)
	{
        cardImg.sprite = spr;
		switch (cardInfo.cardGrade)
		{
            case SkillCard.Grade.epic:
                gradeText.color = new Color32(255, 0, 219, 255);
                gradeText.text = "에픽등급";
                skillNameText.color = new Color32(255, 0, 219, 255);

                break;
            case SkillCard.Grade.rare:
                gradeText.color = new Color32(39, 165, 255, 255);
                skillNameText.color = new Color32(39, 165, 255, 255);
                gradeText.text = "레어등급";
  
                break;
            default: //노멀
                gradeText.color = new Color32(100, 255, 87, 255);
                skillNameText.color = new Color32(100, 255, 87, 255);
                gradeText.text = "일반등급";

                break;
		}
        skillNameText.text = cardInfo.skillName;
        skillExpText.text = cardInfo.skillExp;
        for (int i = 0; i < skillItemExpText.Length; i++)
        {
            skillItemExpText[i].text = cardInfo.skillItemExp[i];

            if (skillItemExpText[i].text == "" || string.IsNullOrEmpty(skillItemExpText[i].text))
            {
                skillItemExpText[i].transform.parent.gameObject.SetActive(false);
            }
            else
            {
                skillItemExpText[i].transform.parent.gameObject.SetActive(true);
            }

        }
        for (int i = 0; i < skillLvUpItemText.Length; i++)
            skillLvUpItemText[i].text = cardInfo.skillLvUpItemExp[i];
	}
    
}
