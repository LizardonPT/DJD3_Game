using UnityEngine;
using UnityEngine.AI;

public class ACHumanoidAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //If he is moving
        if (agent.velocity.magnitude > 0.1)
        {
            anim.SetBool("Fixed", false);
        }
        else //if he is not moving
        {
            anim.SetBool("Fixed", true);
        }
    }
}
