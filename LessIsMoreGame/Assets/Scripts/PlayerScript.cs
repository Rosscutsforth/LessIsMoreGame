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

    //Declaring variables for player movement
    private Vector3 moveAmount;

    //Declaring variables for player jumping
    public float jumpForce;
    public float distanceToGround = 0.1f;

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody>();

        capsu = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveAmount = moveInput.normalized * speed;

        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        playerRB.MovePosition(playerRB.position + moveAmount * Time.fixedDeltaTime);
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
