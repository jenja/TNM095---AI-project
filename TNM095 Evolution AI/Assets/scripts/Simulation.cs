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

		//Instantiate fishes and store them in a list (first generation)
		fishList = new List<GameObject> ();
		for (int i = 0; i < populationSize; i++) {
			fishList.Add ((GameObject)Instantiate (fish, transform.position, Quaternion.identity));
		}

		//Randomize first generations chromosomes
		List<float[]> randomDnaList = new List<float[]>();
		for (int i = 0; i < populationSize; i++) {

			//TODO Replace numbers with variables
			float newSpeed = Random.Range(1, 5);
			float newTurnAngle = Random.Range(1, 180);
			float newVisRange = Random.Range(1, 5);
			randomDnaList.Add (new float[] {newSpeed, newTurnAngle, newVisRange});
		}

		//Assign the random chromosomes to the fish
		for (int i = 0; i < populationSize; i++) {
			GameObject tempFish = fishList [i];
			tempFish.GetComponent<Fish> ().chromosome = randomDnaList [i];
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

		//Pause simulation during reproduction
		Time.timeScale = 0;

		//sort the list by the amount of food eaten
		fishList.Sort (
			delegate(GameObject x, GameObject y) {
				return y.GetComponent<Fish>().food.CompareTo(x.GetComponent<Fish>().food);
			}
		);

		//debug sorting
		/* for (int i = 0; i < populationSize; i++) {
			GameObject temp = fishList [i];
			Fish tempScript = temp.GetComponent<Fish> ();
			Debug.Log (temp + ", ate " + tempScript.food + "food");
		} */
	}
	private void createNewFish () {}
	private void spawnNewFish () {
	}
}
