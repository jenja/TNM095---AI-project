using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

	public float xBoundry = 0f;
	public float yBoundry = 0f;

	// Use this for initialization
	void Start () {
		float camHight = Camera.main.orthographicSize * 2.0f;
		float camWidth = camHight * Screen.width / Screen.height;

		yBoundry = camHight / 2.0f;
		xBoundry = camWidth / 2.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
