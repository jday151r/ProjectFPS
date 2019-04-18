using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rBody;
    public Transform playerCam;
    public Transform firePoint;
    public Transform aimPoint;
    public GameObject bullet;
    public GameManager GM;

    [Header("Gameplay Variables")]
    public float lookSensitivity;
    public float moveSpeed;
    public float jumpSpeed;
    public float velocityDecrement;
    public float maxLookBounds;
    public float simulatedDrag;
    public bool grounded;
    public Vector3 maxVelocity;

    private Vector3 velocity;

    [Header("Input")]
    public Vector2 mouseDelta;

    void Awake()
    {
        rBody = GetComponent<Rigidbody>();
        playerCam = Camera.main.transform;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        //Viewing input
        mouseDelta = new Vector2(Input.GetAxis("Mouse X") * lookSensitivity, -Input.GetAxis("Mouse Y") * lookSensitivity);
        transform.Rotate(0, mouseDelta.x, 0);
        playerCam.Rotate(mouseDelta.y, 0, 0);
        if (Input.GetKey(KeyCode.W)) rBody.AddRelativeForce(new Vector3(0, 0, 1) * moveSpeed);
        if (Input.GetKey(KeyCode.A)) rBody.AddRelativeForce(new Vector3(-1, 0, 0) * moveSpeed);
        if (Input.GetKey(KeyCode.S)) rBody.AddRelativeForce(new Vector3(0, 0, -1) * moveSpeed);
        if (Input.GetKey(KeyCode.D)) rBody.AddRelativeForce(new Vector3(1, 0, 0) * moveSpeed);
        rBody.velocity = new Vector3(rBody.velocity.x / simulatedDrag, rBody.velocity.y, rBody.velocity.z / simulatedDrag);
        //Legacy movement code
        /*
        //moveSpeed = 0.025f in editor
        velocity /= velocityDecrement;
        if (Input.GetKey(KeyCode.W) && velocity.z < maxVelocity.z) velocity += (new Vector3(0, 0, 1) * moveSpeed);
        if (Input.GetKey(KeyCode.A) && velocity.x > -maxVelocity.x) velocity += (new Vector3(-1, 0, 0) * moveSpeed);
        if (Input.GetKey(KeyCode.S) && velocity.z > -maxVelocity.z) velocity += (new Vector3(0, 0, -1) * moveSpeed);
        if (Input.GetKey(KeyCode.D) && velocity.x < maxVelocity.x) velocity += (new Vector3(1, 0, 0) * moveSpeed);
        transform.Translate(velocity);
        //Need to lerp desired velocity to current velocity to prevent jittering when moving/jumping at the same time.
        */

        if (Input.GetKeyDown(KeyCode.Space) && grounded) rBody.AddForce(new Vector3(0, 1, 0) * jumpSpeed);
        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")) grounded = true;
    }
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")) grounded = true;
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")) grounded = false;
    }
    void Shoot()
    {
        GM.clip--;
        Ray bulletTrajectory = new Ray(firePoint.position, aimPoint.position - transform.position);
        //Debug.DrawRay(firePoint.position, aimPoint.position - firePoint.position, Color.red, 0.1f);
        GameObject bul = Instantiate(bullet, firePoint.position, Quaternion.identity);
        bul.GetComponent<BulletBehavior>().target = aimPoint.position;
    }
}