using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Simulation : MonoBehaviour {

	public int populationSize;
    public int foodAmount;
	public float generationTime;
	public GameObject fish;
    public GameObject food;

	private List<GameObject> fishList;
	private float timer;

	// Use this for initialization
	void Start () {

		//Instantiate fishes and store them in a list
		fishList = new List<GameObject> ();
		for (int i = 0; i < populationSize; i++) {
            
			fishList.Add ((GameObject)Instantiate (fish, transform.position, Quaternion.identity));
		}

        //Spawn food in a set interval
        InvokeRepeating("SpawnFood", 0.0f, 5.0f);

		timer = 0;
	}

    //Spawn a certain amount of food
    void SpawnFood() {
		for (int i = 0; i < foodAmount; i++){
            //Randomize position
			World worldScript = GameObject.Find("World").GetComponent<World>();
			Vector3 position = new Vector3(Random.Range(-worldScript.xBoundry, worldScript.xBoundry), Random.Range(-worldScript.yBoundry, worldScript.yBoundry), 0);
            Instantiate(food, position, Quaternion.identity);
        }
    }
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > generationTime) {
			timer = 0;
			ReproduceGeneration ();
		}
	}

	private void ReproduceGeneration () {

		sortListByFitness ();
		createNewFish ();
		spawnNewFish ();
	}

	private void sortListByFitness () {

    }

    private void createNewFish() { 
        List<string> chromosomeList = new List<string>();
        for (int i = 0; i < populationSize; i += 2) {
    
            GameObject tempDad = fishList[i];
            GameObject tempMom = fishList[i + 1];
            string fishDad = tempDad.GetComponent<Fish>().chromosome;
            string fishMom = tempMom.GetComponent<Fish>().chromosome;

            int temp = Random.Range(0, fishDad.Length);

            Debug.Log("temp " + temp);

            chromosomeList.Add(fishDad.Substring(0, temp) + fishMom.Substring(temp, fishMom.Length));
            chromosomeList.Add(fishMom.Substring(0, temp) + fishDad.Substring(temp, fishDad.Length));

            Debug.Log("List " + chromosomeList[0]);
        }
    }

	private void spawnNewFish () {

    }
}
