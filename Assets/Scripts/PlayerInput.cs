using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rBody;
    public Transform playerCam;
    public Camera playerCamScript;
    public GameObject gun;
    public GameManager GM;
    public RawImage scopeImage;
    public GunEvents gunEvents;

    [Header("Gameplay Variables")]
    public float defaultLookSensitivity;
    public float lookSensitivity;
    public float moveSpeed;
    public float jumpSpeed;
    public float velocityDecrement;
    public float maxLookBounds;
    public float simulatedDrag;
    public float bulletRange;
    public float scopeLerp;
    public float scopePosition;
    public float defaultFOV;
    public float zoomedFOV;
    //public float gunBobFactor;
    public bool grounded;
    public bool scoped;
    public Vector3 maxVelocity;
    private Vector3 inputVector;
    public Vector3 hipFirePosition;
    public Vector3 scopedPosition;

    [Header("Input")]
    public Vector2 lookDelta;
    public bool joystick;
    private float fireTimer;
    private float gunBobTimer;

    void Awake()
    {
        rBody = GetComponent<Rigidbody>();
        playerCam = Camera.main.transform;
        playerCamScript = Camera.main;
    }
    void Start()
    {
        if (!joystick) Cursor.lockState = CursorLockMode.Locked;
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        //Timers
        float deltaTime = Time.deltaTime;

        //Viewing input
        if (!joystick) Cursor.lockState = CursorLockMode.Locked;
        else Cursor.lockState = CursorLockMode.None;

        if (!joystick)
        {
            lookDelta = new Vector2(Input.GetAxis("Mouse X") * lookSensitivity, -Input.GetAxis("Mouse Y") * lookSensitivity);
            transform.Rotate(0, lookDelta.x, 0);
            playerCam.Rotate(lookDelta.y, 0, 0);
            inputVector = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) inputVector += new Vector3(0, 0, 1);
            if (Input.GetKey(KeyCode.A)) inputVector += new Vector3(-1, 0, 0);
            if (Input.GetKey(KeyCode.S)) inputVector += new Vector3(0, 0, -1);
            if (Input.GetKey(KeyCode.D)) inputVector += new Vector3(1, 0, 0);
            inputVector.Normalize();
            rBody.AddRelativeForce(inputVector * moveSpeed);
            rBody.velocity = new Vector3(rBody.velocity.x / simulatedDrag, rBody.velocity.y, rBody.velocity.z / simulatedDrag);

            if (Input.GetKeyDown(KeyCode.Space) && grounded) rBody.AddForce(new Vector3(0, 1, 0) * jumpSpeed);

            if (Input.GetKey(KeyCode.LeftShift)) scoped = true;
            else scoped = false;
        }
        if (joystick)
        {
            lookDelta = new Vector2(Input.GetAxisRaw("RightStick X") * lookSensitivity, Input.GetAxisRaw("RightStick Y") * lookSensitivity);
            transform.Rotate(0, lookDelta.x, 0);
            playerCam.Rotate(lookDelta.y, 0, 0);
            inputVector = Vector3.zero;
            inputVector = new Vector3(Input.GetAxis("LeftStick X"), 0, Input.GetAxis("LeftStick Y"));
            rBody.AddRelativeForce(inputVector * moveSpeed);
            rBody.velocity = new Vector3(rBody.velocity.x / simulatedDrag, rBody.velocity.y, rBody.velocity.z / simulatedDrag);

            if (Input.GetKeyDown(KeyCode.Space) && grounded) rBody.AddForce(new Vector3(0, 1, 0) * jumpSpeed);

            if (Input.GetKey(KeyCode.LeftShift)) scoped = true;
            else scoped = false;
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
}