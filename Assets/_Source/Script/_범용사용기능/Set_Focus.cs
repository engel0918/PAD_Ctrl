using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Set_Focus : MonoBehaviour
{
    [SerializeField] Button Btn;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(Btn.gameObject);
    }
}
