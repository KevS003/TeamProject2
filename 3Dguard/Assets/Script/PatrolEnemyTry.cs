using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemyTry : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshAgent enemy;

    public Transform PlayerTarget;
    public Transform[] waypoints;
    int waypointIndex;
    bool playerDetected = false;
    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        UpdateDestination();
        
    }

    // Update is called once per frame
    void Update()
    {


        //enemy.SetDestination(PlayerTarget.position);
        if(Vector3.Distance(transform.position, target)< 1 && playerDetected == false)
        {
            IterateWaypointIndex();
            UpdateDestination();
        }
    }
    void Patrol()
    {

    }

    void Chase()
    {
        enemy.SetDestination(PlayerTarget.position);
    }
    void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        enemy.SetDestination(target);
    }
    void IterateWaypointIndex()
    {
        waypointIndex++;
        if(waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
    }/*
    void EnvironmentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for(int i =0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToPlayer)<viewAngle/2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if(!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))//true if player is in sight of enemy
                {
                    m_PlayerInRange = true;
                    m_IsPatrol = false;
                }
                else//if player is behind an obstacle this is false
                {
                    m_PlayerInRange = false;
                }
            }
            if(Vector3.Distance(transform.position, player.position)>viewRadius)
            {
                m_PlayerInRange = false;
            }
            if(m_PlayerInRange)
            {
                m_PlayerPosition = player.transform.position;
            }
        }
    
    }*/
}
