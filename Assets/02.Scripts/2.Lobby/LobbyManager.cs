using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("NamePanel")]
    public GameObject namePanel;
    public InputField txt_playerNameInput;
    public Text txt_lobbyState;
    public Button btn_loading;
    public Button btn_lobbyEntry;

    [Header("LobbyPanel")]
    public GameObject lobbyPanel;
    public Transform roomsContainer;
    public Text txt_totalPlayer;
    public Text txt_playerName;
    public InputField txt_roomNameInput;
    private List<RoomInfo> roomListings;
    public GameObject roomListingPrefab;
    private string roomName = "";
    private const int roomSize = 2;

    [Header("RoomPanel")]
    public GameObject roomPanel;
    public Transform playersContainer;
    public GameObject playerListingPrefab;
    public GameObject btn_start;
    public GameObject chatTextObject;
    public InputField chatInput;
    public RectTransform content;
    public Text txt_roomNameDisplay;
    private int playerCount;

    [Header("MatchPanel")]
    public GameObject matchPanel;
    private bool matching;

    [Header("GameLoadingPanel")]
    public GameObject loadingPanel;
    public Text txt_loadingPlayerName;
    public Text txt_loadingEnemyName;

    [Header("LobbyMgr")]
    public AudioSource bgmSource;
    public AudioClip[] bgmClips;
    private int bgmNum = 1;
    public AudioClip buttonClickClips;
    public AudioClip sendClips;

    public int mainSceneIndex;
    public int gameSceneIndex;

    private PhotonView pv;
    private string playerName = "";

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        pv = GetComponent<PhotonView>();
        roomListings = new List<RoomInfo>();
        AlwaysObject.Instance.BgmSourceSet(bgmSource);
        StartCoroutine(TotalPlayerCheck());
    }

    void Update()
    {
        if (!bgmSource.isPlaying)
        {
            bgmSource.PlayOneShot(bgmClips[bgmNum]);
            bgmNum++;
        }

        if (bgmNum == bgmClips.Length)
        {
            bgmNum = 0;
        }
    }

    IEnumerator TotalPlayerCheck()
    {
        while (true)
        {
            if (PhotonNetwork.InLobby)
            {
                txt_totalPlayer.text = "로비 : " + (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "명 / 총 : " + PhotonNetwork.CountOfPlayers + "명";
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

    #region 네트워크

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        SceneManager.LoadScene(mainSceneIndex);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        btn_loading.gameObject.SetActive(false);

        btn_lobbyEntry.gameObject.SetActive(true);

        if (playerName == "")
        {
            playerName = "Player " + Random.Range(0, 99);
        }
        txt_playerNameInput.text = playerName;
    }

    #endregion

    #region 이름

    public void Btn_JoinLobby()
    {
        AlwaysObject.Instance.SoundOn(buttonClickClips);
        if (txt_playerNameInput.text != "")
        {
            namePanel.SetActive(false);
            StartCoroutine(JoinLobby());
        }
        else
        {
            AlwaysObject.Instance.InfoStart("플레이어 명을 설정해주세요.");
        }
    }

    IEnumerator JoinLobby()
    {
        AlwaysObject.Instance.LoadingStart();
        PhotonNetwork.NickName = txt_playerNameInput.text;
        playerName = PhotonNetwork.NickName;
        PhotonNetwork.JoinLobby();
        while (!PhotonNetwork.InLobby) yield return null;
        AlwaysObject.Instance.LoadingEnd();
    }

    public void Change_PlayerName(string nameInput)
    {
        PhotonNetwork.NickName = nameInput;
        PlayerPrefs.SetString("NickName", nameInput);
    }

    #endregion

    #region 로비

    #region 버튼

    public void Btn_Match()
    {
        AlwaysObject.Instance.SoundOn(buttonClickClips);
        StartCoroutine(Match());
    }

    public void Btn_ReJoinLobby()
    {
        AlwaysObject.Instance.SoundOn(buttonClickClips);
        StartCoroutine(ReJoinLobby());
    }

    public void Btn_MainOnClick()
    {
        AlwaysObject.Instance.SoundOn(buttonClickClips);
        StartCoroutine(MainSceneChange());
    }

    public void Btn_CreateRoom()
    {
        AlwaysObject.Instance.SoundOn(buttonClickClips);
        CreateRoom(false);
    }

    #endregion

    #region 버튼 처리

    IEnumerator Match()
    {
        AlwaysObject.Instance.LoadingStart();
        matching = true;
        PhotonNetwork.JoinRandomRoom();
        while (!PhotonNetwork.InRoom) yield return null;
        AlwaysObject.Instance.LoadingEnd();
    }

    IEnumerator ReJoinLobby()
    {
        AlwaysObject.Instance.LoadingStart();
        PhotonNetwork.LeaveLobby();
        while (PhotonNetwork.InLobby) yield return null;
        PhotonNetwork.JoinLobby();
        while (!PhotonNetwork.InLobby) yield return null;
        PhotonNetwork.NickName = playerName;
        AlwaysObject.Instance.LoadingEnd();
    }

    IEnumerator MainSceneChange()
    {
        AlwaysObject.Instance.LoadingStart();
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected) yield return null;
        SceneManager.LoadScene(mainSceneIndex);
        AlwaysObject.Instance.LoadingEnd();
    }

    #endregion

    #region 방 생성 관련

    public void CreateRoom(bool nameCheck)
    {
        AlwaysObject.Instance.LoadingStart();
        roomName = txt_roomNameInput.text;
        int randomRoomNumber = Random.Range(0, 100);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        if (nameCheck == false)
        {
            if (roomName == "")
            {
                if (matching)
                {
                    PhotonNetwork.CreateRoom("빠른 매칭 : " + randomRoomNumber + "번 방", roomOps);
                }
                else
                {
                    PhotonNetwork.CreateRoom(randomRoomNumber + "번 방", roomOps);
                }
            }
            else
            {
                PhotonNetwork.CreateRoom(roomName, roomOps);
            }
        }
        else
        {
            if(roomName == "")
            {
                PhotonNetwork.CreateRoom(randomRoomNumber + "번 방", roomOps);
            }
            else
            {
                PhotonNetwork.CreateRoom(roomName + " (1)", roomOps);
            }
        }
        lobbyPanel.SetActive(false);
        AlwaysObject.Instance.LoadingEnd();
    }

    public void Change_RoomName(string nameIn)
    {
        roomName = nameIn;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom(false);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateRoom(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int tempIndex;

        foreach (RoomInfo room in roomList)
        {
            if (roomListings != null)
            {
                for (int i = 0; i < roomsContainer.childCount; i++)
                {
                    tempIndex = roomListings.FindIndex(ByName(room.Name));
                    if (tempIndex != -1)
                    {
                        roomListings.RemoveAt(tempIndex);
                    }

                    if (room.Name == roomsContainer.GetChild(i).GetComponent<RoomButton>().roomName)
                    {
                        Destroy(roomsContainer.GetChild(i).gameObject);
                    }
                }
            }

            if (room.PlayerCount > 0)
            {
                roomListings.Add(room);
                ListRoom(room);
            }
        }
    }

    public void DestroyRoom()
    {
        for (int i = 0; i < roomsContainer.childCount; i++)
        {
            Destroy(roomsContainer.GetChild(i).gameObject);
        }
    }

    private void ListRoom(RoomInfo room)
    {
        if (room.IsOpen && room.IsVisible)
        {
            GameObject tempListing = Instantiate(roomListingPrefab, roomsContainer);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.SetRoom(room.Name, room.MaxPlayers, room.PlayerCount);
        }
    }

    private static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }

    #endregion

    #region 네트워크

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        lobbyPanel.SetActive(true);
        if (playerName != "")
        {
            PhotonNetwork.NickName = playerName;
        }
        txt_totalPlayer.text = "로비 : " + (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "명 / 총 : " + PhotonNetwork.CountOfPlayers + "명";
        txt_playerName.text = PhotonNetwork.NickName;
        roomListings = new List<RoomInfo>();
        roomName = "";
    }

    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        lobbyPanel.SetActive(false);
    }

    #endregion

    #endregion

    #region 매칭

    public void Btn_MatchCancel()
    {
        AlwaysObject.Instance.SoundOn(buttonClickClips);
        StartCoroutine(MatchCancel());
    }

    IEnumerator MatchCancel()
    {
        AlwaysObject.Instance.LoadingStart();
        PhotonNetwork.LeaveRoom();

        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }

        matchPanel.SetActive(false);

        while (!PhotonNetwork.InLobby)
        {
            yield return null;
        }

        AlwaysObject.Instance.LoadingEnd();
        matching = false;
    }

    #endregion

    #region 방

    #region 방 설정

    private void ClearPlayerListings()
    {
        for (int i = playersContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(playersContainer.GetChild(i).gameObject);
        }
    }

    private void ListPlayers()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject tempListing = Instantiate(playerListingPrefab, playersContainer);
            Text tempText = tempListing.transform.GetChild(1).GetComponent<Text>();
            tempText.text = player.NickName;
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        lobbyPanel.SetActive(false);

        if (matching)
        {
            matchPanel.SetActive(true);
        }
        else
        {
            roomPanel.SetActive(true);
            txt_roomNameDisplay.text = PhotonNetwork.CurrentRoom.Name;
            if (PhotonNetwork.IsMasterClient)
            {
                btn_start.SetActive(true);
            }
            else
            {
                btn_start.SetActive(false);
            }

            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }
        ClearPlayerListings();
        ListPlayers();
        PlayerCountUpdate();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        roomPanel.SetActive(false);
        StartCoroutine(LeftRoom());
    }

    IEnumerator LeftRoom()
    {
        AlwaysObject.Instance.LoadingStart();

        while (!PhotonNetwork.InLobby)
        {
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(0.8f);
        }

        AlwaysObject.Instance.LoadingEnd();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        pv.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
        if (!otherPlayer.IsMasterClient)
        {
            btn_start.SetActive(true);
        }
        ClearPlayerListings();
        ListPlayers();
        PlayerCountUpdate();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        pv.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
        PlayerCountUpdate();
        if (matching == false)
        {
            ClearPlayerListings();
            ListPlayers();
        }
    }

    private void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;

        if (playerCount == roomSize && matching)
        {
            StartGame();
        }
    }

    #endregion

    #region 버튼

    public void Btn_Start()
    {
        AlwaysObject.Instance.SoundOn(buttonClickClips);
        if (PhotonNetwork.IsMasterClient)
        {
            if (playerCount == roomSize)
            {
                StartGame();
            }
            else
            {
                AlwaysObject.Instance.InfoStart("플레이어가 부족합니다.");
            }
        }
    }

    private void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        pv.RPC("StartRPC", RpcTarget.All);
    }

    public void Btn_RoomBack()
    {
        if (PhotonNetwork.IsMasterClient && playerCount == 1)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
        PhotonNetwork.LeaveRoom();
    }

    public void Btn_Send()
    {
        Send();
    }

    public void Send()
    {
        if(chatInput.text == "")
        {
            return;
        }
        string msg = PhotonNetwork.NickName + " : " + chatInput.text;
        pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + chatInput.text);
        chatInput.text = "";
        chatInput.ActivateInputField();
        chatInput.Select();
    }

    #endregion

    #endregion

    #region PunRPC

    [PunRPC]
    void ChatRPC(string msg)
    {
        AlwaysObject.Instance.SoundOn(sendClips);
        GameObject _chatText = Instantiate(chatTextObject, content);
        _chatText.GetComponent<Text>().text = msg;
        if (content.childCount > 30)
        {
            Destroy(content.GetChild(0).gameObject);
        }
    }

    [PunRPC]
    void StartRPC()
    {
        roomPanel.SetActive(false);
        matchPanel.SetActive(false);
        txt_loadingPlayerName.text = PhotonNetwork.PlayerList[0].NickName;
        txt_loadingEnemyName.text = PhotonNetwork.PlayerList[1].NickName;
        loadingPanel.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(gameSceneIndex);
        }
    }
    

    #endregion
}
