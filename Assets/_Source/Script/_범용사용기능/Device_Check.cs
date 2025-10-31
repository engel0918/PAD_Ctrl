using UnityEngine;
using UnityEngine.InputSystem;

public class Device_Check : MonoBehaviour
{
    public static string device; 

    void Start()
    {
        DetectStartDevice();
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

    void Update()
    {
        // Gamepad
        if (device != "PAD")
        {
            if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
            {
                Evt_PAD();
                return;
            }
        }

        // Keyboard / Mouse (버튼 입력만 체크)
        if (device != "KM")
        {
            // 키보드 키 입력
            if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            {
                Evt_KM();
                return;
            }

            // 마우스 버튼 입력 (움직임 제외!)
            if (Mouse.current != null &&
                (Mouse.current.leftButton.wasPressedThisFrame ||
                 Mouse.current.rightButton.wasPressedThisFrame ||
                 Mouse.current.middleButton.wasPressedThisFrame))
            {
                Evt_KM();
                return;
            }
        }
    }

    void Evt_PAD()
    {
        if (device != "PAD")
        {
            Debug.Log("Mod: PAD");
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
}