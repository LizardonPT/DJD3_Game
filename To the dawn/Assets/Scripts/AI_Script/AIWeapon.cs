using UnityEngine;
using DigitalRuby.LightningBolt;

public class AIWeapon : MonoBehaviour
{
    [SerializeField] private int damage = default;
    [SerializeField] private LayerMask whatIsPlayer= default;
    [SerializeField] private Transform firePoint = default;
    [SerializeField] private LineRenderer lineRend = default;
    [SerializeField] private GameObject thunder = default;
    [SerializeField] private GameObject explosionelec = default;
    [SerializeField] private GameObject explosionplasma = default;
    [SerializeField] private string damageType = default;
    private Collider[] playerInExplosion;
    
    // Do not remove its useful to destroy objects or components - this case is used.
    void Start() {}

    public void Fire(Vector3 player)
    {
        Ray ray = new Ray(firePoint.transform.position, player - firePoint.transform.position);
        RaycastHit hitInfo;

        if (damageType == "plasma")
        {
            LineRenderer plasmalr;

            //Draw line
            //LineRenderer plasmalr = plasma.GetComponent<LineRenderer>();
            if (Physics.Raycast(ray,out hitInfo, 100, LayerMask.GetMask("isPlayer", "isGround", "Default")))
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
        else if(damageType == "explosionelec")
        {
            // Creates thunder effect
            GameObject newthunder;

            // If it hits something ...
            if (Physics.Raycast(ray,out hitInfo, 100, LayerMask.GetMask("isPlayer", "isGround", "Default")))
            {
                newthunder = Instantiate(thunder, firePoint.position, Quaternion.LookRotation(ray.direction.normalized));
                newthunder.GetComponent<LightningBoltScript>().StartPosition = firePoint.position;
                newthunder.GetComponent<LightningBoltScript>().EndPosition = hitInfo.point;

                playerInExplosion = Physics.OverlapSphere(hitInfo.point, 2, whatIsPlayer);

                if(playerInExplosion.Length > 0)
                {
                    HP hp = playerInExplosion[0].gameObject.GetComponent<HP>();
                    if(hp)
                    {
                        hp.HPModifier(damage, "plasma");
                    }
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
        else if(damageType == "explosionplasma")
        {
            LineRenderer plasmalr;

            //Draw line
            //LineRenderer plasmalr = plasma.GetComponent<LineRenderer>();
            if (Physics.Raycast(ray,out hitInfo, 100, LayerMask.GetMask("isPlayer", "isGround", "Default")))
            {
                plasmalr = Instantiate(lineRend, firePoint.position, Quaternion.LookRotation(ray.direction.normalized));
                plasmalr.SetPosition(1, new Vector3(0,0,hitInfo.distance));
                playerInExplosion = Physics.OverlapSphere(hitInfo.point, 2, whatIsPlayer);

                if(playerInExplosion.Length > 0)
                {
                    HP hp = playerInExplosion[0].gameObject.GetComponent<HP>();
                    if(hp)
                    {
                        hp.HPModifier(damage, "plasma");
                    }
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
            if (Physics.Raycast(ray,out hitInfo, 100, LayerMask.GetMask("isPlayer", "isGround", "Default")))
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

    public void Fire(Vector3 player, string newDamage)
    {
        Ray ray = new Ray(gameObject.transform.position, player - gameObject.transform.position);
        RaycastHit hitInfo;

        // Creates thunder effect
        GameObject newthunder;

        // If it hits something ...
        if (Physics.Raycast(ray, out hitInfo, 100, LayerMask.GetMask("isPlayer", "isGround", "Default")))
        {
            newthunder = Instantiate(thunder, firePoint.position, Quaternion.LookRotation(ray.direction.normalized));
            newthunder.GetComponent<LightningBoltScript>().StartPosition = firePoint.position;
            newthunder.GetComponent<LightningBoltScript>().EndPosition = hitInfo.point;

            HP hp = hitInfo.collider.gameObject.GetComponent<HP>();
            if (hp)
            {
                hp.HPModifier(5, "electric");
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
