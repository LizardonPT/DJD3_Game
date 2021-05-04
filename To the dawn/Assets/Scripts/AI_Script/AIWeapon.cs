using UnityEngine;
using DigitalRuby.LightningBolt;

public class AIWeapon : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer lineRend;
    [SerializeField] private GameObject thunder;
    [SerializeField] private string damageType;

    public void Fire(Vector3 player)
    {
        Ray ray = new Ray(gameObject.transform.position, player - gameObject.transform.position);
        RaycastHit hitInfo;

        if(damageType == "plasma")
        {
            LineRenderer plasmalr;

            //Draw line
            //LineRenderer plasmalr = plasma.GetComponent<LineRenderer>();
            if (Physics.Raycast(ray,out hitInfo, 100))
            {
                plasmalr = Instantiate(lineRend, firePoint.position, Quaternion.LookRotation(ray.direction.normalized));
                plasmalr.SetPosition(1, new Vector3(0,0,hitInfo.distance));

                HP hp = hitInfo.collider.gameObject.GetComponent<HP>();
                if(hp)
                {
                    hp.HPModifier(damage, "plasma");
                }
            }
            else{
                plasmalr = Instantiate(lineRend, firePoint.position, Quaternion.LookRotation(ray.direction));
                plasmalr.SetPosition(1, new Vector3(0,0,500));
            }

            Destroy(plasmalr.gameObject, 0.3f);
        }
        else
        {
            // Creates thunder effect
            GameObject newthunder;

            // If it hits something ...
            if (Physics.Raycast(ray,out hitInfo, 100))
            {
                newthunder = Instantiate(thunder, firePoint.position, Quaternion.LookRotation(ray.direction.normalized));
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

            Destroy(newthunder, 0.1f);
        }
    }
}
