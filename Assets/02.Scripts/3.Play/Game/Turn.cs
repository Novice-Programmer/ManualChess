using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    public GameObject timerA;
    public GameObject timerB;
    public GameObject turnCylinder;
    public TextMesh timeTextA;
    public TextMesh timeTextB;
    private TextMesh timeText;

    private Vector3 aVector;
    private Vector3 bVector;
    private Quaternion aTurnQua = Quaternion.Euler(97.0f, 0.0f, 0.0f);
    private Quaternion bTurnQua = Quaternion.Euler(83.0f, 0.0f, 0.0f);

    public float maxTime;
    public bool turnEnd;
    private bool player;

    public void StartSet(bool _player)
    {
        player = _player;
        if (_player)
        {
            timeText = timeTextA;
        }
        else
        {
            timeText = timeTextB;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkManager.Instance.gamePlay)
        {
            int time = Mathf.FloorToInt(NetworkManager.Instance.time);
            timeText.text = "" + time;
            if (player)
            {
                TimeSet(time);
            }
        }
    }

    public void TimeView()
    {
        timeText.gameObject.SetActive(true);
    }
    
    public void TimeNoneView()
    {
        timeText.gameObject.SetActive(false);
    }

    private void TimeSet(int time)
    {
        Quaternion tQC = Quaternion.Euler(-90.0f - time * (360 / maxTime), -90.0f, 90);
        timerA.transform.rotation = tQC;
        Quaternion tQC2 = Quaternion.Euler(-90.0f + time * (360 / maxTime), -90.0f, 90);
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
        if (player)
        {
            if (playerTurn)
            {
                turnCylinder.transform.rotation = aTurnQua;
            }

            else
            {
                turnCylinder.transform.rotation = bTurnQua;
            }
        }
        else
        {
            if (playerTurn)
            {
                turnCylinder.transform.rotation = bTurnQua;
            }

            else
            {
                turnCylinder.transform.rotation = aTurnQua;
            }
        }
        turnEnd = false;
    }
}
