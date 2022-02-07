using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
//유닛 참조
public class Linggo : MonoBehaviour
{
    public enum LinggoState
    {
        move = 0,   //일반
        stun = 1,   //스턴
        sheild = 2, //무적
        bossDbuff = 3
    }
   
    public LinggoState linggoState = LinggoState.move;
    [HideInInspector]
    public Animator linggoAni;
    public List<Missile> linggoMissiles = new List<Missile>();

    [Header("공격관련 변수")]
    public GameObject target;
    [HideInInspector]
    public bool isAttacking = false;
    IEnumerator attackCour;

    [Header("상태관련 변수")]
    public BGController bgController_top;
    public BGController bgController_bottom;
    [HideInInspector]
    public float saveBgTopSpeed;
    [HideInInspector]
    public float saveBgBottomSpeed;
    [HideInInspector]
    public SpriteRenderer linggoImg;

    private IEnumerator changeColorCour;
    private IEnumerator slowCour;
    private IEnumerator stunCour;
    private IEnumerator dotCour;
    private IEnumerator levelUpCour;
    private IEnumerator shieldCour;

    [Header("이펙트 오브젝트")]
    [HideInInspector]
    public GameObject hitEffect;
    [HideInInspector]
    public GameObject slowEffect;
    [HideInInspector]
    public GameObject stunEffect;
    [HideInInspector]
    public GameObject dotEffect;
    [HideInInspector]
    public GameObject levelUpEffect;
    [HideInInspector]
    public TextMeshPro levelUpTObj;
    [HideInInspector]
    public GameObject shieldEffect;
    [HideInInspector]
    public GameObject iceStunEffect;
    [HideInInspector]
    public GameObject LightningEffect;
    [HideInInspector]
    public GameObject bossDBuffEffect;
    [HideInInspector]
    public GameObject fireDotEffect;

    //public GameObject goldPrefab;

    void Awake()
    {
        SettingMissile();
        SettingEffect();
        saveBgTopSpeed = bgController_top.moveSpeed;
        saveBgBottomSpeed = bgController_bottom.moveSpeed;
        linggoAni = this.GetComponent<Animator>();
        linggoImg = this.GetComponent<SpriteRenderer>();
        linggoAni.SetBool("isMove", true);
    }
    private void Start()
    {
        if (attackCour != null)
            StopCoroutine(attackCour);
        attackCour = ATTACK();
        StartCoroutine(attackCour);
    }
    #region 이펙트세팅
    public void SettingEffect()
    {
        stunEffect = this.transform.GetChild(1).gameObject;
        slowEffect = this.transform.GetChild(2).gameObject;
        dotEffect = this.transform.GetChild(3).gameObject;
        levelUpEffect = this.transform.GetChild(4).gameObject;
        levelUpTObj = levelUpEffect.transform.GetChild(1).GetComponent<TextMeshPro>();
        shieldEffect = this.transform.GetChild(5).gameObject;
        iceStunEffect = this.transform.GetChild(6).gameObject;
        LightningEffect = this.transform.GetChild(7).gameObject;
        bossDBuffEffect = this.transform.GetChild(8).gameObject;
        fireDotEffect = this.transform.GetChild(9).gameObject;
    }
    #endregion

    #region 미사일 세팅 및 미사일 생성(공격)
    public void SettingMissile()
	{
        Transform mPool = this.transform.GetChild(0);
        for (int i = 0; i < mPool.childCount; i++)
        {
            linggoMissiles.Add(mPool.GetChild(i).GetComponent<Missile>());
        }
    }

