using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Splash : MonoBehaviour
{
    public TextMeshProUGUI guideText;
    private void Start()
    {
        StartCoroutine(TextCour());
    }

    IEnumerator TextCour()
    {
        var time = new WaitForSeconds(0.1f);

        while (guideText.gameObject.activeSelf)
        {
            guideText.text = "서버에서 데이터를 가져오는 중.  ";
            for (int i = 0; i < 5; i++) yield return time;
            guideText.text = "서버에서 데이터를 가져오는 중.. ";
            for (int i = 0; i < 5; i++) yield return time;
            guideText.text = "서버에서 데이터를 가져오는 중...";
            for (int i = 0; i < 5; i++) yield return time;
        }
    }

}
