using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsPlayer= default;
    [SerializeField] private float sightRange;
    [SerializeField] private float cooldown;
    private bool alreadyAttacked;

    private bool playerInSightRange;


    // Update is called once per frame
    void FixedUpdate()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if(playerInSightRange) TurretAiAttack();
    }

    private void TurretAiAttack()
    {
        if(!alreadyAttacked)
        {
            Debug.Log("A turret is attacking you!!");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), cooldown);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
