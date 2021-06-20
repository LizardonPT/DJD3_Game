using UnityEngine;

public class SaveCollition : MonoBehaviour
{
    [SerializeField] private GameObject chip = default;
    private void OnTriggerEnter(Collider other)
    {
        chip.SetActive(false);
        SaveSystem.SavePlayer(other.gameObject);
        Destroy(gameObject);
    }
}
