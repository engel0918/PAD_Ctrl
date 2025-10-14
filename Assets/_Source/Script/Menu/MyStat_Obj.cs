using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyStat_Obj : MonoBehaviour
{
    public IGUI_Ctrl IGUI;
    public Inven_Ctrl Inven;

    public GameObject Obj;

    public List<Button> Btns;

    private void Start()
    {
        SetBtn();


    }

    void SetBtn()
    {
        // RWP
        Btns[0].onClick.AddListener(() => Go_Inven(0));
        Btns[1].onClick.AddListener(() => Go_Inven(0));
        // MWP
        Btns[2].onClick.AddListener(() => Go_Inven(1));
        // GRN
        Btns[3].onClick.AddListener(() => Go_Inven(2));
        // PTS
        Btns[4].onClick.AddListener(() => Go_Inven(3));
        Btns[5].onClick.AddListener(() => Go_Inven(3));
        Btns[6].onClick.AddListener(() => Go_Inven(3));
        Btns[7].onClick.AddListener(() => Go_Inven(3));
        Btns[8].onClick.AddListener(() => Go_Inven(3));
        // CON
        Btns[9].onClick.AddListener(() => Go_Inven(4));
    }

    void Go_Inven(int i)
    {
        IGUI.Menu_Obj.Sel_Menu(1);
        Inven.Menu_Obj.Sel_Menu(i);
    }
}