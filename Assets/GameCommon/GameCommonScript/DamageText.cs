using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class DamageText : MonoBehaviour
{
    public float moveY;
    public float moveTime;
    TextMeshPro text;
    private void Awake()
    {
        text = this.GetComponent<TextMeshPro>();
    }

    public  void SetDecreaseText(string damage)
    {
        //text.color = Color.red;
        moveTime = 3.0f;
        moveY = 3.0f;
        text.text = damage;
        this.transform.DOMoveY(this.transform.position.y + moveY, moveTime);
        text.DOFade(0, moveTime).SetDelay(1.0f).OnComplete(() =>
        {
            Destroy(this.gameObject);
        });
    }
    public void SetIncreaseText(string damage)
    {
        moveTime = 3.0f;
        moveY = 3.0f;
        text.text = damage;
        this.transform.DOMoveY(this.transform.position.y + moveY, moveTime);
        text.DOFade(0, moveTime).SetDelay(1.0f).OnComplete(() =>
        {
            Destroy(this.gameObject);
        });
    }

}
