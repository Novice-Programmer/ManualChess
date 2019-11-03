using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hobgoblin : Piece
{
    public override void DataSetting()
    {
        ws = new WaitForSeconds(0.1f);
        pieceTransform = transform;
        piece_Name = "홉 고블린";
        piece_Dir = "고블린에서 진화한 객체!";
        level = 0;
        pieceHP = 33;
        pieceMP = 0;

        maxHP = 33;
        maxMP = 5;

        drowMana = 2;
        fieldMana = 2;

        moveMana = 2;
        spaceMana = 0;
        moveRange = 1;
        spaceRange = 0;

        attackMana = 2;
        attackDamage = 18;
        attackRange = 1;
        targetRange = 1;

        skillMana = 2;
        skillDamage = 22;
        skillRange = 1;
        skillAttackRange = 1;
        skillReduce = true;
        skillReduceDamage = 9;
        skillShakeRange = 30.0f;
        skillShakeForce = 100.0f;

        orderNum = 4;

        piecePosition.y = -0.34f;
        barAddset.x = 0.1f;
        if (GameManager.Instance.player)
        {
            if (tag == "APiece")
            {
                barAddset.y = 0.7f;
            }
            else
            {
                barAddset.y = 0.85f;
            }
        }
        else
        {
            if (tag == "APiece")
            {
                barAddset.y = 0.85f;
            }
            else
            {
                barAddset.y = 0.7f;
            }
        }
    }

    public override bool[,] PossibleMove(Piece[,] pieces, int playerMana)
    {
        bool[,] move = new bool[9, 9];
        if (moveMana <= playerMana || !isPlayer)
        {
            if (tag == "APiece")
            {
                if (CurrentZ + 1 <= 8)
                {
                    if (CurrentX + 1 <= 8)
                    {
                        move[CurrentX + 1, CurrentZ + 1] = ValidMove(pieces, CurrentX + 1, CurrentZ + 1);
                    }

                    move[CurrentX, CurrentZ + 1] = ValidMove(pieces, CurrentX, CurrentZ + 1);

                    if (CurrentX - 1 >= 0)
                    {
                        move[CurrentX - 1, CurrentZ + 1] = ValidMove(pieces, CurrentX - 1, CurrentZ + 1);
                    }
                }

                if (CurrentZ - 1 >= 0)
                {
                    move[CurrentX, CurrentZ - 1] = ValidMove(pieces, CurrentX, CurrentZ - 1);
                }
            }

            else
            {
                if (CurrentZ + 1 <= 8)
                {
                    move[CurrentX, CurrentZ + 1] = ValidMove(pieces, CurrentX, CurrentZ + 1);
                }

                if (CurrentZ - 1 >= 0)
                {
                    if (CurrentX + 1 <= 8)
                    {
                        move[CurrentX + 1, CurrentZ - 1] = ValidMove(pieces, CurrentX + 1, CurrentZ - 1);
                    }

                    move[CurrentX, CurrentZ - 1] = ValidMove(pieces, CurrentX, CurrentZ - 1);

                    if (CurrentX - 1 >= 0)
                    {
                        move[CurrentX - 1, CurrentZ - 1] = ValidMove(pieces, CurrentX - 1, CurrentZ - 1);
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
            if (gameObject.tag == "APiece")
            {
                if(CurrentZ + 1 <= 8)
                {
                    attack[CurrentX, CurrentZ + 1] = ValidAttack(pieces, CurrentX, CurrentZ + 1);
                }
            }
            else
            {
                if(CurrentZ - 1 >= 0)
                {
                    attack[CurrentX, CurrentZ - 1] = ValidAttack(pieces, CurrentX, CurrentZ - 1);
                }
            }
        }

        return attack;
    }

    public override bool[,] PossibleSkill(Piece[,] pieces, int playerMana)
    {
        bool[,] skill = new bool[9, 9];
        if (skillMana <= playerMana || !isPlayer)
        {
            if (gameObject.tag == "APiece")
            {
                if (CurrentZ + 1 <= 8)
                {
                    if (CurrentX + 1 <= 8)
                    {
                        skill[CurrentX + 1, CurrentZ + 1] = ValidSkill(pieces, CurrentX + 1, CurrentZ + 1);
                    }

                    if (CurrentX - 1 >= 0)
                    {
                        skill[CurrentX - 1, CurrentZ + 1] = ValidSkill(pieces, CurrentX - 1, CurrentZ + 1);
                    }
                }
            }

            else
            {
                if (CurrentZ - 1 >= 0)
                {
                    if (CurrentX + 1 <= 8)
                    {
                        skill[CurrentX + 1, CurrentZ - 1] = ValidSkill(pieces, CurrentX + 1, CurrentZ - 1);
                    }

                    if (CurrentX - 1 >= 0)
                    {
                        skill[CurrentX - 1, CurrentZ - 1] = ValidSkill(pieces, CurrentX - 1, CurrentZ - 1);
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


        if (action == 0)
        {
            actionRange[pieceX - 1, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ + 1] = 2;
            actionRange[pieceX + 1, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ - 1] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }
        else if (action == 1)
        {
            actionRange[pieceX, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }
        else if (action == 2)
        {
            actionRange[pieceX - 1, pieceZ + 1] = 2;
            actionRange[pieceX + 1, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }

        return actionRange;
    }
}
