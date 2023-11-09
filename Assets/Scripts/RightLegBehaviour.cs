using UnityEngine;

public class RightLegBehaviour : MonoBehaviour
{
    public bool StickingGround { get; private set; }

    public Collider coll;
    
    private void Start()
    {
        coll = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            this.StickingGround = true;

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            this.StickingGround = false;
        }
    }
}
