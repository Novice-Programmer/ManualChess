using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceData : MonoBehaviour
{
    public Text pieceName;
    public GameObject attack;
    public Text attackDamage;
    public Image[,] attackRange = new Image[3, 3];
    public GameObject skill;
    public Text skillDamage;
    public Image[,] skillRange = new Image[3, 3];
    public GameObject move;
    public Image[,] moveRange = new Image[3, 3];
    public Vector3 offSet;
    public int orderNum = 99;

    private Camera etcCamera;
    private Canvas etcCanvas;
    private RectTransform rectParent;
    private RectTransform rectThis;

    public Sprite pieceSprite;
    public Sprite moveSprite;
    public Sprite targetSprite;
    public Sprite maxSprite;

    // Start is called before the first frame update
    void Start()
    {
        etcCanvas = GetComponentInParent<Canvas>();
        etcCamera = etcCanvas.worldCamera;
        rectParent = etcCanvas.GetComponent<RectTransform>();
        rectThis = gameObject.GetComponent<RectTransform>();
        for (int i = 0; i < 9; i++)
        {
            int x = i % 3;
            int z = i / 3;
            attackRange[x, Mathf.Abs(z - 2)] = attack.transform.GetChild(i).GetComponent<Image>();
            skillRange[x, Mathf.Abs(z - 2)] = skill.transform.GetChild(i).GetComponent<Image>();
            moveRange[x, Mathf.Abs(z - 2)] = move.transform.GetChild(i).GetComponent<Image>();
        }
    }

    public void DataView()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
    }

    public void NoneView()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
    }

    public void DataViewSet(Piece viewPiece)
    {
        var screenPos = Camera.main.WorldToScreenPoint(viewPiece.transform.position + offSet);
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }
        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, etcCamera, out localPos);
        rectThis.localPosition = localPos;
        if (orderNum != viewPiece.orderNum)
        {
            CardData cardData = viewPiece.DataGet();
            pieceName.text = cardData.piece_Name;
            attackDamage.text = "" + cardData.piece_AttackDamage;
            skillDamage.text = "" + cardData.piece_SkillDamage;
            RangeSet(cardData.attackRange, 0);
            RangeSet(cardData.skillRange, 1);
            RangeSet(cardData.moveRange, 2);
            orderNum = viewPiece.orderNum;
        }
    }

    public void RangeSet(int[,] rangeInt,int action)
    {
        Image[,] range;
        if (action == 0)
        {
            range = attackRange;
        }
        else if(action == 1)
        {
            range = skillRange;
        }
        else
        {
            range = moveRange;
        }
        for (int x = 0; x < 3; x++)
        {
            for (int z = 0; z < 3; z++)
            {
                if (rangeInt[x, z] == 1)
                {
                    range[x, z].sprite = pieceSprite;
                }
                else if (rangeInt[x, z] == 2)
                {
                    range[x, z].sprite = moveSprite;
                }
                else if (rangeInt[x, z] == 3)
                {
                    range[x, z].sprite = targetSprite;
                }
                else if (rangeInt[x, z] == 4)
                {
                    range[x, z].sprite = maxSprite;
                }
                else
                {
                    range[x, z].sprite = null;
                }
            }
        }
    }
}
