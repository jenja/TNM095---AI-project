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

        Time.timeScale = 4;

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
        //InvokeRepeating("SpawnFood", 0.0f, 5.0f);
        SpawnFood();

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

		SortFishByFitness ();
        SpawnNextGen (GenerateChromosomes ());
	}

	private void SortFishByFitness () {
        //Pause simulation during reproduction
        Time.timeScale = 0;

        //sort the list by the amount of food eaten
        fishList.Sort(
            delegate (GameObject x, GameObject y) {
                return y.GetComponent<Fish>().food.CompareTo(x.GetComponent<Fish>().food);
            }
        );
    }

	//Get the poopulations total fitness
	private int GetTotalFitness() {
		int totalFitness = 0;
		for (int i = 0; i < populationSize; i++)
			totalFitness += fishList [i].GetComponent<Fish> ().food;
		return totalFitness;
	}

	private List<float[]> GenerateChromosomes() { 
        List<float[]> chromosomeList = new List<float[]>();
        for (int i = 0; i < populationSize; i++) {

            float[] fishDad = calcParent();
            float[] fishMom = calcParent();
            float[] fishKid = fishDad;

            Debug.Log("fishDad: " + fishDad[0] + "," + fishDad[1] + "," + fishDad[2]);
            Debug.Log("fishMom: " + fishMom[0] + "," + fishMom[1] + "," + fishMom[2]);

            int temp = Random.Range(0, fishDad.Length - 1);

            for (int k = 0; k < fishDad.Length; k++) {

                if(k > temp){
                    fishKid[k] = fishMom[k];
                }
            }

//            Debug.Log("fishDad: " + fishDad[0] + "," + fishDad[1] + "," + fishDad[2]);
//            Debug.Log("fishMom: " + fishMom[0] + "," + fishMom[1] + "," + fishMom[2]);
//            Debug.Log("fishSon: " + fishSon[0] + "," + fishSon[1] + "," + fishSon[2]);
//            Debug.Log("fishGal: " + fishGal[0] + "," + fishGal[1] + "," + fishGal[2]);

            chromosomeList.Add(fishKid);
        }

        ClearScene();

		return chromosomeList;
    }

    private void ClearScene () {

        //Destory all GameObjects with the tag food
        GameObject[] gos = GameObject.FindGameObjectsWithTag("food");
        foreach (GameObject go in gos)
            Destroy(go);
        
        //Destroy all fish
        fishList.ForEach(delegate(GameObject fish){
            Destroy(fish);
        });

        //Clear fishList
        fishList.Clear();

    }

	//Apply the new chromosomes and spawn the new generation
	private void SpawnNextGen (List<float[]> chromosomeList) {
		for (int i = 0; i < populationSize; i++) {
			fishList.Add ((GameObject)Instantiate (fish, transform.position, Quaternion.identity));
			fishList [i].GetComponent<Fish> ().chromosome = chromosomeList [i];
		}
		Time.timeScale = 4; //resume simmulation

        SpawnFood();
    }

    private float[] calcParent(){

        float randomSeed = Random.Range(0, GetTotalFitness());
        float counter = 0;

        for(int i = 0; i < populationSize; i++) {
            counter += fishList[i].GetComponent<Fish>().food;
            
            if(randomSeed > counter) {
                continue;
            }
            else {
                return fishList[i].GetComponent<Fish>().chromosome;
            }
        }

        Debug.Log("Error incest");

        return null;
    }
}
