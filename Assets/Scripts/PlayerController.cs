using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;

    [SerializeField]
    private float gravity = -9.81f;

    [SerializeField]
    private float jumpForce;

    private CharacterController characterController;
    private Transform playerCamera;
    private Vector3 velocity;
    private Vector3 playerMoveInput;

    AIDuckManager duckManager => AIDuckManager.Instance;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        duckManager.player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        playerMoveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        Vector3 moveVector = transform.TransformDirection(playerMoveInput);

        if (characterController.isGrounded)
        {
            velocity.y = -1f;

            //Uncomment if we need jump feature in the future
            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     velocity.y = jumpForce;
            // }
        }
        else
        {
            velocity.y -= gravity * -2f * Time.deltaTime;
        }

        characterController.Move(speed * moveVector * Time.deltaTime);
        characterController.Move(velocity * Time.deltaTime);
    }

    private void RotatePlayer()
    {
        transform.forward = playerCamera.forward;
        transform.rotation = new Quaternion(0, playerCamera.rotation.y, 0, playerCamera.rotation.w);
    }
}