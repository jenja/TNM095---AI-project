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

	private Vector2 forwardDirection;

	private const float SPEED_REDUCTION = 0.25f;

    // Use this for initialization
    void Start() {

        this.food = 0;
		//TODO move the random functionality to simulatio scripts

		//Give the fish random values
		speed = chromosome[0];
		turnAngle = chromosome[1];
		visabilityRange = chromosome[2];
        size = chromosome[3];

		//initiate forward direction
		forwardDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		forwardDirection.Normalize ();

        this.GetComponent<SpriteRenderer>().color = color;
        this.GetComponent<Fish>().transform.localScale += new Vector3(size,size,1);
	}

	// Update is called once per frame
	void Update () {

		//Update forward direction with a random angle within the units maximum turnAngle
		float randomAngle = Random.Range(-turnAngle, turnAngle);
		forwardDirection = Quaternion.Euler (0, 0, randomAngle) * forwardDirection;
		forwardDirection.Normalize();

		//Move individual forward with constant speed
		transform.Translate(forwardDirection * Time.deltaTime * speed * SPEED_REDUCTION);

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
		drawLineTo(GetClosestObjectWithTag("food"));
	}

	//Draw line between fish and gameobject (FOR DEBUGGING)
	private void drawLineTo(GameObject obj) {
		GameObject go = new GameObject ();
		LineRenderer lr = go.AddComponent<LineRenderer> ();
		lr.SetWidth (0.01f, 0.01f);

		lr.SetPosition (0, gameObject.transform.position);
		lr.SetPosition (1, obj.transform.position);
	}

	//Draw visability range (FOR DEBUGGING)

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
