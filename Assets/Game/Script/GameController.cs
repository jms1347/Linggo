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
    [SerializeField]public BossDataSo bossDataSO;
    [SerializeField]public StateLevelDataSo stateLevelDataSO;

    [Header("HP ����")]
    public int maxHP;
    public int currentHP;
    public Image hpBar;
    public TextMeshProUGUI hpText;

    [Header("���� ����")]
    public int level;
    public TextMeshProUGUI levelText;
    public int currentExp;
    public int maxExp;
    public Image expBar;
    public int killCnt;
    public TextMeshProUGUI killCntText;

    [Header("��� ����")]
    public int gold;
    public TextMeshProUGUI goldText;
    
    [Header("���̺� ����")]
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

    [Header("���� ����")]
    public Linggo linggo;
    public int att;
    public float attSpeed;
    public float missileSpeed;

    [Header("���� ����")]
    public Image marbleExpBar;
    public int maxMarbleExp;
    public int currentMarbleExp;
    public GameObject[] marbles;
    public GameObject marbleExpEffect;


    [Header("�� �� ����")]
    public GameObject guidePop;
    public GameObject gameOverPop;
    public GameObject rebirthPop;
    public bool canRebirth = true;
    
    //public GameObject shopIcon;

    public bool isStartGame = false;
    private IEnumerator startGameCour;
    private IEnumerator timerCour;
    private IEnumerator waveBarCour;
    private IEnumerator bossBarCour;
    private IEnumerator createMarbleCour;
    private IEnumerator attCour;
    private IEnumerator doubleGoldCour;
    bool isEndTimer = false;
    bool isWaveGoalComplete = false;

    [Header("������")]
    public GameObject damageTPrefab;
    public GameObject criticalDamageTPrefab;
    public GameObject healTPrefab;
    public GameObject dotDamageTPrefab;
    public GameObject deathPrefab;

    [Header("���Ͱ���")]
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

    [Header("��������")]
    public GameObject[] bosses;
    public GameObject[] bossPools;

    [Header("���Ⱥ������ ����")]
    public int plusHpLevel;
    public int plusAttLevel;
    public int plusMarbleAppearPercentLevel;
    public int plusPenetratingCntLevel;
    public LinggoStatBoard linggoStateBoard;
    public GameObject linggoStateIcon;

    [Header("����")]
    public AudioClip bossWaringSound;
    public AudioClip btnClickSound;

    [Header("������ ��ư")]
    public GameObject doubleGoldADBtn;
    public GameObject FarmingCardADBtn;
    public bool isDoubleGold = false;
    public float doubleGold = 60.0f;



    void Start()
    {
        InitGame();
    }
    public void InitGame()
    {
        SettingBossData();

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

        plusHpLevel = 0;
        plusAttLevel = 0;
        plusMarbleAppearPercentLevel = 0;
        plusPenetratingCntLevel = 0;

        if(!isStartGame)
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

    #region ���ӽ���(Wave) �ڷ�ƾ�Լ�

    IEnumerator StartGameCour()
    {
        var t = new WaitForSeconds(0.1f);
        //�����ѱ�
        if (createMarbleCour != null)
            StopCoroutine(createMarbleCour);
        createMarbleCour = CreateMarbleCour();
        StartCoroutine(createMarbleCour);

        for (int i = 0; i < linggoLevelDataSO.levelData.Count; i++)
        {
            yield return null;
            //���� �ý���
            //if (wave % 5 == 0) linggoStateIcon.SetActive(true);           
            //else linggoStateIcon.SetActive(false);

            

            //��������
            if (wave % 10 == 0)
            {
                if (bossBarCour != null)
                    StopCoroutine(bossBarCour);
                bossBarCour = BossBarCour();
                StartCoroutine(bossBarCour);

                bosses[(wave / 10) - 1].SetActive(true);
            }
            else if(wave != 1)
            {
                //������ �ý���
                int ranR = Random.Range(0, 100);
                if (ranR < 30)
                {
                    int ranK = Random.Range(0, 3);

                    if (ranK == 0) doubleGoldADBtn.SetActive(true);
                    else FarmingCardADBtn.SetActive(true);
                }
                else
                {
                    doubleGoldADBtn.SetActive(false);
                    FarmingCardADBtn.SetActive(false);
                }

                //���̺� �� �ѱ�
                if (waveBarCour != null)
                    StopCoroutine(waveBarCour);
                waveBarCour = AnimationWaveBarCour();
                StartCoroutine(waveBarCour);
            }

            //Ÿ�̸� �ѱ�
            if (timerCour != null)
                StopCoroutine(timerCour);
            timerCour = TimerCour((int)linggoLevelDataSO.levelData[wave-1].waveTime);
            StartCoroutine(timerCour);

            //�ʵ��� ���� ������
            FieldMonsterLevelUp();
            //���ͻ���
            CreateMonster();

            yield return new WaitUntil(()=> (isEndTimer || isWaveGoalComplete || fieldMonsters.Count == 0));            //Ÿ�̸Ӱ� ����ǰų� �̼� �����Ҷ�����

            //�ʱ�ȭ(���̺�++, ���̺� �̼�ų �ʱ�ȭ)
            NextWave();
        }
    }
    #endregion
    #region ���� ������ ����
    public void SettingBossData()
    {
        for (int i = 0; i < bossPools.Length; i++)
        {
            bossPools[i].GetComponent<Boss>().SettingBossData(bossDataSO.bossDatas[i]);
        } 
    }
    #endregion
    #region ���� �� �ִϸ��̼�
    IEnumerator BossBarCour()
    {
        var t = new WaitForSeconds(0.1f);
        bossBar.SetActive(true);
        bossBar.GetComponent<Image>().DOFade(1, 0.1f);
        if (bossWaringSound != null)
            SoundManager.Inst.SFXPlay("BossWaring", bossWaringSound);
        for (int j = 0; j < 6; j++) yield return t;
        bossBar.GetComponent<Image>().DOFade(0, 0.5f);
        for (int j = 0; j < 5; j++) yield return t;
        bossBar.SetActive(false);
    }
    #endregion
    #region ���� �ý���
    IEnumerator CreateMarbleCour()
    {
        var t = new WaitForSeconds(0.1f);

        for (int i = 0; i < marbles.Length; i++)
        {
            int createTime = Random.Range(0, 6);

            for (int j = 0; j < createTime * 10; j++) yield return t;
            int ranPercent = Random.Range(0, 100);
            float Percent = stateLevelDataSO.stateLevelData[plusMarbleAppearPercentLevel].plusAppearPercent;
            if (ranPercent > Percent)
            {
                print(Percent + "% Ȯ���� ��ȭ���� ��������");
            }
            else marbles[i].SetActive(true);
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
        //for (int i = 0; i < fieldMonsters.Count; i++)
        //{
        //    if (!fieldMonsters[i].gameObject.activeSelf) fieldMonsters.RemoveAt(i);
        //}
        for (int i = 0; i < fieldMonsters.Count; i++)
        {
            fieldMonsters[i].LevelUpEffect();
            //fieldMonsters[i].LevelUp(); //����Ʈ ����
        }
    }

    #region ���̺� �� �ִϸ��̼�
    IEnumerator AnimationWaveBarCour()
    {
        var t = new WaitForSeconds(0.1f);
        nextWaveBar.SetActive(true);
        nextWaveBarWaveText.text = wave.ToString()+" wave";
        nextWaveBarWaveExText.text = "���� ü��, ���ݷ� ����";
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
    #region Ÿ�̸�
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

    #region Wave �Լ�
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
    #region ��� �Լ�
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
    #region ���ݷ� �Ͻ��� ���� �Լ�
    public void ChangeAtt(int chageAttValue, float time)
    {
        if (attCour != null)
            StopCoroutine(attCour);
        attCour = AttCour(chageAttValue, time);
        StartCoroutine(attCour);
        
    }

    public IEnumerator AttCour(int chageAttValue, float time)
    {
        var t = new WaitForSeconds(0.1f);
        int beforeAtt = att;
        att += chageAttValue;
        for (int i = 0; i < time * 10; i++) yield return t;
        att = beforeAtt;
    }
    #endregion
    #region HP �Լ�
    //�����Լ�
    public void InitHpBar()
    {
        hpBar.fillAmount = 1;
        hpText.text = currentHP + " / " + maxHP;
    }

    //����HP ����(�Ϲ�)
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

            //���ӿ��� �����Լ�
            DeathLinggo();
        }
        hpBar.fillAmount = (float)currentHP / maxHP;
        hpText.text = currentHP + " / " + maxHP;

    }
    //����HP ����(��Ʈ)
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

            //���ӿ��� �����Լ�
            DeathLinggo();
        }
        hpBar.fillAmount = (float)currentHP / maxHP;
        hpText.text = currentHP + " / " + maxHP;

    }
    //����HP ����(ũ��Ƽ��)
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

            //���ӿ��� �����Լ�
            DeathLinggo();
        }
        hpBar.fillAmount = (float)currentHP / maxHP;
        hpText.text = currentHP + " / " + maxHP;

    }
    //����HP ����
    public void IncreaseHP(int increaseHp)
    {
        GameObject damageT = Instantiate(healTPrefab, linggo.transform.position, Quaternion.identity);
        damageT.GetComponent<DamageText>().SetIncreaseText(increaseHp.ToString());
        currentHP += increaseHp;
        
        if (currentHP > maxHP) currentHP = maxHP;
        hpBar.fillAmount = (float)currentHP / maxHP;
        hpText.text = currentHP + " / " + maxHP;

    }

    //����HP ����
    public void SetCurrentHp(int changeValue)
    {
        currentHP = changeValue;
        if (currentHP > maxHP) currentHP = maxHP;
        if (currentHP < 0) currentHP = 0;
        hpBar.fillAmount = (float)currentHP / maxHP;
        hpText.text = currentHP + " / " + maxHP;
    }

    //MAXHP ����(�������Ҷ�)
    public void SetMaxHp(int changeValue)
    {
        maxHP = changeValue;
        currentHP = maxHP;
        hpBar.fillAmount = 1;
        hpText.text = currentHP + " / " + maxHP;
        hpBar.fillAmount = (float)currentHP / maxHP;
        hpText.text = currentHP + " / " + maxHP;
    }

    public void PlusMaxHp(int changeValue)
    {
        int plusValue = changeValue - maxHP;
        maxHP += plusValue;
        currentHP += plusValue;
        hpBar.fillAmount = (float)currentHP / maxHP;
        hpText.text = currentHP + " / " + maxHP;
    }
    #endregion
    #region ���� �Լ�
    public void PlusAtt()
    {
        att = linggoLevelDataSO.levelData[level - 1].upAtt + stateLevelDataSO.stateLevelData[plusAttLevel].plusAtt;
    }
    public void PlusHp()
    {
        PlusMaxHp(linggoLevelDataSO.levelData[level - 1].upHp + stateLevelDataSO.stateLevelData[plusHpLevel].plusHp);
    }
    public void PlusPenetratingCnt()
    {
        for (int i = 0; i < linggo.linggoMissiles.Count; i++)
        {
            linggo.linggoMissiles[i].penetrateCnt =stateLevelDataSO.stateLevelData[plusPenetratingCntLevel].penetratingCnt;
        }
    }
    public void LevelUp()
    {
        level++;
        levelText.text = "Lv." + level.ToString();

        //������ �� ���� ���� ����ġ �����ؾߵ� ��
        currentExp = 0;
        maxExp = linggoLevelDataSO.levelData[level - 1].upKillExp;
        expBar.fillAmount = (float)currentExp / maxExp;

        att = linggoLevelDataSO.levelData[level - 1].upAtt + stateLevelDataSO.stateLevelData[plusAttLevel].plusAtt;

        attSpeed = linggoLevelDataSO.levelData[level - 1].attSpeed;

        SetMaxHp(linggoLevelDataSO.levelData[level - 1].upHp + stateLevelDataSO.stateLevelData[plusHpLevel].plusHp);
    }
    #endregion
    #region ų ī��Ʈ
    public void PlusKillCnt()
    {
        killCnt++;

        //���� üũ
        CheckAchievements();

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
    #region ų ���� üũ
    public void CheckAchievements()
    {
        switch (killCnt)
        {
            case 1251 :                
                //break;
            case 2503:
                //break;
            case 3754:
                //break;
            case 5005:
                //break;
            case 7508:
                //break;
            case 10010:
                //break;
            case 12513:
                //break;
            case 15015:
                //break;
            case 17518:
                //break;
            case 20020:
                //break;
            case 22523:
                //break;
            case 25025:
                Social.ReportProgress(GPGSIds.achievement, 1, (bool isSuccess) =>
                {
                    print("���� : "+killCnt);
                    ShowAchievements();
                });
                break;

                
        }
    }

    public void ShowAchievements()
    {
        if (!Social.localUser.authenticated)
        {
            return;
        }
        Social.ShowAchievementsUI();
    }
    #endregion

    #region ���� ����
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
        //print("nofecnt : " + nofeCnt);
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

    #region ���̵� â ����
    public void OpenGuidePop(string guideT)
    {
        guidePop.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = guideT;
        guidePop.SetActive(true);
    }
    #endregion

    #region ���� ���(����)_���ӿ���
    public void DeathLinggo()
    {
        if (canRebirth)
            rebirthPop.SetActive(true);
        else
            gameOverPop.SetActive(true);
        Time.timeScale = 0;
    }
    public void GameOver()
    {
        Time.timeScale = 1;
        InputLeaderBoard(killCnt);
        LoadingScene.LoadScene("MainScene");
    }

    #endregion
    #region �����Է�(��������)
    public void InputLeaderBoard(int kill)
    {
        GPGSBinder.Inst.ReportLeaderboard(GPGSIds.leaderboard, kill, (bool isSuccess) =>
        {
            print("�������� �Է� : " + killCnt);
        });
    }
    #endregion
    #region ���� ��� �Լ�
    public void DoubleGold()
    {
        if (doubleGoldCour != null)
            StopCoroutine(doubleGoldCour);
        doubleGoldCour = DoubleGoldCour();
        StartCoroutine(doubleGoldCour);
    }

    IEnumerator DoubleGoldCour()
    {
        var t = new WaitForSeconds(1f);
        isDoubleGold = true;
        for (int i = 0; i < doubleGold; i++) yield return t;
        isDoubleGold = false;
    }
    #endregion
    public void SettingRebirth()
    {
        Time.timeScale = 1;

        linggo.ShieldEffect(3.0f);
        canRebirth = false;
        for (int i = 0; i < fieldMonsters.Count; i++)
        {
            fieldMonsters[i].gameObject.SetActive(false);
        }
        fieldMonsters.Clear();

        IncreaseHP(maxHP);
        rebirthPop.SetActive(false);
    }

    #region ����
    public void ClickSound()
    {
        if (btnClickSound != null)
            SoundManager.Inst.SFXPlay("BtnSound", btnClickSound);
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
