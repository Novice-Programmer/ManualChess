using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceBar : MonoBehaviour
{
    private Camera etcCamera;
    private Canvas etcCanvas;
    private RectTransform rectParent;
    private RectTransform rectBar;

    public Vector3 offset = Vector3.zero;
    public Vector3 addset = Vector3.zero;
    public Transform targetTr;

    public void StartSet()
    {
        etcCanvas = GetComponentInParent<Canvas>();
        etcCamera = etcCanvas.worldCamera;
        rectParent = etcCanvas.GetComponent<RectTransform>();
        rectBar = this.gameObject.GetComponent<RectTransform>();
        PosUpdate(Vector3.zero);
    }

    public void PosUpdate(Vector3 addV)
    {
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset + addset + addV);
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }
        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, etcCamera, out localPos);
        rectBar.localPosition = localPos;
    }

    public void DestroyTarget()
    {
        Destroy(gameObject);
    }
}
