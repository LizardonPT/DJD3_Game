using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsPlayer= default;
    [SerializeField] private float sightRange;
    [SerializeField] private float cooldown;
    [SerializeField] private float timer;
    private bool alreadyAttacked;
    private Collider[] playerInSightRange;
    private Vector3 player;


    // Update is called once per frame
    void FixedUpdate()
    {
        playerInSightRange = Physics.OverlapSphere(transform.position, sightRange, whatIsPlayer);

        if(playerInSightRange.Length > 0)
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
}
