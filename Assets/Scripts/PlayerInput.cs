﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerInput : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rBody;
    public Transform playerCam;
    public Transform visualFirePoint;
    public Transform firePoint;
    public Transform aimPoint;
    public GameObject bullet;
    public GameObject bulletSplash;
    public GameManager GM;

    [Header("Gameplay Variables")]
    public float lookSensitivity;
    public float moveSpeed;
    public float jumpSpeed;
    public float velocityDecrement;
    public float maxLookBounds;
    public float simulatedDrag;
    public float bulletRange;
    public bool grounded;
    public Vector3 maxVelocity;
    private Vector3 inputVector;

    [Header("Input")]
    public Vector2 mouseDelta;
    private float fireTimer;
    public float fireRate;
    public bool autoFire;

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
        //Timer updates
        fireTimer += Time.deltaTime;

        //Viewing input
        mouseDelta = new Vector2(Input.GetAxis("Mouse X") * lookSensitivity, -Input.GetAxis("Mouse Y") * lookSensitivity);
        transform.Rotate(0, mouseDelta.x, 0);
        playerCam.Rotate(mouseDelta.y, 0, 0);
        inputVector = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) inputVector += new Vector3(0, 0, 1);
        if (Input.GetKey(KeyCode.A)) inputVector += new Vector3(-1, 0, 0);
        if (Input.GetKey(KeyCode.S)) inputVector += new Vector3(0, 0, -1);
        if (Input.GetKey(KeyCode.D)) inputVector += new Vector3(1, 0, 0);
        inputVector.Normalize();
        rBody.AddRelativeForce(inputVector * moveSpeed);
        rBody.velocity = new Vector3(rBody.velocity.x / simulatedDrag, rBody.velocity.y, rBody.velocity.z / simulatedDrag);

        if (Input.GetKeyDown(KeyCode.Space) && grounded) rBody.AddForce(new Vector3(0, 1, 0) * jumpSpeed);

        if((autoFire ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0)) && fireTimer >= fireRate)
        {
            fireTimer = 0;
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
        if (GM.clip > 0)
        {
            GM.clip--;
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
}