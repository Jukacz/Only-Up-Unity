using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
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

    // get collider

    public Collider coll;


    private readonly int sprintSpeed = 10;
    private readonly int speed = 3;


    private float previousZ = 0;
    private float previousX = 0;


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

    // Update is called once per frame

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            this.canJump = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            this.canJump = false;
        }
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
            transform.Translate((isSprinting ? this.sprintSpeed : this.speed) * Time.deltaTime * Vector3.forward);
            anim.SetFloat("SpeedOfWalking", this.sprinting ? 2 : 1);
        }
    }

    private void GoBackwards()
    {
        if ((Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.DownArrow)))
        {
            transform.Translate(speed * Time.deltaTime * Vector3.back);
            anim.SetFloat("SpeedOfWalking", -1);
        }
    }

    private void GoLeft()
    {
        if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftArrow)))
        {
            transform.Translate(Vector3.left * Time.deltaTime * this.speed);
            anim.SetInteger("Direction", -1);
        }
    }

    private void GoRight()
    {
        if ((Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.RightArrow)))
        {
            transform.Translate(this.speed * Time.deltaTime * Vector3.right);
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