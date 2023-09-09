using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    public string TakeIn = "draw";
    public string Draw_empty = "draw_empty";
    public string Reload = "reload";
    public string Reload_empty = "reload_full";
    public string Shoot = "shoot";
    public string Shoot_last = "shoot_last";
    public float FireAnimationSpeed = 1;
    public float TakeInOutSpeed = 1;
    Animation AnimGun;
    WeaponScript WC;

    private void Start()
    {
        AnimGun = GetComponent<Animation>();
        WC = GameObject.FindObjectOfType<WeaponScript>();
        AnimGun.wrapMode = WrapMode.Once;
        takeIn();
    }
    public void takeIn()
    {
        
        if (WC.bulletsLeft > 0 )
        {
            AnimGun.Rewind(TakeIn);
            AnimGun[TakeIn].speed = TakeInOutSpeed;
            AnimGun[TakeIn].time = 0;
            AnimGun.Play(TakeIn);
        }
        else
        {
            AnimGun.Rewind(TakeIn);
            AnimGun[Draw_empty].speed = TakeInOutSpeed;
            AnimGun[Draw_empty].time = 0;
            AnimGun.Play(Draw_empty);
        }

    }
    public void Fire()
    {
        Debug.Log("Fire");
        if (WC.bulletsLeft > 0)
        {
            PlayAnimShoot(Shoot);
        }
        else
        {
            PlayAnimShoot(Shoot_last);
        }
    }

    public void PlayAnimShoot(string curAnim)
    {
        AnimGun.Rewind(curAnim);
        AnimGun[curAnim].speed = FireAnimationSpeed;
        AnimGun.Play(curAnim);
    }

    public void Reloading(float reloadTime)
    {
        if (WC.bulletsLeft > 0)
        {
            PlayAnimReload(Reload, (AnimGun[Reload].clip.length / reloadTime));
        }
        else
        {
            PlayAnimReload(Reload_empty, (AnimGun[Reload_empty].clip.length / reloadTime));
        }
    }
    public void PlayAnimReload(string curAnim,float curSpeed)
    {
        AnimGun.Rewind(curAnim);
        AnimGun[curAnim].speed = curSpeed;
        AnimGun.Play(curAnim);
    }
}
