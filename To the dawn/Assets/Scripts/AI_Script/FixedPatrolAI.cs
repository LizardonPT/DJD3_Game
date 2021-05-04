using UnityEngine;
using UnityEngine.AI;

public class FixedPatrolAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent = default;
    [SerializeField] private LayerMask whatIsGround= default, whatIsPlayer= default;
    [SerializeField] private Transform[] path = default;
    [SerializeField] private float checkpointArea = default;
    [SerializeField] private float timer = default;
    [SerializeField] private float timeBetweenAttacks = default;
    [SerializeField] float sightRange = default;
    [SerializeField] float attackRange = default;
    private Collider[] playerInSightRange;
    private Collider[] playerInAttackRange;
    private Vector3 walkPoint;
    private int patrolRoute = 0;
    private bool walkPointSet = false;
    private bool alreadyAttacked;
    private bool chaseMode = false;
    private Vector3 player = default;

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
        if(playerInSightRange.Length == 0 && playerInAttackRange.Length == 0)
        {
            chaseMode = false;
            BaseAiPatrol();
        }
        // If player is in sight but not in attack range chase him
        else if(playerInSightRange.Length > 0 && playerInAttackRange.Length == 0)
        {
            // If he saw you, he will chase
            if(chaseMode)
            {
                BaseAiChase();
            }
            // If he sees you for the first time, chase activates
            else if(!Physics.Raycast(transform.position, 
                playerInSightRange[0].transform.position - transform.position,
                Vector3.Distance(playerInSightRange[0].transform.position,
                transform.position), 1 << LayerMask.NameToLayer("isGround")))
            {
                chaseMode = true;
                BaseAiChase();
            }
            // If he never saw you even in sight range, then no chase
            else
            {
                BaseAiPatrol();
            }
        }
        // If player is in sight and attack range attack him
        else if(playerInSightRange.Length > 0 && playerInAttackRange.Length > 0)
        {
            // If he sees you and you are in attack range, then attack
            if(!Physics.Raycast(transform.position, 
                playerInAttackRange[0].transform.position - transform.position,
                Vector3.Distance(playerInAttackRange[0].transform.position,
                transform.position), 1 << LayerMask.NameToLayer("isGround")))
            {
                chaseMode = true;

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
            // If he saw you but you are behind a wall, then he chases
            else if(Physics.Raycast(transform.position,
                playerInAttackRange[0].transform.position - transform.position,
                Vector3.Distance(playerInAttackRange[0].transform.position,
                transform.position), 1 << LayerMask.NameToLayer("isGround"))
                && chaseMode)
            {
                timer = 0;
                BaseAiChase();
            }
            // If he never saw you even in attack range, then no chase
            else
            {
                timer = 0;
                BaseAiPatrol();
            }
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
        agent.speed = 4;
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
