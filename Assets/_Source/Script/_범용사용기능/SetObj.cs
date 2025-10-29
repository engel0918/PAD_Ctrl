using UnityEngine;

public class SetObj : MonoBehaviour
{
    public Set_Ctrl SetCtrl;

    private void OnEnable()
    {
        if(SetCtrl != null)
        {
            SetCtrl.Evt_Wakeup();
        }
    }
}
