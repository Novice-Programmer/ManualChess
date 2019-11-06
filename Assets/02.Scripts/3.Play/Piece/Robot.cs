using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : Piece
{
    public override void DataSetting()
    {
        ws = new WaitForSeconds(0.1f);
        pieceTransform = transform;
        piece_Name = "로봇";
        piece_Dir = "마법으로 움직이는 골렘";
        level = 0;
        pieceHP = 24;
        pieceMP = 0;

        maxHP = 24;
        maxMP = 4;

        drowMana = 1;
        fieldMana = 1;

        moveMana = 1;
        spaceMana = 0;
        moveRange = 1;
        spaceRange = 0;

        attackMana = 1;
        attackDamage = 10;
        attackRange = 1;
        targetRange = 1;

        skillMana = 2;
        skillDamage = 18;
        skillRange = 1;
        skillAttackRange = 1;
        skillShakeRange = 30.0f;
        skillShakeForce = 100.0f;

        orderNum = 5;

        piecePosition.y = -0.34f;
        if (GameManager.Instance.player)
        {
            if (tag == "APiece")
            {
                barAddset.x = 0.15f;
                barAddset.y = 1.2f;
            }
            else
            {
                barAddset.y = 1.25f;
            }
        }
        else
        {
            if (tag == "APiece")
            {
                barAddset.y = 1.25f;
            }
            else
            {
                barAddset.x = 0.15f;
                barAddset.y = 1.2f;
            }
        }
    }

    public override bool[,] PossibleMove(Piece[,] pieces, int playerMana, bool range)
    {
        bool[,] move = new bool[9, 9];

        if (moveMana <= playerMana || !isPlayer || range)
        {
            if (CurrentZ + 1 <= 8)
            {
                move[CurrentX, CurrentZ + 1] = ValidMove(pieces, CurrentX, CurrentZ + 1);
            }

            if (CurrentZ - 1 >= 0)
            {
                move[CurrentX, CurrentZ - 1] = ValidMove(pieces, CurrentX, CurrentZ - 1);
            }

            if(CurrentX + 1 <= 8)
            {
                move[CurrentX + 1, CurrentZ] = ValidMove(pieces, CurrentX + 1, CurrentZ);
            }

            if(CurrentX - 1 >= 0)
            {
                move[CurrentX - 1, CurrentZ] = ValidMove(pieces, CurrentX - 1, CurrentZ);
            }
        }
        return move;
    }

    public override bool[,] PossibleAttack(Piece[,] pieces, int playerMana, bool range)
    {
        bool[,] attack = new bool[9, 9];

        if (attackMana <= playerMana || !isPlayer || range)
        {
            if (CurrentZ + 1 <= 8)
            {
                attack[CurrentX, CurrentZ + 1] = ValidAttack(pieces, range, CurrentX, CurrentZ + 1);
            }

            if (CurrentZ - 1 >= 0)
            {
                attack[CurrentX, CurrentZ - 1] = ValidAttack(pieces, range, CurrentX, CurrentZ - 1);
            }

            if (CurrentX + 1 <= 8)
            {
                attack[CurrentX + 1, CurrentZ] = ValidAttack(pieces, range, CurrentX + 1, CurrentZ);
            }

            if (CurrentX - 1 >= 0)
            {
                attack[CurrentX - 1, CurrentZ] = ValidAttack(pieces, range, CurrentX - 1, CurrentZ);
            }
        }
        return attack;
    }

    public override bool[,] PossibleSkill(Piece[,] pieces, int playerMana, bool range)
    {
        bool[,] skill = new bool[9, 9];

        if (skillMana <= playerMana && pieceMP >= maxMP || !isPlayer || range)
        {
            if (tag == "APiece")
            {
                for (int z = 0; z <= 2; z++)
                {
                    if (CurrentZ + z <= 8)
                    {
                        skill[CurrentX, CurrentZ + z] = ValidSkill(pieces, range, CurrentX, CurrentZ + z);
                    }
                }
            }

            else
            {
                for (int z = 0; z <= 2; z++)
                {
                    if (CurrentZ - z >= 0)
                    {
                        skill[CurrentX, CurrentZ - z] = ValidSkill(pieces, range, CurrentX, CurrentZ - z);
                    }
                }
            }
        }
        return skill;
    }

    public override bool[,] PossibleSkillRange(Piece[,] pieces)
    {
        bool[,] skillRange = new bool[9, 9];

        if (TargetX - 1 >= 0)
        {
            skillRange[TargetX - 1, TargetZ] = ValidSkill(pieces, false, TargetX - 1, TargetZ);
        }
        if (TargetX + 1 <= 8)
        {
            skillRange[TargetX + 1, TargetZ] = ValidSkill(pieces, false, TargetX + 1, TargetZ);
        }

        skillRange[TargetX, TargetZ] = ValidSkill(pieces, false, TargetX, TargetZ);
        return skillRange;
    }

    public override int[,] PossibleNoneRange(int action)
    {
        int[,] actionRange = new int[3, 3];
        int pieceX = 1;
        int pieceZ = 1;

        if (action == 0)
        {
            actionRange[pieceX + 1, pieceZ] = 2;
            actionRange[pieceX - 1, pieceZ] = 2;
            actionRange[pieceX, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ - 1] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }

        else if (action == 1)
        {
            actionRange[pieceX + 1, pieceZ] = 2;
            actionRange[pieceX - 1, pieceZ] = 2;
            actionRange[pieceX, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ - 1] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }

        else if (action == 2)
        {
            actionRange[pieceX, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }

        return actionRange;
    }
}
