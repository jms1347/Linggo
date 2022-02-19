using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FadeObjScript : MonoBehaviour
{
    public float invokeTime;
    public void OnEnable()
    {
        Invoke(nameof(OffObj), invokeTime);
    }

    public void OffObj()
    {
        this.gameObject.SetActive(false);
    }
}
