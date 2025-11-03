using UnityEngine;
using UnityEngine.UI;

public class Sld_Evt : MonoBehaviour
{
    [SerializeField] Slider Sld;
    [SerializeField] float Mov_Val = 3;


    public Button Btn_SldDown;
    public Button Btn_SldUp;

    private void Start()
    {
        Btn_SldDown.onClick.AddListener(() => Evt_SldValue(Mov_Val * -1));
        Btn_SldUp.onClick.AddListener(() => Evt_SldValue(Mov_Val * 1));
    }

    public void Evt_SldValue(float f)
    {
        Sld.value += f; 
    }
}
