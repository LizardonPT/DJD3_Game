using UnityEngine;
using UnityEngine.AI;

public class FixedPatrolAI : MonoBehaviour
{
    [SerializeField] private Transform player = default;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask whatIsGround= default, whatIsPlayer= default;

    // Patroling

    [SerializeField] private Transform startPatrol;
    [SerializeField] private Transform checkpoint1;
    [SerializeField] private Transform checkpoint2;
    [SerializeField] private Transform checkpoint3;
    [SerializeField] private Transform endPatrol;
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
        if (distanceToWalkPoint.magnitude < 5f) walkPointSet = false;       
    }

    private void NextPatrol()
    {
        switch (patrolRoute)
        {
            case 0:
                walkPoint = new Vector3(startPatrol.position.x, startPatrol.position.y, startPatrol.position.z);
                break;
            case 1:
                walkPoint = new Vector3(checkpoint1.position.x, checkpoint1.position.y, checkpoint1.position.z);
                break;
            case 2:
                walkPoint = new Vector3(checkpoint2.position.x, checkpoint2.position.y, checkpoint2.position.z);
                break;
            case 3:
                walkPoint = new Vector3(checkpoint3.position.x, checkpoint3.position.y, checkpoint3.position.z);
                break;
            case 4:
                walkPoint = new Vector3(endPatrol.position.x, endPatrol.position.y, endPatrol.position.z);
                break;
        }
        walkPointSet = true;
        if(++patrolRoute > 4) patrolRoute = 0;
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
