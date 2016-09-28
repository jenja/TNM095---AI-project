using UnityEngine;
using System.Collections;

public class movingObject : MonoBehaviour {

	public int speed;
	public int turnAngle;
	public int visabilityRange;

	private Vector2 forwardDirection;

	// Use this for initialization
	void Start () {

		//Randomize inital forward direction
		float x = Random.Range(-1f, 1f);
		float y = Random.Range(-1f, 1f);
		forwardDirection = new Vector2(x, y);
	}
	
	// Update is called once per frame
	void Update () {

		//Move individual forward with constant speed
		transform.Translate(forwardDirection * Time.deltaTime * speed);
	}
}
