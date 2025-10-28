using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Set_Ctrl : MonoBehaviour
{
    [Header("Order: GQ, AA, BM, RES")]
    [SerializeField] List<Dropdown> Dd_List;

    [Header("Order: Mv, BGMv, SFXv")]
    [SerializeField] List<Dropdown> Sld_List;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
