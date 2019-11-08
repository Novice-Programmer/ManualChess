using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkAction : MonoBehaviourPunCallbacks
{
    public int[] hands;
    private PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    #region PunRPC

    [PunRPC]
    void TurnChangeRPC(int receiveTurnValue)
    {
        GameManager.Instance.TurnChange(receiveTurnValue);
    }

    [PunRPC]
    void ManaChangeRPC(int receiveMana, int receiveUseMana, int receiveAbleMana)
    {
        GameManager.Instance.EnemyManaChange(receiveMana, receiveUseMana, receiveAbleMana);
    }

    [PunRPC]
    void HPChangeRPC(int receiveHP)
    {
        GameManager.Instance.EnemyHPChange(receiveHP);
    }

    [PunRPC]
    void HandSettingRPC(int[] receiveHand)
    {
        for(int i = 0; i < receiveHand.Length; i++)
        {
            GameManager.Instance.handsList.Add(receiveHand[i]);
        }
    }

    [PunRPC]
    void DrowRemoveRPC(int receiveDrowValue)
    {
        GameManager.Instance.DrowRemove(receiveDrowValue);
    }

    [PunRPC]
    void DrowReturnRPC(int[] receiveList)
    {
        for (int i = 0; i < receiveList.Length; i++)
        {
            GameManager.Instance.handsList.Add(receiveList[i]);
        }
    }

    [PunRPC]
    void PlayerHandAddRPC(int receiveNum, bool receivePlayer)
    {
        HandManager.Instance.PlayerHandAdd(receiveNum, receivePlayer);
    }

    [PunRPC]
    void FiledAddSetRPC(int receiveHandNum, int receiveOrderNum, Vector3 receiveVector, bool receivePlayer)
    {
        HandManager.Instance.FiledAddPiece(receiveHandNum, receiveOrderNum, receiveVector, receivePlayer);
    }

    [PunRPC]
    void GraveyardThrowRPC(int receiveHandNum, int receiveOrderNum, bool receivePlayer)
    {
        Graveyard.Instance.NetGraveyardAdd(receiveHandNum, receiveOrderNum, receivePlayer);
    }

    [PunRPC]
    void GraveyardDrowRPC(int receiveNum, bool receivePlayer)
    {
        Graveyard.Instance.NetGraveyardRemove(receiveNum, receivePlayer);
    }

    [PunRPC]
    void PieceActionRPC(Vector2 receiveStart,Vector2 receiveEnd,int receiveNum,bool receivePlayer)
    {
        Board.Instance.PieceAction(receiveStart, receiveEnd, receiveNum, receivePlayer);
    }

    [PunRPC]
    void PieceDamageRPC(Vector2 receiveActionV, Vector2 receiveTargetV, bool receiveAtk,bool receiveReduce)
    {
        Board.Instance.PieceDamage(receiveActionV, receiveTargetV, receiveAtk, receiveReduce);
    }

    [PunRPC]
    void HandActionRPC(int receiveNum, int receiveAction, bool receivePlayer)
    {
        HandManager.Instance.HandAction(receiveNum, receiveAction, receivePlayer);
    }

    [PunRPC]
    void HandNoneSelectRPC(bool receivePlayer)
    {
        HandManager.Instance.HandNoneSelect(receivePlayer);
    }

    [PunRPC]
    void ShakeObjectRPC(Vector3 receiveV, float receiveF1, float receiveF2)
    {
        Shake.Instance.ShakeTarget(receiveV, receiveF1, receiveF2);
    }

    [PunRPC]
    void ShakeKingRPC(Vector3 receiveV, int receiveI)
    {
        Shake.Instance.ShakeKing(receiveV, receiveI);
    }

    #endregion

    #region ActionSend

    public void TurnChange(int sendTurnValue)
    {
        pv.RPC("TurnChangeRPC", RpcTarget.All, sendTurnValue);
    }

    public void ManaChange(int sendMana, int sendUseMana, int sendAbleMana)
    {
        pv.RPC("ManaChangeRPC", RpcTarget.Others, sendMana, sendUseMana, sendAbleMana);
    }

    public void HPChange(int sendHP)
    {
        pv.RPC("HPChangeRPC", RpcTarget.Others, sendHP);
    }

    public void HandSetting(int maxNum,int handsCount)
    {
        HandValueSetting(maxNum, handsCount);
        pv.RPC("HandSettingRPC", RpcTarget.All, hands);
    }

    public void DrowRemove(int sendDrowValue)
    {
        pv.RPC("DrowRemoveRPC", RpcTarget.All, sendDrowValue);
    }

    public void DrowReturn(List<int> _sendList)
    {
        int[] sendList = SendListChange(_sendList);
        pv.RPC("DrowReturnRPC", RpcTarget.All, sendList);
    }

    public void PlayerHandAdd(int sendNum, bool sendPlayer)
    {
        pv.RPC("PlayerHandAddRPC", RpcTarget.All, sendNum, sendPlayer);
    }

    public void FiledAddSet(int sendHandNum, int sendOrderNum, Vector3 sendVector, bool sendPlayer)
    {
        pv.RPC("FiledAddSetRPC", RpcTarget.All, sendHandNum, sendOrderNum, sendVector, sendPlayer);
    }

    public void GraveyardThrow(int sendHandNum, int sendOrderNum, bool sendPlayer)
    {
        pv.RPC("GraveyardThrowRPC", RpcTarget.All, sendHandNum, sendOrderNum, sendPlayer);
    }

    public void GraveyardDrow(int sendNum, bool sendPlayer)
    {
        pv.RPC("GraveyardDrowRPC", RpcTarget.All, sendNum, sendPlayer);
    }

    public void PieceAction(Vector2 sendStartV, Vector2 sendEndV, int sendAction, bool sendPlayer)
    {
        pv.RPC("PieceActionRPC", RpcTarget.All, sendStartV, sendEndV, sendAction, sendPlayer);
    }

    public void PieceDamage(Vector2 sendActionV, Vector2 sendTargetV, bool sendAtk, bool sendReduce)
    {
        pv.RPC("PieceDamageRPC", RpcTarget.All, sendActionV, sendTargetV, sendAtk, sendReduce);
    }

    public void HandAction(int sendNum,int sendAction,bool sendPlayer)
    {
        pv.RPC("HandActionRPC", RpcTarget.All, sendNum, sendAction, sendPlayer);
    }

    public void HandNoneSelect(bool sendPlayer)
    {
        pv.RPC("HandNoneSelectRPC", RpcTarget.All, sendPlayer);
    }

    public void ShakeObject(Vector3 _sendV, float _sendF1, float _sendF2)
    {
        pv.RPC("ShakeObjectRPC", RpcTarget.All, _sendV, _sendF1, _sendF2);
    }

    public void ShakeKing(Vector3 _sendV, int _sendI)
    {
        pv.RPC("ShakeKingRPC", RpcTarget.All, _sendV, _sendI);
    }

    #endregion

    #region Action

    void HandValueSetting(int _maxNum, int _handsCount)
    {
        hands = new int[_handsCount];
        for (int i = 0; i < _handsCount; i++)
        {
            hands[i] = Random.Range(1, _maxNum);
        }
    }

    int[] SendListChange(List<int> changeList)
    {
        int[] _sendList = new int[changeList.Count];
        for(int i = 0; i < _sendList.Length; i++)
        {
            _sendList[i] = changeList[i];
        }
        return _sendList;
    }

    #endregion
}
