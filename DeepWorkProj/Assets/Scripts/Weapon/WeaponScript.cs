using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [Header("����")]
    public float damage;
    [Header("����������������")]
    public float fireRate = 1;//����������������
    float nextFireTime = 0.0f;//���������� �����
    [Header("����� �����������")]
    public float reloadTime = 2;//����� �����������
    [Header("������ � ��������")]
    public int bulletsPerClipDef = 7;
    [Header("����� ������")]
    public int clipsDef = 48;
    [HideInInspector]
    public int bulletsLeft = 7;//������ � ��������
    bool isReloading;//�����������

    [Header("���� ��������")]
    public AudioClip shoot_clip;
    [Header("���� �����������")]
    public AudioClip reload_clip;
    AudioSource audioSorce;

    [Header("������ ����������� ����")]
    public GameObject bulletGo;//����
    [Header("����� ��������� ����")]
    public Transform firePoint;//����� ������ ����

    public float spreadAngle = 5f;   // ���� ��������

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

        // ���� ����� ��������� � ���� ������ ����� ����� ����
        // ���������� ��������� ����� FireTime
        if (Time.time - fireRate > nextFireTime)
        {
            nextFireTime = Time.time - Time.deltaTime;
        }
        // ���������� ��������, ���� �� ����������� ����� ��������
        while (nextFireTime < Time.time && bulletsLeft != 0)
        {
            OneShot();
            nextFireTime += fireRate;
        }
    }
    public void OneShot()
    {
        bulletsLeft--;//�������� ���� ���� � ��������
        weaponAnimation.Fire();//��������� ����� � ������� ��������
        PlayAudioClips(shoot_clip);

        // ���������� ��������� ���� ��������
        float randomX = Random.Range(-spreadAngle, spreadAngle);
        float randomY = Random.Range(-spreadAngle, spreadAngle);

        // ������� ������� ��� ���� � ������ ��������
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
        Ray ray = new Ray(); //�������� ������ ������� Ray.
        ray.origin = mainCamera.position; //��������� ��������� ����� ���� � ������� mainCamera
        ray.direction = mainCamera.transform.forward; //��������� ����������� ���� �� �����������, � ������� ������� ������.
        RaycastHit raycastHit = new RaycastHit(); //�������� ������� RaycastHit ��� �������� ���������� � ����� ������������.
        if (Physics.Raycast(ray, out raycastHit, 300))
        {
            return raycastHit.point;//���� ������������ ��������������, �� ������������ ����� ������������.
        }
        return mainCamera.position + mainCamera.forward * 100; //���� ������������ �� �������������� � ��������� ���������, �� ������������ �����, ����������� � 100 �������� ����� �������.
    }
}
