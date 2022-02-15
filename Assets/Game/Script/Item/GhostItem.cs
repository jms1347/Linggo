using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostItem : MonoBehaviour
{
    public int maxHp;
    public int currentHp;

    [HideInInspector]
    public Transform hpBar;
    [HideInInspector]
    public GameObject maxHpBar;

    protected IEnumerator changeColorCour;
    public SpriteRenderer monsterImg;

    protected IEnumerator dotCour;

    [HideInInspector]
    public GameObject dotEffect;

    [Header("프리팹")]
    public GameObject deathPrefab;
    public GameObject damageTPrefab;
    public GameObject dotDamageTPrefab;
    public GameObject healTPrefab;
    public GameObject criticalDamageTPrefab;

    public int CurrentHp
    {
        get => currentHp;
        set
        {
            if (currentHp > 0)
            {
                currentHp = value;
                hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
            }
            else
            {
                currentHp = 0;

            }
        }
    }

    void Awake()
    {
        monsterImg = this.GetComponent<SpriteRenderer>();

        SettingHpMpBar();
        SettingEffect();
        this.gameObject.SetActive(false);
    }


    public virtual void SettingEffect()
    {
        dotEffect = this.transform.GetChild(1).gameObject;
    }

    public void InitUnit(int hp)
    {
        SetMaxHp(hp);
        this.gameObject.SetActive(true);
    }
    #region HP 관련 함수
    public void PlusMaxHp(int changeValue)
    {
        int plusValue = changeValue - maxHp;
        maxHp += plusValue;
        currentHp += plusValue;
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }
    public void SetMaxHp(int changeValue)
    {
        maxHpBar.SetActive(false);
        maxHp = changeValue;
        currentHp = maxHp;
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }

    public void SettingHpMpBar()
    {
        maxHpBar = this.transform.GetChild(0).gameObject;
        hpBar = this.transform.GetChild(0).GetChild(0);
        if (maxHp == 0) hpBar.transform.parent.gameObject.SetActive(false);

        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }
    public void CriticalDecreaseHP(int decreaseHp)
    {
        ChangeColorEffect(Color.red);

        maxHpBar.SetActive(true);
        currentHp -= decreaseHp;
        GameObject damageT = Instantiate(criticalDamageTPrefab, this.transform.position, Quaternion.identity);
        damageT.GetComponent<DamageText>().SetDecreaseText(decreaseHp.ToString());
        if (currentHp < 0)
        {
            currentHp = 0;
            Instantiate(deathPrefab, this.transform.position, Quaternion.identity);
            this.gameObject.SetActive(false);
        }
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }
    public void DecreaseHP(int decreaseHp)
    {
        ChangeColorEffect(Color.red);

        maxHpBar.SetActive(true);
        currentHp -= decreaseHp;
        GameObject damageT = Instantiate(damageTPrefab, this.transform.position, Quaternion.identity);
        damageT.GetComponent<DamageText>().SetDecreaseText(decreaseHp.ToString());
        if (currentHp < 0)
        {
            currentHp = 0;
            Instantiate(deathPrefab, this.transform.position, Quaternion.identity);
            this.gameObject.SetActive(false);
        }
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }


    public void DotDecreaseHP(int decreaseHp)
    {
        ChangeColorEffect(Color.red);

        maxHpBar.SetActive(true);
        currentHp -= decreaseHp;
        GameObject damageT = Instantiate(dotDamageTPrefab, this.transform.position, Quaternion.identity);
        damageT.GetComponent<DamageText>().SetDecreaseText(decreaseHp.ToString());
        if (currentHp < 0)
        {
            currentHp = 0;
            Instantiate(deathPrefab, this.transform.position, Quaternion.identity);
            this.gameObject.SetActive(false);
        }
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }


    public void IncreaseHP(int increaseHp)
    {
        currentHp += increaseHp;
        GameObject damageT = Instantiate(healTPrefab, this.transform.position, Quaternion.identity);
        damageT.GetComponent<DamageText>().SetIncreaseText(increaseHp.ToString());

        if (currentHp > maxHp) currentHp = maxHp;
        hpBar.localScale = new Vector3((float)currentHp / maxHp, 1, 1);
    }
    #endregion

    #region 색깔 변경 코루틴
    public void ChangeColorEffect(Color color)
    {
        if (!this.gameObject.activeSelf) return;

        if (changeColorCour != null)
            StopCoroutine(changeColorCour);
        changeColorCour = ColorCour(color);
        StartCoroutine(changeColorCour);
    }
    public IEnumerator ColorCour(Color ChangeColor)
    {
        var time = new WaitForSeconds(0.1f);

        monsterImg.color = Color.white;
        for (int i = 0; i < 3; i++) yield return time;
        monsterImg.color = ChangeColor;
        for (int i = 0; i < 3; i++) yield return time;
        monsterImg.color = Color.white;

    }
    #endregion

    #region 도트 데미지 함수
    public void DotEffect(int time, float damage)
    {
        if (!this.gameObject.activeSelf) return;
        if (dotCour != null)
            StopCoroutine(dotCour);
        dotCour = DotEffectCour(time, damage);
        StartCoroutine(dotCour);

    }
    public IEnumerator DotEffectCour(int time, float damage)
    {
        var t = new WaitForSeconds(1.0f);
        dotEffect.SetActive(true);
        for (int i = 0; i < time; i++)
        {
            int dotDam = (int)(damage / time);
            DotDecreaseHP(dotDam);
            //print("도트뎀 : " + dotDam + " / " + damage);

            yield return t;
        }
        dotEffect.SetActive(false);
    }
    #endregion
}
