using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StorySceneController : MonoBehaviour
{
    public float startYPos = -1200f;
    public float endYPos = 1200f;
    public float storyMoveTime = 30f;
    public GameObject bg;
    IEnumerator storySceneCour;

    public AudioClip clickSound;

    void Awake()
    {
        startYPos = -1200f;
        endYPos = 1200f;
    }

    private void Start()
    {
        if (storySceneCour != null)
            StopCoroutine(storySceneCour);
        storySceneCour = StorySceneCour();
        StartCoroutine(storySceneCour);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SoundManager.Inst.SFXPlay("BasicTab", clickSound);
        }
    }
    public void InitScene()
    {
        bg.transform.localPosition = new Vector2(0, startYPos);
    }
    IEnumerator StorySceneCour()
    {
        var time = new WaitForSeconds(1f);

        bg.transform.DOLocalMoveY(endYPos, storyMoveTime).SetEase(Ease.Flash);
        for (int i = 0; i < storyMoveTime; i++) yield return time;
        LoadingScene.LoadScene("MainScene");

    }

    public void SkipBtn()
    {
        LoadingScene.LoadScene("MainScene");

    }
}
