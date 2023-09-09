using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_output: MonoBehaviour
{

    public Slider hp_slider;
    public Text ammo_text;

    public WeaponScript ws;
    public PlayerStatus ps;
    void Start()
    {
        
    }

    void Update()
    {
        hp_slider.value = ps.heath;
        ammo_text.text = string.Format("{0}/{1}",ws.bulletsLeft,ws.clipsDef);
    }
}
