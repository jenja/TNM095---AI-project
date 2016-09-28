using UnityEngine;
using System.Collections;

public class DNA : MonoBehaviour {

    //Attributes
    public float speed { get; set; }
    public float turnAngle { get; set; }
    public float visRange { get; set; }

    //Constructor
    public DNA() {
        speed = 0;
        turnAngle = 0;
        visRange = 0;
    }
}
