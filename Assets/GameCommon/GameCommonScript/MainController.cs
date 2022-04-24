using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainController : MonoBehaviour
{
    public AudioClip clickSound;
    public TextMeshProUGUI myKillCntText;
    public TextMeshProUGUI myWaveText;
    public string maxWave;
    public string maxKill;

    public Button btnRemoveAds;

    [Header("버전(빌드전에 입력하기)")]
    public int majorNum;
    public int minorNum;
    public int patchNum;
    public VersionSo versionSo;
    public GameObject versionPop;

    private void Start()
    {
        CheckVersion();

        GPGSBinder.Inst.Init();

        GoogleLogin();
    }

    public void CheckVersion()
    {
        if(versionSo.versionData.majorNum != majorNum
            || versionSo.versionData.minorNum != minorNum
            || versionSo.versionData.patchNum != patchNum)
        {
            //print("최신 버전이 아님");
            versionPop.SetActive(true);
        }
        //else { 
        //    print("최신 버전임 이상무");
        //}
    }

    public void LoadPlayStoreURL()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.fromscratch.linggolight");
    }

    public void OutPutCloudData()
    {
        
        GPGSBinder.Inst.LoadCloud("myWave", (success, data) => {
            maxWave = data;
        });
        GPGSBinder.Inst.LoadCloud("myKill", (success, data) => {
            maxKill = data;
        });

        if(string.IsNullOrEmpty(maxKill))
            myKillCntText.text = "최대 킬 수 : 0";
        else
            myKillCntText.text = "최대 킬 수 : " + maxKill;

        if(string.IsNullOrEmpty(maxWave))            
            myWaveText.text = "최대 웨이브 : 1";
        else
            myWaveText.text = "최대 웨이브 : " + maxWave;
    }
    public void GoogleLogin()
    {
        // GPGS 로그인이 되어 있지 않은 경우
        if (!Social.localUser.authenticated)
        {
            //계정 인증
            Social.localUser.Authenticate((bool isSuccess) =>
            {
                if (isSuccess)
                {
                    //print("로그인 인증 성공 : " + Social.localUser.userName + " : "+ Social.localUser.id);

                    //OutPutCloudData();

                }
                else
                {
                    //print("로그인 인증 실패");
                    myKillCntText.text = "";
                    myWaveText.text = "";
                }
            });
        }
        else
        {
            //print("로그인 되어있음");

            //OutPutCloudData();
        }      
    }

    public void GoogleLogout()
    {
        if (Social.localUser.authenticated)
        {
            GPGSBinder.Inst.Logout();
        }

    }
    public void StartGame()
    {
        LoadingScene.LoadScene("GameScene");
    }
    public void Tutorial()
    {
        LoadingScene.LoadScene("TutorialScene");
    }

    public void StoryScene()
    {
        LoadingScene.LoadScene("StoryScene");

    }

    public void RankBtn()
    {
        GoogleLogin();
        Social.ShowLeaderboardUI();
    }

    public void ShowLeaderBoard()
    {
        GPGSBinder.Inst.LoadCustomLeaderboardArray(GPGSIds.leaderboard_ranking, 20, 
            GooglePlayGames.BasicApi.LeaderboardStart.PlayerCentered, GooglePlayGames.BasicApi.LeaderboardTimeSpan.AllTime,
            (success, scoreData )=>
        {
            var scores = scoreData.Scores;
            for (int i = 0; i < scores.Length; i++)
            {
                print($"{ i}, { scores[i].rank}, {scores[i].value}, {scores[i].userID},{scores[i].date}\n");
            }
        });
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SoundManager.Inst.SFXPlay("BasicTab", clickSound);
        }
    }

    public void RemoveAdsBtn()
    {
    
    }
}
