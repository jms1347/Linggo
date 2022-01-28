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
    public bool isFire = false;
    public bool isLightning = false;
    public bool isIce = false;
    private void Awake()
    {
        monster = this.GetComponent<Monster>();
    }
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (attackCnt < 2)
        {
            if (coll.gameObject.tag == "Player")
            {
                GameController.Inst.DecreaseHP(monster.att);
                if (isFire)
                {
                    int dotAtt = Mathf.RoundToInt(monster.att * 0.05f);
                    coll.gameObject.GetComponent<Linggo>().FireDotEffect(4, dotAtt);

                }
                if (isLightning)
                {
                    coll.gameObject.GetComponent<Linggo>().LightningStunEffect(2.0f);
                }
                if (isIce)
                {
                    coll.gameObject.GetComponent<Linggo>().IceStunEffect(1.0f);
                }

                Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            if (coll.gameObject.tag == "Player")
            {
                GameController.Inst.CriticalDecreaseHP(monster.att * 2);

                this.transform.DOMoveX(this.transform.position.x - 6.0f, 2.0f).SetEase(Ease.OutQuint);
                this.transform.DOMoveY(this.transform.position.y + Random.Range(-2.0f, 2.1f), 2.0f).SetEase(Ease.OutQuint);

                attackCnt--;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(attackCnt < 2)
        {
            if (coll.tag == "Player")
            {
                GameController.Inst.DecreaseHP(monster.att);
                if (isFire)
                {
                    int dotAtt = Mathf.RoundToInt(monster.att * 0.05f);
                    coll.gameObject.GetComponent<Linggo>().FireDotEffect(4, dotAtt);

                }
                if (isLightning)
                {
                    coll.gameObject.GetComponent<Linggo>().LightningStunEffect(2.0f);
                }
                if (isIce)
                {
                    coll.gameObject.GetComponent<Linggo>().IceStunEffect(1.0f);
                }

                Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
                this.gameObject.SetActive(false);                
            }
        }
        else
        {
            if (coll.tag == "Player")
            {
                GameController.Inst.CriticalDecreaseHP(monster.att*2);

                this.transform.DOMoveX(this.transform.position.x - 6.0f, 2.0f).SetEase(Ease.OutQuint);
                this.transform.DOMoveY(this.transform.position.y + Random.Range(-2.0f,2.1f), 2.0f).SetEase(Ease.OutQuint);

                attackCnt--;
            }
        }
        
    }
}
