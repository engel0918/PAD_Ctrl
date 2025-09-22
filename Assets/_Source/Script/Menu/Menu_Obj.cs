using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Obj : MonoBehaviour
{
    public GameObject Obj;

    [Header("Order => MenuList Order")]
    public Transform Btn_Parents;

    [Header("Menu List")]
    public MyStat_Ctrl MyStat;
    public Inven_Ctrl Inven;
    public Quest_Ctrl Quest;
    public Set_Ctrl Set;
    public Exit_Ctrl Exit;

    Menu_Ctrl Menu;
    PageCtrl Page;

    private void Awake()
    {
        Menu = GetComponent<Menu_Ctrl>();
    }

    public void Sel_Menu(string str)
    {
        if(Page != null)
        { Page = GetComponent<PageCtrl>(); }

        if (Menu.Opend_Menu.Count <= 0)
        {
            if (str == "Menu")
            {
                Page.func_PageCtrl(0);
                Menu.Opend_Menu.Add(str);
            }
            else if (str == "MyStat")
            {
                Page.func_PageCtrl(1);
                Menu.Opend_Menu.Add(str);
            }
            else if (str == "Inventory")
            {
                Page.func_PageCtrl(2);
                Menu.Opend_Menu.Add(str);
            }
            else if (str == "Quest")
            {
                Page.func_PageCtrl(3);
                Menu.Opend_Menu.Add(str);
            }
            else if (str == "Setting")
            {
                Page.func_PageCtrl(4);
                Menu.Opend_Menu.Add(str);
            }
            else if (str == "Exit")
            {
                Page.func_PageCtrl(5);
                Menu.Opend_Menu.Add(str);
            }
            else
            {
                Debug.Log("Error!!: 메뉴가 없음!!");
            }
        }

    }
}
