using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    public MarbleTab marbleTab;
    private void OnEnable()
    {
        marbleTab.OnMarble();
    }
}
