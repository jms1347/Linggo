using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemIcon : MonoBehaviour
{
    public ItemCard item;
    public Image buffIcon;
    public Image coolTimeImg;
    public TextMeshProUGUI guideText;
    public bool isCooldown = false;
    IEnumerator ItemBuffCour;

    private void Awake()
    {
        buffIcon = this.transform.GetChild(0).GetComponent<Image>();
        coolTimeImg = this.transform.GetChild(1).GetComponent<Image>();
        guideText = this.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        buffIcon.fillAmount = 1;
    }

    public void InitBuff()
    {
        item = null;
        buffIcon.gameObject.SetActive(false);
        coolTimeImg.gameObject.SetActive(false);
        guideText.gameObject.SetActive(false);
        isCooldown = false;
    }

    public void UseBuff(ItemCard useItem, Sprite useItemIcon)
    {
        item = useItem;
        this.gameObject.SetActive(true);
        buffIcon.sprite = useItemIcon;
        coolTimeImg.sprite = useItemIcon;
        ExecuteBuff();
    }

    public void ReduplicateUseBuff()
    {
        isCooldown = false;
        ExecuteBuff();
    }
    public void ExecuteBuff()
    {
        if (!isCooldown)
        {
            isCooldown = true;
            buffIcon.gameObject.SetActive(true);
            coolTimeImg.gameObject.SetActive(true);
            guideText.gameObject.SetActive(true);

            if (ItemBuffCour != null)
                StopCoroutine(ItemBuffCour);

            ItemBuffCour = ActivationBuff();
            StartCoroutine(ItemBuffCour);
        }
    }

    IEnumerator ActivationBuff(float time = 1)
    {
        coolTimeImg.fillAmount = time;

        while (isCooldown)
        {
            coolTimeImg.fillAmount -= 1 / item.itemDuration * Time.deltaTime;
            if (coolTimeImg.fillAmount <= 0)
            {
                coolTimeImg.fillAmount = 0;
                isCooldown = false;
                coolTimeImg.gameObject.SetActive(false);
            }
            yield return null;
        }
        InitBuff();
    }
}
