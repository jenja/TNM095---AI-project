using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

	public float simulationSpeed { get; set;}

	void Start() {
		simulationSpeed = 1;
	}

	void Update() {

		Time.timeScale = simulationSpeed;
	}
}
