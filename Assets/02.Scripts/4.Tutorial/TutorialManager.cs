using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("Player")]
    public int playerMana = 3;
    public int playerUseMana = 0;
    public int playerAbleMana = 2;

    [Header("Panel")]
    public CanvasGroup turnPanel;
    public CanvasGroup manaPanel;
    public CanvasGroup drowPanel2;
    public CanvasGroup drowPanel1;
    public CanvasGroup drowPanel;
    public CanvasGroup gravePanel;
    public CanvasGroup elsePanel;
    public GameObject winPanel;

    [Header("UI")]
    public Button btn_Drow;
    public Button btn_TutorialEnd;
    public Image[] manaOn;
    public Image[] manaAble;
    public Text turnView;
    public GameObject pieceSet;
    public GameObject skill;
    public GameObject move;
    public Image enemyHP;

    [Header("GameObject")]
    public GameObject drowCards;
    public GameObject handCard;
    public GameObject robot;
    public GameObject turnCylinder;
    public CanvasGroup turnViewCanvas;
    private Quaternion aTurnQua = Quaternion.Euler(97.0f, 0.0f, 0.0f);
    private Quaternion bTurnQua = Quaternion.Euler(83.0f, 0.0f, 0.0f);
    public GameObject enemyKing;
    public GameObject attackRange;

    [Header("Text")]
    public Text tutorialText;
    public string[] infoStrings;
    public string[] warningStrings;
    private List<string> textStrings;
    private List<string> textWarningStrings;

    [Header("TutorialValue")]
    public bool playB;
    public bool handB;
    public bool turnB;
    public bool endB;
    public int playNum;
    public int warningNum;
    public float textInputSpeed;
    public float textRemoveSpeed;

    [Header("Manager")]
    public DeployRange deployRange;
    public Animator handMove;
    public Animator playerKingAnim;
    public Animator enemyKingAnim;

    [Header("ETC")]
    public int mainSceneNumber;
    public AudioSource bgmSource;

    void Start()
    {
        AlwaysObject.Instance.BgmSourceSet(bgmSource);
        btn_Drow.enabled = false;
        textStrings = new List<string>();
        textWarningStrings = new List<string>();
        warningNum = -1;
        for(int i = 0; i < infoStrings.Length; i++)
        {
            textStrings.Add(infoStrings[i]);
        }
        for(int i = 0; i < warningStrings.Length; i++)
        {
            textWarningStrings.Add(warningStrings[i]);
        }
        deployRange.DeployRangeSet(true);
        tutorialText.text = "";
        StartCoroutine("StartPrint");
    }

    void Update()
    {
        if (playB)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StopCoroutine("TutorialPrint");
                tutorialText.text = "";
                playNum++;
                Panel_Check();
                if (playNum >= infoStrings.Length)
                {
                    playB = false;
                    EndText();
                }
                StartCoroutine("TutorialPrint");
            }
        }
        if (playNum == 19 && handB)
        {
            handB = false;
            StartCoroutine(HandFieldAdd());
        }

        if (turnB)
        {
            turnB = false;
            StartCoroutine(EndTurn());
        }
    }

    #region 텍스트쓰기

    IEnumerator StartPrint()
    {
        for (int i = 0; i < textStrings[playNum].Length; i++)
        {
            tutorialText.text += textStrings[playNum][i];
            yield return new WaitForSeconds(textInputSpeed);
        }
        yield return new WaitForSeconds(1.0f);
        playB = true;
        playNum++;
        StartCoroutine("TutorialPrint");
    }

    IEnumerator TutorialPrint()
    {
        if (playNum >= infoStrings.Length)
        {
            yield break;
        }

        string subString = tutorialText.text;
        for (int i = 0; i < subString.Length; i++)
        {
            int subNum = tutorialText.text.Length;
            tutorialText.text = tutorialText.text.Substring(0, subNum - 1);
            yield return new WaitForSeconds(textRemoveSpeed);
        }
        yield return new WaitForSeconds(0.5f);

        for (int i=0; i < textStrings[playNum].Length; i++)
        {
            tutorialText.text += textStrings[playNum][i];
            yield return new WaitForSeconds(textInputSpeed);
        }
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator WarningPrint()
    {
        if (warningNum >= 0)
        {
            tutorialText.text = "";
            yield return new WaitForSeconds(0.3f);
            tutorialText.text = "<color=#ff0000>!!" + warningStrings[warningNum] + "</color>";
            warningNum = -1;
        }
    }

    void EndText()
    {
        tutorialText.text = "튜토리얼이 끝났습니다.";
        btn_TutorialEnd.gameObject.SetActive(true);
    }

    #endregion

    void Panel_Check()
    {
        if (playNum == 2)
        {
            turnPanel.alpha = 0.0f;
        }
        if (playNum == 4)
        {
            Panel_On();
            manaPanel.alpha = 0.0f;
        }
        if(playNum == 9)
        {
            Panel_On();
            drowPanel2.alpha = 0.0f;
        }
        if(playNum == 11)
        {
            Panel_On();
        }
        if(playNum == 14)
        {
            Panel_On();
            playB = false;
            btn_Drow.enabled = true;
            manaPanel.alpha = 0.0f;
            drowPanel1.alpha = 0.0f;
        }
        if(playNum == 15)
        {
            drowCards.SetActive(true);
            drowPanel.alpha = 0.0f;
        }

        if(playNum == 16)
        {
            manaPanel.alpha = 0.0f;
        }

        if(playNum == 19)
        {
            playB = false;
            handB = true;
            manaPanel.alpha = 0.0f;
        }
        
        if(playNum == 20)
        {
            playB = true;
        }

        if(playNum == 21)
        {
            Panel_On();
            robot.SetActive(false);
            gravePanel.alpha = 0.0f;
        }

        if(playNum == 22)
        {
            Panel_On();
            manaPanel.alpha = 0.0f;
            turnPanel.alpha = 0.0f;
        }

        if(playNum == 23)
        {
            Panel_On();
            playB = false;
            turnB = true;
            turnPanel.alpha = 0.0f;
        }

        if(playNum == 24)
        {
            playB = false;
            StartCoroutine(PlayerTurn());
        }

        if(playNum == 26)
        {
            Panel_On();
            manaPanel.alpha = 0.0f;
        }

        if(playNum == 27)
        {
            Panel_Off();
            enemyKing.SetActive(true);
        }

        if(playNum == 29)
        {
            playB = false;
            StartCoroutine(AttackClick());
        }

        if(playNum == 33)
        {
            winPanel.SetActive(false);
        }

        if(playNum == textStrings.Count)
        {
            Panel_On();
        }
    }

    void Panel_On()
    {
        turnPanel.alpha = 1.0f;
        manaPanel.alpha = 1.0f;
        drowPanel.alpha = 1.0f;
        drowPanel1.alpha = 1.0f;
        drowPanel2.alpha = 1.0f;
        gravePanel.alpha = 1.0f;
        elsePanel.alpha = 1.0f;
    }

    void Panel_Off()
    {
        turnPanel.alpha = 0.0f;
        manaPanel.alpha = 0.0f;
        drowPanel.alpha = 0.0f;
        drowPanel1.alpha = 0.0f;
        drowPanel2.alpha = 0.0f;
        gravePanel.alpha = 0.0f;
        elsePanel.alpha = 0.0f;
    }

    void ManaSet()
    {
        for (int i = 0; i < 3; i++)
        {
            manaOn[i].GetComponent<Image>().fillAmount = 0.0f;
            manaAble[i].GetComponent<Image>().fillAmount = 0.0f;
        }

        for (int i = 0; i < playerMana + playerUseMana; i++)
        {
            if (i >= playerUseMana)
            {
                manaOn[i].GetComponent<Image>().fillAmount = 1.0f;
            }
        }

        int ableNum = playerMana + playerUseMana - playerAbleMana;

        if (ableNum > 0)
        {
            for (int i = playerAbleMana; i < playerMana + playerUseMana; i++)
            {
                manaAble[i].GetComponent<Image>().fillAmount = 1.0f;
            }
        }
    }

    IEnumerator HandFieldAdd()
    {
        handMove.SetTrigger("Move");
        StopCoroutine("TutorialPrint");
        tutorialText.text = textStrings[playNum];
        while (!handMove.GetCurrentAnimatorStateInfo(0).IsName("HandMove"))
        {
            deployRange.DeployRangeView();
            yield return null;
        }

        while (handMove.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
        {
            //애니메이션 중일 때 실행되는 부분
            yield return null;
        }
        handCard.SetActive(false);
        deployRange.DeployRangeNoneView();
        playNum++;
        Panel_Check();
        StartCoroutine("TutorialPrint");
        playerMana--;
        playerUseMana++;
        ManaSet();
        robot.SetActive(true);
    }

    IEnumerator EndTurn()
    {
        yield return new WaitForSeconds((textStrings[playNum-1].Length * textRemoveSpeed )+(textStrings[playNum].Length * textInputSpeed) - 1.0f);
        turnCylinder.transform.rotation = bTurnQua;
        turnView.text = "상대턴";
        StartCoroutine(TurnChange());
        playB = true;
    }

    IEnumerator PlayerTurn()
    {
        yield return new WaitForSeconds((textStrings[playNum - 1].Length * textRemoveSpeed) + (textStrings[playNum].Length * textInputSpeed) + 1.5f);
        playNum++;
        playerMana++;
        playerUseMana = 0;
        ManaSet();
        StartCoroutine("TutorialPrint");
        turnCylinder.transform.rotation = aTurnQua;
        turnView.text = "플레이어턴";
        StartCoroutine(TurnChange());
        playB = true;
    }

    IEnumerator TurnChange()
    {
        turnViewCanvas.alpha = 1.0f;
        turnViewCanvas.blocksRaycasts = true;
        float fadeSpeed = turnViewCanvas.alpha / 1.5f;
        while (!Mathf.Approximately(turnViewCanvas.alpha, 0.0f))
        {
            turnViewCanvas.alpha = Mathf.MoveTowards(turnViewCanvas.alpha, 0.0f, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator AttackClick()
    {
        yield return new WaitForSeconds(textStrings[playNum].Length * textInputSpeed + 1.5f);
        pieceSet.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        skill.SetActive(false);
        move.SetActive(false);
        attackRange.SetActive(true);
        playNum++;
        StartCoroutine("TutorialPrint");
        yield return new WaitForSeconds((textStrings[playNum - 1].Length * textRemoveSpeed) + (textStrings[playNum].Length * textInputSpeed) + 1.0f);
        pieceSet.SetActive(false);
        attackRange.SetActive(false);
        playerKingAnim.gameObject.transform.LookAt(enemyKing.transform);
        playerKingAnim.SetBool("Attack",true);
        while (!playerKingAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            yield return null;
        }

        while (playerKingAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
        {
            playerKingAnim.SetBool("Attack", false);
            yield return null;
        }

        StopCoroutine("TutorialPrint");
        playNum++;
        StartCoroutine("TutorialPrint");
        yield return new WaitForSeconds((textStrings[playNum - 1].Length * textRemoveSpeed) + (textStrings[playNum].Length * textInputSpeed) + 1.0f);
        enemyHP.gameObject.SetActive(true);
        enemyHP.fillAmount = 1.0f;
        float fadeSpeed = enemyHP.fillAmount / 2.0f;
        while (!Mathf.Approximately(enemyHP.fillAmount, 0.0f))
        {
            enemyHP.fillAmount = Mathf.MoveTowards(enemyHP.fillAmount, 0.0f, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        enemyKingAnim.SetBool("Die", true);
        while (!enemyKingAnim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            yield return null;
        }

        while (enemyKingAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
        {
            yield return null;
        }
        enemyKing.SetActive(false);
        StopCoroutine("TutorialPrint");
        playNum++;
        StartCoroutine("TutorialPrint");
        yield return new WaitForSeconds((textStrings[playNum - 1].Length * textRemoveSpeed) + (textStrings[playNum].Length * textInputSpeed) + 1.0f);
        playB = true;
        winPanel.SetActive(true);
    }

    void CardClick(int cardNum)
    {
        StopCoroutine("TutorialPrint");
        if (cardNum == 0)
        {
            drowCards.SetActive(false);
            Panel_On();
            drowPanel2.alpha = 0.0f;
            playB = true;
            playNum++;
            tutorialText.text = "";
            handCard.SetActive(true);
            playerUseMana++;
            playerMana--;
            ManaSet();
            StartCoroutine("TutorialPrint");
        }
        else
        {
            if (warningNum < 0)
            {
                warningNum = 0;
                StartCoroutine("WarningPrint");
            }
        }
    }

    void Btn_DrowClick()
    {
        StopCoroutine("TutorialPrint");
        btn_Drow.gameObject.SetActive(false);
        playNum++;
        Panel_Check();
        StartCoroutine("TutorialPrint");
    }

    void Btn_EndTutorialClick()
    {
        EndTutorial();
    }

    void EndTutorial()
    {
        SceneManager.LoadScene(mainSceneNumber);
    }
}
