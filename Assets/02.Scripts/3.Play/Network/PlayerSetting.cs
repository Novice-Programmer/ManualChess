using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    public GameObject mainLight; // 전체 라이트
    public GameObject graveView;
    public GameObject graveText;
    public GameObject noneRange;

    // 플레이어가 b일경우 설정
    Quaternion cameraQ = Quaternion.Euler(46.0f, 180.0f, 0.0f);
    Quaternion lightQ = Quaternion.Euler(50.0f, 150.0f, 0.0f);
    Quaternion graveQ = Quaternion.Euler(-215.0f, 0.0f, 180.0f);
    Quaternion graveTextQ = Quaternion.Euler(0.0f, 180.0f, 0.0f);

    // 플레이어 설정
    Vector3 cameraV = new Vector3(5.0f, 17.0f, 21.0f);
    Vector3 lightV = new Vector3(0.0f, 50.0f, 0.0f);
    Vector3 graveV = new Vector3(5.0f, 10.9f, 14.4f);
    Vector3 graveTextV = new Vector3(0.5f, 0.0f, 1.4f);
    Vector3 noneV = new Vector3(4.5f, 2.0f, 4.2f);

    void Start()
    { 
        PlayerSeting();
    }

    // 플레이어 기본값 설정
    private void PlayerSeting()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Camera.main.transform.rotation = cameraQ;
            Camera.main.transform.position = cameraV;
            mainLight.transform.rotation = lightQ;
            mainLight.transform.position = lightV;
            graveView.transform.rotation = graveQ;
            graveView.transform.position = graveV;
            graveText.transform.SetPositionAndRotation(graveText.transform.position + graveTextV, graveTextQ);
            noneRange.transform.position = noneV;
        }
    }
}
