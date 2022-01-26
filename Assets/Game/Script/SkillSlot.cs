using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SkillSlot : MonoBehaviour, IDragHandler, IEndDragHandler
{
	public Image[] skillSlots;
	public bool isNull = true;
    public Image skillImg;
    public TextMeshProUGUI levelText;
	public int index;
	public int level;
	public SkillCard skillCard;
	public Image coolTimeImg;
	public float coolTime;
	public bool isCooldown = false;
	IEnumerator skillCour;
	public Image cardGrowColor;

	public Sprite noneSpr;
	
	private void Awake()
	{
		skillImg = this.transform.GetChild(0).GetComponent<Image>();
		coolTimeImg = this.transform.GetChild(1).GetComponent<Image>();
		levelText = skillImg.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		cardGrowColor = skillImg.transform.GetChild(1).GetComponent<Image>();

		InitSkillSlot();
	}
    #region 스킬 초기화
    public void InitSkillSlot()
	{
		this.GetComponent<Button>().interactable = false;
		isNull = true;
		skillCard = null;
		skillImg.sprite = null;
		skillImg.gameObject.SetActive(false);
		level = 0;
		levelText.text = level.ToString();
		coolTimeImg.gameObject.SetActive(false);
		coolTimeImg.fillAmount = 1;
	}
    #endregion
    #region 스킬 선택
    public void SelectSkill()
	{
		if(skillCard != null && !isCooldown)
			SkillCardController.Inst.SelectSkillCard(index, skillCard.skillIndex, level);

	}
    #endregion
    #region 스킬 활성화
    public void OpenSkill()
	{
        if (!isCooldown)
		{
			isCooldown = true;
			coolTimeImg.gameObject.SetActive(true);
			coolTime = skillCard.cardCoolTime - (float)((int)(level / skillCard.cardCoolTimeCoefficient) * skillCard.cardDecreaseCoolTime);
			if (skillCour != null)
				StopCoroutine(skillCour);

			skillCour = OpenSkillCour();
			StartCoroutine(skillCour);
		}
		
	}
	IEnumerator OpenSkillCour(float time = 1)
	{
		coolTimeImg.fillAmount = time;
		cardGrowColor.gameObject.SetActive(false);
		while (isCooldown)
		{
			coolTimeImg.fillAmount -= 1/ coolTime * Time.deltaTime;
			if (coolTimeImg.fillAmount <= 0)
			{
				coolTimeImg.fillAmount = 0;
				isCooldown = false;
				coolTimeImg.gameObject.SetActive(false);
				cardGrowColor.gameObject.SetActive(true);
			}
			yield return null;
		}
	}
    #endregion
    #region 스킬세팅
    public void SettingSkillSlot(Sprite cardImg, SkillCard sc)
	{
		if (skillCard == sc) LevelUp();
		else
		{
			isNull = false;
			if(cardImg != null)
			{
				skillImg.sprite = cardImg;
				coolTimeImg.sprite = cardImg;
			}
			else
			{
				skillImg.sprite = noneSpr;
			}
			skillImg.gameObject.SetActive(true);
			level = 1;
			levelText.text = level.ToString();
			skillCard = sc;
			this.GetComponent<Button>().interactable = true;
            switch (sc.cardGrade)
            {
				case SkillCard.Grade.epic:
					cardGrowColor.color = new Color(255, 51, 155);
					break;
				case SkillCard.Grade.normal:
					cardGrowColor.color = Color.green;

					break;
				case SkillCard.Grade.rare:
					cardGrowColor.color = Color.blue;

					break;


			}
		}
	}

	//스킬 레벨업
	public void LevelUp()
	{
		if(level < 10)
			level++;
		levelText.text = level.ToString();
	}
    #endregion

    public void OnDrag(PointerEventData eventData)
	{
		if (skillCard != null)
		{
			skillImg.transform.SetParent(this.transform.parent);
			skillImg.transform.SetAsLastSibling();
			skillImg.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -Camera.main.transform.position.z);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (skillCard != null)
		{
			for (int i = 0; i < skillSlots.Length; i++)
			{
				if (RectTransformUtility.RectangleContainsScreenPoint(skillSlots[i].rectTransform, skillImg.transform.position))
				{
					SkillSlot collSlot = skillSlots[i].GetComponent<SkillSlot>();
					if (collSlot.isNull)
					{
						Vector2 tempPos = collSlot.transform.position;
						collSlot.transform.position = this.transform.position;
						this.transform.position = tempPos;
						int thisIndex = this.transform.GetSiblingIndex();
						int collIndex = collSlot.transform.GetSiblingIndex();
						collSlot.transform.SetSiblingIndex(thisIndex);
						this.transform.SetSiblingIndex(collIndex);

					}
					else
					{

						Vector2 tempPos = this.transform.position;
						this.transform.position = collSlot.transform.position;
						//collSlot.transform.SetSiblingIndex(collSlot.index);
						collSlot.transform.position = tempPos;
						int thisIndex = this.transform.GetSiblingIndex();
						int collIndex = collSlot.transform.GetSiblingIndex();
						collSlot.transform.SetSiblingIndex(thisIndex);
						this.transform.SetSiblingIndex(collIndex);
					}
					break;
				}
			}
			skillImg.transform.SetParent(this.transform);
			skillImg.transform.SetAsFirstSibling();
			skillImg.transform.localPosition = Vector2.zero;
		}
			
	}

}
