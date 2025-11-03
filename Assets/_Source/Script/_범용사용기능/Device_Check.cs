using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Device_Check : MonoBehaviour
{
    public static string device;

    public GameObject Btn_LastHoved;
    public GameObject Btn_PageSet;

    void Start()
    {
        device = "PAD";

        //DetectStartDevice();
    }

    // 게임 시작 시 1회 확인
    void DetectStartDevice()
    {
        if (Gamepad.current != null)
        { Evt_PAD(); }
        else
        {
            // 기본은 PC 환경 기준
            Evt_KM();
        }
    }

    //void Update()
    //{
    //    // Gamepad
    //    if (device != "PAD")
    //    {
    //        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
    //        {
    //            Evt_PAD();
    //            return;
    //        }
    //    }

    //    // Keyboard / Mouse (버튼 입력만 체크)
    //    if (device != "KM")
    //    {
    //        // 키보드 키 입력
    //        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
    //        {
    //            Evt_KM();
    //            return;
    //        }

    //        // 마우스 버튼 입력 (움직임 제외!)
    //        if (Mouse.current != null &&
    //            (Mouse.current.leftButton.wasPressedThisFrame ||
    //             Mouse.current.rightButton.wasPressedThisFrame ||
    //             Mouse.current.middleButton.wasPressedThisFrame))
    //        {
    //            Evt_KM();
    //            return;
    //        }
    //    }
    //}

    void Evt_PAD()
    {
        if (device != "PAD")
        {
            Debug.Log("Mod: PAD");

            if (Btn_LastHoved.activeSelf == true &&
                Btn_LastHoved.GetComponent<Selectable>().navigation.mode == Navigation.Mode.Automatic)
            {
                // 마지막 클릭된 버튼의 네비게이션이 오토인 경우
                EventSystem.current.SetSelectedGameObject(Btn_LastHoved);
            }
            else { EventSystem.current.SetSelectedGameObject(Btn_PageSet); }
        }
        device = "PAD";
    }

    void Evt_KM()
    {
        if (device != "KM")
        {
            Debug.Log("Mod: KM");
        }
        device = "KM";
    }

    public void HovEvt(GameObject obj)
    { Btn_LastHoved = obj; }
}