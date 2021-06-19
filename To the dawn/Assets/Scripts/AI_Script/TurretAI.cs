using UnityEngine;

public class TurretAI : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsPlayer = default;
    public float sightRange = default;
    [SerializeField] private float cooldown = default;
    [SerializeField] private float timer = default;
    private bool alreadyAttacked;
    private Collider[] playerInSightRange;
    private Vector3 player;

    // Update is called once per frame
    void FixedUpdate()
    {
        playerInSightRange = Physics.OverlapSphere(transform.position, sightRange, whatIsPlayer);

        if(playerInSightRange.Length > 0)
        {
            if(!Physics.Raycast(transform.position, playerInSightRange[0].transform.position - transform.position, Vector3.Distance(playerInSightRange[0].transform.position,transform.position), 1 << LayerMask.NameToLayer("isGround")))
            {
                if(timer == 0)
                {
                    transform.LookAt(playerInSightRange[0].transform);
                    player = playerInSightRange[0].transform.position;
                }

                timer += Time.deltaTime;

                if(timer >= 0.1f)
                {
                    TurretAiAttack();
                    timer = 0;
                }
            }
        }
        else
        {
            timer = 0;
        }
    }

    private void TurretAiAttack()
    {
        if(!alreadyAttacked)
        {
            gameObject.GetComponent<AIWeapon>().Fire(player);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), cooldown);
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

    // Temporary fix i guess
    private void Dead()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<TurretAI>().enabled = false;
    }
}
