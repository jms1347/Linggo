using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGController : MonoBehaviour
{
	[SerializeField] Transform[] bgs = null;
	[SerializeField] float moveSpeed = 0f;
	public bool isNotObj = false;
	float resetPosX = 0;
	float initPosX = 0;

    void Awake()
    {
		float bgWidth = bgs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;

		resetPosX = -bgWidth;
		initPosX = bgWidth * (bgs.Length);
	}

	private void Update()
	{
		for (int i = 0; i < bgs.Length; i++)
		{
			bgs[i].position -= new Vector3(moveSpeed, 0, 0) * Time.deltaTime;

			if(bgs[i].position.x < resetPosX)
			{

				Vector3 selfPos = bgs[i].position;
				selfPos.Set(selfPos.x + initPosX, selfPos.y, selfPos.z);
				bgs[i].position = selfPos;

                if (isNotObj)
                {
					bgs[i].GetComponent<BGObjController>().SettingObjPos();
					bgs[i].GetComponent<BGObjController>().SettingObjSpr();
				}
			}
		}		
	}
}
