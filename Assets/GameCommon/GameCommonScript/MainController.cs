using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public AudioClip clickSound;

    private void Start()
    {
        GoogleLogin();
    }

    public void GoogleLogin()
    {
        GPGSBinder.Inst.Init();

        // GPGS �α����� �Ǿ� ���� ���� ���
        if (!Social.localUser.authenticated)
        {
            //���� ����
            Social.localUser.Authenticate((bool isSuccess) =>
            {
                if (isSuccess)
                {
                    print("�α��� ���� ���� : " + Social.localUser.userName + " : "+ Social.localUser.id);
                }
                else
                {
                    print("�α��� ���� ����");
                }
            });
        }
        else
        {
            print("�α��� �Ǿ�����");
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


    public void ShowLeaderBoard()
    {
        GPGSBinder.Inst.LoadCustomLeaderboardArray(GPGSIds.leaderboard, 20, 
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
}
