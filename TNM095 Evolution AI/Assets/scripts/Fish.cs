using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {

	private int initialMinSpeed;
	private int initialMaxSpeed;
	private int initialMinTurnAngle;
	private int initialMaxTurnAngle;
	private int initialMinVisRange;
	private int initialMaxVisRange;

    //Attributes
    public float food { get; set; }
    public DNA dna;
    public string chromosome;

	//Constructor
	public Fish() {
        generateFish();
    }

	//Reproduction constructor
	public Fish(string motherDna, string fatherDna) {
		//TODO - generate dna string based on in parameters
	}

	//Generate random inital traits
	private void generateFish() {
        this.dna.speed = Random.Range(initialMinSpeed, initialMaxSpeed);
        this.dna.turnAngle = Random.Range(initialMinTurnAngle, initialMaxTurnAngle);
        this.dna.visabilityRange = Random.Range(initialMinVisRange, initialMaxVisRange);

        this.chromosome = "" + this.dna.speed + this.dna.turnAngle + this.dna.visabilityRange;
    }
}
