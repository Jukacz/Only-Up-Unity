using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.ShaderKeywordFilter;
using UnityEditor.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float jumpForce = 10f;
    private bool sprinting = false;

    private bool canJump = false;

    public Rigidbody rb;
    public Animator anim;

    public GameObject Legs;


    // get collider

    public Collider coll;
    [SerializeField] private int speed = 1000;

    private float previousZ = 0;
    private float previousX = 0;

    private float acceleration = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();    
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private bool IsGoingForward()
    {
        return (Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.UpArrow));
    }

    private bool isGoingBackwards()
    {
        return (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.DownArrow));
    }

    private bool isGoingLeft()
    {
        return (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftArrow));
    }

    private bool isGoingRight()
    {
           return (Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.RightArrow));
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            this.sprinting = true;
            Debug.Log("Sprinting");
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            this.sprinting = false;
        }
    }

    private void RotateModel()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -1, 0);
        }
    }

    private void GoForwoard(bool isSprinting)
    {
        bool is_going_forward = IsGoingForward();
        bool is_going_backwards = isGoingBackwards();

        if (is_going_forward)
        {
            if (!isSprinting)
            {
                rb.AddForce(transform.forward * this.speed);
            }
            else
            {
                float speed = this.speed * 1.5f;
                rb.AddForce(transform.forward * speed);
            }
            anim.SetFloat("SpeedOfWalking", this.sprinting ? 2 : 1);


        }
        Debug.Log(acceleration);
    }

    private void GoBackwards()
    {
        if ((Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.DownArrow)))
        {
            rb.AddForce(-transform.forward * this.speed);
            anim.SetFloat("SpeedOfWalking", -1);
        }
    }

    private void GoLeft()
    {
        if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftArrow)))
        {
            rb.AddForce(-transform.right * this.speed);
            anim.SetInteger("Direction", -1);
        }
    }

    private void GoRight()
    {
        if ((Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.RightArrow)))
        {
            rb.AddForce(transform.right * this.speed);
            anim.SetInteger("Direction", 1);
        }
    }

    void CheckIfIsGoingLeftOrRight()
    {
        if ((!isGoingLeft() && !isGoingRight()) || (isGoingLeft() && isGoingRight()))
        {
            anim.SetInteger("Direction", 0);
        }
    }

    void CheckIfIsGoingForwardOrBackward()
    {
        bool is_going_backward = isGoingBackwards();
        bool is_going_forward = IsGoingForward();

        if ((!is_going_backward && !is_going_forward) || (is_going_backward && is_going_forward))
        {
            anim.SetFloat("SpeedOfWalking", 0);
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (this.canJump)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    void RotateCharacterUsingMouse()
    {
        // if right mouse button is pressed, rotate character
        if (Input.GetMouseButton(1))
        {
            return;
        }

        float speedOfRotation = 1f;
        float y = transform.eulerAngles.y + speedOfRotation * Input.GetAxis("Mouse X");

        transform.eulerAngles = new Vector3(0, y, 0);
    }



    private void FixedUpdate()
    {
        bool isSprinting = this.sprinting;
        float current_x = transform.position.x;
        float current_z = transform.position.z;

        bool isMoving = current_z != this.previousZ || current_x != this.previousX;

        if (isMoving)
        {
            this.previousZ = current_z;
            this.previousX = current_x;
        };

        anim.SetBool("Moving", isMoving);
        bool stickingGround = Legs.GetComponent<LegsBehaviour>().GetStickingGround();

        if (stickingGround)
        {
            Debug.Log("Sticking ground");
            this.canJump = true;
            anim.SetTrigger("NotFlying");
            anim.ResetTrigger("Flying");
        } else
        {
            Debug.Log("Not sticking ground");
            this.canJump = false;
            anim.SetTrigger("Flying");
            anim.ResetTrigger("NotFlying");
        }

        Sprint();
        RotateCharacterUsingMouse();
        GoForwoard(isSprinting);
        GoBackwards();
        GoLeft();
        GoRight();
        Jump();

        CheckIfIsGoingLeftOrRight();
        CheckIfIsGoingForwardOrBackward();

    }
}