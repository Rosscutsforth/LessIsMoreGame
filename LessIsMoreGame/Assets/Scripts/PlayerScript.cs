using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed;

    private Rigidbody playerRB;
    private Animator playerAnim;
    public LayerMask groundLayer;
    private CapsuleCollider capsu;
    public Transform camera;

    //Declaring variables for player movement
    private Vector3 moveAmount;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    public Vector3 moveDir;

    public float counterMovement = 0.175f;

    //Declaring variables for player jumping
    public float jumpForce;
    public float distanceToGround = 0.1f;
    private bool canJump;

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody>();

        capsu = GetComponent<CapsuleCollider>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveAmount = moveInput.normalized * speed;

        if (IsGrounded() && canJump == false && Input.GetButtonDown("Jump"))
        {
            canJump = true;
        }

        //Determines angle for player orientation
        if(moveInput.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveInput.x, moveInput.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
    }

    private void FixedUpdate()
    {
        playerRB.MovePosition(playerRB.position + moveDir.normalized * speed * Time.fixedDeltaTime);

        if (canJump)
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
        }
    }

    private void movement()
    {

    }

    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(capsu.bounds.center.x, capsu.bounds.min.y,
        capsu.bounds.center.z);

        bool grounded = Physics.CheckCapsule(capsu.bounds.center, capsuleBottom, distanceToGround,
        groundLayer, QueryTriggerInteraction.Ignore);

        return grounded;
    }

}
