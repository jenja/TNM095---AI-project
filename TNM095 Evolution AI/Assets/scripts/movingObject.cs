using UnityEngine;
using System.Collections;

public class movingObject : MonoBehaviour {

	public int speed;
	public int turnAngle;
	public int visabilityRange;

	private Vector2 forwardDirection;
	private int foodCounter = 0;

	// Use this for initialization
	void Start () {

		//Randomize inital forward direction
//		float x = Random.Range(-1f, 1f);
//		float y = Random.Range(-1f, 1f);
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
	}
}