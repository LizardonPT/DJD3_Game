using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpens : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsPlayer= default;
    [SerializeField] float sightRange = default;
    private Collider[] playerInSightRange;

    // Update is called once per frame
    void FixedUpdate()
    {
        playerInSightRange = Physics.OverlapSphere(transform.position, sightRange, whatIsPlayer);
        if(playerInSightRange.Length > 0)
        {
            GetComponent<Animator>().SetBool("playerInRange",true);
        }
        else
        {
            GetComponent<Animator>().SetBool("playerInRange",false);
        }
    }
}
