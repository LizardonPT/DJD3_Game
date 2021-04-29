using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaGun : MonoBehaviour
{
    [SerializeField] private float fireRate = 1f;

    [SerializeField] private int damage = 3;

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
        Debug.DrawRay(firePoint.position, ray.direction * 100, Color.blue, 2f);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray,out hitInfo, 100))
        {
            Destroy(hitInfo.collider.gameObject);
        }
    }
}
