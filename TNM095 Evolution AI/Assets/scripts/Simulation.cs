using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Simulation : MonoBehaviour {

	public int populationSize;
    public int foodAmount;
	public GameObject fish;
    public GameObject food;

	private List<GameObject> fishList;

	// Use this for initialization
	void Start () {

		//Instantiate fishes and store them in a list
		fishList = new List<GameObject> ();
		for (int i = 0; i < populationSize; i++) {
			fishList.Add ((GameObject)Instantiate (fish, transform.position, Quaternion.identity));
		}

        //Spawn food in a set interval
        InvokeRepeating("SpawnFood", 0.0f, 5.0f);
	}

    //Spawn a certain amount of food
    void SpawnFood() {
		for (int i = 0; i < foodAmount; i++){
            //Randomize position
            Vector3 position = new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(-5.0f, 5.0f), 0);
            Instantiate(food, position, Quaternion.identity);
        }
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

}
