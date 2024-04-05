using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
public class EnemyAI : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Fix,
        Other
    }
    public FSMStates curState;
    public float attackDistance = 5;
    public float chaseDistance = 10;
    public GameObject player;
    public GameObject deadVFX;
    Vector3 distraction = new Vector3(0f, 10f, 0f);
    public Transform enemyEyes;
    public float fov = 45f;
    float elapsedTime = 0f;
    GameObject[] waypoints;
    GameObject[] turrets;
    Animator anim;
    Vector3 nextDestination;
    int currentIndex = 0;
    float distToPlayer;
    Transform deadTransform;
    GameObject curTurret;
    public NavMeshAgent agent;
    PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        anim = GetComponent<Animator>();
        curState = FSMStates.Patrol;
        FindNextPoint();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        deadTransform = transform;

        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        switch (curState)
        {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
            case FSMStates.Fix:
                UpdateFixState();
                break;
            case FSMStates.Other:
                break;
        }
        elapsedTime += Time.deltaTime;
    }

    public void SetDistraction(Vector3 d)
    {
        distraction = d;
    }
    void UpdatePatrolState()
    {
        //print("Patrolling!");
        anim.SetInteger("animState", 1);
        agent.stoppingDistance = 0;
        if (Vector3.Distance(transform.position, nextDestination) < 2)
        {
            if (distraction.y != 10f)
            {
                distraction.y = 10f;
            }
            GameObject[] active = GameObject.FindGameObjectsWithTag("Turret");
            GameObject[] disabled = GameObject.FindGameObjectsWithTag("Disabled");
            turrets = active.Concat(disabled).ToArray();
            for(int i = 0; i < turrets.Length; i++)
            {
                //print(Vector3.Distance(transform.position, turrets[i].transform.position));
                if (Vector3.Distance(transform.position, turrets[i].transform.position) < 3)
                {
                    curTurret = turrets[i];
                    break;
                }
            }
            if (curTurret != null && curTurret.CompareTag("Disabled"))
            {
                curState = FSMStates.Fix;
            }
            else
            {
                FindNextPoint();
            }
        }
        else if (distToPlayer <= chaseDistance && IsPlayerInFOV())
        {
            //curState = FSMStates.Chase;
        }
        if (distraction.y != 10f)
        {
            nextDestination = distraction;
        }
        FaceTarget(nextDestination);
        agent.SetDestination(nextDestination);
    }
    void UpdateChaseState()
    {
        anim.SetInteger("animState", 1);
        nextDestination = player.transform.position;
        agent.stoppingDistance = attackDistance;
        if (distToPlayer <= attackDistance)
        {
            curState = FSMStates.Attack;
        }
        else if (distToPlayer > chaseDistance)
        {
            curState = FSMStates.Patrol;
        }
        FaceTarget(nextDestination);
        agent.SetDestination(nextDestination);
    }
    void UpdateAttackState()
    {
        nextDestination = player.transform.position;
        anim.SetInteger("animState", 5);
        //agent.stoppingDistance = attackDistance;
        if (distToPlayer <= attackDistance)
        {
            curState = FSMStates.Attack;
            
            playerHealth.TakeDamage(50);
        }
        else
        {
            curState = FSMStates.Chase;
        }
        FaceTarget(nextDestination);
    }
    void UpdateFixState()
    {
        curState = FSMStates.Other;
        elapsedTime = 0f;
        anim.SetInteger("animState", 3);
        var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        Invoke("Inspect", animDuration);
    }
    void Inspect()
    {
        anim.SetInteger("animState", 2);
        var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        EnableTurret turretBehavior = curTurret.GetComponent<EnableTurret>();
        turretBehavior.EnableThis();
        Invoke("Stand", animDuration);
    }
    void Stand()
    {
        anim.SetInteger("animState", 4);
        var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        Invoke("BeginPatrolling", animDuration);
    }
    void BeginPatrolling()
    {
        curState = FSMStates.Patrol;
    }
    void FindNextPoint()
    {
        nextDestination = waypoints[currentIndex].transform.position;
        currentIndex = (currentIndex + 1) % waypoints.Length;
        agent.SetDestination(nextDestination);
    }
    void FaceTarget(Vector3 target)
    {
        Vector3 directionTo = (target - transform.position).normalized;
        directionTo.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionTo);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            lookRotation, 10 * Time.deltaTime);
    }

    private void OnDestroy()
    {
        Instantiate(deadVFX, deadTransform.position, deadTransform.rotation);
    }

    bool IsPlayerInFOV()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;

        if (Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fov)
        {
            if (Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    print("Player in sight!");
                    return true;
                }
            }
        }
        return false;
    }
}
