using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 2.0f;
    public float maxYRotation = 80.0f; // Ограничение вертикального вращения

    private float rotationX = 0.0f;
    private Transform playerTransform;
    private Camera mainCamera;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerTransform = transform.parent;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Получение ввода с мыши
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Вращение камеры по горизонтали
        playerTransform.Rotate(Vector3.up * mouseX);

        // Вычисление вертикального вращения и его ограничение
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -maxYRotation, maxYRotation);

        // Вращение камеры по вертикали
        mainCamera.transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
    }
}
