using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("References")]
    public Camera playerCam;
    private Rigidbody rBody;

    [Header("Gameplay Variables")]
    public float lookSensitivity;
    public float moveSpeed;
    public float jumpSpeed;
    public float velocityDecrement;
    public bool grounded;
    public Vector3 maxVelocity;

    private Vector3 velocity;

    [Header("Input")]
    public Vector2 mouseDelta;

    void Awake()
    {
        rBody = GetComponent<Rigidbody>();

    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Viewing input
        mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X") * lookSensitivity, -Input.GetAxisRaw("Mouse Y") * lookSensitivity);
        transform.Rotate(0, mouseDelta.x, 0);
        playerCam.transform.Rotate(mouseDelta.y, 0, 0);

        velocity /= velocityDecrement;
        if (Input.GetKey(KeyCode.W) && velocity.z < maxVelocity.z) velocity += (new Vector3(0, 0, 1) * moveSpeed);
        if (Input.GetKey(KeyCode.A) && velocity.x > -maxVelocity.x) velocity += (new Vector3(-1, 0, 0) * moveSpeed);
        if (Input.GetKey(KeyCode.S) && velocity.z > -maxVelocity.z) velocity += (new Vector3(0, 0, -1) * moveSpeed);
        if (Input.GetKey(KeyCode.D) && velocity.x < maxVelocity.x) velocity += (new Vector3(1, 0, 0) * moveSpeed);
        transform.Translate(velocity);
        //Need to lerp desired velocity to current velocity to prevent jittering when moving/jumping at the same time.

        if (Input.GetKeyDown(KeyCode.Space) && grounded) rBody.AddForce(new Vector3(0, 1, 0) * jumpSpeed);
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
}