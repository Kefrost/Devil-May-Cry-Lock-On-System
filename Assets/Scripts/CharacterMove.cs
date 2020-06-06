using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private Animator animator;
    public GameObject model;
    public Transform cam;
    private LockOnMechanic lockOn;


    Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        animator = this.gameObject.GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        lockOn = gameObject.GetComponent<LockOnMechanic>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        animator.SetBool("Grounded", isGrounded);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (x != 0 || z != 0)
        {
            animator.SetFloat("Speed", 1);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        Vector3 move = cam.right.normalized * x + cam.forward.normalized * z;

        //Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (move != Vector3.zero && !lockOn.IsLocked())
        {
            model.transform.rotation = Quaternion.LookRotation(move, Vector3.up);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            animator.SetTrigger("JumpTrigger");
        }


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }
}
