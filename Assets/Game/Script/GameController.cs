using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class GameController : MonoBehaviour
{
    public static GameController Inst { get; private set; }
    private void Awake() => Inst = this;

    [SerializeField]public LevelDataSo linggoLevelDataSO;
    [SerializeField]public MonsterAppearanceLevelDataSo monsterAppearanceLevelDataSO;

    [Header("HP 관련")]
    public int maxHP;
    public int currentHP;
    public Image hpBar;
    public TextMeshProUGUI hpText;

    [Header("레벨 관련")]
    public int level;
    public TextMeshProUGUI levelText;
    public int currentExp;
    public int maxExp;
    public Image expBar;
    public int killCnt;
    public TextMeshProUGUI killCntText;

    [Header("골드 관련")]
    public int gold;
    public TextMeshProUGUI goldText;
    
    [Header("웨이브 관련")]
    public int wave;
    public TextMeshProUGUI waveText;
    public float nextWaveTime;
    public TextMeshProUGUI nextwaveTimeText;
    public int nextWaveCurrentKillCnt;
    public int nextWaveMaxKillCnt;
    public TextMeshProUGUI nextwaveKillText;

    public GameObject nextWaveBar;
    public TextMeshProUGUI nextWaveBarWaveText;
    public TextMeshProUGUI nextWaveBarWaveExText;
    public GameObject bossBar;

    [Header("링고 스텟")]
    public Linggo linggo;
    public int att;
    public float attSpeed;
    public float missileSpeed;

    [Header("구슬 관련")]
    public Image marbleExpBar;
    public int maxMarbleExp;
    public int currentMarbleExp;
    public GameObject[] marbles;


    [Header("그 외 변수")]
    public GameObject guidePop;


    public bool isStartGame = false;
    private IEnumerator startGameCour;
    private IEnumerator timerCour;
    private IEnumerator waveBarCour;
    private IEnumerator bossBarCour;
    private IEnumerator createMarbleCour;
    bool isEndTimer = false;
    bool isWaveGoalComplete = false;

    [Header("프리팹")]
    public GameObject damageTPrefab;
    public GameObject criticalDamageTPrefab;
    public GameObject healTPrefab;
    public GameObject dotDamageTPrefab;
    public GameObject deathPrefab;

    [Header("몬스터관련")]
    public Transform[] monsterCreatePos;
    public GameObject[] nofes1;
    public GameObject[] nofes2;
    public GameObject[] nofes3;
    public GameObject[] nofes4;
    public GameObject[] nofes5;
    public GameObject[] nofes6;
    public GameObject[] nofes7;
    public GameObject[] nofes8;
    public GameObject[] nofes9;
    public GameObject[] nofes10;
    public List<Monster> fieldMonsters;

    [Header("보스관련")]
    public GameObject[] bosses;
    public GameObject[] bossPools;

    
void Start()
    {
        InitGame();
    }
    public void InitGame()
    {
        level = 1;
        levelText.text = "Lv." + level.ToString();
        att = linggoLevelDataSO.levelData[level - 1].upAtt;
        attSpeed = linggoLevelDataSO.levelData[level - 1].attSpeed;
        SetMaxHp(linggoLevelDataSO.levelData[level - 1].upHp);
        //DecreaseHP(150);
        currentExp = 0;
        maxExp = linggoLevelDataSO.levelData[level - 1].upKillExp;
        expBar.fillAmount = (float)currentExp / maxExp;
        
        SetWave(1);
        nextWaveMaxKillCnt = linggoLevelDataSO.levelData[wave - 1].waveGoalEnmyCnt;
        nextwaveKillText.text = nextWaveCurrentKillCnt + " / " + nextWaveMaxKillCnt;

        SetGold(0);

        killCnt = 0;
        killCntText.text = killCnt.ToString();
        
        StartGame();
    }

    public void StartGame()
    {
        isStartGame = true;
        if (startGameCour != null)
            StopCoroutine(startGameCour);
        startGameCour = StartGameCour();
        StartCoroutine(startGameCour);
    }

    #region 게임시작(Wave) 코루틴함수

    IEnumerator StartGameCour()
    {
        var t = new WaitForSeconds(0.1f);
        //구슬켜기
        if (createMarbleCour != null)
            StopCoroutine(createMarbleCour);
        createMarbleCour = CreateMarbleCour();
        StartCoroutine(createMarbleCour);

        for (int i = 0; i < linggoLevelDataSO.levelData.Count; i++)
        {
            yield return null;
            //웨이브 바 켜기
            if (waveBarCour != null)
                StopCoroutine(waveBarCour);
            waveBarCour = AnimationWaveBarCour();
            StartCoroutine(waveBarCour);
            //타이머 켜기
            if (timerCour != null)
                StopCoroutine(timerCour);
            timerCour = TimerCour((int)linggoLevelDataSO.levelData[wave-1].waveTime);
            StartCoroutine(timerCour);

            //필드위 몬스터 레벨업
            FieldMonsterLevelUp();
            //몬스터생성
            CreateMonster();
            //보스생성
            if(wave % 10 == 0)
            {
                if (bossBarCour != null)
                    StopCoroutine(bossBarCour);
                bossBarCour = BossBarCour();
                StartCoroutine(bossBarCour);

                bosses[(wave / 10)-1].SetActive(true);
            }
            yield return new WaitUntil(()=> (isEndTimer || isWaveGoalComplete));            //타이머가 종료되거나 미션 성공할때까지

            //초기화(웨이브++, 웨이브 미션킬 초기화)
            NextWave();
        }
    }
    #endregion
    #region 보스 바 애니메이션
    IEnumerator BossBarCour()
    {
        var t = new WaitForSeconds(0.1f);
        bossBar.SetActive(true);
        bossBar.GetComponent<Image>().DOFade(1, 0.1f);
        for (int j = 0; j < 6; j++) yield return t;
        bossBar.GetComponent<Image>().DOFade(0, 0.5f);
        for (int j = 0; j < 5; j++) yield return t;
        bossBar.SetActive(false);
    }
    #endregion
    #region 구슬 시스템
    IEnumerator CreateMarbleCour()
    {
        var t = new WaitForSeconds(0.1f);

        for (int i = 0; i < marbles.Length; i++)
        {
            int createTime = Random.Range(0, 6);

            for (int j = 0; j < createTime * 10; j++) yield return t;
            marbles[i].SetActive(true);
        }
    }
    public void SettingMarbleExp(int plusExp)
    {
        currentMarbleExp += plusExp;
        if(currentMarbleExp >= maxMarbleExp)
        {
            SkillCardController.Inst.OnPopUpUI();
            int remindExp = currentMarbleExp - maxMarbleExp;
            currentMarbleExp = remindExp;
        }
        marbleExpBar.fillAmount = (float)currentMarbleExp / maxMarbleExp;
    
    }
    #endregion

    public void FieldMonsterLevelUp()
    {
        for (int i = 0; i < fieldMonsters.Count; i++)
        {
            if (!fieldMonsters[i].gameObject.activeSelf) fieldMonsters.RemoveAt(i);
        }
        for (int i = 0; i < fieldMonsters.Count; i++)
        {
            fieldMonsters[i].LevelUpEffect();
            //fieldMonsters[i].LevelUp(); //이펙트 제거
        }
    }

    #region 웨이브 바 애니메이션
    IEnumerator AnimationWaveBarCour()
    {
        var t = new WaitForSeconds(0.1f);
        nextWaveBar.SetActive(true);
        nextWaveBarWaveText.text = wave.ToString()+" wave";
        nextWaveBarWaveExText.text = "적군 체력, 공격력 증가";
        nextWaveBar.GetComponent<Image>().DOFade(1, 0.1f);
        nextWaveBarWaveExText.DOFade(1, 0.1f);
        nextWaveBarWaveText.DOFade(1, 0.1f);
        for (int j = 0; j < 6; j++) yield return t;
        nextWaveBar.GetComponent<Image>().DOFade(0, 0.5f);
        nextWaveBarWaveExText.DOFade(0, 0.5f);
        nextWaveBarWaveText.DOFade(0, 0.5f);
        for (int j = 0; j < 5; j++) yield return t;
        nextWaveBar.SetActive(false);
    }
    #endregion
    #region 타이머
    IEnumerator TimerCour(int timer)
    {
        isEndTimer = false;
           var t = new WaitForSeconds(0.1f);
        int time = timer;
        for (int i = 0; i < timer; i++)
        {
            nextwaveTimeText.text = time.ToString();
            for (int j = 0; j < 10; j++) yield return t;
            time--;
        }
        if (time <= 0)
        {
            isEndTimer = true;
        }

    }
    #endregion

    #region Wave 함수
    public void SetWave(int w)
    {
        wave = w;
        waveText.text = wave.ToString();
        nextWaveCurrentKillCnt = 0;
        nextWaveMaxKillCnt = linggoLevelDataSO.levelData[wave - 1].waveGoalEnmyCnt;
        nextwaveKillText.text = nextWaveCurrentKillCnt + " / " + nextWaveMaxKillCnt;

    }
    public void NextWave()
    {
        isWaveGoalComplete = false;
        isEndTimer = false;
           wave++;
        waveText.text = wave.ToString();
        nextWaveCurrentKillCnt = 0;
        nextWaveMaxKillCnt = linggoLevelDataSO.levelData[wave - 1].waveGoalEnmyCnt;
        nextwaveKillText.text = nextWaveCurrentKillCnt + " / " + nextWaveMaxKillCnt;

    }
    #endregion
    #region 골드 함수
    public void SetGold(int setG)
    {
        gold = setG;
        goldText.text = gold.ToString();
    }
    public void IncreaseGold(int plusG)
    {
        gold += plusG;
        goldText.text = gold.ToString();
    }
    public void DecreaseGold(int minusG)
    {
        gold -= minusG;
        goldText.text = gold.ToString();
    }
    #endregion
    #region HP 함수
    //세팅함수
    public void InitHpBar()
    {
        hpBar.fillAmount = 1;
        hpText.text = currentHP + " / " + maxHP;
    }

    //현재HP 감소(일반)
    public void DecreaseHP(int decreaseHp)
    {
        if (linggo.linggoState == Linggo.LinggoState.sheild) return;
        linggo.ChangeColorEffect(Color.red);

        currentHP -= decreaseHp;
        GameObject damageT = Instantiate(damageTPrefab, linggo.transform.position, Quaternion.identity);
        damageT.GetComponent<DamageText>().SetDecreaseText(decreaseHp.ToString());

        if (currentHP < 0)
        {
            currentHP = 0;
            Instantiate(deathPrefab, linggo.transform.position, Quaternion.identity);

            //게임오버 관련함수
            print("게임오버_일반");
        }
        hpBar.fillAmount = (float)currentHP / maxHP;
        hpText.text = currentHP + " / " + maxHP;

    }
    //현재HP 감소(도트)
    public void DotDecreaseHP(int decreaseHp)
    {
        if (linggo.linggoState == Linggo.LinggoState.sheild) return;
        linggo.ChangeColorEffect(Color.red);

        currentHP -= decreaseHp;
        GameObject damageT = Instantiate(dotDamageTPrefab, linggo.transform.position, Quaternion.identity);
        damageT.GetComponent<DamageText>().SetDecreaseText(decreaseHp.ToString());

        if (currentHP < 0)
        {
            currentHP = 0;
            Instantiate(deathPrefab, linggo.transform.position, Quaternion.identity);

            //게임오버 관련함수
            print("게임오버_도트");
        }
        hpBar.fillAmount = (float)currentHP / maxHP;
        hpText.text = currentHP + " / " + maxHP;

    }
    //현재HP 감소(크리티컬)
    public void CriticalDecreaseHP(int decreaseHp)
    {
        if (linggo.linggoState == Linggo.LinggoState.sheild) return;
        linggo.ChangeColorEffect(Color.red);

        currentHP -= decreaseHp;
        GameObject damageT = Instantiate(criticalDamageTPrefab, linggo.transform.position, Quaternion.identity);
        damageT.GetComponent<DamageText>().SetDecreaseText(decreaseHp.ToString());

        if (currentHP < 0)
        {
            currentHP = 0;
            Instantiate(deathPrefab, linggo.transform.position, Quaternion.identity);

            //게임오버 관련함수
            print("게임오버_크리티컬");

        }
        hpBar.fillAmount = (float)currentHP / maxHP;
        hpText.text = currentHP + " / " + maxHP;

    }
    //현재HP 증가
    public void IncreaseHP(int increaseHp)
    {
        GameObject damageT = Instantiate(healTPrefab, linggo.transform.position, Quaternion.identity);
        damageT.GetComponent<DamageText>().SetIncreaseText(increaseHp.ToString());
        currentHP += increaseHp;
        
        if (currentHP > maxHP) currentHP = maxHP;
        hpBar.fillAmount = (float)currentHP / maxHP;
        hpText.text = currentHP + " / " + maxHP;

    }

    //현재HP 변경
    public void SetCurrentHp(int changeValue)
    {
        currentHP = changeValue;
        if (currentHP > maxHP) currentHP = maxHP;
        if (currentHP < 0) currentHP = 0;
        hpBar.fillAmount = (float)currentHP / maxHP;
        hpText.text = currentHP + " / " + maxHP;
    }

    //MAXHP 변경(레벨업할때)
    public void SetMaxHp(int changeValue)
    {
        maxHP = changeValue;
        currentHP = maxHP;
        hpBar.fillAmount = 1;
        hpText.text = currentHP + " / " + maxHP;

    }
    #endregion
    #region 레벨 함수

    public void LevelUp()
    {
        level++;
        levelText.text = "Lv." + level.ToString();

        //레벨업 후 다음 레벨 경험치 정의해야될 듯
        currentExp = 0;
        maxExp = linggoLevelDataSO.levelData[level - 1].upKillExp;
        expBar.fillAmount = (float)currentExp / maxExp;

        att = linggoLevelDataSO.levelData[level - 1].upAtt;
        attSpeed = linggoLevelDataSO.levelData[level - 1].attSpeed;

        SetMaxHp(linggoLevelDataSO.levelData[level - 1].upHp);
    }
    #endregion
    #region 킬 카운트
    public void PlusKillCnt()
    {
        killCnt++;
        currentExp++;
        nextWaveCurrentKillCnt++;
        if (nextWaveCurrentKillCnt >= nextWaveMaxKillCnt) isWaveGoalComplete = true;
        nextwaveKillText.text = nextWaveCurrentKillCnt + " / " + nextWaveMaxKillCnt;

        killCntText.text = killCnt.ToString();

        //IncreaseGold(linggoLevelDataSO.levelData[wave - 1].killRewardGold);

        if(currentExp == maxExp)
        {
            linggo.LevelUpEffect();
        }
        expBar.fillAmount = (float)currentExp / maxExp;
    }
    #endregion

    #region 몬스터 생성
    public void CreateMonster()
    {
        int nofe1 = monsterAppearanceLevelDataSO.monsterAppearanceLevelData[wave - 1].nofe1;
        int nofe2 = monsterAppearanceLevelDataSO.monsterAppearanceLevelData[wave - 1].nofe2;
        int nofe3 = monsterAppearanceLevelDataSO.monsterAppearanceLevelData[wave - 1].nofe3;
        int nofe4 = monsterAppearanceLevelDataSO.monsterAppearanceLevelData[wave - 1].nofe4;
        int nofe5 = monsterAppearanceLevelDataSO.monsterAppearanceLevelData[wave - 1].nofe5;
        int nofe6 = monsterAppearanceLevelDataSO.monsterAppearanceLevelData[wave - 1].nofe6;
        int nofe7 = monsterAppearanceLevelDataSO.monsterAppearanceLevelData[wave - 1].nofe7;
        int nofe8 = monsterAppearanceLevelDataSO.monsterAppearanceLevelData[wave - 1].nofe8;
        int nofe9 = monsterAppearanceLevelDataSO.monsterAppearanceLevelData[wave - 1].nofe9;
        int nofe10 = monsterAppearanceLevelDataSO.monsterAppearanceLevelData[wave - 1].nofe10;
        if(nofe1 > 0)
            StartCoroutine(CreateMonsterCour(nofes1, nofe1));
        if(nofe2 > 0)
            StartCoroutine(CreateMonsterCour(nofes2, nofe2));
        if (nofe3 > 0)
            StartCoroutine(CreateMonsterCour(nofes3, nofe3));
        if (nofe4 > 0)
            StartCoroutine(CreateMonsterCour(nofes4, nofe4));
        if (nofe5 > 0)
            StartCoroutine(CreateMonsterCour(nofes5, nofe5));
        if (nofe6 > 0)
            StartCoroutine(CreateMonsterCour(nofes6, nofe6));
        if (nofe7 > 0)
            StartCoroutine(CreateMonsterCour(nofes7, nofe7));
        if (nofe8 > 0)
            StartCoroutine(CreateMonsterCour(nofes8, nofe8));
        if (nofe9 > 0)
            StartCoroutine(CreateMonsterCour(nofes9, nofe9));
        if (nofe10 > 0)
            StartCoroutine(CreateMonsterCour(nofes10, nofe10));
    }

    public IEnumerator CreateMonsterCour(GameObject[] nofe,int nofeCnt)
    {
        print("nofecnt : " + nofeCnt);
        for (int i = 0; i < nofe.Length; i++)
        {
            if(nofeCnt > 0)
            {
                if (!nofe[i].activeSelf)
                {
                    int ranPos = Random.Range(0, monsterCreatePos.Length);
                    nofe[i].SetActive(true);
                    nofe[i].transform.position = monsterCreatePos[ranPos].position;
                    nofe[i].GetComponent<SpriteRenderer>().sortingOrder = monsterCreatePos[ranPos].GetComponent<SpriteRenderer>().sortingOrder;
                    nofe[i].GetComponent<Monster>().InitMonster();
                    nofeCnt--;
                    fieldMonsters.Add(nofe[i].GetComponent<Monster>());
                    yield return new WaitForSeconds(Random.Range(0.5f, 2.1f));
                }
            }            
        }
        yield return null;
    }
    #endregion

    #region 가이드 창 열기
    public void OpenGuidePop(string guideT)
    {
        guidePop.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = guideT;
        guidePop.SetActive(true);
    }
    #endregion

    public int[] GetRandomInt(int length, int min, int max)
    {
        int[] randArray = new int[length];
        bool isSame;

        for (int i = 0; i < length; ++i)
        {
            while (true)
            {
                randArray[i] = Random.Range(min, max);
                isSame = false;

                for (int j = 0; j < i; ++j)
                {
                    if (randArray[j] == randArray[i])
                    {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame) break;
            }
        }
        return randArray;
    }
}
