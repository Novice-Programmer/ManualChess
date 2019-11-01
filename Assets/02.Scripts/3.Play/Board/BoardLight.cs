using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLight : MonoBehaviour
{
    public static BoardLight Instance { set; get; } // 다른 소스에서 사용하기 위해 전역 설정
    public GameObject moveRangePrefab;
    public GameObject attackRangePrefab;
    public GameObject skillRangePrefab;
    public GameObject deathEffectPrefab;
    private List<GameObject> movePrefabs; // 이동 범위
    private List<GameObject> attackPrefabs; // 스킬 범위
    private List<GameObject> skillPrefabs; // 스킬 범위
    private List<GameObject> deathPrefabs; // 죽음 범위

    private void Start()
    {
        StartSet();
    }

    // 기본 설정
    private void StartSet()
    {
        Instance = this;
        movePrefabs = new List<GameObject>();
        attackPrefabs = new List<GameObject>();
        skillPrefabs = new List<GameObject>();
        deathPrefabs = new List<GameObject>();
    }

    // 타일 효과 생성
    private GameObject GetMoveRange()
    {
        GameObject _move = movePrefabs.Find(l => !l.activeSelf);
        if (_move == null)
        {
            _move = Instantiate(moveRangePrefab);
            movePrefabs.Add(_move);
        }
        return _move;
    }

    // 라이트 효과 생성
    private GameObject GetAttackRange()
    {
        GameObject _attack = attackPrefabs.Find(l => !l.activeSelf);
        if (_attack == null)
        {
            _attack = Instantiate(attackRangePrefab);
            attackPrefabs.Add(_attack);
        }
        return _attack;
    }

    private GameObject GetSkillRange()
    {
        GameObject _skill = skillPrefabs.Find(l => !l.activeSelf);
        if (_skill == null)
        {
            _skill = Instantiate(skillRangePrefab);
            skillPrefabs.Add(_skill);
        }
        return _skill;
    }

    // 죽음 효과 생성
    private GameObject GetDeathRange()
    {
        GameObject death = deathPrefabs.Find(l => !l.activeSelf);
        if (death == null)
        {
            death = Instantiate(deathEffectPrefab);
            deathPrefabs.Add(death);
        }
        return death;
    }

    // 모든 효과 숨기기
    public void HideRange()
    {
        foreach (GameObject _move in movePrefabs)
            _move.SetActive(false);
        foreach (GameObject _attack in attackPrefabs)
            _attack.SetActive(false);
        foreach (GameObject _skill in skillPrefabs)
            _skill.SetActive(false);
        foreach (GameObject _death in deathPrefabs)
            _death.SetActive(false);
    }

    // 타일 제외 효과 숨기기
    public void HideLightRange()
    {
        foreach (GameObject _attack in attackPrefabs)
            _attack.SetActive(false);
        foreach (GameObject _skill in skillPrefabs)
            _skill.SetActive(false);
        foreach (GameObject death in deathPrefabs)
            death.SetActive(false);
    }

    // 이동 가능 범위 표시
    public void AllowedMoves(bool[,] _moves)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (_moves[i, j])
                {
                    GameObject effect = GetMoveRange();
                    effect.SetActive(true);
                    effect.transform.position = new Vector3(i + 0.5f, 1.5f, j + 0.5f);
                }
            }
        }
    }

    public void AllowedAttack(bool[,] _attacks)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (_attacks[i, j])
                {
                    GameObject effect = GetAttackRange();
                    effect.SetActive(true);
                    effect.transform.position = new Vector3(i + 0.5f, 1.5f, j + 0.5f);
                }
            }
        }
    }

    public void AllowedSkill(bool[,] _skills)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (_skills[i, j])
                {
                    GameObject effect = GetSkillRange();
                    effect.SetActive(true);
                    effect.transform.position = new Vector3(i + 0.5f, 1.5f, j + 0.5f);
                }
            }
        }
    }

    // 죽음 범위 표시
    public void DeathPieceCheck(bool[,] dpc)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (dpc[i, j] && GameManager.Instance.board.boardPiece[i, j] != null)
                {
                    GameObject death = GetDeathRange();
                    death.SetActive(true);
                    Vector3 pieceT = GameManager.Instance.board.boardPiece[i, j].pieceTransform.position;
                    pieceT.y += 2.5f;
                    death.transform.position = pieceT;
                }
            }
        }
    }
}