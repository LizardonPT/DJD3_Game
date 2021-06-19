using UnityEngine;
using Cinemachine;

public class CMCameraPriority : MonoBehaviour
{
    //[SerializeField] 
    //private CinemachineFreeLook mainCamera;
    [SerializeField] 
    private CinemachineFreeLook aimCamera = default;

    public bool playerAim;

    // Start is called before the first frame update
    void Start()
    {
        playerAim = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1)) // Toggle Aim
        {
            if(aimCamera.Priority == 0) // Aim on
            { 
                aimCamera.Priority = 3;
                playerAim = true;
            }
            else if (aimCamera.Priority == 3) // Aim off
            {
                aimCamera.Priority = 0;
                playerAim = false;
            }
        }
    }
    public void InterruptAim()
    {
        aimCamera.Priority = 0;
        playerAim = false;
    }
}
