using NUnit.Framework;
using Steamworks;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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

    [Header("Prefab")]
    public AudioMixer audioMixer;
    public GameObject Scene_Loading;

    public enum AntiAliasingMode
    { None = 0, FXAA = 1, MSAA2x = 2, MSAA4x = 3, MSAA8x = 4, TAA = 5, }
    AntiAliasingMode postProcessAA = AntiAliasingMode.None;

    private UniversalRenderPipelineAsset urpAsset;

    private const string SETTINGS_FILE = "settings.json";

    private void Start()
    {
        SetBtn();

        Evt_Wakeup();
        Set_Obj.SetCtrl = this;
    }

    public void Evt_Wakeup()
    {
        EventSystem.current.SetSelectedGameObject(PageBtns[0].gameObject);
        func_PageCtrl(0);
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

    public void func_PageCtrl(int i)
    {
        foreach (GameObject page in Pages)
        {
            if (page != null)
            { page.SetActive(false); }
        }

        Pages[i].SetActive(true);
    }

    public void Set_IntroData()
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
}
