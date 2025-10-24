using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DefaultExecutionOrder(-10)] // 다른 스크립트보다 먼저 실행되게
public class PopUp : MonoBehaviour
{
    public PopUp_Obj popup;
    public GameObject Popup_Obj;

    public TMP_Text txt_Tit;
    public TMP_Text txt_Info;

    public GameObject Back_Obj;

    [SerializeField] private Navi_Finder Navifinder;

    [Header("0.Yes, 1.No Btn")]
    public List<Button> popupBtns;

    private List<Button> mainBtns = new List<Button>();
    private List<ScrollRect> mainScrolls = new List<ScrollRect>();

    private Dictionary<Button, Navigation> originalNav = new Dictionary<Button, Navigation>();

    private GameObject lastSel_obj;
    private void Awake()
    {
        popup = GetComponent<PopUp_Obj>();
    }

    void Start()
    {
        if (Popup_Obj.activeSelf == true) { Popup_Obj.SetActive(false); }
        if(Back_Obj.activeSelf == true) { Back_Obj.SetActive(false); }

        if (Navifinder != null)
        {
            mainBtns = Navifinder.GetButtons();
            mainScrolls = Navifinder.GetScrollRects();
        }
    }

    public void PopUP_On(string tit, List<string> Info, List<string> Btntxts, bool Back)
    {
        Popup_Obj.SetActive(true);

        NaviFunc_Popup_On();

        //Title 내용
        txt_Tit.text = tit;
        txt_Info.text = "";
        
        //팝업 내용
        for(int i = 0; i <= (Info.Count-1); i++)
        {
            if (i < Info.Count)
            { txt_Info.text += Info[i] + System.Environment.NewLine; }
            else { txt_Info.text += Info[i]; }
        }

        // 버튼 txt
        if (Btntxts.Count > 0)
        {
            for (int i = 0; i <= (Btntxts.Count - 1); i++)
            { popupBtns[i].transform.GetChild(0).GetComponent<TMP_Text>().text = Btntxts[i]; }
        }

        // Back image를 true로 할지 false로 할지
        if(Back == true) { if (Back_Obj != null) { Back_Obj.SetActive(true); } }
        else { if (Back_Obj != null) { Back_Obj.SetActive(false); } }

        // 버튼에 팝업끄기 기능 추가
        popupBtns[0].onClick.AddListener(PopUP_Off);
        popupBtns[1].onClick.AddListener(PopUP_Off);

        EventSystem.current.SetSelectedGameObject(popupBtns[0].gameObject);
    }

    public void PopUP_Off()
    {
        NaviFunc_Popup_Off();

        for (int i = 0; i <= (popupBtns.Count-1); i++)
        { 
            if (popupBtns[i] != null) 
            { popupBtns[i].onClick.RemoveAllListeners(); }
        }

        Popup_Obj.SetActive(false);

        if (Back_Obj.activeSelf == true) 
        { Back_Obj.SetActive(false); }
    }


    // 팝업 열릴 때
    void NaviFunc_Popup_On()
    {
        // 팝업 열기 전에 현재 선택된 버튼 저장
        lastSel_obj = EventSystem.current.currentSelectedGameObject;

        SaveOriginalNavigation();

        // 스크롤 비활성화
        foreach (var scroll in mainScrolls)
            scroll.enabled = false;

        // 메인 버튼 비활성화
        foreach (var btn in mainBtns)
        {
            btn.interactable = false;
            var nav = btn.navigation;
            nav.mode = Navigation.Mode.None;
            btn.navigation = nav;
        }

        // 팝업 버튼 활성화
        foreach (var btn in popupBtns)
        {
            btn.interactable = true;
            var nav = btn.navigation;
            nav.mode = Navigation.Mode.Automatic;
            btn.navigation = nav;
        }

        if (popupBtns.Count > 0)
            EventSystem.current.SetSelectedGameObject(popupBtns[0].gameObject);
    }

    // 팝업 닫힐 때
    void NaviFunc_Popup_Off()
    {
        RestoreOriginalNavigation();

        // 스크롤 복원
        foreach (var scroll in mainScrolls)
            scroll.enabled = true;

        if (mainBtns.Count > 0)
            EventSystem.current.SetSelectedGameObject(mainBtns[0].gameObject);

        // 마지막 선택된 버튼으로 복원
        if (lastSel_obj != null)
            EventSystem.current.SetSelectedGameObject(lastSel_obj);
        else if (mainBtns.Count > 0)
            EventSystem.current.SetSelectedGameObject(mainBtns[0].gameObject);
    }

    private void SaveOriginalNavigation()
    {
        foreach (var btn in mainBtns)
        {
            if (!originalNav.ContainsKey(btn))
                originalNav[btn] = btn.navigation;
        }
    }

    private void RestoreOriginalNavigation()
    {
        foreach (var btn in mainBtns)
        {
            if (originalNav.ContainsKey(btn))
            {
                btn.navigation = originalNav[btn];
                btn.interactable = true;
            }
        }

        foreach (var btn in popupBtns)
        {
            btn.interactable = false;
            var nav = btn.navigation;
            nav.mode = Navigation.Mode.None;
            btn.navigation = nav;
        }
    }
}