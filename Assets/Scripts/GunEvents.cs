using UnityEngine;

public class GunEvents : MonoBehaviour
{
    [Header("References")]
    public GameManager GM;
    public AudioSource audioSource;
    public Animator animator;
    public Transform visualFirePoint;
    public Transform firePoint;
    public Transform playerCam;
    public Transform aimPoint;
    public GameObject bullet;
    public GameObject bulletSplash;

    public float bulletRange;

    [Header("Gameplay Variables")]
    public int clip;
    public int clipSize;
    public int ammo;
    public bool canShoot;
    public bool autoFire;
    public float fireTimer;
    public float fireRate;

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        //Timers
        // TODO: Remove if deltaTime is not called more than once
        float deltaTime = Time.deltaTime;
        fireTimer += deltaTime;

        //Ensures animations do not play more than once
        animator.SetBool("Reload", false);

        if ((Input.GetKeyDown(KeyCode.R) || clip == 0) && CanReload())
            animator.SetBool("Reload", true);

        if ((autoFire ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0)) && canShoot && clip > 0 && fireTimer >= fireRate)
        {
            fireTimer = 0; //Reset the fire timer
            animator.Play("Shoot", 0, 0f);
        }

    }
    //Animator events
    void EnableShooting()
    {
        canShoot = true;
    }
    void DisableShooting()
    {
        canShoot = false;
    }
    void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    public void Shoot()
    {
        if (clip > 0)
        {
            clip--;
            RaycastHit hit;
            Ray bulletTrajectory = new Ray(firePoint.position, playerCam.transform.forward);
            if (Physics.Raycast(bulletTrajectory, out hit, bulletRange))
            {
                GameObject hitBullet = Instantiate(bullet, visualFirePoint.position, Quaternion.identity);
                hitBullet.transform.SetParent(visualFirePoint);
                hitBullet.GetComponent<BulletBehavior>().target = hit.point;
                Instantiate(bulletSplash, hit.point, Quaternion.identity);
            }
            else
            {
                GameObject missedBullet = Instantiate(bullet, visualFirePoint.position, Quaternion.identity);
                missedBullet.transform.SetParent(visualFirePoint);
                missedBullet.GetComponent<BulletBehavior>().target = aimPoint.position;
            }
        }
    }
    void Reload()
    {
        if (clip < clipSize) //If there are bullets missing from the clip
        {
            if (ammo - (clipSize - clip) >= 0) //See if we have enough ammo to reload the clip
            {
                ammo -= clipSize - clip;
                clip += clipSize - clip;
                animator.SetBool("Reload", true);
            }
            else if (ammo > 0) //Fill the clip with the rest of the ammo
            {
                clip += ammo;
                ammo = 0;
                animator.SetBool("Reload", true);
            }
        }
    }
    public bool CanReload()
    {
        //See if there are bullets missing from the clip, and if there is any ammo left
        return clip < clipSize && ammo > 0;
    }
}
