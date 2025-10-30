using NUnit.Framework;
using Steamworks;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Set_Ctrl : MonoBehaviour
{
    [SerializeField] SetObj Set_Obj;

    [Header("Order:Gp, Res, Aud")]
    [SerializeField] List<GameObject> Pages;
    [SerializeField] List<Button> PageBtns;

    [Header("Order:GQ, AA, GM, RES")]
    [SerializeField] List<TMP_Dropdown> Dd_List;

    [Header("Order:Mv, BGMv, SFXv")]
    [SerializeField] List<Slider> Sld_List;
    [SerializeField] List<TMP_Text> AudTxt_List;

    [SerializeField] GameObject ApplyBtn;

    [Header("Prefab")]
    public AudioMixer audioMixer;
    public GameObject Scene_Loading;

    [Header("Not need Pick")]
    public Volume postProcessVolume; // 씬에서 연결 또는 null 가능

    public enum AntiAliasingMode
    { None = 0, FXAA = 1, MSAA2x = 2, MSAA4x = 3, MSAA8x = 4, TAA = 5, }
    AntiAliasingMode postProcessAA = AntiAliasingMode.None;

    private UniversalRenderPipelineAsset urpAsset;

    private const string SETTINGS_FILE = "settings.json";
    SteamSave Steam;

    private void Awake()
    {
        urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;

        GameObject volumeObj = GameObject.FindGameObjectWithTag("Volume");
        if (volumeObj != null)
        { postProcessVolume = volumeObj.GetComponent<Volume>(); }
        else { Debug.LogError("Post-processing Volume 오브젝트가 없습니다."); }

        UI_SetFunc();
    }

    private void Start()
    {
        SetBtn();

        Evt_Wakeup();
        Set_Obj.SetCtrl = this;

        Debug.Log("SteamManager.Initialized: " + SteamManager.Initialized);
        if (!SteamManager.Initialized)
            Debug.LogError("Steam API 초기화 실패!");

        Start_Set();
    }

    public void Evt_Wakeup()
    {
        EventSystem.current.SetSelectedGameObject(PageBtns[0].gameObject);
        func_PageCtrl(0);

        if(ApplyBtn != null)
        { ApplyBtn.SetActive(false); }
    }

    void SetBtn()
    {
        if (PageBtns.Count > 0)
        {
            for (int i = 0; i <= (PageBtns.Count - 1); i++)
            {
                if (PageBtns[i] != null)
                {
                    int index = i;
                    PageBtns[i].onClick.AddListener(() => func_PageCtrl(index));
                }
            }
        }
    }

    void UI_SetFunc()
    {
        // UI에 기능을 얹기

        Sld_List[0].onValueChanged.AddListener((value) => SetMenu_Func("Mv"));
        Sld_List[1].onValueChanged.AddListener((value) => SetMenu_Func("BGMv"));
        Sld_List[2].onValueChanged.AddListener((value) => SetMenu_Func("SFXv"));

        Dd_List[0].onValueChanged.AddListener((value) => SetMenu_Func("None"));
        Dd_List[1].onValueChanged.AddListener((value) => SetMenu_Func("None"));
        Dd_List[2].onValueChanged.AddListener((value) => SetMenu_Func("None"));
        Dd_List[3].onValueChanged.AddListener((value) => SetMenu_Func("None"));

    }

    void SetMenu_Func(string str)
    {
        if (str == "Mv")
        { AudTxt_List[0].text = Sld_List[0].value.ToString("F0") + "%"; }
        else if (str == "BGMv")
        { AudTxt_List[1].text = Sld_List[1].value.ToString("F0") + "%"; }
        else if (str == "SFXv")
        { AudTxt_List[2].text = Sld_List[2].value.ToString("F0") + "%"; }

        if (ApplyBtn != null)
        { ApplyBtn.SetActive(false); }
    }

    public void func_PageCtrl(int i)
    {
        foreach (GameObject page in Pages)
        {
            if (page != null)
            { page.SetActive(false); }
        }

        Pages[i].SetActive(true);
    }

    void Start_Set()
    {
        //----------------------- 설정 로드 -----------------------
        if (!SteamRemoteStorage.FileExists(SETTINGS_FILE))
        {
            Debug.Log("No settings file found in Steam cloud.");
            return;
        }

        int size = SteamRemoteStorage.GetFileSize(SETTINGS_FILE);
        byte[] buffer = new byte[size];
        SteamRemoteStorage.FileRead(SETTINGS_FILE, buffer, size);

        string json = Encoding.UTF8.GetString(buffer);
        SettingsData data = JsonUtility.FromJson<SettingsData>(json);
        //------------------------------------------------------
        // 현재 상태 체크
        int AAlv = data.AA;
        Aa_Set(AAlv);

    }

    // Title에서 켜는 설정메뉴
    public void Set_TitleData()
    {
        // 현재 상태 체크
        int GQlv = QualitySettings.GetQualityLevel();

        AntiAliasingMode currentAA = GetCurrentAA();
        int AAlv = (int)currentAA;

        int GMlv, RESlv;
        float Mv, BGMv, SFXv;

        // 창모드
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        { GMlv = 0; Dd_List[2].value = 0; }
        // 테두리 없는 창 모드
        else if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        { GMlv = 1; Dd_List[2].value = 1; }
        //창 기반 전체화면
        else if (Screen.fullScreenMode == FullScreenMode.MaximizedWindow)
        { GMlv = 2; Dd_List[2].value = 2; }
        // 전체화면 
        else if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
        { GMlv = 3; Dd_List[2].value = 3; }

        if (Screen.width == 3840) { RESlv = 0; }
        else if (Screen.width == 2560) { RESlv = 1; }
        else if (Screen.width == 1920) { RESlv = 2; }
        else if (Screen.width == 1600) { RESlv = 3; }
        else if (Screen.width == 1280) { RESlv = 4; }
        else { RESlv = -1; } // 기본값을 설정 (예: 알 수 없음)

        Mv = InitSlider("MasterVolume");
        BGMv = InitSlider("BGMVolume");
        SFXv = InitSlider("SFXVolume");

        //------------------------------------------------------
        // 각 UI에 현재상태 지정

        Dd_List[0].value = GQlv;
        Dd_List[1].value = AAlv;
        //Dd_List[2].value = GMlv;
        Dd_List[3].value = RESlv;

        Sld_List[0].value = Mv;
        AudTxt_List[0].text = Mv.ToString("F0") + "%";

        Sld_List[1].value = BGMv;
        AudTxt_List[1].text = BGMv.ToString("F0") + "%";

        Sld_List[2].value = SFXv;
        AudTxt_List[2].text = SFXv.ToString("F0") + "%";

    }

    // 메뉴에서 켜는 설정메뉴
    public void Set_SetData()
    {
        //----------------------- 설정 로드 -----------------------
        if (!SteamRemoteStorage.FileExists(SETTINGS_FILE))
        {
            Debug.Log("No settings file found in Steam cloud.");
            return;
        }

        int size = SteamRemoteStorage.GetFileSize(SETTINGS_FILE);
        byte[] buffer = new byte[size];
        SteamRemoteStorage.FileRead(SETTINGS_FILE, buffer, size);

        string json = Encoding.UTF8.GetString(buffer);
        SettingsData data = JsonUtility.FromJson<SettingsData>(json);
        //------------------------------------------------------

        // 현재 상태 체크
        int GQlv = data.GQ;
        int AAlv = data.AA;
        int GMlv = data.GM;
        int RESlv = data.RES;
        float Mv = data.Mv, BGMv = data.BGMv, SFXv = data.SFXv;
        //------------------------------------------------------
        // 각 UI에 현재상태 지정

        Dd_List[0].value = GQlv;
        Dd_List[1].value = AAlv;
        Dd_List[2].value = GMlv;
        Dd_List[3].value = RESlv;

        Sld_List[0].value = Mv;
        AudTxt_List[0].text = Mv.ToString("F0") + "%";

        Sld_List[1].value = BGMv;
        AudTxt_List[1].text = BGMv.ToString("F0") + "%";

        Sld_List[2].value = SFXv;
        AudTxt_List[2].text = SFXv.ToString("F0") + "%";
    }

    AntiAliasingMode GetCurrentAA()
    {
        var urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        int msaaLevel = (urpAsset != null) ? urpAsset.msaaSampleCount : 0;

        switch (msaaLevel)
        {
            case 2: return AntiAliasingMode.MSAA2x;
            case 4: return AntiAliasingMode.MSAA4x;
            case 8: return AntiAliasingMode.MSAA8x;
            default:
                // MSAA 없으면 포스트 프로세싱 쪽 상태 리턴
                if (postProcessAA == AntiAliasingMode.FXAA || postProcessAA == AntiAliasingMode.TAA)
                    return postProcessAA;
                else
                    return AntiAliasingMode.None;
        }
    }

    // 설정 저장
    public void Func_Apply()
    {
        if (Steam == null)
        { Steam = GameObject.FindGameObjectWithTag("Steam").GetComponent<SteamSave>(); }

        List<float> floats = new List<float>();

        floats.Add(Sld_List[0].value);
        floats.Add(Sld_List[1].value);
        floats.Add(Sld_List[2].value);

        List<int> ints = new List<int>();

        ints.Add(Dd_List[0].value);
        ints.Add(Dd_List[1].value);
        ints.Add(Dd_List[2].value);
        ints.Add(Dd_List[3].value);

        Steam.SaveSettings(ints, floats);
        Apply();
    }

    //실제 적용
    void Apply()
    {
        // 그래픽 품질
        QualitySettings.SetQualityLevel(Dd_List[0].value);

        // 안티 얼라이징
        Aa_Set(Dd_List[1].value);

        // 창모드
        if (Dd_List[2].value == 0)
        { Screen.fullScreenMode = FullScreenMode.Windowed; }
        // 테두리 없는 창 모드
        else if (Dd_List[2].value == 1)
        { Screen.fullScreenMode = FullScreenMode.FullScreenWindow; }
        //창 기반 전체화면
        else if (Dd_List[2].value == 2)
        { Screen.fullScreenMode = FullScreenMode.MaximizedWindow; }
        // 전체화면 
        else if (Dd_List[2].value == 3)
        { Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen; }

        // 해상도 조절
        bool fullscreen = (Dd_List[2].value == 3);

        if (Dd_List[3].value == 0) { Screen.SetResolution(3840, 2160, fullscreen); }
        else if (Dd_List[3].value == 1) { Screen.SetResolution(2560, 1440, fullscreen); }
        else if (Dd_List[3].value == 2) { Screen.SetResolution(1920, 1080, fullscreen); }
        else if (Dd_List[3].value == 3) { Screen.SetResolution(1600, 900, fullscreen); }
        else if (Dd_List[3].value == 4) { Screen.SetResolution(1280, 720, fullscreen); }

        // 사운드 조절
        SetVolume("MasterVolume", Sld_List[0].value);
        SetVolume("BGMVolume", Sld_List[1].value);
        SetVolume("SFXVolume", Sld_List[2].value);
    }

    void Aa_Set(int i)
    {
        // 안티 얼라이징
        var urpCameraData = Camera.main.GetUniversalAdditionalCameraData();


        if (i == 0)
        {
            // None
            urpAsset.msaaSampleCount = 0;
            urpCameraData.antialiasing = AntialiasingMode.None;
        }
        else if (i == 1)
        {
            // FXAA
            urpAsset.msaaSampleCount = 0;
            urpCameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
        }
        else if (i == 2)
        {
            // MSAA 2x
            urpAsset.msaaSampleCount = 2;
            urpCameraData.antialiasing = AntialiasingMode.None;
        }
        else if (i == 3)
        {
            // MSAA 4x
            urpAsset.msaaSampleCount = 4;
            urpCameraData.antialiasing = AntialiasingMode.None;
        }
        else if (i == 4)
        {
            // MSAA 8x
            urpAsset.msaaSampleCount = 8;
            urpCameraData.antialiasing = AntialiasingMode.None;
        }
        else if (i == 5)
        {
            // TAA
            urpAsset.msaaSampleCount = 0;
            urpCameraData.antialiasing = AntialiasingMode.TemporalAntiAliasing;
        }
    }


    private float InitSlider(string parameterName)
    {
        if (audioMixer.GetFloat(parameterName, out float dB))
        {
            // dB → 0~100% 변환
            return Mathf.Pow(10f, dB / 20f) * 100f;
        }
        else
        {
            return 100f; // 값이 없으면 기본 100%
        }
    }

    /// <summary>
    /// 슬라이더 % 값을 AudioMixer dB 값으로 변환해 적용
    /// </summary>
    public void SetVolume(string parameterName, float percent)
    {
        if (percent <= 0.01f)
            audioMixer.SetFloat(parameterName, -80f); // 무음
        else
            audioMixer.SetFloat(parameterName, Mathf.Log10(percent / 100f) * 20f);
    }
}
