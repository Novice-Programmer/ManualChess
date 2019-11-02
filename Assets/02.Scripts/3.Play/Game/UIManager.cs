using System.Collections;
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
    public Text enemyName;
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
    public void PieceSetView(bool _check, Piece _selectPiece,float _playerMana)
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
            pieceSet.GetComponent<CanvasGroup>().alpha = 0.0f;
        }
    }

    // 키 확인
    public void KeyView(int _keyValue)
    {
        if(_keyValue == 0)
        {
            attackTF.gameObject.SetActive(false);
            skillTF.gameObject.SetActive(false);
            moveTF.gameObject.SetActive(false);
        }
        else if(_keyValue == 1)
        {
            skillTF.gameObject.SetActive(false);
            attackTF.gameObject.SetActive(false);
        }
        else if (_keyValue == 2)
        {
            skillTF.gameObject.SetActive(false);
            moveTF.gameObject.SetActive(false);
        }
        else if (_keyValue == 3)
        {
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
        gameTurnText.text = _turn + "턴";
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
            kingHPText.text = _kingHP + " / " + (int)_kingMaxHP;
        }
        else
        {
            kingHP.GetComponent<Image>().fillAmount = 0.0f;
            kingHPText.text = 0 + " / " + (int)_kingMaxHP;
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
            graveHVTxt.text = "보이기";
        }
        else
        {
            Graveyard.Instance.GraveyardView();
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
        userName.text = userData.userName;
    }

    public void EnemyInfoSet(UserData enemyData)
    {
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
            enemyHpText.text = _enemyHP + " / " + enemyMaxHP;
        }
        else
        {
            enemyKingHP.GetComponent<Image>().fillAmount = 0.0f;
            enemyHpText.text = 0 + " / " + enemyMaxHP;
        }
    }

    public void GameWin(string userName)
    {
        gameWin.SetActive(true);
        gameWinText.text = userName + "승리";
    }

    public void GameLose(string userName)
    {
        gameLose.SetActive(true);
        gameLoseText.text = userName + "패배";
    }

    #endregion
}
