﻿using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {

    //Attributes
	public int food;
    public Color color;
	public float[] chromosome;

	private float speed;
	private float turnAngle;
	private float visabilityRange;
    private float size;
	private bool foodInRange = false;

	private Vector2 forwardDirection;

    // Use this for initialization
    void Start() {

        this.food = 0;
		//TODO move the random functionality to simulatio scripts

		//Give the fish random values
		speed = chromosome[0];
		turnAngle = chromosome[1];
		visabilityRange = chromosome[2];
        size = chromosome[3];

        this.GetComponent<SpriteRenderer>().color = color;
        this.GetComponent<Fish>().transform.localScale = new Vector3(size,size,1);

        //initiate forward direction
        forwardDirection = new Vector2(1, 1);
		forwardDirection.Normalize ();
	}

	// Update is called once per frame
	void Update () {

		//Update forward direction with a random angle within the units maximum turnAngle
		if (!foodInRange) {
			float randomAngle = Random.Range (-turnAngle, turnAngle);
			forwardDirection = Quaternion.Euler (0, 0, randomAngle) * forwardDirection;
			forwardDirection.Normalize ();
		} 

		//Target the closest food in range and turn towards it
		else {
			Vector2 foodDirection = (GetClosestObjectWithTag ("food").transform.position - transform.position) ;
			float angleToFood = Vector2.Angle(forwardDirection, foodDirection);

			if(Vector3.Cross (forwardDirection, foodDirection).z < 0)
				angleToFood = -angleToFood;

			if (angleToFood > turnAngle)
				forwardDirection = Quaternion.Euler (0, 0, turnAngle * Time.deltaTime) * forwardDirection;
			else
				forwardDirection = Quaternion.Euler (0, 0, angleToFood * Time.deltaTime) * forwardDirection;
				


//			Debug.Log ("Angle: " + angleToFood);
		}

		//Move individual forward with constant speed
		transform.Translate(forwardDirection * Time.deltaTime * speed);

		//teleport fish to other side when outstepping boundries
		GameObject world = GameObject.Find("World");
		World worldScript = world.GetComponent<World> ();

		float newX = 0;
		float newY = 0;

		if(transform.position.x > worldScript.xBoundry || transform.position.x < -worldScript.xBoundry)
			newX = -transform.position.x;
		if(transform.position.y > worldScript.yBoundry || transform.position.y < -worldScript.yBoundry)
			newY = -transform.position.y;

		if 		(newX != 0 && newY != 0)	transform.position = new Vector2 (newX, newY);
		else if (newX != 0 && newY == 0)	transform.position = new Vector2 (newX, transform.position.y);
		else if (newX == 0 && newY != 0)	transform.position = new Vector2 (transform.position.x, newY);
	
		//TEST: draw line to closest food
		//drawLineTo(GetClosestObjectWithTag("food"));

		//Check if food is in vision
		detectFood ();

		//Debug.Log ("food in range: " + foodInRange);
	}

	//Draw line between fish and gameobject (FOR DEBUGGING)
	private void drawLineTo(GameObject obj) {
		GameObject go = new GameObject ();
		LineRenderer lr = go.AddComponent<LineRenderer> ();
		lr.SetWidth (0.01f, 0.01f);

		lr.SetPosition (0, gameObject.transform.position);
		lr.SetPosition (1, obj.transform.position);
	}

	private void detectFood() {
		GameObject closestFood = GetClosestObjectWithTag ("food"); 
		if (Vector2.Distance (transform.position, closestFood.transform.position) <= visabilityRange)
			foodInRange = true;
		else
			foodInRange = false;
	}

	//Returns the closest gameobject of a specific tag
	private GameObject GetClosestObjectWithTag(string tag) {
		GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag (tag);
		GameObject closestObject = objectsWithTag[0];
	
		foreach (GameObject obj in objectsWithTag) {
			if(Vector2.Distance(transform.position, obj.transform.position) < 
				Vector2.Distance(transform.position, closestObject.transform.position)) {
				closestObject = obj;
			}
		}
		return closestObject;
	}

    //Check collision 
    void OnTriggerEnter2D(Collider2D coll){
        if(coll.gameObject.tag == "food") {
            this.food = this.food + 1;
            Destroy(coll.gameObject);
        }
    }
}
