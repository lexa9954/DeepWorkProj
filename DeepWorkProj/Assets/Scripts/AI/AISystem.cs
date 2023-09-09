using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISystem : MonoBehaviour
{
    public float health = 100; //Здоровье врага
    public float damage = 10;
    public Transform target; //Цель врага
    PlayerStatus player_status;
    NavMeshAgent agent; //Компонент ИИ
    Animator animator; //Компонент аниматора
    public float maxTargetDistance = 1; //Максимальная дистанция для удара игрока
    public float timeAttack;
    public GameObject drop_spawn;
    bool idDead;
    bool isAttack;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player_status = FindObjectOfType<PlayerStatus>();
        target = player_status.transform;
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (health <= 0)
        {
            if (idDead)
                return;
            StartCoroutine(Dead());
            return;
        }
            

        if (Vector3.Distance(transform.position, target.position) < maxTargetDistance)
        {
            if (isAttack)
                return;
            StartCoroutine("Attack");
            animator.SetBool("Walk", false);
            animator.SetBool("Attack",true);
            agent.isStopped = true;
        }
        else
        {
            isAttack = false;
            StopCoroutine("Attack");
            animator.SetBool("Walk",true);
            animator.SetBool("Attack", false);
            agent.destination = target.position;
            agent.isStopped = false;
        }
    }

    IEnumerator Dead()
    {
        idDead = true;
        animator.SetBool("Walk", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Dead", true);
        agent.isStopped = true;
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    IEnumerator Attack()
    {
        isAttack = true;
        yield return new WaitForSeconds(timeAttack);
        player_status.heath -= damage;
        isAttack = false;
    }
}
