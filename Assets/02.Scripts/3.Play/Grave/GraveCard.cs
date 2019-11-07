using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraveCard : CardSet
{
    [Header("GraveCard")]
    public SpriteRenderer gcard_ImageBackground;
    public TextMesh gcard_Name;
    public TextMesh gcard_Dir;
    public TextMesh gcard_Damage;
    public TextMesh gcard_HP;
    public TextMesh gcard_Mana;
    public GameObject gcard_Range;
    public int gcard_DrowMana;
    private SpriteRenderer[,] range = new SpriteRenderer[3, 3];

    public void GraveCardSet(int _orderNum,int _selectNum)
    {
        this.orderNum = _orderNum;
        this.selectNum = _selectNum;
        DataSet(_orderNum);
    }

    public void DataSet(int orderNum)
    {
        thisPieceData = card_data[orderNum].DataGet();
        gcard_ImageBackground.sprite = card_Images[orderNum];
        gcard_Name.text = thisPieceData.piece_Name;
        gcard_Dir.text = thisPieceData.piece_Dir;
        gcard_Damage.text = "" + thisPieceData.piece_AttackDamage;
        gcard_HP.text = "" + thisPieceData.piece_HP;
        gcard_DrowMana = thisPieceData.piece_DrowMana;
        gcard_Mana.text = "" + gcard_DrowMana;
        RangeSet(thisPieceData.moveRange);
    }

    public void RangeSet(int[,] rangeInt)
    {
        for (int i = 0; i < gcard_Range.transform.childCount; i++)
        {
            int x = i % 3;
            int z = i / 3;
            range[x, Mathf.Abs(z - 2)] = gcard_Range.transform.GetChild(i).GetComponent<SpriteRenderer>();
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
                    range[x, z].sprite = moveRangeSprite;
                }
                else if (rangeInt[x, z] == 3)
                {
                    range[x, z].sprite = targetPESprite;
                }
                else if (rangeInt[x, z] == 4)
                {
                    range[x, z].sprite = maxRangeSprite;
                }
            }
        }
    }
}
