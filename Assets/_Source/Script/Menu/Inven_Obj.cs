using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inven_Obj : MonoBehaviour
{
    public GameObject Obj;

    [Header("RWP > VAL")]
    public List<GameObject> MenuObj;

    public List<string> EquipPT;

    public List<string> RWP;
    public List<string> MWP;

    public List<string> GRN;
    public List<int> GRN_Cnt;

    public List<string> CON;
    public List<int> Con_Cnt;

    public List<string> MAT;
    public List<int> Mat_Cnt;

    public List<string> VAL;
    public List<int> VAL_Cnt;

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