using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpens : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsPlayer= default;
    [SerializeField] float sightRange = default;
    private Collider[] playerInSightRange;

    /*For door sound later
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    private AudioSource myAudio;*/

    /*private void Start()
    {
        myAudio = this.GetComponent<AudioSource>();
    }*/

    // Update is called once per frame
    void FixedUpdate()
    {
        playerInSightRange = Physics.OverlapSphere(transform.position, sightRange, whatIsPlayer);
        if(playerInSightRange.Length > 0)
        {
            GetComponent<Animator>().SetBool("playerInRange",true);
            /*myAudio.clip = openSound;
            myAudio.Play();*/
        }
        else
        {
            GetComponent<Animator>().SetBool("playerInRange",false);
            /*myAudio.clip = closeSound;
            myAudio.Play();*/
        }
    }
}
