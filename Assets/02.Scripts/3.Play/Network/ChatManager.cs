using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ChatManager : MonoBehaviourPunCallbacks
{
    private PhotonView pv;
    public GameObject chatPanel;
    private CanvasGroup chatCG;
    public Button btn_chatView;
    public Button btn_chatExit;
    public Animator chatViewAnim;
    public GameObject chatTextObject;
    public RectTransform content;
    public InputField chatInput;
    private bool chatView;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        chatCG = chatPanel.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!chatView)
        {
            if (Input.GetButtonDown("Submit"))
            {
                ChatViewChange(chatView);
            }
        }
        else
        {
            if (Input.GetButtonDown("Cancel"))
            {
                ChatViewChange(chatView);
            }

            else if (Input.anyKey && !chatInput.isFocused)
            {
                chatInput.Select();
            }
        }
    }

    private void ChatViewChange(bool _chatView)
    {
        if (_chatView)
        {
            btn_chatView.gameObject.SetActive(true);
            btn_chatExit.gameObject.SetActive(false);
            chatView = false;
            chatCG.alpha = 0.0f;
            chatCG.blocksRaycasts = false;
        }
        else
        {
            btn_chatView.gameObject.SetActive(false);
            btn_chatExit.gameObject.SetActive(true);
            chatView = true;
            chatCG.alpha = 1.0f;
            chatCG.blocksRaycasts = true;
        }
    }

    public void Btn_ChatView()
    {
        ChatViewChange(chatView);
    }

    public void Btn_Send()
    {
        Send();
    }

    public void Send()
    {
        if (chatInput.text == "")
        {
            return;
        }
        if (!chatView)
        {
            chatInput.text = "";
            return;
        }
        string msg = PhotonNetwork.NickName + " : " + chatInput.text;
        pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + chatInput.text, GameManager.Instance.player);
        chatInput.ActivateInputField();
        chatInput.Select();
        chatInput.text = "";
    }

    [PunRPC]
    void ChatRPC(string msg,bool player)
    {
        GameObject _chatText = Instantiate(chatTextObject, content);
        _chatText.GetComponent<Text>().text = msg;
        if (content.childCount > 30)
        {
            Destroy(content.GetChild(0).gameObject);
        }

        if (GameManager.Instance.player != player && !chatView)
        {
            chatViewAnim.SetTrigger("ChatLight");
            AlwaysObject.Instance.InfoStart("새로운 채팅이 입력되었습니다.");
        }
    }
}
