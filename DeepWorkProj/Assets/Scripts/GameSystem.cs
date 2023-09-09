using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public GameObject objectToSpawn;    // Объект для спавна
    public int numberOfObjects = 10;    // Количество объектов для спавна
    public Vector2 spawnArea = new Vector2(10f, 10f); // Размер квадрата для спавна

    private void Start()
    {
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            // Генерируем случайные координаты внутри квадрата
            float randomX = Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f);
            float randomZ = Random.Range(-spawnArea.y / 2f, spawnArea.y / 2f);

            // Создаем новый объект или клонируем существующий
            Instantiate(objectToSpawn, new Vector3(randomX, 0f, randomZ), Quaternion.identity);
        }
    }

    void Update()
    {
        
    }
}
