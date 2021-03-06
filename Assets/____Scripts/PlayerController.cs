using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Mirror.BugsBoys
{
    public class PlayerController : NetworkBehaviour
    {
        [Header("Components")]
        public GameObject cameraHolder;
        public GameObject cameraArm;
        public Rigidbody rb;
        NetworkTeam nt;

        [Header("Camera")]
        public float cameraYSpeed = 1;
        public float cameraXSpeed = 1;

        [Header("Movement")]
        public float speed = 2.5f;
        public float jumpForce = 50.0f;
        public KeyCode jumpKey = KeyCode.Space;
        private Vector3 playerMovementInput;
        private bool isPaused = false;

        [Header("Firing")] // For further uses; nothing from this header is actually used
        public KeyCode shootKey = KeyCode.Mouse1;
        public GameObject projectilePrefab;
        public Transform projectileMount;

        [Header("Stats")]
        [SyncVar] public int health = 4;
        
        private void Awake()
        {
            rb.freezeRotation = true;
        }

        private void Start()
        {
            if (isLocalPlayer && NetworkClient.ready)
            {
                cameraHolder.SetActive(true);
            }
        }

        void Update()
        {
            // always update health bar.
            // (SyncVar hook would only update on clients, not on server)
            //healthBar.text = new string('-', health);
            
            // Grab isPaused bool from NetworkMangerHUD to complete GUI
            isPaused = NetworkClient.isPaused;

            // movement for local player
            if (isLocalPlayer && !isPaused)
            {
                // look
                float mouseY = -Input.GetAxis("Mouse Y");
                float mouseX = Input.GetAxis("Mouse X");
                transform.Rotate(0, mouseX * cameraXSpeed, 0);
                cameraArm.transform.Rotate(mouseY * cameraYSpeed, 0, 0);
                
                // move
                playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
                MovePlayer();

                // shoot
                if (Input.GetKeyDown(shootKey))
                {
                    Debug.Log("Shoot");
                }
            }
        }

        void MovePlayer()
        {
            Vector3 MoveVector = transform.TransformDirection(playerMovementInput) * speed;
            rb.velocity = new Vector3(MoveVector.x, rb.velocity.y, MoveVector.z);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
