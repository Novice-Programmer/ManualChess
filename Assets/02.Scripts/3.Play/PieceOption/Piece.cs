using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public abstract class Piece : MonoBehaviour
{
    public Transform pieceTransform;

    [Header("Animation")]
    // 애니메이션 설정
    public Animator animator;
    public bool animOn;
    public List<int> aniList = new List<int>();

    public WaitForSeconds ws;

    public bool animAttack;
    public bool animSkill;
    public bool animMove;
    public bool animDamage;
    public bool animDie;

    [Header("Particle")]
    public GameObject eff_AttackTarget; // 공격시 상대방 위에 뜰 이펙트
    public Vector3 eff_AttackTargetV;
    public Quaternion eff_AttackTargetQ;
    public float eff_AttackTargetT;
    public float eff_AttackTargetDT;
    public GameObject eff_SkillTarget; // 스킬시 상대방 위에 뜰 이펙트
    public Vector3 eff_SkillTargetV;
    public Quaternion eff_SkillTargetQ;
    public float eff_SkillTargetT;
    public float eff_SkillTargetDT;

    // 현재 위치
    public int CurrentX { set; get; }
    public int CurrentZ { set; get; }

    // 타겟 위치
    public int TargetX { set; get; }
    public int TargetZ { set; get; }

    [Header("Enemy")]
    public Piece actionPiece; // 상대방 정보

    [Header("PiecePlayer")]
    // 소유 확인
    public bool gamePlayer; // 플레이어 확인용
    public bool isPlayer; // 사용 확인용

    [Header("State")]
    public string piece_Name; // 이름
    public string piece_Dir; // 설명
    public int level; // 레벨 시스템 예정
    public bool move = true; // 이번턴에 이동했는지 확인하는 변수
    public bool action = true; // 이번턴에 공격또는 스킬을 사용했는지 확인하는 변수
    // 체력 마나
    public float pieceHP; // 현재 체력
    public float pieceMP; // 현재 마나

    public int maxHP; // 최대 체력
    public int maxMP; // 최대 마나

    [Header("Mana")]
    public int drowMana; // 드로우할때 마나
    public int fieldMana; // 필드에 배치할려고 할때 마나

    [Header("ActionSet")]
    public int moveMana; // 이동시에 소모되는 기본 마나
    public int spaceMana; // spaceRange에 따른 추가 소모되는 마나
    public int moveRange; // 이동 최대 가능 범위
    public int spaceRange; // n칸마다 추가 마나 (spaceMana)

    public int attackMana; // 공격시에 소모되는 기본 마나
    public int attackDamage; // 공격 데미지
    public int attackRange; // 공격 가능 범위
    public int targetRange; // 공격수

    public int skillMana; //스킬 사용시에 소모되는 기본 마나
    public int skillDamage; // 스킬 데미지
    public int skillRange; // 스킬 사용 가능 범위
    public int skillAttackRange; // 스킬 범위
    public bool skillReduce;
    public int skillReduceDamage;
    public float skillShakeRange; // 스킬 흔들림 범위
    public float skillShakeForce; // 스킬 흔들림 크기

    [Header("ETC")]
    private readonly int turnCreaseMana = 1; // 턴마다 증가하는 마나량
    public int orderNum; // 카드로 바뀔때의 번호
    public Transform charViewT;
    public GameObject charViewA;
    public GameObject charViewB;

    [Header("PieceSet")]
    public Texture selectTexture;
    public Vector3 piecePosition; // 현재 자신 위치
    public Quaternion aPieceRotate = Quaternion.Euler(0.0f, 0.0f, 0.0f); // 위치 회전
    public Quaternion bPieceRotate = Quaternion.Euler(0.0f, 180.0f, 0.0f); // 위치 회전

    // 체력,마나 게이지 바 변수
    [Header("Bar")]
    private GameObject bar;
    public GameObject barPrefabs;
    private Vector3 barOffset = new Vector3(0.0f, 0.9f, 0.0f);
    public Vector3 barAddset = Vector3.zero; // 캐릭터 크기에 따라 위치 설정 변경용
    private Canvas etcCanvas;
    private GameObject bars;
    private Image hpBarImage;
    private Image mpBarImage;

    // 공격 당할시 얼만큼 받았는지 표시
    [Header("Damage")]
    private GameObject damage;
    public GameObject damagePrefabs;
    private Vector3 damageOffset = new Vector3(0.0f, 0.35f, 0.0f);
    public Vector3 damageAddset = Vector3.zero;
    private GameObject damages;
    private Text damageText;

    [Header("Photon")]
    public PhotonView pv;

    private void OnEnable()
    {
        pv = GetComponent<PhotonView>();
        if (GameManager.Instance.player)
        {
            if (pv.IsMine)
            {
                gameObject.tag = "APiece";
                gameObject.layer = 18;
            }
            else
            {
                gameObject.tag = "BPiece";
                gameObject.layer = 19;
            }
        }
        else
        {
            if (pv.IsMine)
            {
                gameObject.tag = "BPiece";
                gameObject.layer = 19;
            }
            else
            {
                gameObject.tag = "APiece";
                gameObject.layer = 18;
            }
        }
        DataSetting();
        if (pv.IsMine)
        {
            StartCoroutine(ActionCheck());
        }
    }

    private void Start()
    {
        FirstSetting();
    }

    public void Update()
    {
        PointBar();
    }

    IEnumerator ActionCheck()
    {
        while(!animDie)
        {
            if (!animOn)
            {
                if (aniList.Count > 0)
                {
                    var animNum = aniList[0];
                    switch (animNum)
                    {
                        case 1:
                            animator.SetTrigger("Move");
                            animMove = true;
                            break;
                        case 2:
                            ActionRotate();
                            animator.SetTrigger("Attack");
                            animAttack = true;
                            break;
                        case 3:
                            ActionRotate();
                            animator.SetTrigger("Skill");
                            animSkill = true;
                            break;
                        case 4:
                            animator.SetTrigger("Damage");
                            animDamage = true;
                            break;
                        case 5:
                            animator.SetTrigger("Die");
                            animDie = true;
                            break;
                    }
                    animOn = true;
                    StartCoroutine(ActionReset());
                }
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    #region 애니메이션

    public IEnumerator ActionAnim(int stateNum)
    {
        ws = new WaitForSeconds(0.2f);
        yield return ws;
        aniList.Add(stateNum);
    }

    public IEnumerator ActionReset()
    {
        float exitTime = 0.3f;
        bool _action = true;
        bool _die = false;

        if (animDie)
        {
            ActionBoolReset();
            _die = true;
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                yield return null;
            }

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < exitTime)
            {
                yield return null;
            }
        }

        else if (animAttack)
        {
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                ActionRotate();
                yield return null;
            }

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < exitTime)
            {
                if (_action)
                {
                    ActionRotate();
                    _action = false;
                    ActionTarget(true);
                }
                yield return null;
            }
            animAttack = false;
        }

        else if (animSkill)
        {
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Skill"))
            {
                ActionRotate();
                yield return null;
            }

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < exitTime)
            {
                if (_action)
                {
                    ActionRotate();
                    _action = false;
                    Shake.shake.ShakeTarget(pieceTransform, skillShakeRange, skillShakeForce);
                    ActionTarget(false);
                }
                yield return null;
            }
            animSkill = false;
        }

        else if (animMove)
        {
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Move"))
            {
                //전환 중일 때 실행되는 부분
                yield return null;
            }

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < exitTime)
            {
                //애니메이션 중일 때 실행되는 부분
                yield return null;
            }
            //애니메이션 끝나고 난 후 실행되는 부분
            animMove = false;
        }

        else if (animDamage)
        {
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
            {
                yield return null;
            }

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < exitTime)
            {
                yield return null;
            }
            animDamage = false;
        }

        if (_die)
        {
            aniList = null;
            PhotonNetwork.Destroy(pieceTransform.gameObject);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            aniList.RemoveAt(0);
            PieceSetting();
            animOn = false;
        }
    }

    private void ActionTarget(bool atk)
    {
        bool[,] _range = new bool[9, 9];
        int _damage;
        if (atk)
        {
            _range = PossibleTarget(GameManager.Instance.board.boardPiece);
            _damage = attackDamage;
        }
        else
        {
            _range = PossibleSkillRange(GameManager.Instance.board.boardPiece);
            _damage = skillDamage;
        }
        if (atk)
        {
            NetworkManager.Instance.networkAction.PieceDamage(new Vector2(CurrentX, CurrentZ), new Vector2(TargetX, TargetZ), atk, false);
        }
        else
        {
            NetworkManager.Instance.networkAction.PieceDamage(new Vector2(CurrentX, CurrentZ), new Vector2(TargetX, TargetZ), atk, skillReduce);
        }
    }

    private void ActionBoolReset()
    {
        animMove = false;
        animAttack = false;
        animSkill = false;
        animDamage = false;
    }

    // 상대편쪽으로 바라봄
    public void ActionRotate()
    {
        if(GameManager.Instance.board.boardPiece[TargetX, TargetZ] != null)
        {
            pieceTransform.LookAt(GameManager.Instance.board.boardPiece[TargetX, TargetZ].transform);
        }
        else
        {
            return;
        }
    }

    #endregion

    #region 이펙트

    public IEnumerator PieceDamage(int _damage,bool _atk,Piece _piece)
    {
        damageText.text = "" + _damage;
        DamageCheck(_damage);
        DamageEffectView(_piece, _atk);
        yield return new WaitForSeconds(1.0f);
        damage.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        damage.SetActive(false);
    }

    private void DamageEffectView(Piece _actionPiece, bool _atk)
    {
        GameObject _effect;
        Vector3 _effectV;
        Quaternion _effectQ;
        float _effectT;

        if (_atk == true)
        {
            _effect = _actionPiece.eff_AttackTarget;
            _effectV = _actionPiece.eff_AttackTargetV;
            _effectQ = _actionPiece.eff_AttackTargetQ;
            _effectT = _actionPiece.eff_AttackTargetT;
        }

        else
        {
            _effect = _actionPiece.eff_SkillTarget;
            _effectV = _actionPiece.eff_AttackTargetV;
            _effectQ = _actionPiece.eff_AttackTargetQ;
            _effectT = _actionPiece.eff_AttackTargetT;
        }
        if (_effect != null)
        {
            _effect = Instantiate(_effect, pieceTransform.position + _effectV, pieceTransform.rotation);
            if (_effectT == 0)
            {
                Destroy(_effect, 3.0f);
            }
            else
            {
                Destroy(_effect, _effectT);
            }
        }
    }

    public virtual void DamageCheck(int _damage)
    {
        pieceHP -= _damage;
        if(pieceHP <= 0)
        {
            pieceHP = 0;
            if (pv.IsMine)
            {
                PieceDead();
            }
        }
    }

    #endregion

    #region 위치

    // 현재 위치 설정
    public void SetPosition(float x, float z)
    {
        CurrentX = (int)(x);
        CurrentZ = (int)(z);
    }

    // 타겟 위치 설정
    public void SetTargetPosition(float x, float z)
    {
        TargetX = (int)(x);
        TargetZ = (int)(z);
        actionPiece = GameManager.Instance.board.boardPiece[TargetX, TargetZ];
    }

    #endregion

    #region 기본 셋팅
    public void FirstSetting()
    {
        SetPosition(pieceTransform.position.x, pieceTransform.position.z);
        if (pv.IsMine)
        {
            gamePlayer = GameManager.Instance.player;
        }
        else
        {
            gamePlayer = !GameManager.Instance.player;
        }
        PieceSetting();
        CharViewSetting();
        BarSetting();
        DamageSetting();
        PieceAdd();
    }

    public void PieceSetting()
    {
        Vector3 pieceV = new Vector3(CurrentX + 0.5f, 1.8f, CurrentZ + 0.5f);
        if (pv.IsMine)
        {
            transform.position = pieceV + piecePosition;
            if (gamePlayer)
            {
                transform.rotation = aPieceRotate;
            }
            else
            {
                transform.rotation = bPieceRotate;
            }
        }
    }

    public void CharViewSetting()
    {
        GameObject charView;

        if (tag == "APiece")
        {
            charView = charViewA;
        }
        else
        {
            charView = charViewB;
        }
        Instantiate(charView, charViewT);
    }

    // 체력 , 마나 바 UI 설정
    public void BarSetting()
    {
        etcCanvas = GameObject.Find("EtcCanvas").GetComponent<Canvas>();
        bars = GameObject.Find("Bars").gameObject;
        bar = Instantiate<GameObject>(barPrefabs, bars.transform);
        hpBarImage = bar.GetComponentsInChildren<Image>()[2];
        mpBarImage = bar.GetComponentsInChildren<Image>()[3];
        bar.transform.position += barAddset;

        var _bar = bar.GetComponent<PieceBar>();
        _bar.targetTr = gameObject.transform;
        _bar.offset = barOffset;
        _bar.addset = barAddset;

        hpBarImage.fillAmount = 1.0f;
        mpBarImage.fillAmount = 0.0f;
    }

    public void DamageSetting()
    {
        damages = GameObject.Find("Damages").gameObject;
        damage = Instantiate<GameObject>(damagePrefabs, damages.transform);
        damageText = damage.GetComponentInChildren<Text>();
        damage.transform.position += damageAddset;

        var _damage = damage.GetComponent<PieceDamage>();
        _damage.targetTr = pieceTransform;
        _damage.offset = damageOffset;
        _damage.addset = damageAddset;

        damage.SetActive(false);
    }

    public virtual void DataSetting()
    {

    }

    #endregion

    #region 상태 변화

    // 체력,마나에 따른 설정
    public void PointBar()
    {
        float fmh;
        float fmp;

        if (pieceHP <= 0)
        {
            fmh = 0.0f;
        }
        else
        {
            fmh = (pieceHP / maxHP);
        }
        if (pieceMP <= 0)
        {
            fmp = 0.0f;
        }
        else
        {
            fmp = pieceMP / maxMP;
        }

        if (fmh > 1.0f)
        {
            fmh = 1.0f;
        }
        if(fmp > 1.0f)
        {
            fmp = 1.0f;
        }

        hpBarImage.fillAmount = fmh;
        mpBarImage.fillAmount = fmp;
    }

    // 마나 확인
    public bool PieceMPCheck()
    {
        if (pieceMP >= maxMP)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PieceTurnChange(int turn)
    {
        PieceManaChange(turn);
        PieceActionChange();
    }

    public virtual void PieceManaChange(int turn)
    {
        pieceMP += turnCreaseMana;

        if (pieceMP > maxMP)
        {
            pieceMP = maxMP;
        }
    }

    public void PieceActionChange()
    {
        move = true;
        action = true;
    }

    #endregion

    #region 피스 변화

    // 말이 생성될경우
    public virtual void PieceAdd()
    {
        if (pieceTransform.tag == "APiece")
        {
            GameManager.Instance.aPiece.Add(this);
        }
        else
        {
            GameManager.Instance.bPiece.Add(this);
        }
        if(GameManager.Instance.player == gamePlayer)
        {
            isPlayer = true;
        }
        GameManager.Instance.board.boardPiece[CurrentX, CurrentZ] = this;
    }

    // 말이 죽을경우
    public void PieceDead()
    {
        StartCoroutine(ActionAnim(5));
    }

    private void OnDestroy()
    {
        if (pieceTransform.tag == "APiece")
        {
            GameManager.Instance.aPiece.Remove(this);
        }
        else
        {
            GameManager.Instance.bPiece.Remove(this);
        }
        Board.Instance.boardPiece[CurrentX, CurrentZ] = null;
    }

    #endregion

    #region 카드

    // 카드 정보
    public CardData DataGet()
    {
        DataSetting();
        CardData cardData = new CardData
        {
            piece_Name = piece_Name,
            piece_Dir = piece_Dir,
            piece_Damage = attackDamage,
            piece_HP = maxHP,
            piece_DrowMana = drowMana,
            piece_FieldMana = fieldMana,
            moveRange = PossibleNoneRange(0),
            attackRange = PossibleNoneRange(1),
            skillRange = PossibleNoneRange(2)
        };
        return cardData;
    }

    // 카드 범위
    public virtual int[,] PossibleNoneRange(int action)
    {
        return new int[3, 3];
    }

    #endregion

    #region 피스 범위

    // 죽음 범위
    public bool[,] DeathPieceCheck(Piece[,] pieces, bool[,] _range, bool attack)
    {
        bool[,] death = new bool[9, 9];
        int thisDamage = 0;
        if (attack)
        {
            thisDamage = attackDamage;
        }
        else
        {
            thisDamage = skillDamage;
        }
        for (int x = 0; x < 9; x++)
        {
            for (int z = 0; z < 9; z++)
            {
                if (_range[x, z] && pieces[x, z] != null)
                {
                    if (pieces[x, z].pieceHP <= thisDamage)
                    {
                        death[x, z] = true;
                    }
                }
            }
        }
        return death;
    }

    // 죽음 체크
    public bool ActionDeathCheck(Piece[,] pieces,bool attack)
    {
        if (attack)
        {
            if(pieces[TargetX, TargetZ] == null)
            {
                return false;
            }
            else
            {
                if (pieces[TargetX, TargetZ].pieceHP <= attackDamage)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            if (pieces[TargetX, TargetZ] == null)
            {
                return false;
            }
            else
            {
                if (pieces[TargetX, TargetZ].pieceHP <= skillDamage)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    // 이동 범위
    public virtual bool[,] PossibleMove(Piece[,] pieces, int playerMana, bool range)
    {
        return new bool[9, 9];
    }

    // 이동 가능 마나
    public virtual int MoveManaCheck(Vector2 v1, Vector2 v2)
    {
        int moveSpaceMana;

        if ((int)Mathf.Abs(v1.x - v2.x) > spaceRange || (int)Mathf.Abs(v1.y - v2.y) > spaceRange)
        {
            moveSpaceMana = moveMana + spaceMana * (int)(Mathf.Abs(v1.x - v2.x) / spaceRange);
        }
        else
        {
            moveSpaceMana = moveMana;
        }

        return -moveSpaceMana;
    }

    // 공격 범위
    public virtual bool[,] PossibleAttack(Piece[,] pieces, int playerMana, bool range)
    {
        return new bool[9, 9];
    }

    public virtual bool[,] PossibleTarget(Piece[,] pieces)
    {
        bool[,] attackRange = new bool[9, 9];
        attackRange[TargetX, TargetZ] = true;
        return attackRange;
    }

    // 스킬 사용 범위
    public virtual bool[,] PossibleSkill(Piece[,] pieces, int playerMana, bool range)
    {
        return new bool[9, 9];
    }

    // 스킬 공격 범위
    public virtual bool[,] PossibleSkillRange(Piece[,] pieces)
    {
        bool[,] skillRange = new bool[9, 9];
        skillRange[TargetX, TargetZ] = true;
        return skillRange;
    }

    // 이동 가능 확인
    public bool ValidMove(Piece[,] pieces, int x2, int y2)
    {
        if (pieces[x2, y2] != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // 공격 가능 확인
    public bool ValidAttack(Piece[,] pieces, bool _range, int x2, int y2)
    {
        if ((pieces[x2, y2] != null && pieces[x2, y2].isPlayer != isPlayer) || _range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // 스킬 가능 확인
    public bool ValidSkill(Piece[,] pieces, bool _range, int x2, int y2)
    {
        if ((pieces[x2, y2] != null && pieces[x2, y2].isPlayer != isPlayer) || _range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
