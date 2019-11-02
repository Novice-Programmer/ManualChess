using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance { set; get; }
    private const float BOARD_OFFSET = 0.5f; // 보드의 공간 차이
    private const int BOARD_X_SIZE = 9; // 보드의 최대 x값
    private const int BOARD_Z_SIZE = 9; // 보드의 최대 z값
    private const int BOARD_ADD_VALUE = 4; // 위치 조절 값
    private Vector3 SELECT_VECTOR = new Vector3(0.5f, 1.5f, 0.5f);

    [Header("SelectionXZ")]
    public int selectionX = -1; // X위치
    public int selectionZ = -1; // Z위치

    public GameObject boardSelectObj; // 현재 선택중인 위치에 나타나는 효과 ( 이펙트 )
    public GameObject[] selectEffects;
    private Vector3[] selectVectors;
    private Vector3[] selectVectorAdds;
    public Texture noneSelectT;
    public ParticleSystem boardSelectMark;
    private Quaternion selectObjQ = Quaternion.Euler(90.0f, 0.0f, 0.0f); // 오브젝트를 x축으로 회전

    public Piece[,] boardPiece = new Piece[9, 9]; // 보드에 저장된 피스

    public bool[,] AllowedMoves { set; get; } // 이동가능 범위
    public bool[,] AllowedAttacks { set; get; } // 공격가능 범위
    public bool[,] AllowedAttackRange { set; get; } // 공격 범위
    public bool[,] AllowedSkills { set; get; } // 스킬 사용 범위
    public bool[,] AllowedSkillRange { set; get; } // 스킬 공격 범위
    public bool[,] DeathPieces { set; get; } // 공격 또는 스킬 사용시에 죽는 피스들

    private void Start()
    {
        StartSet();
    }

    // 기본 설정
    private void StartSet()
    {
        Instance = this;
        selectVectors = new Vector3[selectEffects.Length];
        selectVectorAdds = new Vector3[selectEffects.Length];
        for (int i = 0; i < selectVectors.Length; i++)
        {
            selectVectors[i] = selectEffects[i].transform.localScale;
            selectVectorAdds[i] = selectVectors[i] * 1.5f;
        }
        if (GameManager.Instance.player)
        {
            ParticleSystem.MainModule selectMarkMM = boardSelectMark.main;
            selectMarkMM.startRotation = 0.0f;
        }
    }

    // 현재 선택된 위치로 효과를 이동
    public void ViewSelectBoard(int x, int z,bool select)
    {
        if (select)
        {
            return;
        }

        if (x >= 9 || z >= 9 || x < 0 || z < 0)
        {
            return;
        }

        if (boardPiece[x, z] != null)
        {
            selectEffects[0].transform.localScale = selectVectorAdds[0];
            selectEffects[1].transform.localScale = selectVectorAdds[1];
            selectEffects[2].transform.localScale = selectVectorAdds[2];
            boardSelectMark.GetComponent<Renderer>().material.mainTexture = boardPiece[x, z].selectTexture;
        }

        else
        {
            selectEffects[0].transform.localScale = selectVectors[0];
            selectEffects[1].transform.localScale = selectVectors[1];
            selectEffects[2].transform.localScale = selectVectors[2];
            boardSelectMark.GetComponent<Renderer>().material.mainTexture = noneSelectT;
        }

        selectionX = x;
        selectionZ = z;

        Vector3 selectV = SELECT_VECTOR;
        selectV.x += selectionX;
        selectV.z += selectionZ;
        boardSelectObj.transform.position = selectV;
        selectionX += BOARD_ADD_VALUE;
        selectionZ += BOARD_ADD_VALUE;
    }

    // 선택된게 없거나 위치를 벗어나면 선택위치 초기화
    public void NoneViewSelectBoard()
    {
        selectionX = -1;
        selectionZ = -1;
    }

    // 매턴마다 피스들도 상태 체인지됌
    public void PieceValueChange(int turn)
    {
        for (int x = 0; x < BOARD_X_SIZE; x++)
        {
            for (int z = 0; z < BOARD_Z_SIZE; z++)
            {
                if (boardPiece[x, z] != null)
                {
                    boardPiece[x, z].PieceTurnChange(turn);
                }
            }
        }
    }

    public void NetPieceAction(bool player, int action, Vector2 startV, Vector2 endV)
    {
        NetworkManager.Instance.networkAction.PieceAction(startV, endV, action, player);
    }

    public void PieceAction(Vector2 startV, Vector2 endV, int action, bool player)
    {
        int x1 = (int)startV.x;
        int y1 = (int)startV.y;
        int x2 = (int)endV.x;
        int y2 = (int)endV.y;

        Piece selectPiece = boardPiece[x1, y1];

        if (action == 0)
        {
            if (player == GameManager.Instance.player)
            {
                int decreaseMoveMana = selectPiece.MoveManaCheck(new Vector2(x1, y1), new Vector2(x2, y2));
                GameManager.Instance.NetManaCrease(decreaseMoveMana);
                GameManager.Instance.MovePiece(selectPiece, x2, y2);
                StartCoroutine(selectPiece.ActionAnim(1));
            }
            boardPiece[x2, y2] = selectPiece;
            boardPiece[x1, y1] = null;
            selectPiece.CurrentX = x2;
            selectPiece.CurrentZ = y2;
            selectPiece.pieceBar.PosUpdate();
            selectPiece.move = false;
        }
        else if(action == 1)
        {
            selectPiece.SetTargetPosition(x2, y2);
            if (player == GameManager.Instance.player)
            {
                int decreaseAttackMana = -selectPiece.attackMana;
                GameManager.Instance.NetManaCrease(decreaseAttackMana);
                StartCoroutine(selectPiece.ActionAnim(2));
            }
            selectPiece.action = false;
        }
        else if(action == 2)
        {
            selectPiece.SetTargetPosition(x2, y2);
            if (player == GameManager.Instance.player)
            {
                int decreaseSkillMana = -selectPiece.skillMana;
                GameManager.Instance.NetManaCrease(decreaseSkillMana);
                StartCoroutine(selectPiece.ActionAnim(3));
            }
            selectPiece.pieceMP = 0;
            selectPiece.action = false;
        }
    }

    public void PieceDamage(Vector2 actionV, Vector2 targetV, bool atk, bool reduce)
    {
        Piece _selectPiece = boardPiece[(int)actionV.x, (int)actionV.y];
        _selectPiece.SetTargetPosition(targetV.x, targetV.y);
        bool[,] _range = new bool[9, 9];
        int _damage;
        int _reduceDamage = 0;

        if (atk)
        {
            _range = _selectPiece.PossibleTarget(GameManager.Instance.board.boardPiece);
            _damage = _selectPiece.attackDamage;
            _reduceDamage = 0;
        }
        else
        {
            _range = _selectPiece.PossibleSkillRange(GameManager.Instance.board.boardPiece);
            _damage = _selectPiece.skillDamage;
            _reduceDamage = _selectPiece.skillReduceDamage;
        }

        for (int x = 0; x < 9; x++)
        {
            for (int z = 0; z < 9; z++)
            {
                if (_range[x, z])
                {
                    if (boardPiece[x, z].pv.IsMine)
                    {
                        StartCoroutine(boardPiece[x, z].ActionAnim(4));
                    }
                    int _reDamage = _damage;
                    if (x != targetV.x || z != targetV.y)
                    {
                        if (reduce)
                        {
                            _reDamage = _damage - _reduceDamage;
                        }
                    }
                    StartCoroutine(boardPiece[x, z].PieceDamage(_reDamage, atk, _selectPiece));
                }
            }
        }
    }
}