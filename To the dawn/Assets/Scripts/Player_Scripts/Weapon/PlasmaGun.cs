using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaGun : MonoBehaviour
{
    [SerializeField] private float fireRate = 1f;

    [SerializeField] private int damage = 3;
    [SerializeField] private int useEnergyPerShoot = 2;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer lineRend;

    private float timer;

    void Start()
    {

    }

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
        Debug.DrawRay(firePoint.position, ray.direction * 100, Color.blue, 2f);
        Vector3 direction = (ray.direction * 100 - firePoint.position).normalized;
        RaycastHit hitInfo;

        LineRenderer plasmalr = Instantiate(lineRend, firePoint.position, Quaternion.LookRotation(direction));
        //Draw line
        //LineRenderer plasmalr = plasma.GetComponent<LineRenderer>();
        if (Physics.Raycast(ray,out hitInfo, 100))
        {
            plasmalr.SetPosition(1, new Vector3(0,0,hitInfo.distance));
            HP hp = hitInfo.collider.gameObject.GetComponent<HP>();
            if(hp)
            {
                hp.HPModifier(damage, "plasma");
            }
        }
        else{
            plasmalr.SetPosition(1, new Vector3(0,0,500));
        }
        Destroy(plasmalr.gameObject, 0.3f);
        gameObject.GetComponent<Energy>().UpdateEnergy(useEnergyPerShoot);
    }
}
