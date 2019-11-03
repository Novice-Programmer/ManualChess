using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance { set; get; }

    public NetworkAction networkAction;

    private PhotonView pv;
    private bool playerLoading;
    private bool allLoading;

    public GameObject loadingPanel;
    public CanvasGroup loadingCG;
    public float fadeTime;
    public Text txt_loadingState;
    public Text txt_loadingPlayerName;
    public Text txt_loadingEnemyName;

    public bool gamePlay;

    public float time;

    private void Awake()
    {
        Instance = this;
        StartSet();
        loadingCG.alpha = 1.0f;
        PhotonNetwork.AutomaticallySyncScene = true;
        txt_loadingPlayerName.text = PhotonNetwork.PlayerList[0].NickName;
        txt_loadingEnemyName.text = PhotonNetwork.PlayerList[1].NickName;
        pv = GetComponent<PhotonView>();
        StartCoroutine(ReadyCheck());
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("TimeAdd", RpcTarget.All, Time.deltaTime);
        }
    }

    private IEnumerator ReadyCheck()
    {
        while (!playerLoading)
        {
            pv.RPC("PlayerReadyRPC", RpcTarget.All);
            yield return new WaitForSeconds(0.5f);
        }
        while (!allLoading)
        {
            pv.RPC("PlayerReadyRPC", RpcTarget.All);
            pv.RPC("AllReadyRPC", RpcTarget.All);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void ReadySet()
    {
        Vector3 kingA = new Vector3(4.5f, 1.8f, 3.5f);
        Vector3 kingB = new Vector3(4.5f, 1.8f, 5.5f);
        Vector3 kingQ = new Vector3(0.0f, 180.0f, 0.0f);
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject _kingA = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "King"), kingA, Quaternion.identity);
            kingA.x += 1.0f;
            GameObject _test1 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "1Zombie"), kingA, Quaternion.identity);
            kingA.x += 1.0f;
            GameObject _test2 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "2Skeleton"), kingA, Quaternion.identity);
            kingA.x -= 3.0f;
            GameObject _test3 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "3Knight"), kingA, Quaternion.identity);
            kingA.x -= 1.0f;
            GameObject _test4 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "4Hobgoblin"), kingA, Quaternion.identity);
            kingA.x -= 1.0f;
            GameObject _test5 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "5Robot"), kingA, Quaternion.identity);
            kingA.x -= 1.0f;
            GameObject _test6 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "6Troll"), kingA, Quaternion.identity);
            kingA.x = 7.5f;
            GameObject _test7 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "0Goblin"), kingA, Quaternion.identity);
        }
        else
        {
            GameObject _kingB = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "King"), kingB, Quaternion.identity);
            kingB.x += 1.0f;
            GameObject _test1 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "1Zombie"), kingB, Quaternion.identity);
            kingB.x += 1.0f;
            GameObject _test2 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "2Skeleton"), kingB, Quaternion.identity);
            kingB.x -= 3.0f;
            GameObject _test3 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "3Knight"), kingB, Quaternion.identity);
            kingB.x -= 1.0f;
            GameObject _test4 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "4Hobgoblin"), kingB, Quaternion.identity);
            kingB.x -= 1.0f;
            GameObject _test5 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "5Robot"), kingB, Quaternion.identity);
            kingB.x -= 1.0f;
            GameObject _test6 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "6Troll"), kingB, Quaternion.identity);
            kingB.x = 7.5f;
            GameObject _test7 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "0Goblin"), kingB, Quaternion.identity);
        }
    }

    public void SummonPiece(string pieceName, Vector3 selectV)
    {
        Vector3 offSet = new Vector3(0.5f, 1.8f, 0.5f);
        GameObject _piece = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", pieceName), selectV + offSet, Quaternion.identity);
    }

    public GameObject HandAdd(Transform _tr,bool player)
    {
        string _name;
        if (player)
        {
            _name = "PlayerHandA";
        }
        else
        {
            _name = "PlayerHandB";
        }
        GameObject _card = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", _name), _tr.position, _tr.rotation);
        return _card;
    }

    public void HandRemove(GameObject _hand)
    {
        PhotonNetwork.Destroy(_hand);
    }

    private void StartSet()
    {
        if (GameManager.Instance == null)
        {
            GameManager.Instance = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.Instance.StartSet(true);
            GameManager.Instance.UserDataSet(PhotonNetwork.PlayerList[0].NickName);
            GameManager.Instance.EnemyDataSet(PhotonNetwork.PlayerList[1].NickName);
        }
        else
        {
            GameManager.Instance.StartSet(false);
            GameManager.Instance.UserDataSet(PhotonNetwork.PlayerList[1].NickName);
            GameManager.Instance.EnemyDataSet(PhotonNetwork.PlayerList[0].NickName);
        }
    }

    public void NetworkPassTurn()
    {
        pv.RPC("PassTurn", RpcTarget.Others);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        DisconnectPlayer();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        StartCoroutine(DisconnectAndLoad());
    }

    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    public IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected) yield return null;
        SceneManager.LoadScene(1);
    }

    public IEnumerator LoadingEnd()
    {
        allLoading = true;
        ReadySet();
        txt_loadingState.fontSize = 24;
        txt_loadingState.text = "잠시 후 게임이 시작합니다..";
        loadingCG.blocksRaycasts = true;
        float fadeSpeed = 1.0f / fadeTime;

        while (!Mathf.Approximately(loadingCG.alpha, 0.0f))
        {
            loadingCG.alpha = Mathf.MoveTowards(loadingCG.alpha, 0.0f, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        loadingCG.blocksRaycasts = false;
        yield return new WaitForSeconds(1.0f);
        gamePlay = true;
    }

    [PunRPC]
    void PlayerReadyRPC()
    {
        playerLoading = true;
    }

    [PunRPC]
    void AllReadyRPC()
    {
        allLoading = true;
        StartCoroutine(LoadingEnd());
    }

    [PunRPC]
    void TimeAdd(float deltaTime)
    {
        time += deltaTime;
    }

    [PunRPC]
    void PassTurn()
    {
        GameManager.Instance.NetTurnChange();
    }
}
