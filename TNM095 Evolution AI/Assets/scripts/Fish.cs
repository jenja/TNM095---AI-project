using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {

    //Attributes
    public float food { get; set; }
	public string chromosome;

    private float initialMinSpeed = 0.5f;
	private float initialMaxSpeed = 1.5f;
	private float initialMinTurnAngle = 5f;
	private float initialMaxTurnAngle = 100f;
	private int initialMinVisRange = 0;
	private int initialMaxVisRange = 10;

	private float speed;// = 1;
	private float turnAngle;// = 30;
	private float visabilityRange;// = 10;

	private Vector2 forwardDirection;

	// Use this for initialization
	void Start () {

		this.food = 0;

		//Give the fish random values
		speed = Random.Range(initialMinSpeed, initialMaxSpeed);
		turnAngle = Random.Range(initialMinTurnAngle, initialMaxTurnAngle);
		visabilityRange = Random.Range(initialMinVisRange, initialMaxVisRange);

		chromosome = "" + speed + turnAngle + visabilityRange;

		//initiate forward direction
		forwardDirection = new Vector2(1, 1);
		forwardDirection.Normalize ();
	}

	// Update is called once per frame
	void Update () {

		//Update forward direction with a random angle within the units maximum turnAngle
		float randomAngle = Random.Range(-turnAngle, turnAngle);
		forwardDirection = Quaternion.Euler (0, 0, randomAngle) * forwardDirection;
		forwardDirection.Normalize();

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
	}

    //Check collision 
    void OnTriggerEnter2D(Collider2D coll){
        if(coll.gameObject.tag == "food") {
            Destroy(coll.gameObject);
        }
        this.food = this.food + 1;
    }
}
