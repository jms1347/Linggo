using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public AudioClip clickSound;
    public void StartGame()
    {
        LoadingScene.LoadScene("GameScene");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SoundManager.Inst.SFXPlay("BasicTab", clickSound);
        }
    }
}
