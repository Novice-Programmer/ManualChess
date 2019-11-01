using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public int lobbySceneNumber;
    public int tutorialSceneNumber;
    public GameObject mainCanvas;
    public AudioSource bgmSource;
    public AudioClip[] bgmClips;
    private int bgmNum = 0;
    public AudioClip buttonClickClips;

    // Start is called before the first frame update

    void Start()
    {
        AlwaysObject.Instance.BgmSourceSet(bgmSource);
    }

    void Update()
    {
        if (!bgmSource.isPlaying)
        {
            bgmSource.PlayOneShot(bgmClips[bgmNum]);
            bgmNum++;
        }

        if(bgmNum == bgmClips.Length)
        {
            bgmNum = 0;
        }
    }

    public void Btn_LobbyClick()
    {
        AlwaysObject.Instance.SoundOn(buttonClickClips);
        LobbyMove();
    }

    public void Btn_TutorialClick()
    {
        SceneManager.LoadScene(tutorialSceneNumber);
        AlwaysObject.Instance.SoundOn(buttonClickClips);
    }

    private void LobbyMove()
    {
        AlwaysObject.Instance.SoundOn(buttonClickClips);
        SceneManager.LoadScene(lobbySceneNumber);
    }

    public void Btn_OptionClick()
    {
        AlwaysObject.Instance.SoundOn(buttonClickClips);
        AlwaysObject.Instance.SettingOn(mainCanvas);
    }

    public void Btn_ExitClick()
    {
        StartCoroutine(ExitGame());
        Application.Quit();
    }

    private IEnumerator ExitGame()
    {
        AlwaysObject.Instance.SoundOn(buttonClickClips);
        yield return new WaitForSeconds(1.1f);
    }
}
