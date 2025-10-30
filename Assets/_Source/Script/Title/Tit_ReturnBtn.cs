using UnityEngine;
using UnityEngine.UI;

public class Tit_ReturnBtn : MonoBehaviour
{
    public Button Btn;

    // Update is called once per frame
    void Update()
    {
        // 스킵 (Esc 또는 패드 B)
        if (Input.GetKeyDown(KeyCode.Escape) 
            || Input.GetKeyDown("joystick button 1"))
        {
            Btn.onClick.Invoke();
            return;
        }
    }
}
