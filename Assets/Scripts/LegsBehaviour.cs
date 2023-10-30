using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LegsBehaviour : MonoBehaviour
{

    private bool stickingGround = false;

    public Collider Collider;

    public bool GetStickingGround()
    {
        return this.stickingGround;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            this.stickingGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            this.stickingGround = false;
        }
    }
}
