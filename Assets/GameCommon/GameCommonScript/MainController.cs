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
        // GPGS 로그인이 되어 있지 않은 경우
        if (!Social.localUser.authenticated)
        {
            //계정 인증
            Social.localUser.Authenticate((bool isSuccess) =>
            {
                if (isSuccess)
                {
                    print("로그인 인증 성공 : " + Social.localUser.userName + " : "+ Social.localUser.id);
                }
                else
                {
                    print("로그인 인증 실패");
                }
            });
        }
        else
        {
            print("로그인 되어있음");
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
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SoundManager.Inst.SFXPlay("BasicTab", clickSound);
        }
    }
}
