using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {

    //Attributes
    public float food { get; set; }
    public DNA dna;
    public string chromosome;

    private int initialMinSpeed = 0;
	private int initialMaxSpeed = 100;
	private int initialMinTurnAngle = 0;
	private int initialMaxTurnAngle = 360;
	private int initialMinVisRange = 0;
	private int initialMaxVisRange = 10;

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

        this.food = 0;

        this.dna.speed = Random.Range(initialMinSpeed, initialMaxSpeed);
        this.dna.turnAngle = Random.Range(initialMinTurnAngle, initialMaxTurnAngle);
        this.dna.visabilityRange = Random.Range(initialMinVisRange, initialMaxVisRange);

        this.chromosome = "" + this.dna.speed + this.dna.turnAngle + this.dna.visabilityRange;
    }
}
