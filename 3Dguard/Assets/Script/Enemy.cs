using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    public Camera cam;

    public NavMeshAgent agent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = .5f;
    public float timeBetweenAttacks;
    public Transform player;

    public Transform[] waypoints;
    public GameObject projectile;
    int m_CurrentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;
    bool alreadyAttacked;


    // Update is called once per frame
    void Start() 
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;
        player = GameObject.Find("FpsPlayer").transform;

        m_CurrentWaypointIndex = 0;
        agent = GetComponent<NavMeshAgent>();

        agent.isStopped = false;
        agent.speed = speedWalk;
        agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }


    void Update()
    {
        EnvironmentView();                       //  Check whether or not the player is in the enemy's field of vision
 
        if (!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }
    private void Chasing()
    {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;
        if(!m_CaughtPlayer)
        {
            Move(speedRun);
            agent.SetDestination(m_PlayerPosition);
        }
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            if(m_WaitTime <=0 && !m_CaughtPlayer && Vector3.Distance(transform.position, player.transform.position) >=6f)
            {
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, player.transform.position) >= 2.5f)
                {
                    Stop();
                }
                m_WaitTime -= Time.deltaTime;
            }
        }
    }
    private void Patroling()
    {
        if(m_PlayerNear)
        {
            if(m_TimeToRotate<=0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;
            agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                if(m_WaitTime <=0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
        
    }

    void Move(float speed)//Gets enemy to move again after stopping from looking for player
    {
        agent.isStopped = false;
        agent.speed = speed;
    }

    void Stop()//enemy waits after it lost the player
    {
        agent.isStopped = true;
        agent.speed = 0;
    }
    public void NextPoint()//changes next patrol destination
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

    }

    void CaughtPlayer()//determines if player caught
    {
        m_CaughtPlayer = true;//gameover can add animation here for enemy 
    }
    void LookingPlayer(Vector3 player)//if player is spotted enemy routes to player here
    {
        agent.SetDestination(player);
        if(Vector3.Distance(transform.position, player)<=.3f)
        {
            if(m_WaitTime <=0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

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
    
    }


    private void StunPlayer()//shoot while chasing the player
    {
        //Make sure enemy doesn't move

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);//tazer gets shot at player
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void ShutDown()
    {
        //shutDown bot for a set time
    }
}
