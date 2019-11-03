using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override void PieceAdd()
    {
        base.PieceAdd();
        if (GameManager.Instance.player)
        {
            if (gameObject.tag == "APiece")
            {
                GameManager.Instance.playerKing = this;
                GameManager.Instance.playerKingHP = 50;
                GameManager.Instance.kingV.x = CurrentX;
                GameManager.Instance.kingV.y = CurrentZ;
            }
        }
        else
        {
            if (gameObject.tag == "BPiece")
            {
                GameManager.Instance.playerKing = this;
                GameManager.Instance.playerKingHP = 50;
                GameManager.Instance.kingV.x = CurrentX;
                GameManager.Instance.kingV.y = CurrentZ;
            }
        }
    }

    public override void DataSetting()
    {
        ws = new WaitForSeconds(0.1f);
        pieceTransform = transform;
        piece_Name = "왕";
        piece_Dir = "죽으면 게임이 끝납니다.";
        level = 0;
        pieceHP = 50;
        pieceMP = 0;

        maxHP = 50;
        maxMP = 5;

        drowMana = 1;
        fieldMana = 0;

        moveMana = 1;
        spaceMana = 0;
        moveRange = 1;
        spaceRange = 1;

        attackMana = 1;
        attackDamage = 12;
        attackRange = 1;
        targetRange = 1;

        skillMana = 2;
        skillDamage = 18;
        skillRange = 2;
        skillAttackRange = 2;
        skillShakeRange = 30.0f;
        skillShakeForce = 100.0f;

        orderNum = -1;

        piecePosition.y = -0.34f;
        if (GameManager.Instance.player)
        {
            if (tag == "APiece")
            {
                barAddset.y = 1.4f;
            }
            else
            {
                barAddset.y = 1.3f;

            }
        }
        else
        {
            if (tag == "APiece")
            {
                barAddset.y = 1.3f;
            }
            else
            {
                barAddset.y = 1.4f;

            }
        }
    }

    public override void DamageCheck(int damage)
    {
        if (GameManager.Instance.player)
        {
            if (gameObject.tag == "APiece")
            {
                GameManager.Instance.NetKingCrease(-damage);
            }
            else
            {
                pieceHP -= damage;
            }
        }
        else
        {
            if(gameObject.tag == "BPiece")
            {
                GameManager.Instance.NetKingCrease(-damage);
            }
            else
            {
                pieceHP -= damage;
            }
        }
        if (pieceHP < 0)
        {
            pieceHP = 0;
        }
    }

    public override bool[,] PossibleMove(Piece[,] pieces, int playerMana)
    {
        bool[,] move = new bool[9, 9];

        if (moveMana <= playerMana || !isPlayer)
        {
            for (int x = 0; x <= 1; x++)
            {
                for (int z = 0; z <= 1; z++)
                {
                    if (CurrentX + x <= 8)
                    {
                        if (CurrentZ + z <= 8)
                        {
                            move[CurrentX + x, CurrentZ + z] = ValidMove(pieces, CurrentX + x, CurrentZ + z);
                        }
                        if (CurrentZ - z >= 0)
                        {
                            move[CurrentX + x, CurrentZ - z] = ValidMove(pieces, CurrentX + x, CurrentZ - z);
                        }
                    }
                    if (CurrentX - x >= 0)
                    {
                        if (CurrentZ + z <= 8)
                        {
                            move[CurrentX - x, CurrentZ + z] = ValidMove(pieces, CurrentX - x, CurrentZ + z);
                        }
                        if (CurrentZ - z >= 0)
                        {
                            move[CurrentX - x, CurrentZ - z] = ValidMove(pieces, CurrentX - x, CurrentZ - z);
                        }
                    }
                }
            }
        }
        return move;
    }

    public override bool[,] PossibleAttack(Piece[,] pieces, int playerMana)
    {
        bool[,] attack = new bool[9, 9];

        if (attackMana <= playerMana || !isPlayer)
        {
            for (int x = 0; x <= 1; x++)
            {
                for (int z = 0; z <= 1; z++)
                {
                    if (CurrentX + x <= 8)
                    {
                        if (CurrentZ + z <= 8)
                        {
                            attack[CurrentX + x, CurrentZ + z] = ValidAttack(pieces, CurrentX + x, CurrentZ + z);
                        }
                        if (CurrentZ - z >= 0)
                        {
                            attack[CurrentX + x, CurrentZ - z] = ValidAttack(pieces, CurrentX + x, CurrentZ - z);
                        }
                    }
                    if (CurrentX - x >= 0)
                    {
                        if (CurrentZ + z <= 8)
                        {
                            attack[CurrentX - x, CurrentZ + z] = ValidAttack(pieces, CurrentX - x, CurrentZ + z);
                        }
                        if (CurrentZ - z >= 0)
                        {
                            attack[CurrentX - x, CurrentZ - z] = ValidAttack(pieces, CurrentX - x, CurrentZ - z);
                        }
                    }
                }
            }
        }
        return attack;
    }

    public override bool[,] PossibleSkill(Piece[,] pieces, int playerMana)
    {
        bool[,] skill = new bool[9, 9];

        if (skillMana <= playerMana && pieceMP >= maxMP || !isPlayer)
        {
            for (int x = 0; x <= 1; x++)
            {
                for (int z = 0; z <= 1; z++)
                {
                    if (CurrentX + x <= 8)
                    {
                        if (CurrentZ + z <= 8)
                        {
                            skill[CurrentX + x, CurrentZ + z] = ValidSkill(pieces, CurrentX + x, CurrentZ + z);
                        }
                        if (CurrentZ - z >= 0)
                        {
                            skill[CurrentX + x, CurrentZ - z] = ValidSkill(pieces, CurrentX + x, CurrentZ - z);
                        }
                    }
                    if (CurrentX - x >= 0)
                    {
                        if (CurrentZ + z <= 8)
                        {
                            skill[CurrentX - x, CurrentZ + z] = ValidSkill(pieces, CurrentX - x, CurrentZ + z);
                        }
                        if (CurrentZ - z >= 0)
                        {
                            skill[CurrentX - x, CurrentZ - z] = ValidSkill(pieces, CurrentX - x, CurrentZ - z);
                        }
                    }
                }
            }
        }
        return skill;
    }

    public override bool[,] PossibleSkillRange(Piece[,] pieces)
    {
        bool[,] skillRange = new bool[9, 9];
        for (int x = 0; x <= skillAttackRange; x++)
        {
            for (int z = 0; z <= skillAttackRange; z++)
            {
                if (TargetX + x <= 8)
                {
                    skillRange[TargetX + x, TargetZ] = ValidSkill(pieces, TargetX + x, TargetZ);
                    if (TargetZ + z <= 8)
                    {
                        skillRange[TargetX + x, TargetZ + z] = ValidSkill(pieces, TargetX + x, TargetZ + z);
                    }
                    if (TargetZ - z >= 0)
                    {
                        skillRange[TargetX + x, TargetZ - z] = ValidSkill(pieces, TargetX + x, TargetZ - z);
                    }
                }
                if (TargetX - x >= 0)
                {
                    skillRange[TargetX - x, TargetZ] = ValidSkill(pieces, TargetX - x, TargetZ);
                    if (TargetZ + z <= 8)
                    {
                        skillRange[TargetX - x, TargetZ + z] = ValidSkill(pieces, TargetX - x, TargetZ + z);
                    }
                    if (TargetZ - z >= 0)
                    {
                        skillRange[TargetX - x, TargetZ - z] = ValidSkill(pieces, TargetX - x, TargetZ - z);
                    }
                }
                if (TargetZ + z <= 8)
                {
                    skillRange[TargetX, TargetZ + z] = ValidSkill(pieces, TargetX, TargetZ + z);
                }
                if (TargetZ - z >= 0)
                {
                    skillRange[TargetX, TargetZ - z] = ValidSkill(pieces, TargetX, TargetZ - z);
                }
            }
        }
        skillRange[TargetX, TargetZ] = true;
        return skillRange;
    }

    public override int[,] PossibleNoneRange(int action)
    {
        int[,] actionRange = new int[3, 3];
        int pieceX = 1;
        int pieceZ = 1;

        if (action == 0 || action == 1 || action == 2)
        {
            actionRange[pieceX + 1, pieceZ] = 2;
            actionRange[pieceX - 1, pieceZ] = 2;
            actionRange[pieceX, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ - 1] = 2;
            actionRange[pieceX + 1, pieceZ - 1] = 2;
            actionRange[pieceX + 1, pieceZ + 1] = 2;
            actionRange[pieceX - 1, pieceZ - 1] = 2;
            actionRange[pieceX - 1, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }

        return actionRange;
    }
}
