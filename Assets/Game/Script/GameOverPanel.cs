using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameOverPanel : MonoBehaviour
{
    public TextMeshProUGUI countDownText;
    public float time ;
    public float countTime = 5.0f;
    public bool isCountDown = true;

    public void OnEnable()
    {
        isCountDown = true;
        countTime = 5.0f;
        time = countTime;


        StartCoroutine(CountDownCour());
    }


    IEnumerator CountDownCour()
    {
        while (isCountDown)
        {
            time -= 1 / countTime * Time.deltaTime;
            countDownText.text = Mathf.RoundToInt(time * countTime).ToString();

            if (time <= 0)
            {
                time = 0;
                isCountDown = false;

                GameController.Inst.TimeOffBtn();

                LoadingScene.LoadScene("MainScene");
            }
            yield return null;
        }
    }

}
