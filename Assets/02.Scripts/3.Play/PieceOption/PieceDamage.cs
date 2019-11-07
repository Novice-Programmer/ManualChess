using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDamage : MonoBehaviour
{
    private Camera etcCamera;
    private Canvas etcCanvas;
    private RectTransform rectParent;
    private RectTransform rectDamage;

    public Vector3 offset = Vector3.zero;
    public Vector3 addset = Vector3.zero;
    public Transform targetTr;

    void Start()
    {
        etcCanvas = GetComponentInParent<Canvas>();
        etcCamera = etcCanvas.worldCamera;
        rectParent = etcCanvas.GetComponent<RectTransform>();
        rectDamage = this.gameObject.GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        if (targetTr == null)
        {
            Destroy(gameObject);
            return;
        }
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset + addset);
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }
        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, etcCamera, out localPos);
        rectDamage.localPosition = localPos;
    }
}
