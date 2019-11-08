using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveText : MonoBehaviour
{
    public GameObject background;
    public TextMesh graveText;

    public void MouseView()
    {
        background.SetActive(true);
    }
    
    public void NoneMouseView()
    {
        background.SetActive(false);
    }

    public void GraveNumChange(int _graveNum)
    {
        if (_graveNum == 32)
        {
            graveText.text = "Max";
        }
        else
        {
            graveText.text = "" + _graveNum;
        }
    }

    public void GraveTextAddPos(Vector3 _pos)
    {
        transform.position = transform.position + _pos;
    }
}
