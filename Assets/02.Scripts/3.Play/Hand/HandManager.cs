using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance { set; get; }

    [Header("Hand")]
    public HandPos aHandPos;
    public HandPos bHandPos;
    public PlayerHand[] playerAHand;
    public PlayerHand[] playerBHand;
    public PlayerHand selectEnemyHand;

    [Header("Transform")]
    public Transform[] handATransform;
    public Transform[] handBTransform;

    [Header("Object")]
    public GameObject playerA;
    public GameObject playerB;
    public GameObject playerHandObj;
    public GameObject[] pieceObjs;
    public int[] pieceDMana;

    [Header("Num")]
    public int aHandsNum = 0;
    public int bHandsNum = 0;
    public const int maxHandNum = 9;

    public string[] pieceName;

    private void Start()
    {
        Instance = this;
        playerAHand = new PlayerHand[9];
        playerBHand = new PlayerHand[9];
        handATransform = new Transform[9];
        handBTransform = new Transform[9];
        aHandPos.HandPosSet();
        bHandPos.HandPosSet();
        for (int i = 0; i < aHandPos.handPos.Length; i++)
        {
            handATransform[i] = aHandPos.handPos[i].transform;
        }
        for (int i = 0; i < bHandPos.handPos.Length; i++)
        {
            handBTransform[i] = bHandPos.handPos[i].transform;
        }
        pieceDMana = new int[pieceObjs.Length];
        for(int i= 0; i < pieceObjs.Length; i++)
        {
            pieceDMana[i] = pieceObjs[i].GetComponent<Piece>().drowMana;
        }
    }

    // 네트워크 : 드로우 시작
    public void NetPlayerDrow(int _orderNum)
    {
        NetworkManager.Instance.networkAction.PlayerHandAdd(_orderNum, GameManager.Instance.player);
    }

    // 네트워크 : 드로우
    public void PlayerHandAdd(int orderNum, bool player)
    {
        if (GameManager.Instance.player == player)
        {
            GameObject thisHand;
            int handNum;
            Transform _tr;
            if (player)
            {
                handNum = aHandsNum;
                _tr = handATransform[handNum];
            }
            else
            {
                handNum = bHandsNum;
                _tr = handBTransform[handNum];
            }

            thisHand = NetworkManager.Instance.HandAdd(_tr, player);
            thisHand.GetComponent<PlayerHand>().PlayerHandSet(orderNum, pieceObjs[orderNum]);
        }
    }

    // 네트워크 : 필드 소환
    public void FiledAddPiece(int handNum, int orderNum, Vector3 selectV, bool player)
    {
        PlayerHand[] _playerHands;
        HandPos _handPos;

        if (player)
        {
            _playerHands = playerAHand;
            _handPos = aHandPos;
        }

        else
        {
            _playerHands = playerBHand;
            _handPos = bHandPos;
        }


        if (GameManager.Instance.player == player)
        {
            NetworkManager.Instance.SummonPiece(pieceName[orderNum], selectV);
            _playerHands[handNum].RemoveHand();
        }
    }

    public bool HandAddCheck(bool player)
    {
        if (player)
        {
            if (aHandsNum <= maxHandNum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (bHandsNum <= maxHandNum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void NumReset()
    {
        int numA = 0;
        int numB = 0;
        for (int i = 0; i < maxHandNum; i++)
        {
            for (int j = maxHandNum-1; j >= 0; j--)
            {
                if (playerAHand[j] != null)
                {
                    if (numA < j + 1)
                    {
                        numA = j + 1;
                    }
                    if (j != 0 && playerAHand[j - 1] == null)
                    {
                        playerAHand[j - 1] = playerAHand[j];
                        playerAHand[j - 1].handNum = j - 1;
                        playerAHand[j].transform.parent = handATransform[j - 1].transform;
                        playerAHand[j] = null;
                        numA = j;
                        aHandPos.handPosRayCast[j].SetActive(false);
                        if (GameManager.Instance.player == playerAHand[j - 1].player)
                        {
                            playerAHand[j - 1].transform.SetPositionAndRotation(handATransform[j - 1].position, handATransform[j - 1].rotation);
                            aHandPos.handPosRayCast[j - 1].SetActive(true);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < maxHandNum; i++)
        {
            for (int j = maxHandNum - 1; j >= 0; j--)
            {
                if (playerBHand[j] != null)
                {
                    if (numB < j + 1)
                    {
                        numB = j + 1;
                    }
                    if (j != 0 && playerBHand[j - 1] == null)
                    {
                        playerBHand[j - 1] = playerBHand[j];
                        playerBHand[j - 1].handNum = j - 1;
                        playerBHand[j].transform.parent = handBTransform[j - 1].transform;
                        playerBHand[j] = null;
                        numB = j;
                        bHandPos.handPosRayCast[j].SetActive(false);
                        if (GameManager.Instance.player == playerBHand[j - 1].player)
                        {
                            playerBHand[j - 1].transform.SetPositionAndRotation(handBTransform[j - 1].position, handBTransform[j - 1].rotation);
                            bHandPos.handPosRayCast[j - 1].SetActive(true);
                        }
                    }
                }
            }
        }
        aHandsNum = numA;
        bHandsNum = numB;
        selectEnemyHand = null;
    }

    public void HandAction(int _selectNum, int _action, bool _player)
    {
        if (selectEnemyHand != null)
        {
            HandNoneSelect(_player);
        }

        PlayerHand[] _playerHands;

        if (_player)
        {
            _playerHands = playerAHand;
        }
        else
        {
            _playerHands = playerBHand;
        }

        if (_action == 0)
        {
            _playerHands[_selectNum].HandView();
        }
        else if (_action == 1)
        {
            _playerHands[_selectNum].HandSelect();
        }

        selectEnemyHand = _playerHands[_selectNum];
    }

    public void HandNoneSelect(bool _player)
    {
        PlayerHand[] _playerHands;
        int _size;

        if (_player)
        {
            _playerHands = playerAHand;
            _size = aHandsNum;
        }
        else
        {
            _playerHands = playerBHand;
            _size = bHandsNum;
        }

        for (int i = 0; i < _size; i++)
        {
            _playerHands[i].HandNoneSelect();
        }

        selectEnemyHand = null;
    }
}
