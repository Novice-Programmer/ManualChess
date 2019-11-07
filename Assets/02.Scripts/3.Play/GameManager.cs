using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; } // 전역 변수 설정

    // 다른 설정들 포함
    [Header("Mgr")]
    public Turn turn;
    public Board board;
    public UIManager uiManager;
    public DeployRange deployRange;

    [Header("King")]
    public Piece playerKing;
    public Vector2 kingV;

    [Header("GameState")]
    public int turnValue;
    public AudioSource audioSource;
    public bool option;

    // 플레이어 상태
    [Header("State")]
    public UserData userData;
    public int playerLayerNum;
    public bool player;
    public bool playerTurn;
    public int playerKingHP;
    public int actionNum;

    // 적 상태
    [Header("EnemyState")]
    public UserData enemyData;
    public int enemyLayerNum;
    public int enemyKingHP;
    public int enemyMana;
    public int enemyAbleMana;

    // 마나 관련
    [Header("Mana")]
    public int playerMana;
    public int creaseMana;
    public const int playerMaxMana = 10;
    public const int drowMana = -1;
    public int playerUseMana;
    public int playerAbleMana;
    public int increaseManaValue = 1;
    private const int turnIncreaseMana = 4;
    private const int turnIncreaseAbleMana = 3;

    // 드로우 관련
    [Header("Drow")]
    public int handsFullValue = 80;
    public int imageNumvalue;
    public int drowNumValue = 4;
    public int drowChangeValue = 4;

    public static float drowCountTime;
    public bool drowSelect;
    private float drowMaxTime;
    public int drowHandNum = 99;
    private bool f_drow;

    public List<int> handsList;
    public List<int> drowsList;
     
    [Header("Piece")]
    public List<Piece> aPiece;
    public List<Piece> bPiece;
    private Piece selectPiece;
    private bool select;
    private bool moveP;
    private bool attackP;
    private bool skillP;

    [Header("Hand")]
    public PlayerHand selectHand;
    public Vector3 selectHandV;
    public Quaternion selectHandQ;
    public GameObject selectViewHand;
    public Vector3 selectViewHandV;
    public Quaternion selectViewHandQ;
    public Vector3 selectViewHandScale;

    [Header("MousePoint")]
    public Vector2 mouseOver;
    public Vector2 startDrag;
    public Vector2 endDrag;
    public Vector3 mouseHandOver;
    public Vector3 startHandDrag;
    public Vector3 endHandDrag;

    [Header("ETC")]
    public int lobbySceneIntValue;
    public bool graveSelect;
    public bool gameState;
    private Vector3 boardOffset = new Vector3(0.5f, 1.8f, 0.5f);
    private Vector3 moveOffset = new Vector3(0.0f, 1.8f, 0.0f);

    private void Start()
    {
        AlwaysObject.Instance.BgmSourceSet(audioSource);
        drowMaxTime = turn.maxTime * 5 / 6;
    }

    private void Update()
    {
        if (NetworkManager.Instance.gamePlay)
        {
            RayCastCheck();
            DrowTimeCheck();
            KeyValueCheck();
            SelectCheck();
        }
    }

    #region 기본 셋팅

    // 기본 설정
    public void StartSet(bool isPlayer)
    {
        userData = new UserData();
        enemyData = new UserData();
        uiManager.StartSet();
        if (isPlayer)
        {
            player = true;
            playerTurn = true;
            playerLayerNum = 18;
            enemyLayerNum = 19;
            playerMana++;
            playerAbleMana++;
            StartCoroutine(WaitForWork());
        }
        else
        {
            player = false;
            playerTurn = false;
            playerLayerNum = 19;
            enemyLayerNum = 18;
            uiManager.EnemyMana(1, 0, 1);
        }
        handsList = new List<int>();
        turnValue++;
        PlayerCheck();
        deployRange.DeployRangeSet(isPlayer);
        board.PieceValueChange(turnValue);
        turn.TurnObjectSet(playerTurn);
        uiManager.TurnObjectSet(playerTurn, playerMana, playerAbleMana, turnValue);
        gameState = true;
    }

    IEnumerator WaitForWork()
    {
        yield return new WaitForSeconds(1.0f);
        NetworkManager.Instance.networkAction.HandSetting(imageNumvalue, handsFullValue);
    }

    // 플레이어에 따른 설정
    private void PlayerCheck()
    {
        GameObject[] pieceA = GameObject.FindGameObjectsWithTag("APiece");
        GameObject[] pieceB = GameObject.FindGameObjectsWithTag("BPiece");
        for (int i = 0; i < pieceA.Length; i++)
        {
            if (player)
            {
                pieceA[i].GetComponent<Piece>().isPlayer = true;
            }
            else
            {
                pieceA[i].GetComponent<Piece>().isPlayer = false;
            }
        }
        for (int i = 0; i < pieceB.Length; i++)
        {
            if (player)
            {
                pieceB[i].GetComponent<Piece>().isPlayer = false;
            }
            else
            {
                pieceB[i].GetComponent<Piece>().isPlayer = true;
            }
        }
    }

    public void KingHPCrease(int creaseHP)
    {
        playerKingHP += creaseHP;
        playerKing.pieceHP = playerKingHP;
        uiManager.KingHPSet(playerKingHP, playerKing.maxHP);
        if (playerKingHP <= 0)
        {
            StartCoroutine(GameOver());
        }
    }

    public IEnumerator GameOver()
    {
        uiManager.GameLose(userData.userName);
        Shake.shake.ShakeKing(playerKing.transform, playerLayerNum);
        yield return new WaitForSeconds(6.0f);
        gameState = false;
        NetworkManager.Instance.DisconnectPlayer();
    }

    public IEnumerator GameWin()
    {
        uiManager.GameWin(userData.userName);
        yield return new WaitForSeconds(7.5f);
        gameState = false;
        NetworkManager.Instance.DisconnectPlayer();
    }

    #endregion

    #region 업데이트

    // 마우스 위치 , 클릭 확인
    public void RayCastCheck()
    {
        if (!Camera.main) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!drowSelect && !graveSelect)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("PlayerCard")))
            {
                PlayerHandCheck(hit.collider.gameObject);
            }
            else
            {
                if (selectViewHand != null)
                {
                    SelectViewHandReset();
                }
            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("PlayerHand")))
            {
                HandCheck(hit.point);
            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Board")))
            {
                PieceCheck(hit.point);
                if (selectHand != null)
                {
                    deployRange.DeployRangeView();
                    SelectHandCheck(hit.point);
                }
            }
            else
            {
                deployRange.DeployRangeNoneView();
                board.NoneViewSelectBoard();
            }
            if (playerTurn && Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Turn")))
            {
                if (Input.GetMouseButtonDown(0) && !drowSelect)
                {
                    NetTurnChange();
                }
            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Graveyard")))
            {
                GraveyardClick();
            }
            else
            {
                Graveyard.Instance.GraveNoneMouseView();
            }
        }
        if (Graveyard.Instance.graveViewB)
        {
            if (Physics.Raycast(ray, out hit, 10.0f, LayerMask.GetMask("GraveCard")))
            {
                if (Physics.Raycast(ray, out RaycastHit hit2, 10.0f, LayerMask.GetMask("GraveCardView")))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Graveyard.Instance.SelectGrave(hit);
                    }
                    else
                    {
                        Graveyard.Instance.ViewGraveCard(hit);
                    }
                }
                else
                {
                    Graveyard.Instance.GraveCardReset();
                }
            }
            else
            {
                Graveyard.Instance.GraveCardReset();
            }
        }
    }
    #endregion

    #region 업데이트 함수

    // 눌린 키 확인
    public void KeyValueCheck()
    {
        if (select)
        {
            if (Input.GetMouseButtonDown(1))
            {
                uiManager.KeyView(0);
                if (selectPiece != null)
                {
                    MovePiece(selectPiece, startDrag.x, startDrag.y);
                }
                PieceReset();
            }
            if (!playerTurn && selectPiece.isPlayer)
            {
                return;
            }
            else
            {
                if (Input.GetKeyDown("d"))
                {
                    uiManager.KeyView(1);
                    actionNum = 1;
                    moveP = true;
                    BoardLight.Instance.HideRange();
                    BoardLight.Instance.AllowedMoveTarget(board.AllowedMoveTarget);
                }
                else if (Input.GetKeyDown("a"))
                {
                    uiManager.KeyView(2);
                    actionNum = 2;
                    attackP = true;
                    BoardLight.Instance.HideRange();
                    BoardLight.Instance.AllowedAttackTarget(board.AllowedAttackTarget);
                }
                else if (Input.GetKeyDown("s"))
                {
                    uiManager.KeyView(3);
                    actionNum = 3;
                    skillP = true;
                    BoardLight.Instance.HideRange();
                    BoardLight.Instance.AllowedSkillTarget(board.AllowedSkillTarget);
                }
            }
        }

        if (Input.GetAxis("Tab") > 0.0f)
        {
            uiManager.bars.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            uiManager.bars.GetComponent<CanvasGroup>().alpha = 0;
        }

        if (Input.GetAxis("Cancel") > 0.0f)
        {
            OptionCheck();
        }
    }

    public void OptionCheck()
    {
        if (!option && !drowSelect && !graveSelect)
        {
            if (selectPiece != null)
            {
                MovePiece(selectPiece, (int)startDrag.x, (int)startDrag.y);
                PieceReset();
            }
            if (selectHand != null)
            {
                MoveHandReset();
                SelectHandReset();
            }
            option = true;
            AlwaysObject.Instance.SettingOn(null, false);
        }
    }

    // 드로우 제한 시간 확인
    public void DrowTimeCheck()
    {
        if (f_drow)
        {
            if (NetworkManager.Instance.time >= drowMaxTime)
            {
                DrowNoneSelectCheck();
            }
        }
    }

    public void DrowNoneSelectCheck()
    {
        List<int> _drowList = drowsList;
        if(drowHandNum == 99 && playerTurn)
        {
            while (_drowList.Count>0)
            {
                int selectRandomNum = Random.Range(0, _drowList.Count);
                if (ManaTestCheck(-HandManager.Instance.pieceDMana[drowsList[selectRandomNum]]))
                {
                    DrowNotice(drowsList[selectRandomNum], selectRandomNum);
                    break;
                }
                else
                {
                    _drowList.RemoveAt(selectRandomNum);
                }
            }
        }
        else
        {
            f_drow = false;
        }
    }

    // 현재 선택된 말 확인
    public void SelectCheck()
    {
        if (selectPiece != null)
        {
            uiManager.PieceSetView(true, selectPiece, playerMana, playerTurn);
        }
        else
        {
            uiManager.PieceSetView(false, null, playerMana, playerTurn);
        }
    }

    // 카메라 확인
    public void CameraCheck()
    {
        if (!Graveyard.Instance.graveViewB)
        {
            float wheelValue = Input.GetAxis("Mouse ScrollWheel");

        }
    }

    #endregion

    #region 네트워크

    public void NetTurnChange()
    {
        EndTurn();
        ++turnValue;
        NetworkManager.Instance.networkAction.TurnChange(turnValue);
    }

    public void NetManaCrease(int _creaseMana)
    {
        creaseMana = _creaseMana;
        if (_creaseMana < 0)
        {
            playerUseMana++;
        }
        ManaChange(creaseMana);
        NetworkManager.Instance.networkAction.ManaChange(playerMana, playerUseMana, playerAbleMana);
    }

    public void NetKingCrease(int _creaseHP)
    {
        KingHPCrease(_creaseHP);
        NetworkManager.Instance.networkAction.HPChange(playerKingHP);
    }

    public void NetDrowRemove(int _drowNumValue)
    {
        NetworkManager.Instance.networkAction.DrowRemove(_drowNumValue);
    }

    public void NetDrowReturn(List<int> returnList)
    {
        NetworkManager.Instance.networkAction.DrowReturn(returnList);
    }

    public void EnemyHPChange(int enemyHP)
    {
        enemyKingHP = enemyHP;
        uiManager.EnemyHP(enemyKingHP);
        if (enemyKingHP <= 0)
        {
            StartCoroutine(GameWin());
        }
    }

    public void EnemyManaChange(int _enemyMana, int _enemyUseMana, int _enemyAbleMana)
    {
        enemyMana = _enemyMana;
        enemyAbleMana = _enemyAbleMana;
        uiManager.EnemyMana(this.enemyMana, _enemyUseMana, enemyAbleMana);
    }

    public void UserDataSet(string userName)
    {
        userData.userName = userName;
        uiManager.UserInfoSet(userData);
    }

    public void EnemyDataSet(string enemyName)
    {
        enemyData.userName = enemyName;
        uiManager.EnemyInfoSet(enemyData);
    }

    #endregion

    #region 턴 관련

    public void EndTurn()
    {
        DrowNoneSelectCheck();
        DrowEnd(drowsList);
        PieceReset();
        MoveHandReset();
        SelectHandReset();
    }

    // 턴이 바뀔경우
    public void TurnChange(int turnValue)
    {
        this.turnValue = turnValue;
        playerTurn = !playerTurn;
        TurnManaIncrease();
        PieceReset();
        MoveHandReset();
        SelectHandReset();
        if (playerTurn)
        {
            TurnAddMana();
        }
        board.PieceValueChange(this.turnValue);
        uiManager.TurnObjectSet(playerTurn, playerMana, playerAbleMana, this.turnValue);
        turn.TurnObjectSet(playerTurn);
    }

    // 턴이 바뀌면 마나 증가
    public void TurnAddMana()
    {
        playerUseMana = 0;
        playerAbleMana = TurnAbleManaIncrease() + 1;
        if (playerAbleMana > 10)
        {
            playerAbleMana = 10;
        }
        NetManaCrease(increaseManaValue);
    }

    // turnIncreaseAbleMana 턴마다 사용가능한 마나 증가 ( 기본 1 )
    private int TurnAbleManaIncrease()
    {
        int returnInt = 0;

        returnInt = turnValue / turnIncreaseAbleMana;

        return returnInt;
    }

    // turnIncreaseMana 턴마다  increaseManaValue값 증가
    private void TurnManaIncrease()
    {
        if (turnValue % turnIncreaseMana == 0)
        {
            increaseManaValue++;
        }
    }

    #endregion

    #region 마나 관련

    // 마나 확인
    public bool ManaCheck(int checkMana)
    {
        float resultMana = playerMana + checkMana;
        if (resultMana >= 0 && playerUseMana - checkMana <= playerAbleMana)
        {
            return true;
        }
        else
        {
            if (resultMana < 0)
            {
                AlwaysObject.Instance.InfoStart("마나가 부족합니다.");
            }
            if (playerUseMana - checkMana <= playerAbleMana)
            {
                AlwaysObject.Instance.InfoStart("이번턴에 사용 가능한 마나가 없습니다.");
            }
            return false;
        }
    }

    public bool ManaTestCheck(int checkMana)
    {
        float resultMana = playerMana + checkMana;
        if (resultMana >= 0 && playerUseMana - checkMana <= playerAbleMana)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // 마나 변경
    public void ManaChange(int creaseMana)
    { 
        playerMana += creaseMana;
        if (playerMana > playerMaxMana)
        {
            playerMana = playerMaxMana;
        }
        uiManager.ManaSet(playerMana, playerAbleMana, playerUseMana);
        uiManager.ManaChange(playerUseMana);
    }

    #endregion

    #region 드로우 관련

    // 드로우가 가능한지 확인하고 가능하면 드로우 시작
    public bool Drow()
    {
        if (ManaCheck(drowMana) && handsList.Count > 0 && HandManager.Instance.HandAddCheck(player))
        {
            drowSelect = true;
            DrowTimeCheckStart();
            DrowDeckSet();
            return true;
        }
        else
        {
            if(handsList.Count == 0)
            {
                AlwaysObject.Instance.InfoStart("덱에 카드가 없습니다.");
            }
            return false;
        }
    }

    // 드로우 제한 시간 체크 시작
    public void DrowTimeCheckStart()
    {
        drowCountTime = 0;
        drowHandNum = 99;
        f_drow = true;
    }

    // drowNumValue 장의 카드를 드로우하고 덱에서 제거
    public void DrowDeckSet()
    {
        if (handsList.Count < drowNumValue)
        {
            drowNumValue = handsList.Count;
        }

        for (int i = 0; i < drowNumValue; i++)
        {
            drowsList.Add(handsList[i]);
        }

        NetDrowRemove(drowNumValue);
    }

    public void DrowRemove(int _drowNumValue)
    {
        drowNumValue = _drowNumValue;
        handsList.RemoveRange(0, drowNumValue);
    }

    // 드로우 한 번호를 확인 , 묘지에서 갖고 온 번호 확인
    public void DrowNotice(int drowOrderNum, int drowSelectNum)
    {
        drowHandNum = drowSelectNum;
        drowsList.Remove(drowsList[drowSelectNum]);
        uiManager.DrowListClear(drowSelectNum);
        drowSelect = false;
        f_drow = false;
        NetManaCrease(-HandManager.Instance.pieceDMana[drowOrderNum]);
        HandManager.Instance.NetPlayerDrow(drowOrderNum);
    }

    // 나머지 카드는 덱에 돌려둠
    public void DrowEnd(List<int> returnList)
    {
        drowSelect = false;
        f_drow = false;
        NetDrowReturn(returnList);
        drowsList = new List<int>();
        uiManager.DrowEndClear();
    }

    public void DrowReturn(List<int> returnList)
    {
        handsList.AddRange(returnList);
    }

    // 패의 갯수 확인
    public bool DrowCheck(int cardMana)
    {
        if (ManaCheck(-cardMana))
        {
            if (player)
            {
                if (HandManager.Instance.aHandsNum < 8)
                {
                    return true;
                }
                else
                {
                    if (HandManager.Instance.aHandsNum >= 8)
                    {
                        AlwaysObject.Instance.InfoStart("패가 최대입니다.");
                    }
                    return false;
                }
            }
            else
            {
                if (HandManager.Instance.bHandsNum < 8)
                {
                    return true;
                }
                else
                {
                    if (HandManager.Instance.bHandsNum >= 8)
                    {
                        AlwaysObject.Instance.InfoStart("패가 최대입니다.");
                    }
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region 말 선택 관련

    // 선택 또는 행동
    private void PieceCheck(Vector3 hit)
    {
        int mouseX = (int)hit.x;
        int mouseZ = (int)hit.z;
        board.ViewSelectBoard(mouseX, mouseZ, select);
        mouseOver.x = mouseX;
        mouseOver.y = mouseZ;
        MouseCursor.mouseCursor.NoneCursor();
        if (selectPiece != null)
        {
            DeathPieceView(hit);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (selectPiece != null && selectPiece.isPlayer && playerTurn)
            {
                TryAction(startDrag, mouseOver, actionNum);
            }
            else
            {
                SelectPiece((int)mouseOver.x, (int)mouseOver.y);
            }
        }
    }

    // 선택
    private void SelectPiece(int x, int y)
    {
        if (x < 0 || x >= 9 || y < 0 || y >= 9)
            return;

        if (board.boardPiece[x, y] != null)
        {
            PieceReset();
            Piece p = board.boardPiece[x, y];
            board.AllowedMoves = p.PossibleMove(board.boardPiece, playerMana, false);
            board.AllowedMoveTarget = p.PossibleMove(board.boardPiece, playerMana, true);
            board.AllowedAttacks = p.PossibleAttack(board.boardPiece, playerMana, false);
            board.AllowedAttackTarget = p.PossibleAttack(board.boardPiece, playerMana, true);
            board.AllowedSkills = p.PossibleSkill(board.boardPiece, playerMana, false);
            board.AllowedSkillTarget = p.PossibleSkill(board.boardPiece, playerMana, true);
            BoardLight.Instance.HideRange();
            BoardLight.Instance.AllowedMoves(board.AllowedMoves);
            BoardLight.Instance.AllowedMoveTarget(board.AllowedMoveTarget);
            BoardLight.Instance.AllowedSkill(board.AllowedSkills);
            BoardLight.Instance.AllowedAttackTarget(board.AllowedAttackTarget);
            BoardLight.Instance.AllowedAttack(board.AllowedAttacks);
            BoardLight.Instance.AllowedSkillTarget(board.AllowedSkillTarget);
            selectPiece = p;
            uiManager.KeyView(4);
            startDrag = mouseOver;
            if (select)
            {
                board.ViewSelectBoard(x, y, false);
            }
            select = true;
            if (!playerTurn && selectPiece.isPlayer)
            {
                AlwaysObject.Instance.InfoStart("플레이어의 턴에만 움직일 수 있습니다.");
            }
            MoveHandReset();
            SelectHandReset();
        }
        else
        {
            PieceReset();
        }
    }

    private void DeathPieceView(Vector3 hit)
    {
        if (selectPiece.isPlayer && moveP && playerTurn && ManaTestCheck(selectPiece.MoveManaCheck(startDrag, mouseOver)))
        {
            selectPiece.transform.position = hit + Vector3.up;
        }
        if (mouseOver.x <= 8 && mouseOver.y <= 8 && mouseOver.x >= 0 && mouseOver.y >= 0)
        {
            if (selectPiece.isPlayer && board.boardPiece[(int)mouseOver.x, (int)mouseOver.y] != null && (skillP || attackP))
            {
                Piece target = board.boardPiece[(int)mouseOver.x, (int)mouseOver.y];
                selectPiece.TargetX = target.CurrentX;
                selectPiece.TargetZ = target.CurrentZ;
                if (skillP && board.AllowedSkills[(int)mouseOver.x, (int)mouseOver.y] && ManaTestCheck(-selectPiece.skillMana))
                {
                    BoardLight.Instance.HideLightRange();
                    board.AllowedSkillRange = selectPiece.PossibleSkillRange(board.boardPiece);
                    board.DeathPieces = selectPiece.DeathPieceCheck(board.boardPiece, board.AllowedSkillRange, false);
                    if (selectPiece.ActionDeathCheck(board.boardPiece, false))
                    {
                        MouseCursor.mouseCursor.DeathCursor();
                    }
                    else
                    {
                        MouseCursor.mouseCursor.SkillCursor();
                    }
                    BoardLight.Instance.AllowedSkill(board.AllowedSkillRange);
                }
                else if (attackP && board.AllowedAttacks[(int)mouseOver.x, (int)mouseOver.y] && ManaTestCheck(-selectPiece.attackMana))
                {
                    BoardLight.Instance.HideLightRange();
                    board.AllowedAttackRange = selectPiece.PossibleTarget(board.boardPiece);
                    board.DeathPieces = selectPiece.DeathPieceCheck(board.boardPiece, board.AllowedAttackRange, true);
                    if (selectPiece.ActionDeathCheck(board.boardPiece, true))
                    {
                        MouseCursor.mouseCursor.DeathCursor();
                    }
                    else
                    {
                        MouseCursor.mouseCursor.AttackCursor();
                    }
                    BoardLight.Instance.AllowedAttack(board.AllowedAttackRange);
                }
                else
                {
                    board.DeathPieces = new bool[9, 9];
                    BoardLight.Instance.HideTarget();
                }
                BoardLight.Instance.DeathPieceCheck(board.DeathPieces);
            }

        }
    }

    private void TryAction(Vector2 _startDrag, Vector2 _endDrag, int actionNum)
    {
        endDrag = _endDrag;

        if (endDrag.x < 0 || endDrag.x >= board.boardPiece.Length || endDrag.y < 0 || endDrag.y >= board.boardPiece.Length)
        {
            MovePiece(selectPiece, startDrag.x, startDrag.y);
            PieceReset();
            return;
        }

        if (endDrag == startDrag)
        {
            MovePiece(selectPiece, startDrag.x, startDrag.y);
            PieceReset();
            return;
        }

        if (actionNum == 1)
        {
            if (selectPiece.move)
            {
                int decreaseMoveMana = selectPiece.MoveManaCheck(startDrag, endDrag);
                if (board.AllowedMoves[(int)endDrag.x, (int)endDrag.y] && playerTurn && ManaCheck(decreaseMoveMana))
                {
                    board.NetPieceAction(player, --actionNum, startDrag, endDrag);
                }
                else
                {
                    if (!board.AllowedMoves[(int)endDrag.x, (int)endDrag.y] && ManaTestCheck(decreaseMoveMana))
                    {
                        AlwaysObject.Instance.InfoStart("배치할 수 없는 구역입니다.");
                    }
                    MovePiece(selectPiece, startDrag.x, startDrag.y);
                }
                PieceReset();
            }
        }
        else if (actionNum == 2)
        {
            int decreaseAttackMana = -selectPiece.attackMana;
            if (board.AllowedAttacks[(int)endDrag.x, (int)endDrag.y] && playerTurn && ManaCheck(decreaseAttackMana) && selectPiece.action)
            {
                board.NetPieceAction(player, --actionNum, startDrag, endDrag);
            }
            else
            {
                if (!board.AllowedAttacks[(int)endDrag.x, (int)endDrag.y] && ManaTestCheck(decreaseAttackMana))
                {
                    AlwaysObject.Instance.InfoStart("공격 범위가 아닙니다.");
                }
                else if (!selectPiece.action)
                {
                    AlwaysObject.Instance.InfoStart("이번 턴에 행동을 하였습니다.");
                }
            }
            PieceReset();
        }
        else if (actionNum == 3)
        {
            int decreaseSkillMana = -selectPiece.skillMana;
            if (board.AllowedSkills[(int)endDrag.x, (int)endDrag.y] && playerTurn && ManaCheck(decreaseSkillMana) && selectPiece.PieceMPCheck() && selectPiece.action)
            {
                board.NetPieceAction(player, --actionNum, startDrag, endDrag);
            }
            else
            {
                if(!board.boardPiece[(int)endDrag.x, (int)endDrag.y].PieceMPCheck())
                {
                    AlwaysObject.Instance.InfoStart("피스의 마나가 최대일때만 사용 가능합니다.");
                }
                else if (!board.AllowedSkills[(int)endDrag.x, (int)endDrag.y] && ManaTestCheck(decreaseSkillMana))
                {
                    AlwaysObject.Instance.InfoStart("스킬 범위가 아닙니다.");
                }
                else if (!selectPiece.action)
                {
                    AlwaysObject.Instance.InfoStart("이번 턴에 행동을 하였습니다.");
                }
            }
            PieceReset();
        }
    }

    // 이동
    public void MovePiece(Piece p, float x, float z)
    {
        if (p.pv.IsMine)
        {
            p.transform.position = (Vector3.right * x) + (Vector3.forward * z) + boardOffset + p.piecePosition;
        }
        p.CurrentX = (int)x;
        p.CurrentZ = (int)z;
    }

    // 초기화
    private void PieceReset()
    {
        selectPiece = null;
        select = false;
        moveP = false;
        attackP = false;
        skillP = false;
        actionNum = -1;
        BoardLight.Instance.HideRange();
    }
    #endregion

    #region 패 선택 관련

    private void PlayerHandCheck(GameObject target)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectHand == null && playerTurn)
            {
                if (selectViewHand != null)
                {
                    SelectViewHandReset();
                }
                if (target.transform.parent.childCount >= 2)
                {
                    GameObject playerHand = target.transform.parent.GetChild(1).gameObject;
                    selectHand = playerHand.GetComponent<PlayerHand>();
                    selectHandV = selectHand.transform.position;
                    selectHandQ = selectHand.transform.rotation;
                    NetworkManager.Instance.networkAction.HandAction(selectHand.handNum, 1, player);
                    if (player)
                    {
                        selectHand.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        startHandDrag = HandManager.Instance.handATransform[selectHand.handNum].position;
                    }
                    else
                    {
                        selectHand.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                        startHandDrag = HandManager.Instance.handBTransform[selectHand.handNum].position;
                    }
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            if (selectHand == null)
            {
                if (selectViewHand == null)
                {
                    selectViewHand = target.transform.parent.gameObject;
                    selectViewHandV = selectViewHand.transform.position;
                    selectViewHandQ = selectViewHand.transform.rotation;
                    selectViewHandScale = selectViewHand.transform.localScale;
                    selectViewHand.transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);
                    int sendHandNum = target.transform.parent.GetChild(1).gameObject.GetComponent<PlayerHand>().handNum;
                    NetworkManager.Instance.networkAction.HandAction(sendHandNum, 0, player);
                }
                else if (selectViewHand != null)
                {
                    if (selectViewHand != target.transform.parent.gameObject)
                    {
                        SelectViewHandReset();
                    }
                }
            }
        }
    }

    private void SelectViewHandReset()
    {
        selectViewHand.transform.position = selectViewHandV;
        selectViewHand.transform.rotation = selectViewHandQ;
        selectViewHand.transform.localScale = selectViewHandScale;
        selectViewHand = null;
        NetworkManager.Instance.networkAction.HandNoneSelect(player);
    }

    private void HandCheck(Vector3 hit)
    {
        mouseHandOver.x = hit.x;
        mouseHandOver.z = hit.z;
        if (selectHand != null)
        {
            MoveHandG(selectHand, mouseHandOver);
            if (Input.GetMouseButtonUp(0))
            {
                TryMoveHandOver(mouseHandOver);
            }
        }
        else
        {

        }
    }

    private void MoveHandG(PlayerHand playerHand, Vector3 movePoint)
    {
        playerHand.transform.position = (Vector3.right * movePoint.x) + (Vector3.forward * movePoint.z) + moveOffset;
    }

    private void TryMoveHandOver(Vector3 mouseClickOver)
    {
        if (!Camera.main) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Graveyard")))
        {
            if (Graveyard.Instance.graveNum < 32)
            {
                GraveyardHandRemove(selectHand);
                return;
            }
            else
            {
            }
        }
        MoveHandReset();
        SelectHandReset();
    }

    private void SelectHandCheck(Vector3 hit)
    {
        mouseHandOver.x = (int)hit.x;
        mouseHandOver.z = (int)hit.z;
        if (mouseHandOver.x < 0 || mouseHandOver.x >= 9)
        {
            MoveHandReset();
            SelectHandReset();
            return;
        }

        MoveHand(selectHand, mouseHandOver);
        if (Input.GetMouseButtonUp(0))
        {
            TryMoveHand(mouseHandOver);
        }

    }

    private void TryMoveHand(Vector3 mouseClickOver)
    {
        endHandDrag = mouseClickOver;
        
        int x1 = (int)startHandDrag.x;
        int z1 = (int)startHandDrag.z;
        int x2 = (int)endHandDrag.x;
        int z2 = (int)endHandDrag.z;

        if (x2 < 0 || x2 >= 9 || z2 < 0 || z2 >= 9)
        {
            MoveHandReset();
            SelectHandReset();
            return;
        }

        if (!ManaCheck(-selectHand.playerHand_fieldMana) || !playerTurn)
        {
            MoveHandReset();
            SelectHandReset();
            return;
        }

        if (selectHand != null)
        {
            if (player)
            {
                if (z2 >= 2)
                {
                    MoveHandReset();
                    SelectHandReset();
                    AlwaysObject.Instance.InfoStart("배치할 수 없는 구역입니다.");
                    return;
                }
            }
            else
            {
                if (z2 <= 6)
                {
                    MoveHandReset();
                    SelectHandReset();
                    AlwaysObject.Instance.InfoStart("배치할 수 없는 구역입니다.");
                    return;
                }
            }

            if (board.boardPiece[x2, z2] == null)
            {
                int decreaseFieldMana = -selectHand.playerHand_fieldMana;
                NetManaCrease(decreaseFieldMana);
                NetworkManager.Instance.networkAction.FiledAddSet(selectHand.handNum, selectHand.orderNum, new Vector3(x2, 0.0f, z2), player);
                selectHand = null;
                HandManager.Instance.selectEnemyHand = null;
                deployRange.DeployRangeNoneView();
                SelectHandReset();
            }
            else
            {
                MoveHandReset();
                SelectHandReset();
                return;
            }
        }
    }

    private void MoveHand(PlayerHand playerHand, Vector3 movePoint)
    {
        playerHand.transform.position = (Vector3.right * movePoint.x) + (Vector3.forward * movePoint.z) + boardOffset;
    }

    private void MoveHandReset()
    {
        if (selectHand != null)
        {
            selectHand.transform.position = selectHandV;
            selectHand.transform.rotation = selectHandQ;
        }
        startHandDrag = Vector3.zero;
        endHandDrag = Vector3.zero;
        NetworkManager.Instance.networkAction.HandNoneSelect(player);
    }

    private void SelectHandReset()
    {
        selectHand = null;
    }

    #endregion

    #region 묘지 관련

    public void GraveyardClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectPiece == null && selectHand == null)
            {
                if (Graveyard.Instance.GraveyardCheck())
                {
                    graveSelect = true;
                    uiManager.GravePanelView();
                    Graveyard.Instance.GraveyardView();
                }
            }
        }
        else
        {
            Graveyard.Instance.GraveMouseView();
        }
    }

    public void GraveNotice()
    {
        uiManager.GraveyardEnd();
    }

    public void GraveyardHandRemove(PlayerHand selectRemoveHand)
    {
        NetworkManager.Instance.networkAction.GraveyardThrow(selectHand.handNum, selectHand.orderNum, player);
        selectHand.RemoveHand();
        HandManager.Instance.selectEnemyHand = null;
        selectHand = null;
    }
    #endregion
}
