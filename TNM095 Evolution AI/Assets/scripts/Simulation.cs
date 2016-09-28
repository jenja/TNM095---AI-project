using UnityEngine;
using System.Collections;

public class Simulation : MonoBehaviour {

	public int populationSize;
	public GameObject fish;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < populationSize; i++) {
			Instantiate (fish);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
