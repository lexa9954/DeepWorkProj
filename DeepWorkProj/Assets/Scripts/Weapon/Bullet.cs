using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Урон пули")]
    public float damage = 10;
    [Header("Скорость пули")]
    public int speed = 500;
    [Header("Сила пули")]
    public int impactForce = 10;
    [Header("Дырки от попадания")]
    public List<GameObject> impactObjects = new List<GameObject>();//Имя префаба Impact соответствует тегу, по которому он должен попасть.

    Vector3 velocity;
    Vector3 newPos;
    Vector3 oldPos;
    bool hasHit = false;
    void Start()
    {
        newPos = transform.position;
        oldPos = newPos;
        velocity = speed * transform.forward;

        Destroy(gameObject, 3);
    }

    void Update()
    {
        if (hasHit)
            return;
        // assume we move all the way
        newPos += velocity * Time.deltaTime;
        // Check if we hit anything on the way
        Vector3 direction = newPos - oldPos;
        float distance = direction.magnitude;

        if (distance > 0)
        {
            Ray Ray;
            RaycastHit hit;
            if (Physics.Raycast(oldPos, direction, out hit, distance))
            {
                // adjust new position
                newPos = hit.point;
                // notify hit
                hasHit = true;
                //Apply force if we hit rigidbody
                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForce(transform.forward * impactForce, ForceMode.Impulse);
                }
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                Debug.Log(rotation);
                for (int i = 0; i < impactObjects.Count; i++)
                {
                    if (hit.transform.tag == impactObjects[i].name)
                    {
                        
                        GameObject hole = Instantiate(impactObjects[i], hit.point, rotation);
                        hole.transform.parent = hit.transform;
                        hole.transform.localRotation = rotation;
                    }
                }

                switch (hit.transform.tag)
                {
                    case "Enemy":
                        AISystem enemy = hit.transform.GetComponent<AISystem>();
                        enemy.health -= damage;
                        Debug.Log("Shot to enemy!");
                        Destroy(gameObject);
                        break;
                }


            }
        }
        oldPos = transform.position;
        transform.position = newPos;
    }
}
