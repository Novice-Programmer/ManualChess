using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLight : MonoBehaviour
{
    public static BoardLight Instance { set; get; } // 다른 소스에서 사용하기 위해 전역 설정
    public GameObject moveRangePrefab;
    public GameObject moveTargetRangePrefab;
    public GameObject attackRangePrefab;
    public GameObject attackTargetRangePrefab;
    public GameObject skillRangePrefab;
    public GameObject skillTargetRangePrefab;
    public GameObject deathEffectPrefab;
    private List<GameObject> moveList; // 이동 범위
    private List<GameObject> moveTargetList;
    private List<GameObject> attackList; // 스킬 범위
    private List<GameObject> attackTargetList;
    private List<GameObject> skillList; // 스킬 범위
    private List<GameObject> skillTargetList;
    private List<GameObject> deathPrefabs; // 죽음 범위

    private void Start()
    {
        StartSet();
    }

    // 기본 설정
    private void StartSet()
    {
        Instance = this;
        moveList = new List<GameObject>();
        attackList = new List<GameObject>();
        skillList = new List<GameObject>();
        moveTargetList = new List<GameObject>();
        attackTargetList = new List<GameObject>();
        skillTargetList = new List<GameObject>();
        deathPrefabs = new List<GameObject>();
    }

    // 타일 효과 생성
    private GameObject GetMoveRange()
    {
        GameObject _move = moveList.Find(l => !l.activeSelf);
        if (_move == null)
        {
            _move = Instantiate(moveRangePrefab);
            moveList.Add(_move);
        }
        return _move;
    }

    private GameObject GetMoveTargetRange()
    {
        GameObject _moveTarget = moveTargetList.Find(l => !l.activeSelf);
        if(_moveTarget == null)
        {
            _moveTarget = Instantiate(moveTargetRangePrefab);
            moveTargetList.Add(_moveTarget);
        }
        return _moveTarget;
    }

    // 라이트 효과 생성
    private GameObject GetAttackRange()
    {
        GameObject _attack = attackList.Find(l => !l.activeSelf);
        if (_attack == null)
        {
            _attack = Instantiate(attackRangePrefab);
            attackList.Add(_attack);
        }
        return _attack;
    }

    private GameObject GetAttackTargetRange()
    {
        GameObject _attackTarget = attackTargetList.Find(l => !l.activeSelf);
        if (_attackTarget == null)
        {
            _attackTarget = Instantiate(attackTargetRangePrefab);
            attackTargetList.Add(_attackTarget);
        }
        return _attackTarget;
    }

    private GameObject GetSkillRange()
    {
        GameObject _skill = skillList.Find(l => !l.activeSelf);
        if (_skill == null)
        {
            _skill = Instantiate(skillRangePrefab);
            skillList.Add(_skill);
        }
        return _skill;
    }

    private GameObject GetSkillTargetRange()
    {
        GameObject _skillTarget = skillTargetList.Find(l => !l.activeSelf);
        if (_skillTarget == null)
        {
            _skillTarget = Instantiate(skillTargetRangePrefab);
            skillTargetList.Add(_skillTarget);
        }
        return _skillTarget;
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
        foreach (GameObject _move in moveList)
            _move.SetActive(false);
        foreach (GameObject _attack in attackList)
            _attack.SetActive(false);
        foreach (GameObject _skill in skillList)
            _skill.SetActive(false);
        foreach (GameObject _death in deathPrefabs)
            _death.SetActive(false);
        foreach (GameObject _moveTarget in moveTargetList)
            _moveTarget.SetActive(false);
        foreach (GameObject _attackTarget in attackTargetList)
            _attackTarget.SetActive(false);
        foreach (GameObject _skillTarget in skillTargetList)
            _skillTarget.SetActive(false);
    }

    // 타일 제외 효과 숨기기
    public void HideLightRange()
    {
        foreach (GameObject _move in moveList)
            _move.SetActive(false);
        foreach (GameObject _attack in attackList)
            _attack.SetActive(false);
        foreach (GameObject _skill in skillList)
            _skill.SetActive(false);
        foreach (GameObject death in deathPrefabs)
            death.SetActive(false);
    }

    public void HideTarget()
    {
        foreach (GameObject _attack in attackList)
            _attack.SetActive(false);
        foreach (GameObject _skill in skillList)
            _skill.SetActive(false);
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

    public void AllowedMoveTarget(bool[,] _moveTarget)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (_moveTarget[i, j])
                {
                    GameObject effect = GetMoveTargetRange();
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

    public void AllowedAttackTarget(bool[,] _attacksTarget)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (_attacksTarget[i, j])
                {
                    GameObject effect = GetAttackTargetRange();
                    effect.SetActive(true);
                    effect.transform.position = new Vector3(i + 0.5f, 1.5f, j + 0.5f);
                }
            }
        }
    }

    public void AllowedSkillTarget(bool[,] _skillsTarget)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (_skillsTarget[i, j])
                {
                    GameObject effect = GetSkillTargetRange();
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