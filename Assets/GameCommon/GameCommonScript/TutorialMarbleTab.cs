using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class TutorialMarbleTab : MonoBehaviour
{
    public Sprite[] marbleSprs;
    [HideInInspector]
    public Image marbleImg;
    public int tapCnt;
    public int tapEx;
    public bool[] tapCheck;
    public float fadeTime;
    public int minCreateTime;
    public int maxCreateTime;
    public GameObject marbleTab;
    [HideInInspector]
    public GameObject[] tapEffect = new GameObject[6];
    [HideInInspector]
    public GameObject exEffect;
    public IEnumerator tabCour;
    public IEnumerator createCycleCour;
    [HideInInspector]
    public AudioClip[] audioClip;
    private Vector3 oriPos;


    private void Awake()
    {
        oriPos = this.transform.position;
        marbleTab = this.transform.GetChild(0).gameObject;

        marbleImg = marbleTab.GetComponent<Image>();

        for (int i = 0; i < tapEffect.Length; i++)
            tapEffect[i] = this.transform.GetChild(0).GetChild(i).gameObject;
        exEffect = this.transform.GetChild(0).GetChild(6).gameObject;

        fadeTime = 99999999999999999.0f;

    }



    public IEnumerator CreateCycleCour()
    {
        var t = new WaitForSeconds(1.0f);
        int createTime = Random.Range(minCreateTime, maxCreateTime + 1);

        for (int j = 0; j < createTime; j++) yield return t;
        marbleImg.enabled = true;
        marbleTab.SetActive(true);
    }

    public void OnMarble()
    {

        this.transform.position = oriPos;
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        int ranNum = Random.Range(0, 100);
        if (ranNum < 1) tapCnt = 3;
        else if (ranNum < 6) tapCnt = 2;
        else tapCnt = 1;
        //tapCnt = Random.Range(1, 4);
        marbleImg.sprite = marbleSprs[tapCnt - 1];
        tapEffect[tapCnt + 2].SetActive(true);
        tapEx = 0;

        if (tabCour != null)
            StopCoroutine(tabCour);
        tabCour = TabCour();
        StartCoroutine(tabCour);
    }

    public void TabBtn()
    {
        if (tapCnt > 0 && tapCnt != tapEx)
        {
            //audioSource.PlayOneShot(audioClip[0]);
            if (audioClip.Length > 0)
                SoundManager.Inst.SFXPlay("MarbleTab", audioClip[0]);
            tapCheck[tapEx] = true;
            tapEffect[tapEx].SetActive(true);
            tapEx++;
            if (tapCnt == tapEx)
            {
                //audioSource.PlayOneShot(audioClip[1]);
                if (audioClip.Length > 0)
                    SoundManager.Inst.SFXPlay("MarbleTab", audioClip[1]);
                tapEffect[tapEx + 2].SetActive(false);
                this.transform.DOShakeRotation(1.0f, 5);
                this.transform.DOShakePosition(1.0f, 1, 0).OnComplete(() =>
                {
                    this.marbleImg.enabled = false;
                    exEffect.SetActive(true);
                });

                TutorialController.Inst.SettingMarbleExp(tapEx);
                TutorialController.Inst.marbleExpEffect.SetActive(true);
                Invoke(nameof(SetFalse), 2.0f);
            }
            else
            {


                this.transform.DOShakePosition(1.0f, 10);
                this.transform.DOShakeRotation(1.0f, 5);

                //if (tabCour != null)
                //    StopCoroutine(tabCour);
                //tabCour = TabCour();
                //StartCoroutine(tabCour);
            }

        }
    }

    public void SetFalse()
    {
        this.DOKill();

        marbleTab.SetActive(false);
        EndInit();
    }


    public void EndInit()
    {
        this.DOKill();
        exEffect.SetActive(false);
        for (int i = 0; i < tapCheck.Length; i++)
        {
            tapCheck[i] = false;
        }

        for (int i = 0; i < tapEffect.Length; i++)
        {
            tapEffect[i].SetActive(false);
        }
        if (tabCour != null)
            StopCoroutine(tabCour);

        //if (createCycleCour != null)
        //    StopCoroutine(createCycleCour);
        //createCycleCour = CreateCycleCour();
        //StartCoroutine(createCycleCour);
    }
    IEnumerator TabCour()
    {
        var t = new WaitForSeconds(1.0f);

        for (int i = 0; i < fadeTime; i++) yield return t;
        marbleTab.SetActive(false);
        EndInit();
    }
}
