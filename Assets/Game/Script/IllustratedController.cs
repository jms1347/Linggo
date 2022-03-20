using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllustratedController : MonoBehaviour
{
    public SkillCardSo skillCardSo;
    public SkillCardUI illDetailPanel;
    public Sprite[] skillStartSpr;



    public void IllCardBtn(int index)
    {
        illDetailPanel.SettingCard(skillStartSpr[index], skillCardSo.skillCards[index]);
        illDetailPanel.gameObject.SetActive(true);
    }
}
