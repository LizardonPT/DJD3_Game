using UnityEngine;
using DigitalRuby.LightningBolt;

public class ElectricGun : MonoBehaviour
{
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private GameObject thunder;
    [SerializeField] private int damage = 2;
    [SerializeField] private int useEnergyPerShoot = 1;
    [SerializeField] private Transform firePoint;

    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer>= fireRate && Input.GetButton("Fire1"))
        {
            timer = 0f;
            FireGun();
        }
    }

    private void FireGun()
    {
        // Create ray from the camera, with the directiom from the gun to 
        // the end of the ray
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        Vector3 direction;
        RaycastHit hitInfo;

        // Creates thunder effect
        GameObject newthunder;

        // If it hits something ...
        if (Physics.Raycast(ray,out hitInfo, 100))
        {
            direction = (hitInfo.point - firePoint.position).normalized;

            newthunder = Instantiate(thunder, firePoint.position, Quaternion.LookRotation(direction));
            newthunder.GetComponent<LightningBoltScript>().StartPosition = firePoint.position;
            newthunder.GetComponent<LightningBoltScript>().EndPosition = hitInfo.point;

            HP hp = hitInfo.collider.gameObject.GetComponent<HP>();
            if(hp)
            {
                hp.HPModifier(damage, "electric");
            }
        }
        // If it does not ...
        else
        {
            newthunder = Instantiate(thunder, firePoint.position, Quaternion.LookRotation(ray.direction));
            newthunder.GetComponent<LightningBoltScript>().StartPosition = firePoint.position;
            newthunder.GetComponent<LightningBoltScript>().EndPosition = ray.direction * 100;
        }

        // Removes thunder from the game
        Destroy(newthunder, 0.1f);

        // Removes Energy
        gameObject.GetComponent<Energy>().UpdateEnergy(useEnergyPerShoot);
    }
}
