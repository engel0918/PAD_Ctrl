using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Set_Ctrl : MonoBehaviour
{
    [Header("Order: Gp, Res, Aud")]
    [SerializeField] List<GameObject> Pages;
    [SerializeField] List<Button> PageBtns;

    [Header("Order: GQ, AA, BM, RES")]
    [SerializeField] List<Dropdown> Dd_List;

    [Header("Order: Mv, BGMv, SFXv")]
    [SerializeField] List<Dropdown> Sld_List;

    private void Start()
    {
        SetBtn();
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(PageBtns[0].gameObject);
        func_PageCtrl(0);
    }

    void SetBtn()
    {
        if (PageBtns.Count > 0)
        {
            for (int i = 0; i <= (PageBtns.Count - 1); i++)
            {
                if (PageBtns[i] != null)
                {
                    int index = i;
                    PageBtns[i].onClick.AddListener(() => func_PageCtrl(index));
                }
            }
        }
    }

    public void func_PageCtrl(int i)
    {
        foreach (GameObject page in Pages)
        {
            if (page != null)
            { page.SetActive(false); }
        }

        Pages[i].SetActive(true);
    }
}
