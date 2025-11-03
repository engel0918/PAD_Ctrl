using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dvc_HovEvt : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Device_Check DvcCheck;
    GameObject SelObj;

    private void Start()
    {
        SelObj = this.gameObject;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Debug.Log($"{gameObject.name} 이(가) 선택되었습니다!");
        // 선택 시 실행할 코드
        if (DvcCheck != null) 
        { DvcCheck.Btn_LastHoved = SelObj; }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //Debug.Log($"{gameObject.name} 이(가) 선택 해제되었습니다!");
        // 선택 해제 시 실행할 코드
    }
}
