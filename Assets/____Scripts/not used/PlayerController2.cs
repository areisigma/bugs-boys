using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class PlayerController2 : MonoBehaviour
{
    Transform cameraHolder;
    GameObject cameraHolderObject;
    Vector3 cameraPosition;
    NetworkIdentity ni;

    public bool isGrounded;
    public GameObject player;
    public GameObject playerHolder;
    public Vector3 rotation;
    public Rigidbody rb;
    public CharacterController cc;
    
    void Start()
    {
        ni = gameObject.GetComponent<NetworkIdentity>();

        cameraHolder = player.transform.Find("cameraHolder");
        cameraHolderObject = cameraHolder.gameObject;
        cameraHolderObject.SetActive(true);
        cameraPosition = new Vector3();
        //camera = GameObject.FindGameObjectWithTag("MainCamera");
        rotation.y = -90.0f;
        rotation.x = 10.0f;
        cameraHolder.transform.Rotate(rotation);

        isGrounded = true;
    }
    
    void Update()
    {
        CameraAttach();
        isGrounded = cc.isGrounded;
    }

    void FixedUpdate()
    {
        MovePlayer();
        Jump(1);
    }

    void CameraAttach()
    {
        cameraPosition = player.transform.position; // Camera desired position
        //Debug.Log("Position of player: " + cameraPosition);

        cameraPosition.x += 1.25f;
        cameraPosition.y += 0.75f;
        cameraPosition.z += 0.0f;
        //Debug.Log("cameraPosition: " + cameraPosition);

        GetComponent<Camera>().transform.position = cameraPosition; // World to space position
        //Debug.Log("Camera position set");
    }

    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Forward(1);
        }
        if (Input.GetKey(KeyCode.S))
        {

            Forward(-1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Sideways(1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Sideways(-1);
        }
        if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            rb.velocity *= 0.9f;
        }
    }

    void Forward(int k)
    {
        rb.velocity = new Vector3(-2 * k, 0, 0);
        Debug.Log("forward");
    }
    void Sideways(int k)
    {
        rb.velocity = new Vector3(0, 0, -2 * k);
        Debug.Log("side");
    }

    void Jump(int k)
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(0, 5, 0);
            Debug.Log("jump");

            isGrounded = false;
        }
    }
}
