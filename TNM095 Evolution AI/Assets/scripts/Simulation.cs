using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Simulation : MonoBehaviour {

	public int populationSize;
    public int foodAmount;
	public int mutatonChanse;
	public float mutationMagnitude;
	public float generationTime;
	public GameObject fish;
    public GameObject food;

	private List<GameObject> fishList;
    public List<Color> colorList;
	private float timer;
    private int generation = 1;

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
			float newSpeed = Random.Range(1.0f, 5.0f);
			float newTurnAngle = Random.Range(1.0f, 180.0f);
			float newVisRange = Random.Range(1.0f, 5.0f);
            float newSize = Random.Range(1.0f, 3.0f);

			randomDnaList.Add (new float[] {newSpeed, newTurnAngle, newVisRange, newSize});

			//Randomize color
            Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);

            //Assign the random chromosomes to the fish
            GameObject tempFish = fishList[i];
            tempFish.GetComponent<Fish>().chromosome = randomDnaList[i];
            tempFish.GetComponent<Fish>().color = newColor;
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

        generation++;
        Debug.Log("GENERATION: " + generation);
	}

	private void SortFishByFitness () {
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

            int temp = Random.Range(0, fishDad.Length - 1);

            for (int k = 0; k < fishDad.Length; k++) {

                if(k > temp){
                    fishKid[k] = fishMom[k];
                }
            }

            chromosomeList.Add(fishKid);
        }

        ClearScene();
		return chromosomeList;
    }

	//Clear scene of gameobjects and clear lists
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
			fishList[i].GetComponent<Fish>().chromosome = chromosomeList [i];
            fishList[i].GetComponent<Fish>().color = colorList[i + 1];
        }

        colorList.Clear();

		//Mutate
		Mutate();
        
		SpawnFood();
    }

	//Get a random parent based on fitness
    private float[] calcParent(){

        float randomSeed = Random.Range(0, GetTotalFitness());
        float counter = 0;

        for(int i = 0; i < populationSize; i++) {
            counter += fishList[i].GetComponent<Fish>().food;
            
            if(randomSeed > counter) {
                continue;
            }
            else {
                colorList.Add(fishList[i].GetComponent<Fish>().color);
                return fishList[i].GetComponent<Fish>().chromosome;
            }
        }

        Debug.Log("Error incest");
        return null;
    }

	private void Mutate() {

		for (int i = 0; i < populationSize; i++) {

			float[] currentChromosome = fishList [i].GetComponent<Fish> ().chromosome;
			for (int k = 0; k < currentChromosome.Length; k++) {

				if (Random.Range (0, 100) <= mutatonChanse) {
					
					float mutateAmount = mutationMagnitude * currentChromosome [k];
					currentChromosome[k] += Random.Range (-mutateAmount, mutateAmount);

					Debug.Log ("SPAWN X-MEN: k = " + k + ", new value = " + currentChromosome [k]);
				}
				else
					continue;
			}
		}
	}

}
