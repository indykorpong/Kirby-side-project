using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement: MonoBehaviour
{
    private float x;
    private float z;

    private Vector3 jumpForce;
    private Vector3 force;

    public float speed = 5f;
    public float jumpSpeed = 5f;
    public float step = 10f;

    public bool isGrounded = true;
    private bool canDoubleJump = false;

    public Animator animator;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        jumpForce = new Vector3(0, jumpSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        force = new Vector3(x, 0, z);

        transform.position += force * speed * Time.deltaTime;

        if (transform.position.y > 0.1f)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
            else
            {
                DoubleJump();
            }
        }

        SetAnimation();
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(jumpForce);
        canDoubleJump = true;
    }

    private void DoubleJump()
    {
        if (canDoubleJump)
        {
            canDoubleJump = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(jumpForce);
        }
    }

    private void SetAnimation()
    {
        Debug.Log(force.magnitude);

        // TODO: Remove double click (two clicks in a short time) to slash twice
        if (Input.GetMouseButtonDown(0))
        {
            animator.ResetTrigger("Walk");
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Slash");
        }
        else
        {
            if (force.magnitude > 0.3f)
            {
                animator.ResetTrigger("Idle");
                animator.SetTrigger("Walk");
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(force), step);
            }
            else
            {
                animator.ResetTrigger("Walk");
                animator.SetTrigger("Idle");
            }
        }
    }
}
