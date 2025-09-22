using UnityEngine;
using UnityEngine.EventSystems;

public class Auto_Hover : MonoBehaviour
{
    public GameObject Start_Hover;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(Start_Hover);
    }

    public void Sel_(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(obj);
    }
}
