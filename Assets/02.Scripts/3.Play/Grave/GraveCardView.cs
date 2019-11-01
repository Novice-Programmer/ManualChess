using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveCardView : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        float angleY = transform.rotation.y;
        if (transform.childCount > 1)
        {
            if (GameManager.Instance.player)
            {
                if (Mathf.Abs(angleY) > 0.7)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                else
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                if (Mathf.Abs(angleY) < 0.7)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                else
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
