using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LinggoStatBoard : MonoBehaviour
{
    public TextMeshProUGUI levelText;

    public TextMeshProUGUI plusAttText;
    public TextMeshProUGUI plusAttGoldText;
    public TextMeshProUGUI plusHpText;
    public TextMeshProUGUI plusHpGoldText;
    public TextMeshProUGUI plusMarbleAppearanceText;
    public TextMeshProUGUI plusMarbleAppearanceGoldText;

    public TextMeshProUGUI linggoAccumulatedAttText;
    public TextMeshProUGUI linggoAccumulatedHpText;
    public TextMeshProUGUI linggoAccumulatedAttSpeedText;
    public TextMeshProUGUI linggoAttText;
    public TextMeshProUGUI linggoHpText;
    public TextMeshProUGUI linggoAttSpeedText;

    public void OnEnable()
    {
        levelText.text = "Lv."+ GameController.Inst.level.ToString();
        plusHpText.text = "(+" + GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusHpLevel+1].plusHp + ")";
        plusHpGoldText.text = GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusHpLevel+1].plusHpGold.ToString();

        plusAttText.text = "(+" + GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusAttLevel+1].plusAtt + ")";
        plusAttGoldText.text = GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusAttLevel+1].plusAttGold.ToString();
        
        plusMarbleAppearanceText.text = (10 + GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusMarbleAppearPercentLevel+1].plusAppearPercent).ToString() + "%";
        plusMarbleAppearanceGoldText.text = GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusMarbleAppearPercentLevel+1].plusAppearPercentGold.ToString();

        int plusHP = GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusHpLevel].plusHp;
        int plusATT = GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusAttLevel].plusAtt;
        //float plusMarbleAppearPerct = GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusMarbleAppearPercentLevel].plusAppearPercent;

        linggoHpText.text = (GameController.Inst.maxHP + plusHP).ToString();
        linggoAccumulatedHpText.text = "(+" + plusHP.ToString() + ")";
        linggoAttText.text = (GameController.Inst.att + plusATT).ToString();
        linggoAccumulatedAttText.text = "(+" + plusATT.ToString() + ")";
        linggoAttSpeedText.text = GameController.Inst.attSpeed.ToString() + "�� �� 1ȸ";
        linggoAccumulatedAttSpeedText.text = "";
    }

    public void InitStatBoard()
    {
        levelText.text = "Lv.1";
        plusAttText.text = "+0";
        plusHpText.text = "+0";
        plusMarbleAppearanceText.text = "+0";

        //1���� ���� ������ ��������(DB)
        plusHpGoldText.text = "10";
        plusAttGoldText.text = "10";
        plusMarbleAppearanceGoldText.text = "10";

        //1���� ���� ���� ��������
        linggoHpText.text = GameController.Inst.linggoLevelDataSO.levelData[0].upHp.ToString();
        linggoAttText.text = GameController.Inst.linggoLevelDataSO.levelData[0].upAtt.ToString();
        linggoAttSpeedText.text = GameController.Inst.linggoLevelDataSO.levelData[0].attSpeed.ToString();

        //���� ���� ������ ��������
        linggoAccumulatedHpText.text = "";
        linggoAccumulatedAttText.text = "";
        linggoAccumulatedAttSpeedText.text = "";
    }

    public void PlusStat(string btnKey)
    {
        if (btnKey == "Hp")
        {
            if (GameController.Inst.gold < GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusHpLevel + 1].plusHpGold)
            {
                GameController.Inst.OpenGuidePop("��尡 �����մϴ�.");
                return;
            }
            //DB���� �߰��ϱ�
            GameController.Inst.plusHpLevel++;
            GameController.Inst.DecreaseGold(GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusHpLevel + 1].plusHpGold);
            plusHpText.text = "(+" + GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusHpLevel + 1].plusHp + ")";
            plusHpGoldText.text = GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusHpLevel + 1].plusHpGold.ToString();

            int plusHP = GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusHpLevel].plusHp;
            linggoHpText.text = (GameController.Inst.maxHP + plusHP).ToString();
            linggoAccumulatedHpText.text = "(+" + plusHP.ToString() + ")";
        }
        else if (btnKey == "Att")
        {
            if (GameController.Inst.gold < GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusAttLevel + 1].plusAttGold)
            {
                GameController.Inst.OpenGuidePop("��尡 �����մϴ�.");
                return;
            }
            GameController.Inst.plusAttLevel++;
            GameController.Inst.DecreaseGold(GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusAttLevel + 1].plusAttGold);
            plusAttText.text = "(+" + GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusAttLevel + 1].plusAtt + ")";
            plusAttGoldText.text = GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusAttLevel + 1].plusAttGold.ToString();
            int plusATT = GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusAttLevel].plusAtt;
            linggoAttText.text = (GameController.Inst.att + plusATT).ToString();
            linggoAccumulatedAttText.text = "(+" + plusATT.ToString() + ")";
        }
        else if (btnKey == "Marble")
        {
            if (GameController.Inst.gold < GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusMarbleAppearPercentLevel + 1].plusAppearPercentGold)
            {
                GameController.Inst.OpenGuidePop("��尡 �����մϴ�.");
                return;
            }
            GameController.Inst.plusMarbleAppearPercentLevel++;
            GameController.Inst.DecreaseGold(GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusMarbleAppearPercentLevel + 1].plusAppearPercentGold);
            if (GameController.Inst.plusMarbleAppearPercentLevel == 90)
            {
                print("���� Ȯ�� MAX");
                plusMarbleAppearanceText.text = "100% (MAX)";
                plusMarbleAppearanceGoldText.text = "";
            }
            else
            {
                plusMarbleAppearanceText.text = GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusMarbleAppearPercentLevel + 1].plusAppearPercent.ToString() + "%";
                plusMarbleAppearanceGoldText.text = GameController.Inst.stateLevelDataSO.stateLevelData[GameController.Inst.plusMarbleAppearPercentLevel + 1].plusAppearPercentGold.ToString();

            }
        }

    }

    public void OnOffBtn(string keyStr)
    {
        if(keyStr == "Open")
        {
            this.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else if(keyStr == "Close")
        {
            this.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
}