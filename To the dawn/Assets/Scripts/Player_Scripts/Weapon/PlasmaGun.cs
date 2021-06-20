using UnityEngine;

public class PlasmaGun : MonoBehaviour
{
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private int damage = 3;
    [SerializeField] private int useEnergyPerShoot = 2;
    [SerializeField] private Transform firePoint = default;
    [SerializeField] private LineRenderer lineRend = default;
    [SerializeField] private AudioClip plasmaSound = default;
    private AudioSource myAudio;
    private float timer;

    private void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireRate && Input.GetButton("Fire1"))
        {
            timer = 0f;
            FireGun();
        }
    }

    private void FireGun()
    {
        if(gameObject.GetComponent<Energy>().energy - useEnergyPerShoot > 0)
        {
            Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
            RaycastHit hitInfo;
            Vector3 direction;
            LineRenderer plasmalr;

            //Plays Electric gun sound
            myAudio.clip = plasmaSound;
            myAudio.Play();

            //Draw line
            //LineRenderer plasmalr = plasma.GetComponent<LineRenderer>();
            if (Physics.Raycast(ray,out hitInfo, 100,~(1 << LayerMask.NameToLayer("isPlayer"))))
            {
                direction = (hitInfo.point - firePoint.position).normalized;
                plasmalr = Instantiate(lineRend, firePoint.position, Quaternion.LookRotation(direction));
                plasmalr.SetPosition(1, new Vector3(0,0,hitInfo.distance));

                HP hp = hitInfo.collider.gameObject.GetComponent<HP>();
                if(hp)
                {
                    hp.HPModifier(damage, "plasma");
                }
            }
            else{
                plasmalr = Instantiate(lineRend, firePoint.position, Quaternion.LookRotation(ray.direction));
                plasmalr.SetPosition(1, new Vector3(0,0,100));
            }

            Destroy(plasmalr.gameObject, 0.3f);
            gameObject.GetComponent<Energy>().UpdateEnergy(useEnergyPerShoot);
        }
    }
}
