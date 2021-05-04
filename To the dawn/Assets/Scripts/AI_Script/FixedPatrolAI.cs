using UnityEngine;
using UnityEngine.AI;

public class FixedPatrolAI : MonoBehaviour
{
    private Vector3 player = default;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask whatIsGround= default, whatIsPlayer= default;

    // Patroling
    [SerializeField] private Transform[] path;
    [SerializeField] private float checkpointArea;
    [SerializeField] private float timer;
    private Collider[] playerInAttackRange;
    private Vector3 walkPoint;
    private int patrolRoute = 0;
    private bool walkPointSet = false;


    // Attacking
    [SerializeField] private float timeBetweenAttacks = default;
    private bool alreadyAttacked;

    // States
    [SerializeField] float sightRange = default;
    [SerializeField] float attackRange = default;
    private Collider[] playerInSightRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        // Checkif the player is in sight and attack range
        playerInSightRange = Physics.OverlapSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.OverlapSphere(transform.position, attackRange, whatIsPlayer);

        // If the player is neither is sight nor in attack range
        // AI will patroll
        if(playerInSightRange.Length == 0 && playerInAttackRange.Length == 0) BaseAiPatrol();
        // If player is in sight but not in attack range chase him
        if(playerInSightRange.Length > 0 && playerInAttackRange.Length == 0) BaseAiChase();
        // If player is in sight and attack range attack him
        if(playerInSightRange.Length > 0 && playerInAttackRange.Length > 0)
        {
            if(timer == 0)
            {
                transform.LookAt(playerInAttackRange[0].transform);
                player = playerInAttackRange[0].transform.position;
            }

            timer += Time.deltaTime;

            if(timer >= 0.1f)
            {
                BaseAiAttack();
                timer = 0;
            }
        }
        else
        {
            timer = 0;
        }
    }

    private void BaseAiPatrol()
    {
        if(walkPointSet)
        agent.destination = walkPoint;
        else NextPatrol();

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            Debug.Log("PROBLABLY AN Invalid PatrolPoint");

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < checkpointArea) walkPointSet = false;
    }

    private void NextPatrol()
    {
        agent.speed = 2;
        walkPoint = new Vector3(path[patrolRoute].position.x,path[patrolRoute].position.y, path[patrolRoute].position.z);
        walkPointSet = true;
        if(++patrolRoute >= path.Length) patrolRoute = 0;
    }

    private void BaseAiChase()
    {
        agent.speed = 6;
        agent.SetDestination(playerInSightRange[0].transform.position);
    }

    private void BaseAiAttack()
    {
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        if(!alreadyAttacked)
        {
            gameObject.GetComponent<AIWeapon>().Fire(player);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
