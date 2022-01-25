using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 

public class BGObjController : MonoBehaviour
{
    [Header("�����ų �� ������Ʈ")]
    public Sprite[] objSpr;

    [Header("�����ų ��ġ")]
    public Transform[] objPos;

	private void Start()
	{
        SettingObjPos();    //������Ʈ ��ġ ���� ����
        SettingObjSpr();    //������Ʈ �̹��� ���� ����
    }

	public void SettingObjPos()
	{
        for (int i = 0; i < objPos.Length; i++)
        {
            switch (i)
            {
                case 0:
                    objPos[i].transform.localPosition = new Vector2(Random.Range(-9f, 0f), Random.Range(-1.25f, 2f));
                    objPos[i].gameObject.SetActive(Random.Range(0, 3) == 0 ? false : true);
                    break;
                case 1:
                    objPos[i].transform.localPosition = new Vector2(Random.Range(0f, 9f), Random.Range(-1.25f, 2f));
                    objPos[i].gameObject.SetActive(Random.Range(0, 3) == 0 ? false : true);
                    break;
                case 2:
                    objPos[i].transform.localPosition = new Vector2(Random.Range(-9f, 0f), Random.Range(-4.5f, -1.25f));
                    objPos[i].gameObject.SetActive(Random.Range(0, 3) == 0 ? false : true);
                    break;
                case 3:
                    objPos[i].transform.localPosition = new Vector2(Random.Range(0f, 9f), Random.Range(-4.5f, -1.25f));
                    objPos[i].gameObject.SetActive(Random.Range(0, 3) == 0 ? false : true);
                    break;
                default:
                    objPos[i].transform.localPosition = new Vector2(Random.Range(-9f, 9f), Random.Range(-4.5f, 2f));
                    objPos[i].gameObject.SetActive(Random.Range(0, 2) == 0 ? false : true);
                    break;
            }
        }
    }

    public void SettingObjSpr()
	{
        for (int i = 0; i < objPos.Length; i++)
        {
            objPos[i].GetComponent<SpriteRenderer>().sprite = objSpr[Random.Range(0, objSpr.Length)];
        }
    }
}
