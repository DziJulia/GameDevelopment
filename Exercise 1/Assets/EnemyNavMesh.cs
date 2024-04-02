using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyNavMesh : MonoBehaviour
{
    public Animator animator;
    public Transform[] patrolPoints;
    public NavMeshAgent agent;
    private bool aggro;
    public float patrolSpeed;
    public float aggroSpeed;
    private bool destinationReached;
    public float destinationThreshold;
    public float currentVelocity;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        destinationReached = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentVelocity = agent.velocity.magnitude;
        
        animator.SetFloat("velocity", currentVelocity);

        if (aggro == true)
        {
            agent.destination = player.position;
        }
        
        if (aggro == false && destinationReached == true)
        {
            destinationReached = false;
            agent.speed = patrolSpeed;
            agent.destination = patrolPoints[Random.Range(0, patrolPoints.Length)].position;
        }
        
        if (Vector3.Distance(transform.position, agent.destination) < destinationThreshold)
        {
            destinationReached = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            aggro = true;
            agent.speed = aggroSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.destination = player.position;
            StopCoroutine("AggroTimer");
            StartCoroutine("AggroTimer");
        }
    }

    IEnumerator AggroTimer()
    {
        yield return new WaitForSeconds(5);
        aggro = false;
        destinationReached = true;
    }
}
