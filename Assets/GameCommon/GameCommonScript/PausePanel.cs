using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PausePanel : MonoBehaviour
{
    public TextMeshProUGUI currentWaveText;
    public Image[] currentSkillCards = new Image[5];
    public TextMeshProUGUI[] currentSkillCardLevelText = new TextMeshProUGUI[5];

    private void OnEnable()
    {
        Time.timeScale = 0;

        currentWaveText.text = "현재 킬수 : " + GameController.Inst.killCnt.ToString();
        for (int i = 0; i < SkillCardController.Inst.skillSlots.Length; i++)
        {
            if (!SkillCardController.Inst.skillSlots[i].isNull)
            {
                currentSkillCards[i].sprite = SkillCardController.Inst.skillSlots[i].skillImg.sprite;
                currentSkillCardLevelText[i].text = SkillCardController.Inst.skillSlots[i].levelText.text;
                currentSkillCards[i].gameObject.SetActive(true);

            }
            else
            {
                currentSkillCards[i].sprite = null;
                currentSkillCardLevelText[i].text = "";
                currentSkillCards[i].gameObject.SetActive(false);

            }
        }
    }

    public void ContinuBtn()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }
}
