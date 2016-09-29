using UnityEngine;
using System.Collections;

public class Simulation : MonoBehaviour {

	public int populationSize;
    public int foodSize;
	public GameObject fish;
    public GameObject food;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < populationSize; i++) {
			Instantiate (fish);
		}
        //Spawn food in a set interval
        InvokeRepeating("SpawnFood", 2.0f, 3.0f);
	}

    //Spawn a certain amount of food
    void SpawnFood() {
        for (int i = 0; i < foodSize; i++){
            //Randomize position
            Vector3 position = new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(-5.0f, 5.0f), 0);
            Instantiate(food, position, Quaternion.identity);
        }
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
