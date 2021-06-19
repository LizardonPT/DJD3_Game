using UnityEngine;

public class SaveCollition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SaveSystem.SavePlayer(other.gameObject);
        Destroy(gameObject);
    }
}
