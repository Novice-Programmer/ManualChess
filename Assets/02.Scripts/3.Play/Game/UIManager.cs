﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Setting")]
    public GameObject bars;
    public GameObject pieceSet;
    public CanvasGroup attackF;
    public CanvasGroup skillF;
    public CanvasGroup moveF;
    public GameObject attackTF;
    public GameObject skillTF;
    public GameObject moveTF;
    public GameObject atkActiveLine;
    public GameObject skiActiveLine;
    public GameObject movActiveLine;
    public Text attackMana;
    public Text skillMana;
    public Text moveMana;
    public Canvas etcCanvas;
    private Camera etcCamera;
    private RectTransform rectParent;
    private RectTransform rectPiece;

    [Header("GameState")]
    public Text gameTurnText;
    public GameObject gameWin;
    public Text gameWinText;
    public GameObject gameLose;
    public Text gameLoseText;

    [Header("Drow")]
    public Text drowViewTxt;
    public GameObject drowPanel;
    public GameObject drowGroup;
    public GameObject drowBtn;
    public GameObject drowViewBtn;
    public GameObject drowCard;
    public List<Card> drowCardLists;

    [Header("Grave")]
    public Text graveHVTxt;
    public Text graveNumTxt;
    public GameObject gravePanel;
    public GameObject graveExitBtn;
    public CanvasGroup graveBackground;

    [Header("State")]
    public GameObject kingHP;
    public Text kingHPText;
    public GameObject mana;
    public GameObject[] manas;
    public GameObject[] manaOn;
    public GameObject[] manaAble;

    [Header("Turn")]
    public CanvasGroup turnViewCanvas;
    public Text turnViewText;
    private readonly float fadeDuration = 2.1f;

    [Header("Network")]
    public Text userName;
    public Outline userNameline;
    public Text enemyName;
    public Outline enemyNameline;
    public Text enemyHpText;
    public GameObject enemyKingHP;
    public GameObject enemyMana;
    public GameObject[] enemyManas;
    public GameObject[] enemyManaOn;
    public GameObject[] enemyManaAble;

    public void StartSet()
    {
        rectParent = etcCanvas.GetComponent<RectTransform>();
        rectPiece = pieceSet.GetComponent<RectTransform>();
        etcCamera = etcCanvas.worldCamera;
        manas = new GameObject[mana.transform.childCount];
        manaOn = new GameObject[mana.transform.childCount];
        manaAble = new GameObject[mana.transform.childCount];
        for (int i = 0; i < mana.transform.childCount; i++)
        {
            manas[i] = mana.transform.GetChild(i).gameObject;
            manaOn[i] = manas[i].transform.GetChild(1).gameObject;
            manaAble[i] = manas[i].transform.GetChild(2).gameObject;
        }
        enemyManas = new GameObject[enemyMana.transform.childCount];
        enemyManaOn = new GameObject[enemyMana.transform.childCount];
        enemyManaAble = new GameObject[enemyMana.transform.childCount];
        for (int i = 0; i < enemyMana.transform.childCount; i++)
        {
            enemyManas[i] = enemyMana.transform.GetChild(i).gameObject;
            enemyManaOn[i] = enemyManas[i].transform.GetChild(1).gameObject;
            enemyManaAble[i] = enemyManas[i].transform.GetChild(2).gameObject;
        }
        pieceSet.GetComponent<CanvasGroup>().alpha = 0.0f;
    }

    #region 셋팅관련

    // 턴 바뀌면 변경되는 UI들
    public void TurnObjectSet(bool _playerTurn, int _playerMana, int _ableMana, int _turnValue)
    {
        if (_playerTurn)
        {
            drowBtn.SetActive(true);
        }
        else
        {
            drowBtn.SetActive(false);
        }
        TurnViewSet(_playerTurn);
        drowViewTxt.text = "필드보기";
        TurnSet(_turnValue);
        ManaSet(_playerMana, _ableMana, 0);
    }

    // 키 UI 위치 수정
    public void PieceSetView(bool _check, Piece _selectPiece,int _playerMana,bool _playerTurn)
    {
        if (_check)
        {
            pieceSet.GetComponent<CanvasGroup>().alpha = 1.0f;
            RectTransform rectParent = etcCanvas.GetComponent<RectTransform>();
            var screenPos = Camera.main.WorldToScreenPoint(_selectPiece.transform.position);
            if (screenPos.z < 0.0f)
            {
                screenPos *= -1.0f;
            }
            var localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, etcCamera, out localPos);
            rectPiece.localPosition = localPos;
            attackMana.text = ""+_selectPiece.attackMana;
            skillMana.text = "" + _selectPiece.skillMana;
            moveMana.text = "" + _selectPiece.moveMana;
            attackF.alpha = 0.0f;
            skillF.alpha = 0.0f;
            moveF.alpha = 0.0f;
            if (!_selectPiece.action)
            {
                attackF.alpha = 1.0f;
                skillF.alpha = 1.0f;
            }
            if (!_selectPiece.move)
            {
                moveF.alpha = 1.0f;
            }
            if (!_selectPiece.isPlayer)
            {
                return;
            }
            if (_playerTurn)
            {
                if (_selectPiece.attackMana > _playerMana)
                {
                    attackF.alpha = 1.0f;
                }
                if (_selectPiece.skillMana > _playerMana || !_selectPiece.PieceMPCheck())
                {
                    skillF.alpha = 1.0f;
                }
                if (_selectPiece.moveMana > _playerMana)
                {
                    moveF.alpha = 1.0f;
                }
            }
            else
            {
                attackF.alpha = 1.0f;
                skillF.alpha = 1.0f;
                moveF.alpha = 1.0f;
            }

        }

        else
        {
            pieceSet.GetComponent<CanvasGroup>().alpha = 0.0f;
        }
    }

    // 키 확인
    public void KeyView(int _keyValue)
    {
        atkActiveLine.SetActive(false);
        skiActiveLine.SetActive(false);
        movActiveLine.SetActive(false);
        if (_keyValue == 0)
        {
            attackTF.gameObject.SetActive(false);
            skillTF.gameObject.SetActive(false);
            moveTF.gameObject.SetActive(false);
        }
        else if(_keyValue == 1)
        {
            movActiveLine.SetActive(true);
            skillTF.gameObject.SetActive(false);
            attackTF.gameObject.SetActive(false);
        }
        else if (_keyValue == 2)
        {
            atkActiveLine.SetActive(true);
            skillTF.gameObject.SetActive(false);
            moveTF.gameObject.SetActive(false);
        }
        else if (_keyValue == 3)
        {
            skiActiveLine.SetActive(true);
            attackTF.gameObject.SetActive(false);
            moveTF.gameObject.SetActive(false);
        }
        else if(_keyValue == 4)
        {
            attackTF.gameObject.SetActive(true);
            skillTF.gameObject.SetActive(true);
            moveTF.gameObject.SetActive(true);
            BoardLight.Instance.HideRange();
        }
    }
    #endregion

    #region 턴

    public void TurnSet(int _turn)
    {
        gameTurnText.text = "" + _turn;
    }

    public void TurnViewSet(bool _playerTurn)
    {
        if (_playerTurn)
        {
            turnViewText.text = "플레이어 턴";
        }
        else
        {
            turnViewText.text = "상대 턴";
        }
        StartCoroutine(FadeTurnView());
    }

    private IEnumerator FadeTurnView()
    {
        turnViewCanvas.alpha = 1.0f;
        turnViewCanvas.blocksRaycasts = true;
        float fadeSpeed = turnViewCanvas.alpha / fadeDuration;
        while (!Mathf.Approximately(turnViewCanvas.alpha, 0.0f))
        {
            turnViewCanvas.alpha = Mathf.MoveTowards(turnViewCanvas.alpha, 0.0f, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        turnViewCanvas.blocksRaycasts = false;
    }

    #endregion

    #region 마나 및 체력

    public void ManaSet(int mana,int ableMana,int useMana)
    {
        for(int i = 0; i < manas.Length; i++)
        {
            manaOn[i].GetComponent<Image>().fillAmount = 0.0f;
            manaAble[i].GetComponent<Image>().fillAmount = 0.0f;
        }

        for (int i = 0; i < mana + useMana; i++)
        {
            if (i >= useMana)
            {
                manaOn[i].GetComponent<Image>().fillAmount = 1.0f;
            }
        }

        int ableNum = mana + useMana - ableMana;

        if (ableNum > 0)
        {
            for (int i = ableMana; i < mana + useMana; i++)
            {
                manaAble[i].GetComponent<Image>().fillAmount = 1.0f;
            }
        }
    }

    public void ManaChange(int useMana)
    {
        if (useMana > 0)
        {
            for (int i = 0; i < useMana; i++)
            {
                manaOn[i].GetComponent<Image>().fillAmount = 0.0f;
            }
        }

    }

    public void KingHPSet(int _kingHP,float _kingMaxHP)
    {
        if (_kingHP > 0)
        {
            kingHP.GetComponent<Image>().fillAmount = _kingHP / _kingMaxHP;
            kingHPText.text = "" + _kingHP;
        }
        else
        {
            kingHP.GetComponent<Image>().fillAmount = 0.0f;
            kingHPText.text = "" + 0;
        }
    }

    #endregion

    #region 드로우

    public void BtnDrowClick()
    {
        if (!GameManager.Instance.option)
        {
            if (GameManager.Instance.Drow())
            {
                drowPanel.SetActive(true);
                drowGroup.SetActive(true);
                drowViewBtn.SetActive(true);
                drowViewTxt.text = "필드보기";
                DrowListView();
            }
            else
            {

            }
            drowBtn.SetActive(false);
        }
    }

    public void BtnDrowViewClick()
    {
        if (!GameManager.Instance.option && !Graveyard.Instance.graveViewB)
        {
            if (drowViewTxt.text == "카드보기")
            {
                GameManager.Instance.drowSelect = true;
                drowPanel.SetActive(true);
                drowGroup.SetActive(true);
                drowViewTxt.text = "필드보기";
            }
            else
            {
                GameManager.Instance.drowSelect = false;
                drowPanel.SetActive(false);
                drowGroup.SetActive(false);
                drowViewTxt.text = "카드보기";
            }
        }
    }

    public void DrowListView()
    {
        List<int> drowList = GameManager.Instance.drowsList;
        GameObject card;
        for (int i = 0; i < drowList.Count; i++)
        {
            card = Instantiate(drowCard, drowGroup.transform);
            card.GetComponent<Card>().NumSet(drowList[i],i);
            drowCardLists.Add(card.GetComponent<Card>());
        }
    }

    public void DrowListClear(int selectNum)
    {
        Destroy(drowGroup.transform.GetChild(selectNum).gameObject);
        drowCardLists.RemoveAt(selectNum);
        List<int> drowList = GameManager.Instance.drowsList;
        drowPanel.SetActive(false);
        drowViewTxt.text = "카드보기";
        for (int i = 0; i < drowCardLists.Count; i++)
        {
            drowCardLists[i].SelectNumSet(i);
        }
    }

    public void DrowEndClear()
    {
        for(int i = 0; i < drowGroup.transform.childCount; i++)
        {
            Destroy(drowGroup.transform.GetChild(i).gameObject);
        }
        drowCardLists.RemoveRange(0,drowCardLists.Count);
        drowPanel.SetActive(false);
        drowViewBtn.SetActive(false);
    }

    #endregion

    #region 묘지
    public void GravePanelView()
    {
        gravePanel.SetActive(true);
    }

    public void GraveNumSet(int _viewNum, int _graveNum)
    {
        graveNumTxt.text = (_viewNum + 1) + " / " + _graveNum;
    }

    public void BtnGHVClick()
    {
        if (graveHVTxt.text == "숨기기")
        {
            Graveyard.Instance.GraveyardViewHide();
            graveBackground.alpha = 0.1f;
            graveHVTxt.text = "보이기";
        }
        else
        {
            Graveyard.Instance.GraveyardView();
            graveBackground.alpha = 1.0f;
            graveHVTxt.text = "숨기기";
        }
    }

    public void BtnGExitClick()
    {
        GraveyardEnd();
    }

    public void GraveyardEnd()
    {
        GameManager.Instance.graveSelect = false;
        Graveyard.Instance.GraveyardViewHide();
        gravePanel.SetActive(false);
    }
    #endregion

    #region 네트워크

    public void UserInfoSet(UserData userData)
    {
        if (GameManager.Instance.player)
        {
            userNameline.effectColor = new Color(255.0f,0.0f,0.0f);
        }
        else
        {
            userNameline.effectColor = new Color(0.0f, 0.0f, 255.0f);
        }
        userName.text = userData.userName;
    }

    public void EnemyInfoSet(UserData enemyData)
    {
        if (GameManager.Instance.player)
        {
            enemyNameline.effectColor = new Color(0.0f, 0.0f, 255.0f);
        }
        else
        {
            enemyNameline.effectColor = new Color(255.0f, 0.0f, 0.0f);
        }
        enemyName.text = enemyData.userName;
    }

    public void EnemyMana(int _enemyMana, int _enemyUseMana, int _enemyAbleMana)
    {
        for (int i = 0; i < enemyManas.Length; i++)
        {
            enemyManaOn[i].GetComponent<Image>().fillAmount = 0.0f;
            enemyManaAble[i].GetComponent<Image>().fillAmount = 0.0f;
        }

        for (int i = 0; i < _enemyMana + _enemyUseMana; i++)
        {
            if (i >= _enemyUseMana)
            {
                enemyManaOn[i].GetComponent<Image>().fillAmount = 1.0f;
            }
        }

        int ableNum = _enemyMana + _enemyUseMana - _enemyAbleMana;

        if (ableNum > 0)
        {
            for (int i = _enemyAbleMana; i < _enemyMana + _enemyUseMana; i++)
            {
                enemyManaAble[i].GetComponent<Image>().fillAmount = 1.0f;
            }
        }
    }

    public void EnemyHP(int _enemyHP)
    {
        int enemyMaxHP = 50;
        if (_enemyHP > 0)
        {
            enemyKingHP.GetComponent<Image>().fillAmount = (float)_enemyHP / enemyMaxHP;
            enemyHpText.text = "" + _enemyHP;
        }
        else
        {
            enemyKingHP.GetComponent<Image>().fillAmount = 0.0f;
            enemyHpText.text = "" + 0;
        }
    }

    public void GameWin(string userName)
    {
        gameWin.SetActive(true);
        gameWinText.text = userName + " 승리";
    }

    public void GameLose(string userName)
    {
        gameLose.SetActive(true);
        gameLoseText.text = userName + " 패배";
    }

    #endregion
}
