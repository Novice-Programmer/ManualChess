using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : Piece
{
    public override void DataSetting()
    {
        ws = new WaitForSeconds(0.1f);
        pieceTransform = transform;
        piece_Name = "트롤";
        piece_Dir = "잔인무도한 포악의 아인";
        level = 0;
        pieceHP = 37;
        pieceMP = 0;

        maxHP = 37;
        maxMP = 7;

        drowMana = 3;
        fieldMana = 2;

        moveMana = 1;
        spaceMana = 1;
        moveRange = 1;
        spaceRange = 1;

        attackMana = 2;
        attackDamage = 22;
        attackRange = 1;
        targetRange = 1;

        skillMana = 3;
        skillDamage = 27;
        skillRange = 2;
        skillAttackRange = 1;
        skillShakeRange = 30.0f;
        skillShakeForce = 100.0f;

        orderNum = 6;

        piecePosition.y = -0.31f;
        if (GameManager.Instance.player)
        {
            if (tag == "APiece")
            {
                barAddset.x = 0.35f;
                barAddset.y = 1.2f;
            }
            else
            {
                barAddset.y = 1.0f;
            }
        }
        else
        {
            if (tag == "APiece")
            {
                barAddset.y = 1.0f;
            }
            else
            {
                barAddset.x = 0.35f;
                barAddset.y = 1.2f;
            }
        }
    }

    public override bool[,] PossibleMove(Piece[,] pieces, int playerMana)
    {
        bool[,] move = new bool[9, 9];
        if (moveMana <= playerMana || !isPlayer)
        {
            for (int x = 0; x <= 2; x++)
            {
                for (int z = 0; z <= 2; z++)
                {
                    if (CurrentX + x <= 8)
                    {
                        if (CurrentZ + z <= 8)
                        {
                            if (playerMana + MoveManaCheck(new Vector2(CurrentX, CurrentZ), new Vector2(CurrentX + x, CurrentZ + z)) >= 0 || !isPlayer)
                            {
                                move[CurrentX + x, CurrentZ + z] = ValidMove(pieces, CurrentX + x, CurrentZ + z);
                            }
                        }

                        if (CurrentZ - z >= 0)
                        {
                            if (playerMana + MoveManaCheck(new Vector2(CurrentX, CurrentZ), new Vector2(CurrentX + x, CurrentZ - z)) >= 0 || !isPlayer)
                            {
                                move[CurrentX + x, CurrentZ - z] = ValidMove(pieces, CurrentX + x, CurrentZ - z);
                            }
                        }
                    }

                    if (CurrentX - x >= 0)
                    {
                        if (CurrentZ + z <= 8)
                        {
                            if (playerMana + MoveManaCheck(new Vector2(CurrentX, CurrentZ), new Vector2(CurrentX - x, CurrentZ + z)) >= 0 || !isPlayer)
                            {
                                move[CurrentX - x, CurrentZ + z] = ValidMove(pieces, CurrentX - x, CurrentZ + z);
                            }
                        }

                        if (CurrentZ - z >= 0)
                        {
                            if (playerMana + MoveManaCheck(new Vector2(CurrentX, CurrentZ), new Vector2(CurrentX - x, CurrentZ - z)) >= 0 || !isPlayer)
                            {
                                move[CurrentX - x, CurrentZ - z] = ValidMove(pieces, CurrentX - x, CurrentZ - z);
                            }
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
            if (tag == "APiece")
            {
                if (CurrentZ + 1 <= 8)
                {
                    attack[CurrentX, CurrentZ + 1] = ValidAttack(pieces, CurrentX, CurrentZ + 1);
                    if(CurrentX + 1 <= 8)
                    {
                        attack[CurrentX + 1, CurrentZ + 1] = ValidAttack(pieces, CurrentX + 1, CurrentZ + 1);
                    }
                }
            }
            else
            {
                if (CurrentZ - 1 >= 0)
                {
                    attack[CurrentX, CurrentZ - 1] = ValidAttack(pieces, CurrentX, CurrentZ - 1);
                    if (CurrentX - 1 >= 0)
                    {
                        attack[CurrentX - 1, CurrentZ - 1] = ValidAttack(pieces, CurrentX - 1, CurrentZ - 1);
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
            if (tag == "APiece")
            {
                for (int x = 0; x <= 1; x++)
                {
                    for (int z = 0; z <= skillRange; z++)
                    {
                        if (CurrentZ + z <= 8 && CurrentX + x <= 8)
                        {
                            skill[CurrentX + x, CurrentZ + z] = ValidSkill(pieces, CurrentX + x, CurrentZ + z);
                        }
                    }
                }
            }

            else
            {
                for (int x = 0; x <= 1; x++)
                {
                    for (int z = 0; z <= skillRange; z++)
                    {
                        if (CurrentZ - z >= 0 && CurrentX - x >= 0)
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

        for (int x = 0; x <= 1; x++)
        {
            for (int z = 0; z <= 1; z++)
            {
                if (TargetX + x <= 8 && TargetZ + z <= 8)
                {
                    _skillRange[TargetX + x, TargetZ + z] = ValidSkill(pieces, TargetX + x, TargetZ + z);
                }
                if (TargetX - x >= 0 && TargetZ - z >= 0)
                {
                    _skillRange[TargetX - x, TargetZ - z] = ValidSkill(pieces, TargetX - x, TargetZ - z);
                }
            }
        }

        return _skillRange;
    }

    public override int[,] PossibleNoneRange(int action)
    {
        int[,] actionRange = new int[3, 3];
        int pieceX = 1;
        int pieceZ = 1;

        if (action == 0)
        {
            actionRange[pieceX - 1, pieceZ + 1] = 2;
            actionRange[pieceX - 1, pieceZ] = 2;
            actionRange[pieceX + 1, pieceZ] = 2;
            actionRange[pieceX + 1, pieceZ + 1] = 2;
            actionRange[pieceX - 1, pieceZ - 1] = 2;
            actionRange[pieceX + 1, pieceZ - 1] = 2;
            actionRange[pieceX, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ - 1] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }

        else if(action == 1)
        {
            actionRange[pieceX, pieceZ + 1] = 2;
            actionRange[pieceX + 1, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }
        else if(action == 2)
        {
            actionRange[pieceX, pieceZ + 1] = 2;
            actionRange[pieceX + 1, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ + 1] = 2;
            actionRange[pieceX + 1, pieceZ + 1] = 2;
            actionRange[pieceX, pieceZ] = 1;
        }
        return actionRange;
    }
}
