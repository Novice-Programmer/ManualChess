using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graveyard : MonoBehaviour
{
    public static Graveyard Instance { set; get; }

    public GraveText graveText;

    public GameObject graveView;
    public GameObject graveCardViewC;
    public GameObject graveGroup;
    public GameObject graveEffect;
    public GameObject cardTransform;
    public GameObject gCard;
    public GameObject graveCard;
    public GameObject graveAdd;

    public Animator addAnimator;

    public List<int> graveIntList; // 묘지에 저장된 인트형 변수들
    public List<GameObject> gravePosList;
    public List<GraveCard> graveCardList;

    public int graveNum;
    public int selectNum;
    public bool graveViewB;
    public GraveCard selectGraveCard;
    public GraveCard viewGraveCard;

    private Vector3 cardScale;
    private Vector3 cardViewScale;
    private Vector3 cardAddScale;

    private Vector3 graveCardV = new Vector3(-2.0f, 0.85f, 4.5f);
    private Vector3 graveCardAddV = new Vector3(0.0f, 0.05f, 0.0f);
    private Vector3 graveViewQ = new Vector3(0.0f, 112.5f, 0.0f);
    private Vector3 graveViewTQ = new Vector3(0.0f,65.0f,0.0f);

    private Quaternion graveViewAQ = Quaternion.Euler(35.0f, 0.0f, 0.0f);
    private Quaternion graveViewBQ = Quaternion.Euler(-215.0f, 0.0f, 180.0f);

    private Quaternion graveViewT;
    private Quaternion graveViewF;

    // Start is called before the first frame update
    void Start()
    {
        StartSet();
    }

    private void Update()
    {
        if (graveViewB)
        {
            float wheelValue = Input.GetAxis("Mouse ScrollWheel");

            if (Mathf.Abs(wheelValue) > 0.0f)
            {
                WheelCheck(wheelValue);
            }
        }
    }

    // 시작 셋팅
    private void StartSet()
    {
        Instance = this;
        cardScale = graveCard.transform.localScale;
        cardAddScale = cardScale;
        cardAddScale.x *= 1.25f;
        cardAddScale.y *= 1.25f;
        cardViewScale = cardScale;
        cardViewScale.x *= 1.1f;
        cardViewScale.y *= 1.1f;
        gravePosList = new List<GameObject>();
        graveCardList = new List<GraveCard>();
        for (int i = 0; i < graveGroup.transform.childCount; i++)
        {
            gravePosList.Add(graveGroup.transform.GetChild(i).gameObject);
        }
        if (GameManager.Instance.player)
        {
            graveViewF = graveViewAQ;
        }
        else
        {
            graveViewF = graveViewBQ;
        }
    }

    // 묘지 회전 체크
    private void WheelCheck(float _wheelValue)
    {
        int addValue = (int)(_wheelValue * 10.0f);
        if (GraveViewNumCheck(addValue))
        {
            Vector3 _graveViewQ;

            if (_wheelValue > 0.0f)
            {
                _graveViewQ = graveViewQ * 0.1f;
            }
            else
            {
                _graveViewQ = graveViewQ * -0.1f;
            }
            graveGroup.transform.Rotate(_graveViewQ);
            GameManager.Instance.uiManager.GraveNumSet(selectNum, graveNum);
        }
    }

    // 묘지 회전값 체크
    private bool GraveViewNumCheck(int _addValue)
    {
        if (_addValue < 0)
        {
            if (graveNum == 32)
            {
                if (selectNum == 0)
                {
                    selectNum = 31;
                }
                else
                {
                    selectNum--;
                }
                return true;
            }
            else
            {
                if (selectNum == 0)
                {
                    return false;
                }
                else
                {
                    selectNum--;
                    return true;
                }
            }
        }
        else
        {
            if (selectNum != graveNum - 1)
            {
                selectNum++;
                return true;
            }
            else
            {
                if (selectNum == 31 && graveNum == 32)
                {
                    selectNum = 0;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    // 묘지 텍스트 변경
    private void GraveNumSet(int _graveNum)
    {
        graveText.GraveNumChange(_graveNum);
    }

    public void GraveMouseView()
    {
        graveText.MouseView();
    }

    public void GraveNoneMouseView()
    {
        graveText.NoneMouseView();
    }

    // 카드 확인
    public bool GraveCardCheck(GameObject _gParent)
    {
        if (_gParent.transform.childCount > 1)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    // 카드 선택
    public void SelectGrave(RaycastHit _hit)
    {
        GameObject _gParent = _hit.transform.parent.gameObject;
        if (GraveCardCheck(_gParent))
        {
            selectGraveCard = _gParent.transform.GetChild(1).GetComponent<GraveCard>();
            if (GameManager.Instance.DrowCheck(selectGraveCard.gcard_DrowMana))
            {
                NetworkManager.Instance.networkAction.GraveyardDrow(selectGraveCard.selectNum, GameManager.Instance.player);
                GameManager.Instance.GraveNotice();
            }
        }
        else
        {
            selectGraveCard = null;
        }
    }

    // 현재 보고 있는 카드
    public void ViewGraveCard(RaycastHit _hit)
    {
        GameObject _gParent = _hit.transform.parent.gameObject;
        if (GraveCardCheck(_gParent))
        {
            GraveCard _viewGraveCard = _gParent.transform.GetChild(1).GetComponent<GraveCard>();
            if (viewGraveCard != null)
            {
                if (viewGraveCard == _viewGraveCard)
                {
                    return;
                }
                else
                {
                    viewGraveCard.transform.localScale = cardScale;
                }
            }
            else
            {

            }
            viewGraveCard = _viewGraveCard;
            graveCardViewC.transform.localScale = cardAddScale;
            viewGraveCard.transform.localScale = cardAddScale;
        }
        else
        {
            viewGraveCard = null;
        }
    }

    // 묘지 카드 상태 초기화
    public void GraveCardReset()
    {
        if (viewGraveCard != null)
        {
            viewGraveCard.transform.localScale = cardScale;
            graveCardViewC.transform.localScale = cardScale;
            viewGraveCard = null;
        }
        else
        {
            return;
        }
    }

    // 묘지 카드 수 확인
    public bool GraveyardCheck()
    {
        if (graveIntList.Count>0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // 묘지 보기
    public void GraveyardView()
    {
        graveGroup.transform.Rotate(graveViewTQ);
        graveViewT = graveGroup.transform.rotation;
        graveGroup.transform.rotation = graveViewF;
        graveViewB = true;
        graveGroup.SetActive(true);
        graveEffect.SetActive(true);
        selectNum = 0;
        GameManager.Instance.uiManager.GraveNumSet(selectNum, graveNum);
    }

    // 묘지 가리기
    public void GraveyardViewHide()
    {
        graveViewB = false;
        graveGroup.SetActive(false);
        graveEffect.SetActive(false);
        GraveCardReset();
        graveGroup.transform.rotation = graveViewF;
    }

    #region 네트워크

    public void NetGraveyardAdd(int _handNum, int _orderNum,  bool _player)
    {
        if (_player != GameManager.Instance.player)
        {
            AlwaysObject.Instance.InfoStart("상대가 묘지에서 카드를 버렸습니다.");
            addAnimator.SetTrigger("Add");
        }

        graveIntList.Add(_orderNum);
        GameObject gVCard = Instantiate(gCard, cardTransform.transform);
        Quaternion randomQ = Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
        gVCard.transform.rotation = randomQ;
        Vector3 randomV = new Vector3(Random.Range(-0.2f, 0.2f), 0.0f, Random.Range(-0.2f, 0.2f));
        gVCard.transform.position = graveCardV + randomV + (graveCardAddV * graveNum);
        graveAdd.transform.position += graveCardAddV;
        graveText.GraveTextAddPos(graveCardAddV);
        GameObject _graveCardObj;
        _graveCardObj = Instantiate(graveCard, gravePosList[graveNum].transform);
        _graveCardObj.GetComponent<GraveCard>().GraveCardSet(_orderNum, graveNum);
        graveCardList.Add(_graveCardObj.GetComponent<GraveCard>());
        graveNum++;
        GraveNumSet(graveNum);
    }

    public void NetGraveyardRemove(int _selectNum, bool _player)
    {
        if (_player == GameManager.Instance.player)
        {
            GameManager.Instance.NetManaCrease(-graveCardList[_selectNum].gcard_DrowMana);
            HandManager.Instance.NetPlayerDrow(graveIntList[_selectNum]);
        }
        else
        {
            AlwaysObject.Instance.InfoStart("상대가 묘지에서 카드를 드로우 했습니다.");
        }
        graveText.GraveTextAddPos(-graveCardAddV);
        graveAdd.transform.position = graveAdd.transform.position - graveCardAddV;
        graveIntList.RemoveAt(_selectNum);
        graveNum--;
        GraveNumSet(graveNum);
        Destroy(cardTransform.transform.GetChild(cardTransform.transform.childCount - 1).gameObject);
        graveCardList.RemoveAt(_selectNum);
        if (_selectNum >= 16)
        {
            gravePosList.RemoveAt(_selectNum);
            Destroy(gravePosList[_selectNum].gameObject);
        }
        else
        {
            Destroy(gravePosList[_selectNum].transform.GetChild(1).gameObject);
        }
        GraveyardArray(_selectNum);
    }

    public void GraveyardArray(int _selectNum)
    {
        for(int i = 0; i < graveCardList.Count; i++)
        {
            if (_selectNum <= i)
            {
                graveCardList[i].selectNum--;
                graveCardList[i].transform.SetPositionAndRotation(gravePosList[i].transform.position, gravePosList[i].transform.rotation);
                graveCardList[i].transform.parent = gravePosList[i].transform;
            }
        }
    }
    #endregion
}
