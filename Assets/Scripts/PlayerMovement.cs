using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 10f;
    private bool sprinting = false;

    private bool canJump = false;

    [SerializeField] public Rigidbody rb;
    [SerializeField] public Animator anim;
    [SerializeField] public GameObject LegsCheckerObject;

    [SerializeField] private int speed = 100;

    private float previousZ = 0;
    private float previousX = 0;

    private LegsBehaviour _legsBehaviour;
    
    private bool _stickingGround;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        
        _legsBehaviour = LegsCheckerObject.GetComponent<LegsBehaviour>();
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

    private void GoForward(bool isSprinting)
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
            if (!_stickingGround)
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



    private void Update()
    {
        bool isSprinting = this.sprinting;
        float currentX = transform.position.x;
        float currentZ = transform.position.z;

        bool isMoving = currentZ != this.previousZ || currentX != this.previousX;

        if (isMoving)
        {
            this.previousZ = currentZ;
            this.previousX = currentX;
        };

        anim.SetBool("Moving", isMoving);
        
        this._stickingGround = _legsBehaviour.StickingGround;
        
        if (_stickingGround)
        {
            this.canJump = true;
            anim.SetTrigger("NotFlying");
            // anim.ResetTrigger("Flying");
        } else
        {
            Debug.Log("Not sticking ground");
            this.canJump = false;
            // anim.SetTrigger("Flying");
            anim.ResetTrigger("NotFlying");
        }

        Sprint();
        RotateCharacterUsingMouse();
        GoForward(isSprinting);
        GoBackwards();
        GoLeft();
        GoRight();
        Jump();

        CheckIfIsGoingLeftOrRight();
        CheckIfIsGoingForwardOrBackward();

    }
}