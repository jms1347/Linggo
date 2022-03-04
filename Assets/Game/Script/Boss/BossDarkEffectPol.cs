using System.Collections.Generic;
using System.Collections;

using UnityEngine;

public class BossDarkEffectPol : MonoBehaviour
{
    public List<GameObject> darkSuns = new List<GameObject>();
    void Awake()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            darkSuns.Add(this.transform.GetChild(i).gameObject);
        }

    }

    private void Start()
    {
        StartCoroutine(DarkEffectCour());
    }

    
    IEnumerator DarkEffectCour()
    {
        var t = new WaitForSeconds(0.1f);

        while (true)
        {
            for (int i = 0; i < darkSuns.Count; i++)
            {
                float ranX = Random.Range(-1.5f, 1.6f);
                float ranY = Random.Range(-1.5f, 1.6f);
                float ranScale = Random.Range(0.10f, 0.50f);
                darkSuns[i].transform.position = new Vector2(this.transform.position.x + ranX, this.transform.position.y + ranY);
                darkSuns[i].transform.localScale = new Vector3(ranScale, ranScale, ranScale);
                darkSuns[i].gameObject.SetActive(true);
                int ranTime = Random.Range(1, 5);
                for (int j = 0; j < ranTime; j++) yield return t;
            }
            for (int i = 0; i < 10; i++) yield return t;
            for (int i = 0; i < darkSuns.Count; i++)
            {
                darkSuns[i].gameObject.SetActive(false);
                int ranTime = Random.Range(1, 5);
                for (int j = 0; j < ranTime; j++) yield return t;
            }
        }
    }

}
