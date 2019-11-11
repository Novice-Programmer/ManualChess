using UnityEngine;
using System.Collections;

public class MouseCursor : MonoBehaviour
{
    public static MouseCursor Instance { set; get; }
    private Texture2D cursorTexture;
    public Texture2D attackTexture;
    public Texture2D skillTexture;
    public Texture2D healTexture;
    public Texture2D deathTexture;
    public Texture2D moveTexture;
    public Texture2D noneTexture;
    public bool attackCursor;
    public bool skillCursor;
    public bool healCursor;
    public bool deathCursor;
    public bool moveCursor;
    private bool noneCursor;
    private Vector2 hotSpot;
    public void Start()
    {
        Instance = this;
        StartCoroutine("CursorCheck");
    }

    IEnumerator CursorCheck()
    {
        while (!GameManager.Instance.gameState)
        {
            yield return null;
        }

        while (GameManager.Instance.gameState)
        {
            yield return new WaitForSeconds(0.05f);
            if (attackCursor)
            {
                cursorTexture = attackTexture;
            }
            else if (skillCursor)
            {
                cursorTexture = skillTexture;
            }
            else if (healCursor)
            {
                cursorTexture = healTexture;
            }
            else if (deathCursor)
            {
                cursorTexture = deathTexture;
            }
            else if (moveCursor)
            {
                cursorTexture = moveTexture;
            }
            else
            {
                cursorTexture = noneTexture;
                noneCursor = true;
            }

            if (noneCursor || moveCursor)
            {
                hotSpot.x = cursorTexture.width / 3;
                hotSpot.y = cursorTexture.height / 5;
            }
            else
            {
                hotSpot.x = cursorTexture.width / 2;
                hotSpot.y = cursorTexture.height / 2;
            }
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
        }
    }

    public void AttackCursor()
    {
        CursorValueReset(0);
    }
    public void SkillCursor()
    {
        CursorValueReset(1);
    }

    public void DeathCursor()
    {
        CursorValueReset(3);
    }

    public void NoneCursor()
    {
        CursorValueReset(-1);
    }

    public void MoveCursor()
    {
        CursorValueReset(4);
    }

    public void CursorValueReset(int actionNum)
    {
        if (actionNum == 0)
        {
            attackCursor = true;
            skillCursor = false;
            healCursor = false;
            deathCursor = false;
            moveCursor = false;
        }
        else if(actionNum == 1)
        {
            attackCursor = false;
            skillCursor = true;
            healCursor = false;
            deathCursor = false;
            moveCursor = false;
        }
        else if(actionNum == 2)
        {
            attackCursor = false;
            skillCursor = false;
            healCursor = true;
            deathCursor = false;
            moveCursor = false;
        }
        else if(actionNum == 3)
        {
            attackCursor = false;
            skillCursor = false;
            healCursor = false;
            deathCursor = true;
            moveCursor = false;
        }

        else if(actionNum == 4)
        {
            attackCursor = false;
            skillCursor = false;
            healCursor = false;
            deathCursor = false;
            moveCursor = true;
        }

        else
        {
            attackCursor = false;
            skillCursor = false;
            healCursor = false;
            deathCursor = false;
            moveCursor = false;
        }
    }
}