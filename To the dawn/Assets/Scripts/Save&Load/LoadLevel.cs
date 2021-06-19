using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    void Awake()
    {
        GameObject loader = GameObject.Find("Load");
        if(loader.GetComponent<LoadingGame>().loadIsOn)
        {
            PlayerData data = SaveSystem.LoadPlayer();

            gameObject.GetComponent<ThirdPersonMovement>().enabled = false;

            gameObject.GetComponent<HP>().hp = data.health;

            Vector3 position;
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];
            gameObject.transform.position = position;

            gameObject.GetComponent<ThirdPersonMovement>().enabled = true;
        }
    }
}