    public IEnumerator ATTACK(/*GameObject e*/)
    {
        while (true)
        {
            if (target == null)
                target = FindNearestObjectByTag("Enemy");
            if (target != null && target.activeSelf)
            {
                for (int i = 0; i < linggoMissiles.Count; i++)
                {
                    if (!linggoMissiles[i].gameObject.activeSelf)
                    {
                        linggoMissiles[i].SettingTarget(target);
                        break;
                    }
                }
            }
            else
            {
                target = FindNearestObjectByTag("Enemy");
            }
            yield return new WaitForSeconds(GameController.Inst.attSpeed);

        }


    }
    #endregion
    #region 가까운 적 찾는 함수
    public GameObject FindNearestObjectByTag(string tag)
    {
        // 탐색할 오브젝트 목록을 List 로 저장합니다.
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();

        // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
        var neareastObject = objects
            .OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject;
    }
    #endregion
    #region 몬스터 상태 관련 함수
    public void ChangeState(LinggoState state)
    {
        linggoState = state;
        linggoImg.color = Color.white;

        switch (linggoState)
        {
            case LinggoState.stun:
                linggoAni.speed = 0;
                linggoAni.SetBool("Attack", false);
                break;
            case LinggoState.move:
                linggoAni.speed = 1;
                linggoAni.SetBool("Attack", false);
                bgController_bottom.moveSpeed = saveBgBottomSpeed;
                bgController_top.moveSpeed = saveBgTopSpeed;
                break;
            case LinggoState.sheild:
                if (slowCour != null)
                    StopCoroutine(slowCour);
                slowEffect.SetActive(false);                
                if (stunCour != null)
                    StopCoroutine(stunCour);
                stunEffect.SetActive(false);
                iceStunEffect.SetActive(false);
                LightningEffect.SetActive(false);
                if (dotCour != null)
                    StopCoroutine(dotCour);
                dotEffect.SetActive(false);
                fireDotEffect.SetActive(false);
                break;
        }
    }
    #endregion

    #region 스턴 함수
    public void StunEffect(float time)
    {
        if (linggoState == LinggoState.sheild) return;
        if (!this.gameObject.activeSelf) return;
        if (stunCour != null)
            StopCoroutine(stunCour);
        stunCour = StunEffectCour(time);
        StartCoroutine(stunCour);
    }

    public IEnumerator StunEffectCour(float time)
    {
        var t = new WaitForSeconds(0.1f);
        ChangeState(LinggoState.stun);
        stunEffect.SetActive(true);

        bgController_top.moveSpeed = 0;
        bgController_bottom.moveSpeed = 0;
        for (int i = 0; i < time * 10; i++) yield return t;
        ChangeState(LinggoState.move);

        stunEffect.SetActive(false);
        bgController_top.moveSpeed = saveBgTopSpeed;
        bgController_bottom.moveSpeed = saveBgBottomSpeed;
    }

    public void IceStunEffect(float time)
    {
        if (linggoState == LinggoState.sheild) return;
        if (!this.gameObject.activeSelf) return;
        if (stunCour != null)
            StopCoroutine(stunCour);
        stunCour = IceStunEffectCour(time);
        StartCoroutine(stunCour);
    }

    public IEnumerator IceStunEffectCour(float time)
    {
        var t = new WaitForSeconds(0.1f);
        ChangeState(LinggoState.stun);
        ChangeColorEffect(new Color32(137, 210, 255, 255));
        iceStunEffect.SetActive(true);

        bgController_top.moveSpeed = 0;
        bgController_bottom.moveSpeed = 0;
        for (int i = 0; i < time * 10; i++) yield return t;
        ChangeState(LinggoState.move);

        iceStunEffect.SetActive(false);
        bgController_top.moveSpeed = saveBgTopSpeed;
        bgController_bottom.moveSpeed = saveBgBottomSpeed;
    }

    public void LightningStunEffect(float time)
    {
        if (linggoState == LinggoState.sheild) return;
        if (!this.gameObject.activeSelf) return;
        if (stunCour != null)
            StopCoroutine(stunCour);
        stunCour = LightningStunEffectCour(time);
        StartCoroutine(stunCour);
    }

