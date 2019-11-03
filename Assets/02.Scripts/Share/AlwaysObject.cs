using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlwaysObject : MonoBehaviour
{
    public static AlwaysObject Instance { set; get; }

    [Header("Screen")]
    public int gameWidth = 1280; // 화면 가로 사이즈
    public int gameHeight = 720; // 화면 세로 사이즈
    public bool gameFullScreen = false; // 풀 화면
    public Dropdown dropdown_Screen; // 화면 크기 목록
    public Toggle toggle_Screen; // 풀 화면 토글 버튼
    private List<string> screenOptionString; // 화면 크기 목록 값

    [Header("Sound")]
    private AudioSource audioSource; // 효과음
    public AudioSource bgmSource; // 배경음
    [Range(0.0f, 1.0f)]
    public float soundVolume = 0.5f; // 사운드 값

    [Header("Loading")]
    public GameObject obj_loading; // 로딩 패널
    private Text loadingText; // 로딩중일때 나오는 텍스트
    private bool loading; // 로딩중
    private string loadingString = "로딩중 "; // 텍스트

    [Header("Info")]
    public Animator infoAnimator; // 애니메이션
    public GameObject obj_info; // 패널
    private CanvasGroup cg_info; // 캔버스 그룹
    private Text txt_info; // 내용
    public float fadeDuration; // 지속 시간

    [Header("SoundVolume")]
    public Slider sld_sound; // 사운드 슬라이더
    private float sliderValue = 0.5f; // 사운드값
    public Text txt_soundValue; // 사운드 벨류 값
    public Toggle toggle_bgm; // 배경음 음소거
    private bool bgmB;
    public Toggle toggle_sound; // 효과음 음소거
    private bool soundB;

    public GameObject obj_setting;
    public GameObject obj_panel;
    public GameObject obj_exitBtn;
    public Text txt_before;

    public AudioClip buttonClickClip;

    private GameObject obj_beforeCanvas;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        ScreenSetting(gameWidth, gameHeight, gameFullScreen);
        ScreenOption();
        SetDropdownOptions(screenOptionString);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = soundVolume;
        loadingText = obj_loading.transform.GetChild(0).GetComponent<Text>();
        cg_info = obj_info.GetComponent<CanvasGroup>();
        txt_info = obj_info.transform.GetChild(1).GetComponent<Text>();
    }

    #region 화면

    private void ScreenOption()
    {
        screenOptionString = new List<string>
        {
            "1280x720",
            "1360x768",
            "1600x900",
            "1920x1080"
        };
    }

    private void SetDropdownOptions(List<string> _optionString)
    {
        dropdown_Screen.options.Clear();
        for (int i = 0; i < _optionString.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = _optionString[i];
            dropdown_Screen.options.Add(option);
            dropdown_Screen.captionText.text = gameWidth + "x" + gameHeight;
        }
        toggle_Screen.isOn = gameFullScreen;
    }

    public void ScreenSetting(int _width, int _height, bool _full)
    {
        gameWidth = _width;
        gameHeight = _height;
        gameFullScreen = _full;
        Screen.SetResolution(gameWidth, gameHeight, gameFullScreen);
    }

    public void ScreenOptionSet()
    {
        string value = dropdown_Screen.captionText.text;
        string[] spValue = value.Split('x');
        int width = int.Parse(spValue[0]);
        int height = int.Parse(spValue[1]);
        bool full = gameFullScreen;
        ScreenSetting(width, height, full);
    }

    public void ScreenFullCheck()
    {
        gameFullScreen = !gameFullScreen;
        Screen.SetResolution(gameWidth, gameHeight, gameFullScreen);
    }

    #endregion

    #region 사운드

    public void BgmSourceSet(AudioSource _bgmSource)
    {
        bgmSource = _bgmSource;
        bgmSource.volume = soundVolume;
        bgmSource.mute = bgmB;
    }

    public void SoundVolumeSet(float _soundValue)
    {
        soundVolume = _soundValue;
        audioSource.volume = soundVolume;
        bgmSource.volume = soundVolume;
    }

    public void ETCSoundSet(AudioSource _audioSource)
    {
        _audioSource.volume = soundVolume;
        _audioSource.mute = soundB;
    }

    public void SoundOn(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }

    #endregion

    #region 캔버스

    public void SettingOn(GameObject _beforeCanvas)
    {
        obj_beforeCanvas = _beforeCanvas;
        sld_sound.value = soundVolume;
        if (obj_beforeCanvas != null)
        {
            obj_beforeCanvas.SetActive(false);
            txt_before.text = "이전으로";
            obj_panel.SetActive(false);
            obj_exitBtn.SetActive(false);
        }
        else
        {
            txt_before.text = "메뉴닫기";
            obj_panel.SetActive(true);
            obj_exitBtn.SetActive(true);
        }
        obj_setting.gameObject.SetActive(true);
    }

    public void Btn_OptionBefore()
    {
        SoundOn(buttonClickClip);
        obj_setting.gameObject.SetActive(false);
        if (obj_beforeCanvas != null)
        {
            obj_beforeCanvas.SetActive(true);
        }
        else
        {
            GameManager.Instance.option = false;
        }
    }

    public void Btn_BgmMuteCheck()
    {
        SoundOn(buttonClickClip);
        bgmB = !bgmB;
        bgmSource.mute = bgmB;
    }

    public void Btn_SoundMuteCheck()
    {
        soundB = !soundB;
        audioSource.mute = soundB;
    }

    public void SliderValueChange()
    {
        sliderValue = sld_sound.value;
        int _sliderValue = (int)(sliderValue * 100.0f);
        txt_soundValue.text = _sliderValue + "%";
        SoundVolumeSet(sliderValue);
    }

    public void Btn_ExitGame()
    {
        SoundOn(buttonClickClip);
        obj_setting.gameObject.SetActive(false);
        NetworkManager.Instance.OnLeftRoom();
    }

    #endregion

    #region 로딩

    public void LoadingStart()
    {
        loading = true;
        StartCoroutine(Loading());
    }

    private IEnumerator Loading()
    {
        while (loading)
        {
            for (int i = 0; i < 1; i++)
            {
                loadingString += ".";
                loadingText.text = loadingString;
                yield return new WaitForSeconds(0.3f);
            }
        }
        loadingText.text = "";
        loadingString = "로딩중 ";
    }

    public void LoadingEnd()
    {
        loading = false;
    }

    #endregion

    #region 정보

    public void InfoStart(string _info)
    {
        txt_info.text = _info;
        StartCoroutine(InfoView());
    }

    private IEnumerator InfoView()
    {
        cg_info.alpha = 1.0f;
        infoAnimator.SetBool("InfoMove", true);
        while (!infoAnimator.GetCurrentAnimatorStateInfo(0).IsName("InfoUpdown"))
        {
            yield return null;
        }

        while (infoAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.75f)
        {
            yield return null;
        }
        txt_info.text = "";
        infoAnimator.SetBool("InfoMove", false);
    }

    #endregion
}
