using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {

	public int initialMinSpeed;
	public int initialMaxSpeed;
	public int initialMinTurnAngle;
	public int initialMaxTurnAngle;
	public int initialMinVisRange;
	public int initialMaxVisRange;

	private int speed;
	private int turnAngle;
	private int visabilityRange;

	private Dna dna;

	private int fitness;

	//Constructor
	public Fish() {
		generateFish ();
		this.dna = new Dna();
	}

	//Reproduction constructor
	public Fish(string motherDna, string fatherDna) {
		//TODO - generate dna string based on in parameters
	}

	//Generate random inital traits
	private void generateFish () {
		this.speed = Random.Range (initialMinSpeed, initialMaxSpeed);
		this.speed = Random.Range (initialMinTurnAngle, initialMaxTurnAngle);
		this.speed = Random.Range (initialMinVisRange, initialMaxVisRange);
	}

	//Getters

	public int getSpeed() {
		return this.speed;
	}

	public int getTurnAngle() {
		return this.turnAngle;
	}

	public int getVisabilityRange() {
		return this.visabilityRange;
	}

	public int getFitness() {
		return this.fitness;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//TODO 	create movement, create counter for fitness
	}
}
