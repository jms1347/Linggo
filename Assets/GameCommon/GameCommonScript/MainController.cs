using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public AudioClip clickSound;
    string log;

    private void Start()
    {
        GoogleLogin();
    }

    public void GoogleLogin()
    {
        GPGSBinder.Inst.Login((success, localUser) =>
                log = $"{success}, {localUser.userName}, {localUser.id}, {localUser.state}, {localUser.underage}");

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
