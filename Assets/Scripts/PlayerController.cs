using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public Transform duckAIGatherPoint;

    //animation
    public GameObject duckPos;
    public GameObject duckMesh;
    Vector3 lastJumpPos;
    float distToJump = 1;
    bool jumpCooldown = false;
    bool isLasering = false;

    AIDuckManager duckManager => AIDuckManager.Instance;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        duckManager.player = this;

        duckPos.transform.parent = null;
        JumpToPosition();
    }

    // Update is called once per frame
    void Update()
    {
        playerMoveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        MovePlayer();
        RotatePlayer();
        RecallAllDucks();

        if (Vector3.Distance(transform.position, lastJumpPos) >= distToJump || 
            ( isLasering && Quaternion.Angle(transform.rotation,duckPos.transform.rotation)>20.0f ) )
        {
            if (!jumpCooldown)
            {
                JumpToPosition();
            }
        }
    }

    private void RecallAllDucks()
    {
        if (Input.GetKeyDown("e"))
        {
            duckManager.RecallAllDucks();
        }
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

    async void JumpToPosition()
    {
        lastJumpPos = transform.position;
        jumpCooldown = true;
        float airTime = 0.3f;
        float groundTime = 0.15f;
        float upTime = airTime * 0.7f;
        float dropTime = airTime - upTime;
        LeanTween.move(duckPos, transform.position, airTime);
        LeanTween.rotate(duckPos, transform.rotation.eulerAngles, airTime);

        LeanTween.moveLocalY(duckMesh, 0.6f, upTime).setEaseOutCirc();
        await Task.Delay(Mathf.RoundToInt(upTime * 1000));
        LeanTween.moveLocalY(duckMesh, 0.0f, dropTime).setEaseInCirc();
        await Task.Delay(Mathf.RoundToInt((dropTime + groundTime) * 1000));
        jumpCooldown = false;
    }
}