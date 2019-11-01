using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : CardSet
{
    [Header("CardData")]
    public Image card_Image;
    public Text card_Name;
    public Text card_Dir;
    public Text card_Damage;
    public Text card_HP;
    public Text card_DrowMana;
    public int card_Mana;
    public bool cardRangeView;

    [Header("CardRange")]
    public Image thisImageP;
    public Image thisAction;
    public Image thisImageMax;
    public GameObject attack;
    public GameObject skill;
    public GameObject moveRangeParent;
    public GameObject attackRangeParent;
    public GameObject skillRangeParent;
    public GameObject[,] moveRange = new GameObject[3, 3];
    public GameObject[,] attackRange = new GameObject[3, 3];
    public GameObject[,] skillRange = new GameObject[3, 3];

    void Start()
    {
        StartSet(transform);
    }

    public void RangeSet(GameObject rangeParent, GameObject[,] range)
    {
        for (int i = 0; i < rangeParent.transform.childCount; i++)
        {
            int x = i % 3;
            int z = i / 3;
            range[x, Mathf.Abs(z - 2)] = rangeParent.transform.GetChild(i).gameObject;
        }
    }

    public void BtnDrowHandClick()
    {
        if (GameManager.Instance.DrowCheck(card_Mana))
        {
            GameManager.Instance.DrowNotice(orderNum, selectNum);
        }
    }
    public void NumSet(int orderNum, int selectNum)
    {
        this.orderNum = orderNum;
        this.selectNum = selectNum;
        DataSet(orderNum);
    }

    public void DataSet(int orderNum)
    {
        thisPieceData = card_data[orderNum].DataGet();
        card_Image.sprite = card_Images[orderNum];
        card_Name.text = thisPieceData.piece_Name;
        card_Dir.text = thisPieceData.piece_Dir;
        card_Damage.text = "" + thisPieceData.piece_Damage;
        card_HP.text = "" + thisPieceData.piece_HP;
        card_Mana = thisPieceData.piece_DrowMana;
        card_DrowMana.text = "" + card_Mana;
        RangeSet(moveRangeParent, moveRange);
        RangeSet(attackRangeParent, attackRange);
        RangeSet(skillRangeParent, skillRange);
        RangeSetting(thisPieceData.moveRange, 0, moveRange);
        RangeSetting(thisPieceData.attackRange, 1, attackRange);
        RangeSetting(thisPieceData.skillRange, 2, skillRange);
    }

    public void Btn_MoveRangeClick()
    {
        if (cardRangeView)
        {
            attack.SetActive(false);
            skill.SetActive(false);
            cardRangeView = false;
        }
        else
        {
            attack.SetActive(true);
            skill.SetActive(true);
            cardRangeView = true;
        }
    }

    public void RangeSetting(int[,] setInt, int action, GameObject[,] range)
    {
        if (action == 0)
        {
            thisAction.sprite = moveRangeSprite;
        }
        else if (action == 1)
        {
            thisAction.sprite = attackRangeSprite;
        }
        else if (action == 2)
        {
            thisAction.sprite = skillRangeSprite;
        }
        thisImageP.sprite = pieceSprite;

        for (int x = 0; x < 3; x++)
        {
            for (int z = 0; z < 3; z++)
            {
                if (setInt[x, z] == 1)
                {
                    Image thisImage = Instantiate(thisImageP, range[x, z].transform);
                }
                else if (setInt[x, z] == 2)
                {
                    Image thisImage = Instantiate(thisAction, range[x, z].transform);
                }
                else if (setInt[x, z] == 3)
                {
                    Image thisImagePE = thisImageP;
                    thisImagePE.sprite = targetPESprite;
                    Image thisImage = Instantiate(thisImagePE, range[x, z].transform);
                }
                else if (setInt[x, z] == 4)
                {
                    Image thisImage = Instantiate(thisImageMax, range[x, z].transform);
                }
            }
        }
    }
}
