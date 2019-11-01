using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPos : MonoBehaviour
{
    public GameObject[] handPos;
    public GameObject[] handPosRayCast;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void HandPosSet()
    {
        handPos = new GameObject[transform.childCount];
        handPosRayCast = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            handPos[i] = transform.GetChild(i).gameObject;
            handPosRayCast[i] = handPos[i].transform.GetChild(0).gameObject;
        }
    }
}
