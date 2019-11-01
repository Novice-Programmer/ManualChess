using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployRange : MonoBehaviour
{
    public GameObject noneRange;
    public GameObject aRange;
    public GameObject bRange;
    private GameObject deployRange;

    public void DeployRangeSet(bool _player)
    {
        if (_player)
        {
            deployRange = aRange;
        }
        else
        {
            deployRange = bRange;
        }
    }

    public void DeployRangeView()
    {
        noneRange.SetActive(true);
        deployRange.SetActive(true);
    }

    public void DeployRangeNoneView()
    {
        noneRange.SetActive(false);
        deployRange.SetActive(false);
    }
}
