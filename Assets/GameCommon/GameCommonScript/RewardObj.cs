using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RewardObj : MonoBehaviour
{
    Vector2 oriPos;
    private IEnumerator moveCour;
    public float moveTime;
    public float moveDistan;
    private void Awake()
    {
        oriPos = this.transform.position;
    }
    private void OnEnable()
    {
        if (moveCour != null)
            StopCoroutine(moveCour);
        moveCour = UpDownMove();
        StartCoroutine(moveCour);
    }

    private void OnDisable()
    {
        if (moveCour != null)
            StopCoroutine(moveCour);
        this.transform.position = oriPos;
    }

    IEnumerator UpDownMove()
    {
        var t = new WaitForSeconds(0.1f);

        while (this.gameObject.activeSelf) {
            yield return null;
            this.transform.DOMoveY(this.transform.position.y + moveDistan, moveTime);
            for (int j = 0; j < moveTime*10; j++) yield return t;
            this.transform.DOMoveY(this.transform.position.y - moveDistan, moveTime);
            for (int j = 0; j < moveTime * 10; j++) yield return t;
        }
    }
}
