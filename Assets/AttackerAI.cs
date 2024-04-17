using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackerAI : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead
    }
    public FSMStates curState;
    public float attackDistance = 2;
    public float chaseDistance = 10;
    public GameObject player;
    public float enemySpeed = 5;
    public GameObject spellProjectile;
    public GameObject wandTip;
    public float shootRate = 1.0f;
    public GameObject deadVFX;
    public PlayerHealth playerHealth;
    public Transform enemyEyes;
    public float fov = 45f;
    public AudioClip punchSFX;
    GameObject[] wanderPoints;
    Animator anim;
    Vector3 nextDestination;
    int currentIndex = 0;
    float distToPlayer;
    float elapsedTime = 0;

    int health;
    Transform deadTransform;

    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        wanderPoints = GameObject.FindGameObjectsWithTag("Waypoint");
        anim = GetComponent<Animator>();
        curState = FSMStates.Patrol;
        FindNextPoint();
        player = GameObject.FindGameObjectWithTag("Player");
        wandTip = GameObject.FindGameObjectWithTag("WandTip");

        deadTransform = transform;

        //agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //print(distToPlayer);
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
        }

        elapsedTime += Time.deltaTime;
    }

    void UpdatePatrolState()
    {
        print("Patrolling!");
        anim.SetInteger("animState", 1);
        agent.speed = 0.5f;
        agent.stoppingDistance = 0;
        if (Vector3.Distance(transform.position, nextDestination) < 1)
        {
            FindNextPoint();
        }
        else if (distToPlayer <= chaseDistance && IsPlayerInFOV())
        {
            curState = FSMStates.Chase;
        }
        FaceTarget(nextDestination);
        agent.SetDestination(nextDestination);
    }
    void UpdateChaseState()
    {
        anim.SetInteger("animState", 1);
        nextDestination = player.transform.position;
        agent.speed = 1;
        agent.stoppingDistance = attackDistance;
        if (distToPlayer <= attackDistance)
        {
            curState = FSMStates.Attack;
            elapsedTime = 0.2f;
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

        }
        else
        {
            curState = FSMStates.Chase;
        }
        FaceTarget(nextDestination);
        if (elapsedTime >= shootRate)
        {
            AudioSource.PlayClipAtPoint(punchSFX, transform.position, 0.1f);
            playerHealth.TakeDamage(20);
            elapsedTime = 0;
        }
        
    }

    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentIndex].transform.position;
        currentIndex = (currentIndex + 1) % wanderPoints.Length;
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

    void EnemySpellCast()
    {
        if (elapsedTime >= shootRate)
        {
            var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;
            Invoke("SpellCasting", animDuration/2.0f);
            elapsedTime = 0.0f;
        }


    }

    void SpellCasting()
    {
        GameObject projectile = Instantiate(spellProjectile,
                wandTip.transform.position + transform.forward, transform.rotation) as GameObject;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.AddForce((transform.position - player.transform.position) * 50, ForceMode.VelocityChange);
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
