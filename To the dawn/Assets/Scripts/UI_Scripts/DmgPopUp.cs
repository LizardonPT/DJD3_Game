using UnityEngine;

public class DmgPopUp : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // THE FOLLOWING CODE WAS GIVEN BY ALL MIGHTY NELSON
        float step = 100 * Time.deltaTime;
        Vector3 camPos = Camera.main.transform.position;

        // Get the normalized direction to the target
        Vector3 directionToTarget = (camPos - transform.position);

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, directionToTarget * -1, step, 0.0f);

        // Draw a ray pointing at our target in
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
