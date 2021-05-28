using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent = default;
    [SerializeField] private LayerMask whatIsPlayer= default;
    [SerializeField] private float timer = default;
    [SerializeField] private float timeBetweenAttacks = default;
    public float sightRange = default;
    [SerializeField] float attackRange = default;
    private Collider[] playerInSightRange;
    private Collider[] playerInAttackRange;
    private bool alreadyAttacked;
    private bool chaseMode = false;
    private Vector3 player = default;
    private float defaultSight = default;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        defaultSight = sightRange;
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
            sightRange = defaultSight;
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

                if(timer == 0)
                {
                    transform.LookAt(playerInAttackRange[0].transform);
                    player = playerInAttackRange[0].transform.position;
                }

                timer += Time.deltaTime;

                if(timer >= 0.3f)
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
            }
        }
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

    private void UnderAttack()
    {
        sightRange *= 2;
    }
}
