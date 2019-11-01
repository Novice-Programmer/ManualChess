﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Piece
{
    public override void DataSetting()
    {
        ws = new WaitForSeconds(0.1f);
        pieceTransform = transform;
        piece_Name = "고블린";
        piece_Dir = "장난을 좋아하는 종족";
        level = 0;
        pieceHP = 25;
        pieceMP = 0;

        maxHP = 25;
        maxMP = 3;

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
        skillDamage = 13;
        skillRange = 1;
        skillAttackRange = 1;
        skillShakeRange = 30.0f;
        skillShakeForce = 100.0f;
        skillReduce = true;
        skillReduceDamage = 7;

        orderNum = 0;

        piecePosition.y = -0.34f;
        if (tag == "APiece")
        {
            barAddset.y = 1.4f;
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
            if (CurrentX + 1 <= 8)
            {
                if (CurrentZ + 1 <= 8)
                {
                    move[CurrentX + 1, CurrentZ + 1] = ValidMove(pieces, CurrentX + 1, CurrentZ + 1);
                }

                if (CurrentZ - 1 >= 0)
                {
                    move[CurrentX + 1, CurrentZ - 1] = ValidMove(pieces, CurrentX + 1, CurrentZ - 1);
                }
            }

            if(CurrentX -1 >= 0)
            {
                if (CurrentZ + 1 <= 8)
                {
                    move[CurrentX - 1, CurrentZ + 1] = ValidMove(pieces, CurrentX - 1, CurrentZ + 1);
                }

                if (CurrentZ - 1 >= 0)
                {
                    move[CurrentX - 1, CurrentZ - 1] = ValidMove(pieces, CurrentX - 1, CurrentZ - 1);
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
            if(tag == "APiece")
            {
                if (CurrentZ + 1 <= 8)
                {
                    attack[CurrentX, CurrentZ + 1] = ValidAttack(pieces, CurrentX, CurrentZ + 1);
                }
            }
            else
            {
                if (CurrentZ - 1 >= 0)
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

        if (skillMana <= playerMana && pieceMP >= maxMP || !isPlayer)
        {
            if (CurrentX + 1 <= 8)
            {
                skill[CurrentX + 1, CurrentZ] = ValidSkill(pieces, CurrentX + 1, CurrentZ);
                if (CurrentZ + 1 <= 8)
                {
                    skill[CurrentX, CurrentZ + 1] = ValidSkill(pieces, CurrentX, CurrentZ + 1);
                    skill[CurrentX + 1, CurrentZ + 1] = ValidSkill(pieces, CurrentX + 1, CurrentZ + 1);
                }
                if (CurrentZ - 1 >= 0)
                {
                    skill[CurrentX, CurrentZ - 1] = ValidSkill(pieces, CurrentX, CurrentZ - 1);
                    skill[CurrentX + 1, CurrentZ - 1] = ValidSkill(pieces, CurrentX + 1, CurrentZ - 1);
                }
            }

            if (CurrentX - 1 >= 0)
            {
                skill[CurrentX - 1, CurrentZ] = ValidSkill(pieces, CurrentX - 1, CurrentZ);
                if (CurrentZ + 1 <= 8)
                {
                    skill[CurrentX - 1, CurrentZ + 1] = ValidSkill(pieces, CurrentX - 1, CurrentZ + 1);
                }
                if (CurrentZ - 1 >= 0)
                {
                    skill[CurrentX - 1, CurrentZ - 1] = ValidSkill(pieces, CurrentX - 1, CurrentZ - 1);
                }
            }
        }
        return skill;
    }

    public override bool[,] PossibleSkillRange(Piece[,] pieces)
    {
        bool[,] skillRange = new bool[9, 9];

        if (CurrentX + 1 <= 8)
        {
            skillRange[CurrentX + 1, CurrentZ] = ValidSkill(pieces, CurrentX + 1, CurrentZ);
            if (CurrentZ + 1 <= 8)
            {
                skillRange[CurrentX, CurrentZ + 1] = ValidSkill(pieces, CurrentX, CurrentZ + 1);
                skillRange[CurrentX + 1, CurrentZ + 1] = ValidSkill(pieces, CurrentX + 1, CurrentZ + 1);
            }
            if (CurrentZ - 1 >= 0)
            {
                skillRange[CurrentX, CurrentZ - 1] = ValidSkill(pieces, CurrentX, CurrentZ - 1);
                skillRange[CurrentX + 1, CurrentZ - 1] = ValidSkill(pieces, CurrentX + 1, CurrentZ - 1);
            }
        }

        if (CurrentX - 1 >= 0)
        {
            skillRange[CurrentX - 1, CurrentZ] = ValidSkill(pieces, CurrentX - 1, CurrentZ);
            if (CurrentZ + 1 <= 8)
            {
                skillRange[CurrentX - 1, CurrentZ + 1] = ValidSkill(pieces, CurrentX - 1, CurrentZ + 1);
            }
            if (CurrentZ - 1 >= 0)
            {
                skillRange[CurrentX - 1, CurrentZ - 1] = ValidSkill(pieces, CurrentX - 1, CurrentZ - 1);
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
            actionRange[pieceX + 1, pieceZ + 1] = 2;
            actionRange[pieceX + 1, pieceZ - 1] = 2;
            actionRange[pieceX - 1, pieceZ + 1] = 2;
            actionRange[pieceX - 1, pieceZ - 1] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }

        else if (action == 1)
        {
            actionRange[pieceX, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }

        else if (action == 2)
        {
            for (int x = 0; x <= 1; x++)
            {
                for (int z = 0; z <= 1; z++)
                {
                    actionRange[pieceX + x, pieceZ + z] = 2;
                    actionRange[pieceX + x, pieceZ - z] = 2;
                    actionRange[pieceX - x, pieceZ + z] = 2;
                    actionRange[pieceX - x, pieceZ - z] = 2;
                }
            }
            actionRange[pieceX, pieceZ] = 1;
        }

        return actionRange;
    }
}
