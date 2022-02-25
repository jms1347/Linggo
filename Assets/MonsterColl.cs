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
    public bool attackItem = true;
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
                if (coll.gameObject.name.Contains("Linggo"))
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


                }
                else if(coll.gameObject.name.Contains("Item") && attackItem)
                {
                    coll.gameObject.GetComponent<GhostItem>().DecreaseHP(monster.att);
                    if (isFire)
                    {
                        int dotAtt = Mathf.RoundToInt(monster.att * 0.05f);
                        coll.gameObject.GetComponent<GhostItem>().DotEffect(4, dotAtt);
                    }
                }
                else if (coll.gameObject.name.Contains("Nek"))
                {
                    coll.gameObject.GetComponent<Monster>().DecreaseHP(coll.gameObject.GetComponent<Monster>().maxHp);

                    //coll.gameObject.GetComponent<Monster>().DecreaseHP(monster.att);
                    //if (isFire)
                    //{
                    //    int dotAtt = Mathf.RoundToInt(monster.att * 0.05f);
                    //    coll.gameObject.GetComponent<Monster>().DotEffect(4, dotAtt);
                    //}
                }
                Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
                this.gameObject.SetActive(false);


                for (int i = 0; i < GameController.Inst.fieldMonsters.Count; i++)
                {
                    if (GameController.Inst.fieldMonsters[i].gameObject.Equals(this.gameObject))
                    {
                        GameController.Inst.fieldMonsters.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        else
        {
            if (coll.gameObject.tag == "Player")
            {
                if (coll.gameObject.name.Contains("Linggo"))
                {
                    GameController.Inst.CriticalDecreaseHP(monster.att * 2);
                }
                else if (coll.gameObject.name.Contains("Item") && attackItem)
                {
                    coll.gameObject.GetComponent<GhostItem>().CriticalDecreaseHP(monster.att); 
                }
                else if (coll.gameObject.name.Contains("Nek"))
                {
                    //coll.gameObject.GetComponent<Monster>().CriticalDecreaseHP(monster.att);
                    coll.gameObject.GetComponent<Monster>().CriticalDecreaseHP(coll.gameObject.GetComponent<Monster>().maxHp);

                }
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
                if (coll.name.Contains("Linggo"))
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
                }
                else if (coll.name.Contains("Item") && attackItem)
                {
                    coll.gameObject.GetComponent<GhostItem>().DecreaseHP(monster.att);
                    if (isFire)
                    {
                        int dotAtt = Mathf.RoundToInt(monster.att * 0.05f);
                        coll.gameObject.GetComponent<GhostItem>().DotEffect(4, dotAtt);
                    }
                }
                else if(coll.name.Contains("Nek"))
                {
                    coll.gameObject.GetComponent<Monster>().DecreaseHP(coll.gameObject.GetComponent<Monster>().maxHp);
                    //coll.gameObject.GetComponent<Monster>().DecreaseHP(monster.att);
                    //if (isFire)
                    //{
                    //    int dotAtt = Mathf.RoundToInt(monster.att * 0.05f);
                    //    coll.gameObject.GetComponent<Monster>().DotEffect(4, dotAtt);
                    //}
                }

                Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
                this.gameObject.SetActive(false);

                for (int i = 0; i < GameController.Inst.fieldMonsters.Count; i++)
                {
                    if (GameController.Inst.fieldMonsters[i].gameObject.Equals(this.gameObject))
                    {
                        GameController.Inst.fieldMonsters.RemoveAt(i);
                        break;
                    }
                }
            }
            
        }
        else
        {
            if (coll.tag == "Player")
            {
                if (coll.name.Contains("Linggo"))
                {
                    GameController.Inst.CriticalDecreaseHP(monster.att * 2);

                }
                else if (coll.name.Contains("Item") && attackItem)
                {
                    coll.gameObject.GetComponent<GhostItem>().CriticalDecreaseHP(monster.att*2);
                }
                else if (coll.name.Contains("Nek"))
                {
                    //coll.gameObject.GetComponent<Monster>().CriticalDecreaseHP(monster.att*2);
                    coll.gameObject.GetComponent<Monster>().CriticalDecreaseHP(coll.gameObject.GetComponent<Monster>().maxHp);
                }

                this.transform.DOMoveX(this.transform.position.x - 6.0f, 2.0f).SetEase(Ease.OutQuint);
                this.transform.DOMoveY(this.transform.position.y + Random.Range(-2.0f, 2.1f), 2.0f).SetEase(Ease.OutQuint);

                attackCnt--;
            }
        }        
    }
}
