﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public override void DataSetting()
    {
        ws = new WaitForSeconds(0.1f);
        pieceTransform = transform;
        piece_Name = "기사";
        piece_Dir = "왕을 위하여 피를!";
        level = 0;
        pieceHP = 33;
        pieceMP = 0;

        maxHP = 33;
        maxMP = 5;

        drowMana = 3;
        fieldMana = 1;

        moveMana = 1;
        spaceMana = 0;
        moveRange = 2;
        spaceRange = 0;

        attackMana = 2;
        attackDamage = 18;
        attackRange = 2;
        targetRange = 1;

        skillMana = 2;
        skillDamage = 19;
        skillRange = 2;
        skillAttackRange = 1;
        skillShakeRange = 30.0f;
        skillShakeForce = 100.0f;

        orderNum = 1;

        piecePosition.y = -0.34f;
        if (tag == "APiece")
        {
            barAddset.y = 1.25f;
        }
        else
        {
            barAddset.y = 0.85f;
        }
    }

    public override bool[,] PossibleMove(Piece[,] pieces, int playerMana)
    {
        bool[,] move = new bool[9, 9];

        if (moveMana <= playerMana || !isPlayer)
        {
            for (int x = 0; x <= moveRange; x++)
            {
                for (int z = 0; z <= moveRange; z++)
                {
                    if (gameObject.tag == "APiece")
                    {
                        if (z == 1)
                        {
                            if (CurrentZ + z <= 8 && x == 1)
                            {

                                if (CurrentX + x <= 8)
                                {
                                    move[CurrentX + x, CurrentZ + z] = ValidMove(pieces, CurrentX + x, CurrentZ + z);
                                }

                                if (CurrentX - x >= 0)
                                {
                                    move[CurrentX - x, CurrentZ + z] = ValidMove(pieces, CurrentX - x, CurrentZ + z);
                                }
                            }

                            if (CurrentZ - z >= 0)
                            {
                                move[CurrentX, CurrentZ - z] = ValidMove(pieces, CurrentX, CurrentZ - z);
                            }
                        }

                        else if (z == 2 && x == 1)
                        {
                            if (CurrentZ + z <= 8)
                            {

                                if (CurrentX + x <= 8)
                                {
                                    move[CurrentX + x, CurrentZ + z] = ValidMove(pieces, CurrentX + x, CurrentZ + z);
                                }

                                if (CurrentX - x >= 0)
                                {
                                    move[CurrentX - x, CurrentZ + z] = ValidMove(pieces, CurrentX - x, CurrentZ + z);
                                }

                            }
                        }
                    }
                    else
                    {
                        if (z == 1)
                        {
                            if (CurrentZ + z <= 8)
                            {
                                move[CurrentX, CurrentZ + z] = ValidMove(pieces, CurrentX, CurrentZ + z);
                            }

                            if (CurrentZ - z >= 0 && x == 1)
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

                        else if (z == 2 && x == 1)
                        {
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
            if (tag == "APiece")
            {
                attack[CurrentX - 1, CurrentZ + 1] = ValidAttack(pieces, CurrentX - 1, CurrentZ + 1);
                attack[CurrentX, CurrentZ + 1] = ValidAttack(pieces, CurrentX, CurrentZ + 1);
                attack[CurrentX + 1, CurrentZ + 1] = ValidAttack(pieces, CurrentX + 1, CurrentZ + 1);
                attack[CurrentX - 1, CurrentZ] = ValidAttack(pieces, CurrentX - 1, CurrentZ);
                attack[CurrentX + 1, CurrentZ] = ValidAttack(pieces, CurrentX + 1, CurrentZ);
            }
            else
            {
                attack[CurrentX - 1, CurrentZ - 1] = ValidAttack(pieces, CurrentX - 1, CurrentZ - 1);
                attack[CurrentX, CurrentZ - 1] = ValidAttack(pieces, CurrentX, CurrentZ - 1);
                attack[CurrentX + 1, CurrentZ - 1] = ValidAttack(pieces, CurrentX + 1, CurrentZ - 1);
                attack[CurrentX - 1, CurrentZ] = ValidAttack(pieces, CurrentX - 1, CurrentZ);
                attack[CurrentX + 1, CurrentZ] = ValidAttack(pieces, CurrentX + 1, CurrentZ);
            }
        }
        return attack;
    }

    public override bool[,] PossibleSkill(Piece[,] pieces, int playerMana)
    {
        bool[,] skill = new bool[9, 9];
        if (skillMana <= playerMana && pieceMP >= maxMP || !isPlayer)
        {
            if (tag == "APiece")
            {
                if(CurrentZ + 1 <= 8)
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

                if(CurrentZ + 2 <= 8)
                {
                    if (CurrentX + 1 <= 8)
                    {
                        skill[CurrentX + 1, CurrentZ + 2] = ValidSkill(pieces, CurrentX + 1, CurrentZ + 2);
                    }

                    skill[CurrentX, CurrentZ + 2] = ValidSkill(pieces, CurrentX, CurrentZ + 2);

                    if (CurrentX - 1 >= 0)
                    {
                        skill[CurrentX - 1, CurrentZ + 2] = ValidSkill(pieces, CurrentX - 1, CurrentZ + 2);
                    }
                }

                if (CurrentZ - 2 >= 0)
                {
                    if (CurrentX + 1 <= 8)
                    {
                        skill[CurrentX + 1, CurrentZ - 2] = ValidSkill(pieces, CurrentX + 1, CurrentZ - 2);
                    }
                    if (CurrentX - 1 >= 0)
                    {
                        skill[CurrentX - 1, CurrentZ - 2] = ValidSkill(pieces, CurrentX - 1, CurrentZ - 2);
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

                if (CurrentZ - 2 >= 0)
                {
                    if (CurrentX + 1 <= 8)
                    {
                        skill[CurrentX + 1, CurrentZ - 2] = ValidSkill(pieces, CurrentX + 1, CurrentZ - 2);
                    }

                    skill[CurrentX, CurrentZ - 2] = ValidSkill(pieces, CurrentX, CurrentZ - 2);

                    if (CurrentX - 1 >= 0)
                    {
                        skill[CurrentX - 1, CurrentZ - 2] = ValidSkill(pieces, CurrentX - 1, CurrentZ - 2);
                    }
                }

                if (CurrentZ + 2 <= 8)
                {
                    if (CurrentX + 1 <= 8)
                    {
                        skill[CurrentX + 1, CurrentZ - 2] = ValidSkill(pieces, CurrentX + 1, CurrentZ + 2);
                    }
                    if (CurrentX - 1 >= 0)
                    {
                        skill[CurrentX - 1, CurrentZ - 2] = ValidSkill(pieces, CurrentX - 1, CurrentZ + 2);
                    }
                }
            }
        }
        return skill;
    }

    public override int[,] PossibleNoneRange(int action)
    {
        int[,] actionRange = new int[3, 3];
        int pieceX = 1;
        int pieceZ = 1;

        if (action == 0)
        {
            actionRange[pieceX + 1, pieceZ + 1] = 2;
            actionRange[pieceX - 1, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ - 1] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }
        else if (action == 1)
        {

            actionRange[pieceX + 1, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ + 1] = 2;
            actionRange[pieceX - 1, pieceZ + 1] = 2;
            actionRange[pieceX + 1, pieceZ] = 2;
            actionRange[pieceX - 1, pieceZ] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }
        else if (action == 2)
        {
            actionRange[pieceX + 1, pieceZ + 1] = 2;
            actionRange[pieceX - 1, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }

        return actionRange;
    }
}