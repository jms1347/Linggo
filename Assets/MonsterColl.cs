using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MonsterColl : MonoBehaviour
{
    public GameObject explosionPrefab;

    [HideInInspector]
    public Monster monster;
    public int attackCnt =1;
    private void Awake()
    {
        monster = this.GetComponent<Monster>();
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(attackCnt < 2)
        {
            if (coll.tag == "Player")
            {
                Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
                this.gameObject.SetActive(false);

                
            }
        }
        else
        {
            if (coll.tag == "Player")
            {
                
                this.transform.DOMoveX(this.transform.position.x - 8.0f, 2.0f).SetEase(Ease.OutQuint);
                this.transform.DOMoveY(this.transform.position.y + Random.Range(-2.0f,2.1f), 2.0f).SetEase(Ease.OutQuint);

                attackCnt--;
            }
        }
        
    }
}
