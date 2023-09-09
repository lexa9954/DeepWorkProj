using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float heath = 100;
    public UI_output ui_output;

    private void Start()
    {
        ui_output = FindObjectOfType<UI_output>();
        ui_output.ps = this;
    }
    public void AttackPlayer(int damage)
    {
        heath -= damage;
    }

}
