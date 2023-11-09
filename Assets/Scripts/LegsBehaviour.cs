using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LegsBehaviour : MonoBehaviour
{

    public bool StickingGround { get; private set; }
    
    [SerializeField] private GameObject leftLeg;
    [SerializeField] private GameObject rightLeg;
    
    private LeftLegBehaviour _leftLegScript;
    private RightLegBehaviour _rightLegScript;

    void Start()
    {
        _leftLegScript = leftLeg.GetComponent<LeftLegBehaviour>();
        _rightLegScript = rightLeg.GetComponent<RightLegBehaviour>();
    }
    
    void Update()
    {
        this.StickingGround = _leftLegScript.StickingGround || _rightLegScript.StickingGround;
        
        Debug.Log("StickingGround: " + StickingGround);
        
        Debug.Log("Left Leg sticking " + _leftLegScript.StickingGround);
        Debug.Log("Right Leg sticking " + _rightLegScript.StickingGround);

        if (StickingGround)
        {
            Debug.Log("Siema");
        }
        else
        {
            Debug.Log("Nie śiema");
        }
    }

}
