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
	//private SkillCard selectSkillCard;
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

    [Header("0Wave 시스템 관련 변수")]
    public GameObject skillSupportSystem;
    public GameObject[] skillSupportEffect;
    public Image[] supportCardImg;
    public Image[] supportEffectCardImg;
    public TextMeshProUGUI[] supportCardText;

    public List<SkillCard> mixList = new List<SkillCard>();
    bool PosSelecting = false;

    

    void Start()
	{
		oriSkillStartPos = skillStartEffectSprR.transform.position;

		SettingSkillCardList();
        Mix();
        SettingSupportCard();
    }

    #region 0Wave 함수
    public void SettingSupportCard()
    {
        GameController.Inst.TimeOnBtn();
        for (int i = 0; i < mixList.Count; i++)
        {
            supportCardImg[i].sprite = skillSprs[mixList[i].skillIndex].skillCardSpr;
            supportEffectCardImg[i].sprite = skillSprs[mixList[i].skillIndex].skillCardSpr;
            supportCardText[i].text = mixList[i].skillName;
        }
        skillSupportSystem.SetActive(true);
    }
    public void SupportCardBtn()
    {
        GameController.Inst.TimeOffBtn();

        StartCoroutine(SupportCardBtnCour());
    }

    IEnumerator SupportCardBtnCour()
    {
        skillSupportSystem.SetActive(false);
        for (int i = 0; i < skillSupportEffect.Length; i++)
        {
            skillSupportEffect[i].transform.position = supportCardImg[i].transform.position;

            supportEffectCardImg[i].gameObject.SetActive(true);
            supportEffectCardImg[i].GetComponent<Image>().DOFade(0, 1.0f);
            skillSupportEffect[i].SetActive(true);            
        }
        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < skillSupportEffect.Length; i++)
        {
            skillSupportEffect[i].SetActive(true);
            skillSupportEffect[i].transform.DOMove(skillSlots[i].transform.position, 1.0f)
                .SetEase(Ease.InExpo);
        }
        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < skillSupportEffect.Length; i++)
        {
            skillSlots[i].SettingSkillSlot(supportCardImg[i].sprite, mixList[i]);

            supportEffectCardImg[i].gameObject.SetActive(false);
            skillSupportEffect[i].SetActive(false);
        }
    }
    public void Mix()
    {
        
        List<SkillCard> list = new List<SkillCard>();
        //int count = mixList.Count;
        int count = 3;
        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, mixList.Count);
            list.Add(mixList[rand]);
            mixList.RemoveAt(rand);
        }
        mixList = list;
    }
    #endregion
    void Update()
    {
        
        if (isSelectSkill)
        {
            
            skillUseRange.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -Camera.main.ScreenToWorldPoint(Input.mousePosition).z);

            if (Input.GetMouseButtonDown(0))
            {
                PosSelecting = true;
                skillUseRange.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -Camera.main.ScreenToWorldPoint(Input.mousePosition).z);

                
                
            }
            if (Input.GetMouseButtonUp(0) && PosSelecting)
            {
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
        GameController.Inst.ClickSound();   //버튼 사운드
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
            if(skillCard.cardGrade == SkillCard.Grade.normal)
                mixList.Add(skillCard);

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
    #region 스킬 보상 함수
    public void ChangeCardRewardAD()
    {
        if (skillCardLists.Count < 2)
        {
            SettingSkillCardList();
        }

        for (int i = 0; i < skillCardLists.Count; i++)
        {
            if (skillSprs[skillCardLists[0].skillIndex].skillCardSpr != skillSprs[skillCardLists[1].skillIndex].skillCardSpr)
            {
                //2개씩으로 변경
                Sprite[] sprs = new Sprite[]{skillSprs[skillCardLists[0].skillIndex].skillCardSpr,
            skillSprs[skillCardLists[1].skillIndex].skillCardSpr};
                SkillCard[] cards = new SkillCard[]
                {
             skillCardLists[0],  skillCardLists[1]
                };
                GetSkillCardUI.GetComponent<DoubleSkillCardUI>().SettingCards(sprs, cards);

                skillCardLists.RemoveAt(0);
                skillCardLists.RemoveAt(0);
                //GetSkillCardUI.SetActive(true);
                break;
            }
            else
            {
                skillCardLists.RemoveAt(0);

            }
        }
    }
    #endregion
    #region 스킬UI 팝업 On/Off
    public void OnPopUpUI()
	{
        GameController.Inst.TimeOnBtn();

		if(skillCardLists.Count < 2)
		{
			SettingSkillCardList();
		}


        for (int i = 0; i < skillCardLists.Count; i++)
        {
            if (skillSprs[skillCardLists[0].skillIndex].skillCardSpr != skillSprs[skillCardLists[1].skillIndex].skillCardSpr)
            {
                //2개씩으로 변경
                Sprite[] sprs = new Sprite[]{skillSprs[skillCardLists[0].skillIndex].skillCardSpr,
            skillSprs[skillCardLists[1].skillIndex].skillCardSpr};
                SkillCard[] cards = new SkillCard[]
                {
             skillCardLists[0],  skillCardLists[1]
                };
                GetSkillCardUI.GetComponent<DoubleSkillCardUI>().SettingCards(sprs, cards);

                skillCardLists.RemoveAt(0);
                skillCardLists.RemoveAt(0);
                GetSkillCardUI.SetActive(true);
                break;
            }
            else
            {
                skillCardLists.RemoveAt(0);

            }
        }
        //GetSkillCardUI.GetComponent<SkillCardUI>().SettingCard(skillSprs[skillCardLists[0].skillIndex].skillCardSpr, skillCardLists[0]);
        
        //GetSkillCardUI.GetComponent<DoubleSkillCardUI>().farmingCardUI.SetActive(true);
		//selectSkillCard = skillCardLists[0];
	}

    //일단 이제 안씀(1스킬 파밍 일때)
	public void OffFarmingSKillCardSystem()
    {
        //Time.timeScale = 1;

        for (int i = 0; i < skillSlots.Length; i++)
        {
            skillSlots[i].SelectSkillPostProcessing();
        }
		//GetSkillCardUI.SetActive(false);

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

    #region 스킬슬롯에서 중복되는 것 찾기
    public int CheckDupliSkillCard(int cardIndex)
    {
        int duplicardIndex = -1;
        for (int i = 0; i < skillSlots.Length; i++)
        {
            if(skillSlots[i].skillCard != null)
            {
                if (skillSlots[i].skillCard.skillIndex == cardIndex)
                {
                    duplicardIndex = i;
                    break;
                }
            }           
        }

        return duplicardIndex;
    }


    #endregion


}
