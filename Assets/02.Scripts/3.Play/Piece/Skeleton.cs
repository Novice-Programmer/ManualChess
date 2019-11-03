using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Piece
{
    public override void DataSetting()
    {
        ws = new WaitForSeconds(0.1f);
        pieceTransform = transform;
        piece_Name = "해골";
        piece_Dir = "강력한 해골기사 입니다.";
        level = 0;
        pieceHP = 30;
        pieceMP = 0;

        maxHP = 28;
        maxMP = 3;

        drowMana = 2;
        fieldMana = 1;

        moveMana = 1;
        spaceMana = 0;
        moveRange = 1;
        spaceRange = 1;

        attackMana = 2;
        attackDamage = 17;
        attackRange = 1;
        targetRange = 1;

        skillMana = 2;
        skillDamage = 16;
        skillRange = 2;
        skillAttackRange = 2;
        skillShakeRange = 30.0f;
        skillShakeForce = 100.0f;

        orderNum = 2;

        piecePosition.y = -0.34f;
        if (tag == "APiece")
        {
            barAddset.x = -0.1f;
            barAddset.y = 1.1f;
        }
        else
        {
            barAddset.y = 0.5f;
        }
    }

    public override bool[,] PossibleMove(Piece[,] pieces, int playerMana)
    {
        bool[,] move = new bool[9, 9];

        if (moveMana <= playerMana || !isPlayer)
        {
            if (CurrentZ + 1 <= 8)
            {
                if (CurrentX + 1 <= 8)
                {
                    move[CurrentX + 1, CurrentZ + 1] = ValidMove(pieces, CurrentX + 1, CurrentZ + 1);
                }
                if (CurrentX - 1 >= 0)
                {
                    move[CurrentX - 1, CurrentZ + 1] = ValidMove(pieces, CurrentX - 1, CurrentZ + 1);
                }
            }
            if (CurrentZ - 1 >= 0)
            {
                if (CurrentX + 1 <= 8)
                {
                    move[CurrentX + 1, CurrentZ - 1] = ValidMove(pieces, CurrentX + 1, CurrentZ - 1);
                }
                if (CurrentX - 1 >= 0)
                {
                    move[CurrentX - 1, CurrentZ - 1] = ValidMove(pieces, CurrentX - 1, CurrentZ - 1);
                }
            }


            if (CurrentZ + 2 <= 8)
            {
                move[CurrentX, CurrentZ + 2] = ValidMove(pieces, CurrentX, CurrentZ + 2);
            }

            if (CurrentZ - 2 >= 0)
            {
                move[CurrentX, CurrentZ - 2] = ValidMove(pieces, CurrentX, CurrentZ - 2);
            }

            if (CurrentX + 2 <= 8)
            {
                move[CurrentX + 2, CurrentZ] = ValidMove(pieces, CurrentX + 2, CurrentZ);
            }

            if (CurrentX - 2 >= 0)
            {
                move[CurrentX - 2, CurrentZ] = ValidMove(pieces, CurrentX - 2, CurrentZ);
            }
        }
        return move;
    }

    public override bool[,] PossibleAttack(Piece[,] pieces, int playerMana)
    {
        bool[,] attack = new bool[9, 9];

        if (attackMana <= playerMana || !isPlayer)
        {
            if (CurrentZ + 1 <= 8)
            {
                attack[CurrentX, CurrentZ + 1] = ValidAttack(pieces, CurrentX, CurrentZ + 1);

                if (CurrentX - 1 >= 0)
                {
                    attack[CurrentX - 1, CurrentZ + 1] = ValidAttack(pieces, CurrentX - 1, CurrentZ + 1);
                }

                if (CurrentX + 1 <= 8)
                {
                    attack[CurrentX + 1, CurrentZ + 1] = ValidAttack(pieces, CurrentX + 1, CurrentZ + 1);
                }
            }

            if (CurrentZ - 1 >= 0)
            {
                attack[CurrentX, CurrentZ - 1] = ValidAttack(pieces, CurrentX, CurrentZ - 1);

                if (CurrentX - 1 >= 0)
                {
                    attack[CurrentX - 1, CurrentZ - 1] = ValidAttack(pieces, CurrentX - 1, CurrentZ - 1);
                }

                if (CurrentX + 1 <= 8)
                {
                    attack[CurrentX + 1, CurrentZ - 1] = ValidAttack(pieces, CurrentX + 1, CurrentZ - 1);
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
            for (int x = 0; x <= 2; x++)
            {
                for (int z = 0; z <= 2; z++)
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
        for (int x = 0; x <= 2; x++)
        {
            for (int z = 0; z <= 2; z++)
            {
                if (CurrentX + x <= 8)
                {
                    if (CurrentZ + z <= 8)
                    {
                        skillRange[CurrentX + x, CurrentZ + z] = ValidSkill(pieces, CurrentX + x, CurrentZ + z);
                    }
                    if (CurrentZ - z >= 0)
                    {
                        skillRange[CurrentX + x, CurrentZ - z] = ValidSkill(pieces, CurrentX + x, CurrentZ - z);
                    }
                }
                if (CurrentX - x >= 0)
                {
                    if (CurrentZ + z <= 8)
                    {
                        skillRange[CurrentX - x, CurrentZ + z] = ValidSkill(pieces, CurrentX - x, CurrentZ + z);
                    }
                    if (CurrentZ - z >= 0)
                    {
                        skillRange[CurrentX - x, CurrentZ - z] = ValidSkill(pieces, CurrentX - x, CurrentZ - z);
                    }

                }
            }
        }
        return skillRange;
    }

    public override int[,] PossibleNoneRange(int action)
    {
        int[,] actionRange = new int[3, 3];
        int pieceX = 1;
        int pieceZ = 1;

        if (action == 0)
        {
            actionRange[pieceX - 1, pieceZ - 1] = 2;
            actionRange[pieceX + 1, pieceZ - 1] = 2;
            actionRange[pieceX - 1, pieceZ + 1] = 2;
            actionRange[pieceX + 1, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }
        else if (action == 1)
        {
            actionRange[pieceX - 1, pieceZ - 1] = 2;
            actionRange[pieceX + 1, pieceZ - 1] = 2;
            actionRange[pieceX, pieceZ - 1] = 2;
            actionRange[pieceX - 1, pieceZ + 1] = 2;
            actionRange[pieceX + 1, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }
        else if (action == 2)
        {
            actionRange[pieceX - 1, pieceZ - 1] = 2;
            actionRange[pieceX + 1, pieceZ - 1] = 2;
            actionRange[pieceX, pieceZ - 1] = 2;
            actionRange[pieceX - 1, pieceZ] = 2;
            actionRange[pieceX - 1, pieceZ + 1] = 2;
            actionRange[pieceX + 1, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ + 1] = 2;
            actionRange[pieceX + 1, pieceZ] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }

        return actionRange;
    }
}
