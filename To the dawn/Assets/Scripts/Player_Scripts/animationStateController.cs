using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
        {
            animator.SetBool("isRunning", true);
        }
        if (animator.GetBool("isRunning") && !Input.GetButton("Horizontal") && !Input.GetButton("Vertical"))
        {
            animator.SetBool("isRunning", false);
        }
    }
}
