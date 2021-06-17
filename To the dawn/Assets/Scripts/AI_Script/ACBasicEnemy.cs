using UnityEngine;
using UnityEngine.AI;

public class ACBasicEnemy : MonoBehaviour
{
    [SerializeField] private GameObject avatar = default;
    private NavMeshAgent agent;
    private Animator anim;

    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = avatar.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Animation Controller
        if (agent.velocity.magnitude > 0)
        {
            anim.SetBool("Fixed", false);
        }
        else
        {
            anim.SetBool("Fixed", true);
        }
    }
}
