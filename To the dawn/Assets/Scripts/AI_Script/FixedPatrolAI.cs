using UnityEngine;
using UnityEngine.AI;

public class FixedPatrolAI : MonoBehaviour
{
    [SerializeField] private Transform player = default;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask whatIsGround= default, whatIsPlayer= default;

    // Patroling
    [SerializeField] private Transform[] path;
    [SerializeField] private float checkpointArea;

    private Vector3 walkPoint;
    private int patrolRoute = 0;
    private bool walkPointSet = false;


    // Attacking
    [SerializeField] private float timeBetweenAttacks = default;
    private bool alreadyAttacked;

    // States
    [SerializeField] float sightRange = default;
    [SerializeField] float attackRange = default;
    private bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        // Checkif the player is in sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // If the player is neither is sight nor in attack range
        // AI will patroll
        if(!playerInSightRange && !playerInAttackRange) BaseAiPatrol();
        // If player is in sight but not in attack range chase him
        if(playerInSightRange && !playerInAttackRange) BaseAiChase();
        // If player is in sight and attack range attack him
        if(playerInSightRange && playerInAttackRange) BaseAiAttack();
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
        walkPoint = new Vector3(path[patrolRoute].position.x,path[patrolRoute].position.y, path[patrolRoute].position.z);
        walkPointSet = true;
        if(++patrolRoute >= path.Length) patrolRoute = 0;
    }

    private void BaseAiChase()
    {
        agent.SetDestination(player.position);
    }

    private void BaseAiAttack()
    {
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            // Attack Code here
            Debug.Log("A robot has attacked you!"); 



            //

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
