using UnityEngine;
using System.Collections;

public class MouseCursor : MonoBehaviour
{
    public static MouseCursor mouseCursor { set; get; }
    private Texture2D cursorTexture;
    public Texture2D attackTexture;
    public Texture2D skillTexture;
    public Texture2D healTexture;
    public Texture2D deathTexture;
    public Texture2D noneTexture;
    public bool attackCursor;
    public bool skillCursor;
    public bool healCursor;
    public bool deathCursor;
    private bool noneCursor;
    private Vector2 hotSpot;
    public void Start()
    {
        mouseCursor = this;
        StartCoroutine("CursorCheck");
    }

    public void Update()
    {
        StartCoroutine("CursorCheck");
    }

    IEnumerator CursorCheck()
    {
        yield return new WaitForEndOfFrame();
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
        else
        {
            cursorTexture = noneTexture;
            noneCursor = true;
        }

        if (noneCursor)
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

    public void AttackCursor()
    {
        CursorValueReset();
        attackCursor = true;
    }
    public void SkillCursor()
    {
        CursorValueReset();
        skillCursor = true;
    }

    public void DeathCursor()
    {
        CursorValueReset();
        deathCursor = true;
    }

    public void NoneCursor()
    {
        CursorValueReset();
    }

    public void CursorValueReset()
    {
        attackCursor = false;
        skillCursor = false;
        healCursor = false;
        deathCursor = false;
    }
}