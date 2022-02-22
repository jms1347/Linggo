using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class SkillCardController : MonoBehaviour
{
    public static SkillCardController Inst { get; private set; }
    private void Awake() => Inst = this;

    [SerializeField] SkillCardSo skillCardSO;
	public List<SkillCard> skillCardLists = new List<SkillCard>();
	private SkillCard selectSkillCard;
	public GameObject GetSkillCardUI;

	public SkillSlot[] skillSlots = new SkillSlot[5];

	public SpriteRenderer skillStartEffectSprR;
	public GameObject skillUseRange;
	public bool isSelectSkill = false;
	private Vector2 clickPos;
	private int selectSkillIndex;
	private SkillSlot selectSlot;

	[System.Serializable]
	public class SkillSprs
	{
		public Sprite skillCardSpr;
		public Sprite skillStartSpr;
		public GameObject skillObj;
		public GameObject skillRangeObj;
	}

	public SkillSprs[] skillSprs;
	public GameObject dimmedObj;

	void Start()
	{
		oriSkillStartPos = skillStartEffectSprR.transform.position;

		SettingSkillCardList();
	}


    bool PosSelecting = false;
    void Update()
	{
		if (isSelectSkill)
        {
            
            skillUseRange.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -Camera.main.ScreenToWorldPoint(Input.mousePosition).z);

            if (Input.GetMouseButtonDown(0))
            {
                print("다운");
                PosSelecting = true;
                skillUseRange.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -Camera.main.ScreenToWorldPoint(Input.mousePosition).z);

                
                
            }
            if (Input.GetMouseButtonUp(0) && PosSelecting)
            {
                print("업");

                skillUseRange.SetActive(false);
                clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (selectSkillIndex != -1)
                {
                    selectSlot.OpenSkill(); //스킬슬롯 쿨타임
                    UseSkill(selectSkillIndex);
                    //GoSkill(selectSkillIndex, clickPos); //스킬 발동
                    StartCoroutine(GoSkillCour(selectSkillIndex, clickPos));
                    isSelectSkill = false;
                    skillUseRange = null;
                    PosSelecting = false;
                }
            }
        }
	}

    #region 스킬 선택
    public void SelectSkillCard(int slotIndex, int skillIndex,int level)
	{
        PosSelecting = false;
        selectSlot = skillSlots[slotIndex];
		selectSkillIndex = skillIndex;
		skillSprs[skillIndex].skillObj.GetComponent<Skill>().skillLevel = level;
		skillUseRange = skillSprs[skillIndex].skillRangeObj;
		skillUseRange.SetActive(true);
		isSelectSkill = true;
		dimmedObj.SetActive(true);
		Time.timeScale = 0f;

	}
	#endregion
	#region 스킬 리스트 세팅
	void SettingSkillCardList()
	{
		for (int i = 0; i < skillCardSO.skillCards.Count; i++)
		{
            SkillCard skillCard = skillCardSO.skillCards[i];
			for (int j = 0; j < skillCard.cardAppearPercent; j++)
			{
                skillCardLists.Add(skillCard);
			}
		}

		for (int i = 0; i < skillCardLists.Count; i++)
		{
			int rand = Random.Range(i, skillCardLists.Count);
			SkillCard temp = skillCardLists[i];
			skillCardLists[i] = skillCardLists[rand];
			skillCardLists[rand] = temp;
		}
	}
	#endregion

	#region 스킬UI 팝업 On/Off
	public void OnPopUpUI()
	{
		Time.timeScale = 0;
		if(skillCardLists.Count == 0)
		{
			SettingSkillCardList();
		}
		
		GetSkillCardUI.GetComponent<SkillCardUI>().SettingCard(skillSprs[skillCardLists[0].skillIndex].skillCardSpr, skillCardLists[0]);
		GetSkillCardUI.SetActive(true);
		selectSkillCard = skillCardLists[0];
	}

	public void OffPopUpUi(bool isOk)
	{
		if (isOk)
		{
			int duplicardIndex = -1;
			for (int i = 0; i < skillSlots.Length; i++)
			{
				if(skillSlots[i].skillImg.sprite == skillSprs[skillCardLists[0].skillIndex].skillCardSpr)
				{
					duplicardIndex = i;
					break;
				}
			}

			bool isAllNotNull = false;
			if (duplicardIndex != -1)
			{
				skillSlots[duplicardIndex].LevelUp();
			}
			else
			{
				for (int i = 0; i < skillSlots.Length; i++)
				{
					if (skillSlots[i].isNull)
					{
						isAllNotNull = true;
						skillSlots[i].SettingSkillSlot(skillSprs[skillCardLists[0].skillIndex].skillCardSpr, skillCardLists[0]);
						break;
					}
				}

				//다 꽉차있는데 승인하면 무조건0번 교체
				if (!isAllNotNull)
				{
					SkillSlot deleSlot = skillSlots[0].transform.parent.transform.GetChild(0).GetComponent<SkillSlot>();
					deleSlot.SettingSkillSlot(skillSprs[skillCardLists[0].skillIndex].skillCardSpr, skillCardLists[0]);
				}
			}				
		}
		
		GetSkillCardUI.SetActive(false);
		Time.timeScale = 1;
		skillCardLists.RemoveAt(0);

	}
	#endregion

	#region 스킬 사용 이펙트
	Vector3 oriSkillStartPos;
	public void UseSkill(int skillIndex)
	{
		skillStartEffectSprR.transform.position = oriSkillStartPos;
		skillStartEffectSprR.gameObject.transform.DOKill();

		if (skillSprs[skillIndex].skillStartSpr != null)
			skillStartEffectSprR.sprite = skillSprs[skillIndex].skillStartSpr;
		Time.timeScale = 0.5f;
		dimmedObj.SetActive(false);
		skillStartEffectSprR.gameObject.transform.DOMoveX(skillStartEffectSprR.transform.position.x +6.1f, 0.75f).SetEase(Ease.OutQuint)
			.OnComplete(() => {
				Time.timeScale = 1f;

				skillStartEffectSprR.gameObject.transform.DOMoveX(skillStartEffectSprR.transform.position.x -6.1f, 0.1f).SetDelay(0.25f)
				.OnComplete(()=>
				{
				});
		});
	}
    #endregion

    #region 스킬 발동
    public void GoSkill(int skillIndex, Vector2 clickPos)
	{
		skillSprs[skillIndex].skillObj.transform.position = clickPos;
		skillSprs[skillIndex].skillObj.SetActive(true);
	}

	public IEnumerator GoSkillCour(int skillIndex, Vector2 clickPos)
    {
		yield return new WaitForSeconds(0.55f);
		skillSprs[skillIndex].skillObj.transform.position = clickPos;
		skillSprs[skillIndex].skillObj.SetActive(true);
	}
    #endregion


}
