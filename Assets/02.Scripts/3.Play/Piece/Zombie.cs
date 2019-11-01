﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Piece
{
    public override void DataSetting()
    {
        ws = new WaitForSeconds(0.1f);
        pieceTransform = transform;
        piece_Name = "좀비";
        piece_Dir = "좀비도 인간입니다만?";
        level = 0;
        pieceHP = 23;
        pieceMP = 0;

        maxHP = 23;
        maxMP = 4;

        drowMana = 1;
        fieldMana = 1;

        moveMana = 1;
        spaceMana = 0;
        moveRange = 1;
        spaceRange = 0;

        attackMana = 1;
        attackDamage = 8;
        attackRange = 1;
        targetRange = 1;

        skillMana = 1;
        skillDamage = 14;
        skillRange = 1;
        skillAttackRange = 1;
        skillShakeRange = 30.0f;
        skillShakeForce = 100.0f;

        orderNum = 1;

        piecePosition.y = -0.34f;
        if (tag == "APiece")
        {
            barAddset.y = 1.2f;
        }
        else
        {
            barAddset.y = 0.8f;
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
                    if (CurrentZ + z <= 8)
                    {
                        if (CurrentX - x >= 0)
                        {
                            move[CurrentX - x, CurrentZ + z] = ValidMove(pieces, CurrentX - x, CurrentZ + z);
                        }

                        if (CurrentX + x <= 8)
                        {
                            move[CurrentX + x, CurrentZ + z] = ValidMove(pieces, CurrentX + x, CurrentZ + z);
                        }
                    }
                    if (CurrentZ - z >= 0)
                    {
                        if (CurrentX + x <= 8)
                        {
                            move[CurrentX + x, CurrentZ - z] = ValidMove(pieces, CurrentX + x, CurrentZ - z);
                        }
                        if (CurrentX - x >= 0)
                        {
                            move[CurrentX - x, CurrentZ - z] = ValidMove(pieces, CurrentX - x, CurrentZ - z);
                        }
                    }
                }
            }
            move[CurrentX, CurrentZ] = false;
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
            for (int x = 0; x <= 1; x++)
            {
                for (int z = 0; z <= 1; z++)
                {
                    if (CurrentZ + z <= 8)
                    {
                        if (CurrentX - x >= 0)
                        {
                            skill[CurrentX - x, CurrentZ + z] = ValidSkill(pieces, CurrentX - x, CurrentZ + z);
                        }

                        if (CurrentX + x <= 8)
                        {
                            skill[CurrentX + x, CurrentZ + z] = ValidSkill(pieces, CurrentX + x, CurrentZ + z);
                        }
                    }
                    if (CurrentZ - z >= 0)
                    {
                        if (CurrentX + x <= 8)
                        {
                            skill[CurrentX + x, CurrentZ - z] = ValidSkill(pieces, CurrentX + x, CurrentZ - z);
                        }
                        if (CurrentX - x >= 0)
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
        bool[,] _skillRange = new bool[9, 9];
        return _skillRange;
    }

    public override int[,] PossibleNoneRange(int action)
    {
        int[,] actionRange = new int[3, 3];
        int pieceX = 1;
        int pieceZ = 1;

        if (action == 0 || action == 2)
        {
            actionRange[pieceX + 1, pieceZ] = 2;
            actionRange[pieceX - 1, pieceZ] = 2;
            actionRange[pieceX, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ - 1] = 2;
            actionRange[pieceX + 1, pieceZ - 1] = 2;
            actionRange[pieceX + 1, pieceZ + 1] = 2;
            actionRange[pieceX - 1, pieceZ - 1] = 2;
            actionRange[pieceX - 1, pieceZ + 1] = 2;
        }

        else if (action == 1)
        {
            actionRange[pieceX + 1, pieceZ - 1] = 2;
            actionRange[pieceX + 1, pieceZ + 1] = 2;
            actionRange[pieceX - 1, pieceZ - 1] = 2;
            actionRange[pieceX - 1, pieceZ + 1] = 2;
        }

        actionRange[pieceX, pieceZ] = 1;
        return actionRange;
    }
}