    public IEnumerator LightningStunEffectCour(float time)
    {
        var t = new WaitForSeconds(0.1f);
        ChangeState(LinggoState.stun);
        LightningEffect.SetActive(true);

        bgController_top.moveSpeed = 0;
        bgController_bottom.moveSpeed = 0;
        for (int i = 0; i < time * 10; i++) yield return t;
        ChangeState(LinggoState.move);

        LightningEffect.SetActive(false);
        bgController_top.moveSpeed = saveBgTopSpeed;
        bgController_bottom.moveSpeed = saveBgBottomSpeed;
    }
    #endregion
    #region 슬로우 함수
    public void SlowEffect(float time, float moveDecreasePercent)
    {
        if (!this.gameObject.activeSelf) return;

        if (slowCour != null)
            StopCoroutine(slowCour);
        slowCour = SlowEffectCour(time, moveDecreasePercent);
        StartCoroutine(slowCour);
    }

    public IEnumerator SlowEffectCour(float time, float moveDecreasePercent = 0.5f)
    {
        var t = new WaitForSeconds(0.1f);
        slowEffect.SetActive(true);

        if (linggoState != LinggoState.stun)
            linggoAni.speed = moveDecreasePercent;

        bgController_top.moveSpeed *= moveDecreasePercent;
        bgController_bottom.moveSpeed *= moveDecreasePercent;
        for (int i = 0; i < time * 10; i++) yield return t;
        slowEffect.SetActive(false);
        bgController_top.moveSpeed = saveBgTopSpeed;
        bgController_bottom.moveSpeed = saveBgBottomSpeed;
    }
    #endregion
    #region 도트 데미지 함수
    public void DotEffect(int time, float damage)
    {
        if (linggoState == LinggoState.sheild) return;
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
            GameController.Inst.DotDecreaseHP(dotDam);
            print("도트뎀 : " + dotDam + " / " + damage);

            yield return t;
        }
        dotEffect.SetActive(false);
    }

    public void FireDotEffect(int time, float damage)
    {
        if (linggoState == LinggoState.sheild) return;
        if (!this.gameObject.activeSelf) return;
        if (dotCour != null)
            StopCoroutine(dotCour);
        dotCour = FireDotEffectCour(time, damage);
        StartCoroutine(dotCour);

    }
    public IEnumerator FireDotEffectCour(int time, float damage)
    {
        var t = new WaitForSeconds(1.0f);
        fireDotEffect.SetActive(true);
        for (int i = 0; i < time; i++)
        {
            int dotDam = (int)(damage / time);
            GameController.Inst.DotDecreaseHP(dotDam);
            print("도트뎀 : " + dotDam + " / " + damage);

            yield return t;
        }
        fireDotEffect.SetActive(false);
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

        linggoImg.color = Color.white;
        for (int i = 0; i < 3; i++) yield return time;
        linggoAni.SetTrigger("Hit");
        linggoImg.color = ChangeColor;
        for (int i = 0; i < 3; i++) yield return time;
        linggoImg.color = Color.white;

    }
    #endregion

    #region 성장(레벨업) 함수
    public void LevelUpEffect()
    {
        if (!this.gameObject.activeSelf) return;

        if (levelUpCour != null)
            StopCoroutine(levelUpCour);
        levelUpCour = LevelUpEffectCour();
        StartCoroutine(levelUpCour);
    }

    public IEnumerator LevelUpEffectCour()
    {
        var t = new WaitForSeconds(0.1f);
        levelUpEffect.SetActive(true);
        levelUpTObj.DOFade(1, 0.1f).OnComplete(() =>
        {
            levelUpTObj.DOFade(0, 2.0f);

        }) ;

        GameController.Inst.LevelUp();
        for (int i = 0; i < 60; i++) yield return t;
        levelUpEffect.SetActive(false);

    }
    #endregion

    #region 무적 함수
    public void ShieldEffect(float time)
    {
        if (!this.gameObject.activeSelf) return;
        if (shieldCour != null)
            StopCoroutine(shieldCour);
        shieldCour = ShieldEffectCour(time);
        StartCoroutine(shieldCour);
    }

    public IEnumerator ShieldEffectCour(float time)
    {
        var t = new WaitForSeconds(0.1f);
        shieldEffect.SetActive(true);
        ChangeState(LinggoState.sheild);

        for (int i = 0; i < time * 10; i++) yield return t;
        shieldEffect.SetActive(false);
        ChangeState(LinggoState.move);
    }
    #endregion


}
