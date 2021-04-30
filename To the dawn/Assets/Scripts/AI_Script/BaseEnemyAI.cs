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

    private void Update()
    {
        // Checkfor sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        if(!playerInSightRange && !playerInAttackRange) BaseAiPatrol();
        if(playerInSightRange && !playerInAttackRange) BaseAiChase();
        if(playerInSightRange && playerInAttackRange) BaseAiAttack();
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void BaseAiPatrol() // PATROLING IS KILLING THE FPS
    {
        Debug.Log("patroll"); 
        if(!walkPointSet) SearchWalkPoint();

        if(walkPointSet) agent.SetDestination(walkPoint);
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
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
            //Attack Code here
            Debug.Log("A robot has attacked you!"); 



            /// 

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
