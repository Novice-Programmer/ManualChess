using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : CardSet
{
    [Header("PlayerHand")]
    public SpriteRenderer playerHand_ImageBackground;
    public TextMesh playerHand_Name;
    public TextMesh playerHand_Dir;
    public TextMesh playerHand_Damage;
    public TextMesh playerHand_HP;
    public TextMesh playerHand_Mana;
    public GameObject playerHand_Range;
    public SpriteRenderer[,] range = new SpriteRenderer[3, 3];
    public GameObject pieceObj;

    [Header("Outline")]
    public GameObject handViewOutline;
    public GameObject handMoveOutline;

    [Header("NonePlayer")]
    public GameObject[] noneObject;

    public bool player;
    public int handNum;
    public int playerHand_fieldMana;

    private void Awake()
    {
        HandManager hm = HandManager.Instance;
        if (player)
        {
            hm.playerAHand[hm.aHandsNum] = this;
            transform.parent = hm.handATransform[hm.aHandsNum];
            if (GameManager.Instance.player)
            {
                hm.aHandPos.handPosRayCast[hm.aHandsNum].SetActive(true);
            }
            handNum = hm.aHandsNum;
            hm.aHandsNum++;
        }
        else
        {
            hm.playerBHand[hm.bHandsNum] = this;
            transform.parent = hm.handBTransform[hm.bHandsNum];
            if (!GameManager.Instance.player)
            {
                hm.bHandPos.handPosRayCast[hm.bHandsNum].SetActive(true);
            }
            handNum = hm.bHandsNum;
            hm.bHandsNum++;
        }

        if (GameManager.Instance.player != player)
        {
            for(int i = 0; i < noneObject.Length; i++)
            {
                noneObject[i].SetActive(false);
            }
        }

    }

    public void PlayerHandSet(int orderNum, GameObject pieceObj)
    {
        this.orderNum = orderNum;
        this.pieceObj = pieceObj;
        DataSet(orderNum);
    }

    public void DataSet(int orderNum)
    {
        thisPieceData = card_data[orderNum].DataGet();
        playerHand_ImageBackground.sprite = card_Images[orderNum];
        playerHand_Name.text = thisPieceData.piece_Name;
        playerHand_Dir.text = thisPieceData.piece_Dir;
        playerHand_Damage.text = "" + thisPieceData.piece_AttackDamage;
        playerHand_HP.text = "" + thisPieceData.piece_HP;
        playerHand_fieldMana = thisPieceData.piece_FieldMana;
        playerHand_Mana.text = "" + playerHand_fieldMana;
        RangeSet(thisPieceData.moveRange);
    }

    public void RangeSet(int[,] rangeInt)
    {
        for (int i = 0; i < playerHand_Range.transform.childCount; i++)
        {
            int x = i % 3;
            int z = i / 3;
            range[x, Mathf.Abs(z - 2)] = playerHand_Range.transform.GetChild(i).GetComponent<SpriteRenderer>();
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

    public void RemoveHand()
    {
        NetworkManager.Instance.HandRemove(gameObject);
    }

    public void OnDestroy()
    {
        if (player)
        {
            HandManager.Instance.playerAHand[handNum] = null;
            HandManager.Instance.aHandPos.handPosRayCast[handNum].SetActive(false);
            HandManager.Instance.aHandsNum--;
        }

        else
        {
            HandManager.Instance.playerBHand[handNum] = null;
            HandManager.Instance.bHandPos.handPosRayCast[handNum].SetActive(false);
            HandManager.Instance.bHandsNum--;
        }

        HandManager.Instance.NumReset();
    }

    public void HandView()
    {
        handViewOutline.SetActive(true);
    }

    public void HandSelect()
    {
        handMoveOutline.SetActive(true);
    }

    public void HandNoneSelect()
    {
        handViewOutline.SetActive(false);
        handMoveOutline.SetActive(false);
    }

    public void HandMoveReset()
    {
        if (GameManager.Instance.player)
        {
            transform.position = HandManager.Instance.handBTransform[handNum].position;
        }
        else
        {
            transform.position = HandManager.Instance.handATransform[handNum].position;
        }
    }
}
