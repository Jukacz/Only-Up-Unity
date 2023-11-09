using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 10f;
    private bool _sprinting;
    
    [SerializeField] public new Rigidbody rigidbody;
    [SerializeField] public Animator animator;
    [SerializeField] public GameObject legsCheckerObject;

    [SerializeField] public float speed = 50;

    private float _previousZ;
    private float _previousX;

    private LegsBehaviour _legsBehaviour;
    private bool _stickingGround;
    

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        
        _legsBehaviour = legsCheckerObject.GetComponent<LegsBehaviour>();
    }


    private bool IsGoingForward()
    {
        return (Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.UpArrow));
    }

    private bool IsGoingBackwards()
    {
        return (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.DownArrow));
    }

    private bool IsGoingLeft()
    {
        return (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftArrow));
    }

    private bool IsGoingRight()
    {
           return (Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.RightArrow));
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            this._sprinting = true;
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            this._sprinting = false;
        }
    }

    private void GoForward(bool isSprinting)
    {
        bool isGoingForward = IsGoingForward();

        if (isGoingForward)
        {
            if (!isSprinting)
            {
                rigidbody.AddForce(transform.forward * this.speed);
            }
            else
            {
                float sprintSpeed = this.speed * 1.5f;
                rigidbody.AddForce(transform.forward * sprintSpeed);
            }
            animator.SetFloat("SpeedOfWalking", this._sprinting ? 2 : 1);


        }
    }

    private void GoBackwards()
    {
        if ((Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.DownArrow)))
        {
            rigidbody.AddForce(-transform.forward * this.speed);
            animator.SetFloat("SpeedOfWalking", -1);
        }
    }

    private void GoLeft()
    {
        if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftArrow)))
        {
            rigidbody.AddForce(-transform.right * this.speed);
            animator.SetInteger("Direction", -1);
        }
    }

    private void GoRight()
    {
        if ((Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.RightArrow)))
        {
            rigidbody.AddForce(transform.right * this.speed);
            animator.SetInteger("Direction", 1);
        }
    }

    void CheckIfIsGoingLeftOrRight()
    {
        if ((!IsGoingLeft() && !IsGoingRight()) || (IsGoingLeft() && IsGoingRight()))
        {
            animator.SetInteger("Direction", 0);
        }
    }

    void CheckIfIsGoingForwardOrBackward()
    {
        bool isGoingBackward = IsGoingBackwards();
        bool isGoingForward = IsGoingForward();

        if ((!isGoingBackward && !isGoingForward) || (isGoingBackward && isGoingForward))
        {
            animator.SetFloat("SpeedOfWalking", 0);
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_stickingGround)
            {
                rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    void RotateCharacterUsingMouse()
    {
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
        bool isSprinting = this._sprinting;
        float currentX = transform.position.x;
        float currentZ = transform.position.z;

        /*bool isMoving = currentZ != _previousZ || currentX != _previousX;

        if (isMoving)
        {
            this._previousZ = currentZ;
            this._previousX = currentX;
        };

        animator.SetBool("Moving", isMoving);*/
        
        this._stickingGround = _legsBehaviour.StickingGround;
        
        if (_stickingGround)
        {
            animator.SetTrigger("NotFlying");
            // animator.ResetTrigger("Flying");
        } else
        {
            Debug.Log("Not sticking ground");
            // animator.SetTrigger("Flying");
            animator.ResetTrigger("NotFlying");
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