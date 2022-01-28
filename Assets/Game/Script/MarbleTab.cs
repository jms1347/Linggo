using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleTab : MonoBehaviour
{
    public int tapCnt;
    public int tapEx;

    IEnumerator tabCour;
    public void Start()
    {
        if (tabCour != null)
            StopCoroutine(tabCour);
        tabCour = TabCour();
        StartCoroutine(tabCour);
    }
    public void TabBtn()
    {
        if(tapCnt > 0)
        {
            tapEx++;
            tapCnt--;

        }
    }

    IEnumerator TabCour()
    {
        yield return null;
    }

}
