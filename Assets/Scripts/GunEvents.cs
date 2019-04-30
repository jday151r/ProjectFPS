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
    public PlayerInput player;
    public MeshRenderer scopeLens;
    public Camera playerCamScript;

    public float bulletRange;

    [Header("Gameplay Variables")]
    public int clip;
    public int clipSize;
    public int ammo;
    public bool canShoot;
    public bool autoFire;
    public float fireTimer;
    public float fireRate;
    public float gunBobTimer;
    public float gunBobFactor;
    public float defaultFOV;
    public float zoomedFOV;
    public Vector3 hipFirePosition;
    public Vector3 scopedPosition;
    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        playerCamScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>().playerCamScript;
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

        if (!player.joystick ?
        (autoFire ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0)) && canShoot && clip > 0 && fireTimer >= fireRate :
        (autoFire ? Input.GetAxis("RightTrigger") > 0 : Input.GetAxis("RightTrigger") > 0) && canShoot && clip > 0 && fireTimer >= fireRate)
        {
            fireTimer = 0; //Reset the fire timer
            if (!player.scoped) animator.Play("Shoot", 0, 0f);
            else Shoot();
        }

        //Gun bobs up and down - slow if idle, fast if moving
        //if (!player.scoped && player.grounded)
        //{
        //    gunBobTimer += deltaTime * (player.rBody.velocity.magnitude + 1);
        //    transform.localPosition += new Vector3(0, (Mathf.Sin(gunBobTimer * Mathf.PI) / gunBobFactor) * deltaTime, 0);
        //}
        if (player.scoped && canShoot)
        {
            player.lookSensitivity = player.defaultLookSensitivity * (zoomedFOV / defaultFOV);
            transform.localPosition = Vector3.Lerp(transform.localPosition, scopedPosition, player.scopeLerp);
            playerCamScript.fieldOfView = Mathf.Lerp(playerCamScript.fieldOfView, zoomedFOV, player.scopeLerp);
            Color scopeColor = player.scopeImage.color;
            Color lensColor = scopeLens.material.color;
            player.scopeImage.color = new Color(scopeColor.r, scopeColor.g, scopeColor.b, Mathf.Lerp(player.scopeImage.color.a, 1, player.scopeLerp / 2));
            scopeLens.material.color = new Color(lensColor.r, lensColor.g, lensColor.b, Mathf.Lerp(scopeLens.material.color.a, 0, player.scopeLerp / 2));
        }
        else
        {
            player.lookSensitivity = player.defaultLookSensitivity;
            transform.localPosition = Vector3.Lerp(transform.localPosition, hipFirePosition, player.scopeLerp);
            playerCamScript.fieldOfView = Mathf.Lerp(playerCamScript.fieldOfView, defaultFOV, player.scopeLerp);
            Color scopeColor = player.scopeImage.color;
            Color lensColor = scopeLens.material.color;
            player.scopeImage.color = new Color(scopeColor.r, scopeColor.g, scopeColor.b, Mathf.Lerp(player.scopeImage.color.a, 0, player.scopeLerp / 2));
            scopeLens.material.color = new Color(lensColor.r, lensColor.g, lensColor.b, Mathf.Lerp(scopeLens.material.color.a, 1, player.scopeLerp / 2));
        }
        if (!player.scoped)
        {
            gunBobTimer += deltaTime * (!player.grounded ? 0.1f : (player.rBody.velocity.magnitude + 1));
            transform.localPosition += new Vector3(0, (Mathf.Sin(gunBobTimer * Mathf.PI) / gunBobFactor) * deltaTime, 0);
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
