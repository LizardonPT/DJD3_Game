using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricGun : MonoBehaviour
{
    [SerializeField] private float fireRate = 0.5f;

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
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        Debug.DrawRay(firePoint.position, ray.direction * 100, Color.yellow, 2f);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray,out hitInfo, 100))
        {
            HP hp = hitInfo.collider.gameObject.GetComponent<HP>();
            if(hp)
            {
                hp.HPModifier(damage, "electric");
            }
        }

        gameObject.GetComponent<Energy>().UpdateEnergy(useEnergyPerShoot);
    }
}
