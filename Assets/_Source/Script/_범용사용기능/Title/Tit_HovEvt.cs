using UnityEngine;
using UnityEngine.EventSystems;

public class Tit_HovEvt : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Tit_Ctrl Tit;
    private int index;
    void Start()
    {
        // 자기 자신이 Tit_Btns 몇 번째인지 저장
        index = Tit.Tit_Btns.IndexOf(GetComponent<UnityEngine.UI.Button>());
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Debug.Log($"{gameObject.name} 이(가) 선택되었습니다!");
        // 선택 시 실행할 코드
        if (Tit != null) { Tit.Hov_Evt(index); }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //Debug.Log($"{gameObject.name} 이(가) 선택 해제되었습니다!");
        // 선택 해제 시 실행할 코드
        if (Tit != null) { Tit.HoverOut_Evt(); }
    }
}
