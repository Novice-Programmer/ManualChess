using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    public AudioClip buttonClickClip;
    public Text nameText;
    public Text sizeText;

    public string roomName;
    private int roomSize; 
    private int playerCount;

    public void JoinRoomOnClick() 
    {
        AlwaysObject.Instance.SoundOn(buttonClickClip);
        PhotonNetwork.JoinRoom(roomName);
    }

    public void SetRoom(string nameInput, int sizeInput, int countInput)
    {
        roomName = nameInput;
        roomSize = sizeInput;
        playerCount = countInput;
        nameText.text = nameInput;
        sizeText.text = countInput + "/" + sizeInput;
    }
}
