using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MarbleTab : MonoBehaviour
{
    public Sprite[] marbleSprs;
    private Image marbleImg;
    public int tapCnt;
    public int tapEx;
    public bool[] tapCheck;
    public float fadeTime;
    private GameObject[] tapEffect = new GameObject[6];
    private GameObject exEffect;
    IEnumerator tabCour;
    private AudioSource audioSource;
    public AudioClip[] audioClip;

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
           marbleImg = this.GetComponent<Image>();

        for (int i = 0; i < tapEffect.Length; i++) 
            tapEffect[i] = this.transform.GetChild(i).gameObject;
        exEffect = this.transform.GetChild(6).gameObject;
    }


    private void OnEnable()
    {
        fadeTime = 3.0f;
        tapCnt = Random.Range(1,4);
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
        if(tapCnt > 0 && tapCnt != tapEx)
        {
               tapCheck[tapEx] = true;
            tapEffect[tapEx].SetActive(true);
            tapEx++;
            if(tapCnt == tapEx)
            {
                audioSource.PlayOneShot(audioClip[1]);

                tapEffect[tapEx + 2].SetActive(false);
                this.transform.DOShakeRotation(1.0f, 5);
                this.transform.DOShakePosition(1.0f, 1, 0).OnComplete(()=>
                {
                    this.marbleImg.enabled = false;
                    exEffect.SetActive(true);
                });

                GameController.Inst.SettingMarbleExp(tapEx);
                Invoke(nameof(SetFalse), 2.0f);
            }
            else
            {
                audioSource.PlayOneShot(audioClip[0]);

                this.transform.DOShakePosition(1.0f, 10);
                this.transform.DOShakeRotation(1.0f, 5);

                if (tabCour != null)
                    StopCoroutine(tabCour);
                tabCour = TabCour();
                StartCoroutine(tabCour);
            }

        }
    }

    public void SetFalse()
    {
        this.gameObject.SetActive(false);
        this.DOKill();
    }

    public void OnDisable()
    {
        this.DOKill();
        this.marbleImg.enabled = true;
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
    }

    IEnumerator TabCour()
    {
        var t = new WaitForSeconds(1.0f);
        
        for (int i = 0; i < fadeTime; i++) yield return t;
        this.gameObject.SetActive(false);
    }

}
