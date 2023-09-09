using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [Header("Урон")]
    public float damage;
    [Header("Скорострельность")]
    public float fireRate = 1;//скорострельность
    float nextFireTime = 0.0f;//обнуляемое время
    [Header("Время перезарядки")]
    public float reloadTime = 2;//время перезарядки
    [Header("Патрон в магазине")]
    public int bulletsPerClipDef = 7;
    [Header("Всего патрон")]
    public int clipsDef = 48;
    [HideInInspector]
    public int bulletsLeft = 7;//патрон в магазине
    bool isReloading;//перезарядка

    [Header("Звук стрельбы")]
    public AudioClip shoot_clip;
    [Header("Звук перезарядки")]
    public AudioClip reload_clip;
    AudioSource audioSorce;

    [Header("Объект создаваемой пули")]
    public GameObject bulletGo;//пуля
    [Header("Точка появления пули")]
    public Transform firePoint;//точка спавна пули

    public float spreadAngle = 5f;   // Угол разброса

    WeaponAnimation weaponAnimation; //
    UI_output ui_outputs; //
    Transform mainCamera; //
    private void Awake()
    {
        ui_outputs = FindObjectOfType<UI_output>();
        ui_outputs.ws = this;
        mainCamera = FindObjectOfType<Camera>().transform;
    }
    private void Start()
    {
        weaponAnimation = transform.GetComponentInChildren<WeaponAnimation>();
        audioSorce = transform.GetComponentInChildren<AudioSource>();
    }
    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    public void Shot()
    {
        if (bulletsLeft == 0 || isReloading)
            return;

        firePoint.LookAt(GetCenterOfScreen());

        // Если между последним и этим кадром более одной пули
        // Сбрасываем следующее время FireTime
        if (Time.time - fireRate > nextFireTime)
        {
            nextFireTime = Time.time - Time.deltaTime;
        }
        // Продолжаем стрелять, пока не израсходуем время стрельбы
        while (nextFireTime < Time.time && bulletsLeft != 0)
        {
            OneShot();
            nextFireTime += fireRate;
        }
    }
    public void OneShot()
    {
        bulletsLeft--;//Отнимаем одну пулю с магазина
        weaponAnimation.Fire();//запускаем метод в скрипте анимации
        PlayAudioClips(shoot_clip);

        // Генерируем случайные углы разброса
        float randomX = Random.Range(-spreadAngle, spreadAngle);
        float randomY = Random.Range(-spreadAngle, spreadAngle);

        // Создаем поворот для пули с учетом разброса
        Quaternion bulletRotation = Quaternion.Euler(randomX, randomY, 0f);

        Bullet bullScript = Instantiate(bulletGo, firePoint.position, firePoint.rotation* bulletRotation).GetComponent<Bullet>();
        bullScript.damage = damage;
    }
    public void Reload()
    {
        if (isReloading || bulletsLeft == bulletsPerClipDef || clipsDef==0)
            return;
  
        StartCoroutine(Reloading());
    }

    IEnumerator Reloading()
    {
        isReloading = true;
        weaponAnimation.Reloading(reloadTime);
        PlayAudioClips(reload_clip);
        
        yield return new WaitForSeconds(reloadTime);
        int bulletsReload = bulletsPerClipDef - bulletsLeft;
        if (bulletsReload> clipsDef)
        {
            bulletsLeft = clipsDef;
            clipsDef = 0;
        }
        else
        {
            clipsDef -= bulletsReload;
            bulletsLeft = bulletsPerClipDef;
        }
        isReloading = false;
    }

    public void PlayAudioClips(AudioClip curClip)
    {
        audioSorce.clip = curClip;
        audioSorce.Play();
    }

    Vector3 GetCenterOfScreen()
    {
        Ray ray = new Ray(); //Создание нового объекта Ray.
        ray.origin = mainCamera.position; //Установка начальной точки луча в позицию mainCamera
        ray.direction = mainCamera.transform.forward; //Установка направления луча на направление, в котором смотрит камера.
        RaycastHit raycastHit = new RaycastHit(); //Создание объекта RaycastHit для хранения информации о точке столкновения.
        if (Physics.Raycast(ray, out raycastHit, 300))
        {
            return raycastHit.point;//Если столкновение обнаруживается, то возвращается точка столкновения.
        }
        return mainCamera.position + mainCamera.forward * 100; //Если столкновение не обнаруживается в указанном диапазоне, то возвращается точка, находящаяся в 100 единицах перед камерой.
    }
}
