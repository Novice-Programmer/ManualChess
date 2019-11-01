using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CardSet : MonoBehaviour
{
    [Header("Number")]
    public int orderNum;
    public int selectNum;

    [Header("CardOption")]
    public List<Sprite> card_Images;
    public List<Piece> card_data;
    public CardData thisPieceData;

    [Header("RangeImg")]
    public Sprite pieceSprite;
    public Sprite targetPSprite;
    public Sprite targetPESprite;
    public Sprite moveRangeSprite;
    public Sprite attackRangeSprite;
    public Sprite skillRangeSprite;
    public Sprite maxRangeSprite;


    public void StartSet(Transform cTransform)
    {
        card_Images = new List<Sprite>();
        card_data = new List<Piece>();
    }

    public void SelectNumSet(int selectNum)
    {
        this.selectNum = selectNum;
    }

}
