using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp_Load : MonoBehaviour
{
    PopUp popup;

    public string Title;
    public List<string> info;
    public List<string> BtnTxt;

    [SerializeField] bool Start_On;

    public enum Type { NewGame, LoadGame, SetApply, QuitGame }
    public Type type;

    private void OnEnable()
    {
        if (Start_On == true)
        {
            PopUP_On();
        }
    }

    public void PopUP_On()
    {
        if (popup == null) { popup = GameObject.FindGameObjectWithTag("PopUp").GetComponent<PopUp>(); }

        // 버튼에 내용적용
        if (type == Type.NewGame)
        {
            popup.popupBtns[0].onClick.AddListener(() => NewGame_A(0));
            popup.popupBtns[1].onClick.AddListener(() => NewGame_A(1));
        }
        else if (type == Type.LoadGame)
        {
            popup.popupBtns[0].onClick.AddListener(() => LoadGame_A(0));
            popup.popupBtns[1].onClick.AddListener(() => LoadGame_A(1));
        }
        else if (type == Type.SetApply)
        {
            popup.popupBtns[0].onClick.AddListener(() => SetApply_A(0));
            popup.popupBtns[1].onClick.AddListener(() => SetApply_A(1));
        }
        else if (type == Type.QuitGame)
        {
            popup.popupBtns[0].onClick.AddListener(() => QuitGame_A(0));
            popup.popupBtns[1].onClick.AddListener(() => QuitGame_A(1));
        }


        popup.PopUP_On(Title, info, BtnTxt, true);
    }

    void NewGame_A(int i)
    {
        popup.popup.Tit.SetReturn();

        if (i == 0)
        { Debug.Log("새로운 여정을 떠납니다."); }
        else if (i == 1) { Debug.Log("새로운 여정을 떠나지않습니다."); }
    }

    void LoadGame_A(int i)
    {
        if (i == 0)
        { Debug.Log("저장된 여행을 떠납니다."); }
        else if (i == 1) { Debug.Log("저장된 여행을 떠나지않습니다."); }
    }

    void SetApply_A(int i)
    {
        if (i == 0)
        {
            Debug.Log("변경된 설정을 적용하였습니다.");
            GetComponent<Set_Ctrl>().Func_Apply();
        }
        else if (i == 1) { Debug.Log("변경된 설정을 적용하지않습니다."); }
    }

    void QuitGame_A(int i)
    {
        popup.popup.Tit.SetReturn();
        if (i == 0)
        {
            Debug.Log("게임을 종료합니다.");
            OnApplicationQuit();
        }
        else if (i == 1)
        { Debug.Log("게임을 종료하지않습니다."); }
    }

    void OnApplicationQuit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // 에디터 플레이 모드 종료
        #else
        Application.Quit();  // 빌드된 게임 종료
        #endif
    }
}
