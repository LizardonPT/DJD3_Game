using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player = default;
    [SerializeField] private LayerMask whatIsGround= default, whatIsPlayer= default;
    [SerializeField] private int runSpeed = default;
    private NavMeshAgent agent;

    // Wandering Variables
    private bool walkPointSet;
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private float walkPointRange = default;


    // Sight Variables
    [SerializeField] float sightRange = default;
    private bool playerInSightRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        // Checkif the player is in sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        // If the player is not in sight, move random
        if(!playerInSightRange) AIWander();
        // If player is in sight  run
        if(playerInSightRange) BaseAiRun();
    }

    private void AIWander()
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

    private void BaseAiRun()
    {
        // Runs away to the contrary direction of the player
        agent.Move((transform.position - player.position).normalized* runSpeed * Time.deltaTime);
    }
}
