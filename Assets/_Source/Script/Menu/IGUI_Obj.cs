using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IGUI_Obj : MonoBehaviour
{
    public GameObject Obj;

    [Header("IGUI > Quest")]
    public List<GameObject> MenuObj;
    public List<Button> MenuBtns;


    private void Start()
    {
        SetBtn();
    }

    void SetBtn()
    {
        if (MenuBtns.Count > 0)
        {
            for (int i = 0; i <= (MenuBtns.Count - 1); i++)
            {
                if (MenuBtns[i] != null)
                {
                    int index = i;
                    MenuBtns[i].onClick.AddListener(() => Sel_Menu(index));
                }
            }
        }
    }

    public void Sel_Menu(int i)
    {
        foreach (GameObject page in MenuObj)
        {
            if (page != null)
            { page.SetActive(false); }
        }

        MenuObj[i].SetActive(true);
    }
}
