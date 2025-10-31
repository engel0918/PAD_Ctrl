using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tit_Ctrl : MonoBehaviour
{

    public int Order;

    [SerializeField] GameObject ReturnPage;
    public List<Button> Tit_Btns;
    public List<PopUp_Load> PopUp_Evts;

    public List<Button> Btn_Returns;

    [Header("Hover Evt: txt, Img")]
    [SerializeField] GameObject Hover_Obj;

    [SerializeField] Image Thum;
    [SerializeField] TMP_Text Txt_Tit;
    [SerializeField] TMP_Text Txt_info;

    [Header("Hover Evt: string, sprite")]
    public List<Hov_info> Hov;

    bool SizeCheck;
    PageCtrl page;

    SteamSave SteamMgr;

    private void Start()
    {
        Order = -1;

        gameObject.AddComponent<SteamManager>();
        SteamMgr = gameObject.AddComponent<SteamSave>();
        transform.tag = "Steam";

        Hover_Obj.SetActive(false);

        SetBtn();
        SetReturn();
    }

    private void OnEnable()
    {
        if(Hover_Obj.activeSelf == true)
        { Hover_Obj.SetActive(false); }
    }

    public void SetReturn()
    {
        if (page == null) { page = GetComponent<PageCtrl>(); }

        page.All_Check(false);
        ReturnPage.SetActive(true);

        if (Order < 0)
        { ReturnPage.GetComponent<Set_Focus>().enabled = true; }
        else
        {
            ReturnPage.GetComponent<Set_Focus>().enabled = false;

            if (Device_Check.device == "PAD")
            { EventSystem.current.SetSelectedGameObject(Tit_Btns[Order].gameObject); }
        }
    }

    void False_ReturnPage()
    { ReturnPage.SetActive(false); }

    void SetOrder(int i)
    {
        Order = i;
        //Debug.Log("Ord: " + Order);
    }

    void SetBtn()
    {
        if (page == null) { page = GetComponent<PageCtrl>(); }

        for (int i = 0; i < page.PageBtns.Count; i++)
        { page.PageBtns[i].onClick.AddListener(False_ReturnPage); }

        for (int i = 0; i < Tit_Btns.Count; i++)
        {
            int index = i; // ⚠️ 반드시 지역 변수로 복사해야 각 버튼이 자기 인덱스를 기억합니다.

            if (Tit_Btns[index] != null)
            { Tit_Btns[index].onClick.AddListener(() => SetOrder(index)); }

            // 버튼에 EventTrigger가 없다면 자동으로 추가
            EventTrigger trigger = Tit_Btns[i].gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = Tit_Btns[i].gameObject.AddComponent<EventTrigger>();

            // --- 기존 이벤트 중복 방지 ---
            if (trigger.triggers == null)
                trigger.triggers = new List<EventTrigger.Entry>();
            else
                trigger.triggers.Clear();

            // --- 패드용 버튼에 이벤트 조정 ---
            Tit_Btns[i].AddComponent<Tit_HovEvt>().Tit = this;

            // --- Pointer Enter Event ---
            AddEventTrigger(Tit_Btns[i].gameObject, EventTriggerType.PointerEnter, () => Hov_Evt(index));

            // --- Pointer Exit Event ---
            AddEventTrigger(Tit_Btns[i].gameObject, EventTriggerType.PointerExit, HoverOut_Evt);

            // --- PopUp Event ---
            if (PopUp_Evts[i] != null)
            { Tit_Btns[i].onClick.AddListener(PopUp_Evts[i].PopUP_On); }
        }

        if (Btn_Returns.Count > 0)
        {
            for (int i = 0; i <= (Btn_Returns.Count - 1); i++)
            {
                if (Btn_Returns[i] != null)
                {
                    int index = i;
                    Btn_Returns[i].onClick.AddListener(SetReturn);
                    Btn_Returns[i].gameObject.AddComponent<Tit_ReturnBtn>().Btn = Btn_Returns[i];
                }
            }
        }

    }

    void AddEventTrigger(GameObject obj, EventTriggerType type, System.Action action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = obj.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener((eventData) => { action(); });
        trigger.triggers.Add(entry);
    }

    public void Hov_Evt(int value)
    {
        // 방어 코드 추가 (예외 방지)
        if (value < 0 || value >= Hov.Count)
        {
            Debug.LogWarning("잘못된 인덱스 접근: {value}");
            return;
        }

        Thum.sprite = Hov[value].Hov_Spr;
        Txt_Tit.text = Hov[value].Hov_Tit;
        Txt_info.text = string.Join("\n", Hov[value].Hov_Txts);

        if (!SizeCheck) // 실행 중이 아닐 때만 실행
            StartCoroutine(Rect_sIzing());

        Hover_Obj.SetActive(true);
    }

    public void HoverOut_Evt()
    {
        //Hover_Obj.SetActive(false);
    }

    void SizeCtrl()
    {
        RectTransform rect = Hover_Obj.GetComponent<RectTransform>();
        RectTransform Txt_rect = Txt_info.GetComponent<RectTransform>();

        Vector2 Std = rect.sizeDelta;
        float space = 110f;

        float Size_Y = Txt_rect.sizeDelta.y + space;

        if (Size_Y >= 210)
        {
            rect.sizeDelta = new Vector2(Std.x, Size_Y);
        }
        else { rect.sizeDelta = new Vector2(Std.x, 210); }
    }

    public IEnumerator Rect_sIzing()
    {
        SizeCheck = true;

        // 1초 동안 실행 (여기서 SizeCtrl을 계속 유지하거나 반복하고 싶다면 while 문 사용)
        float elapsed = 0f;
        while (elapsed < 1f)
        {
            SizeCtrl(); // 1초 동안만 동작시킬 함수
            elapsed += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        SizeCheck = false;
    }
}

[System.Serializable]
public class Hov_info
{
    public Sprite Hov_Spr;
    public string Hov_Tit;
    public List<string> Hov_Txts;
}