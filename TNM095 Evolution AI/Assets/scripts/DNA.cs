using UnityEngine;
using System.Collections;

public class DNA : MonoBehaviour {

    //Attributes
    public float speed { get; set; }
    public float turnAngle { get; set; }
    public float visabilityRange { get; set; }

    //Constructor
    public DNA() {
        speed = 0;
        turnAngle = 0;
        visabilityRange = 0;
    }
}
