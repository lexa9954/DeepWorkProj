using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public GameObject objectToSpawn;    // ������ ��� ������
    public int numberOfObjects = 10;    // ���������� �������� ��� ������
    public Vector2 spawnArea = new Vector2(10f, 10f); // ������ �������� ��� ������

    private void Start()
    {
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            // ���������� ��������� ���������� ������ ��������
            float randomX = Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f);
            float randomZ = Random.Range(-spawnArea.y / 2f, spawnArea.y / 2f);

            // ������� ����� ������ ��� ��������� ������������
            Instantiate(objectToSpawn, new Vector3(randomX, 0f, randomZ), Quaternion.identity);
        }
    }

    void Update()
    {
        
    }
}
