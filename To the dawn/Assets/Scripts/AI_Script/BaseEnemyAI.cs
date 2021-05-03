using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player = default;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask whatIsGround= default, whatIsPlayer= default;

    // Patroling
    private bool walkPointSet;
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private float walkPointRange = default;

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
        // If no walkpoint is not set, define a new one
        if(!walkPointSet) SearchWalkPoint();

        // If walkpoint is set go there
        if(walkPointSet) agent.destination = walkPoint;
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 15f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // Generate random point in range and define it as walkpoint
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Verifies if the point is on the ground
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
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
