using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Turn : MonoBehaviour
{
    public GameObject timerA;
    public GameObject timerB;
    public GameObject turnCylinder;

    private Vector3 aVector;
    private Vector3 bVector;
    private Quaternion aTurnQua = Quaternion.Euler(97.0f, 0.0f, 0.0f);
    private Quaternion bTurnQua = Quaternion.Euler(83.0f, 0.0f, 0.0f);

    public float maxTime;
    public bool turnEnd;

    // Update is called once per frame
    void Update()
    {
        if (NetworkManager.Instance.gamePlay)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                TimeSet();
            }
        }
    }

    private void TimeSet()
    {
        int tQ = Mathf.FloorToInt(NetworkManager.Instance.time);
        Quaternion tQC = Quaternion.Euler(-90.0f - tQ * (360 / maxTime), -90.0f, 90);
        timerA.transform.rotation = tQC;
        Quaternion tQC2 = Quaternion.Euler(-90.0f + tQ * (360 / maxTime), -90.0f, 90);
        timerB.transform.rotation = tQC2;
        if (NetworkManager.Instance.time >= maxTime)
        {
            if (!turnEnd)
            {
                if (GameManager.Instance.playerTurn)
                {
                    GameManager.Instance.NetTurnChange();
                }
                else
                {
                    turnEnd = true;
                    NetworkManager.Instance.NetworkPassTurn();
                }
            }
        }
    }

    public void TurnObjectSet(bool playerTurn)
    {
        NetworkManager.Instance.time = 0.0f;
        if (PhotonNetwork.IsMasterClient)
        {
            if (playerTurn)
            {
                turnCylinder.transform.rotation = aTurnQua;
            }

            else if (!playerTurn)
            {
                turnCylinder.transform.rotation = bTurnQua;
            }
        }
        turnEnd = false;
    }
}
