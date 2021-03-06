﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Simulation : MonoBehaviour {

	public int populationSizeFish;
    public int populationSizeShark;

	//Initial constraints
	public float minSpeed;
	public float minTurnAngle;
	public float minVisRange;
	public float minSize;
	public float maxSpeed;
	public float maxTurnAngle;
	public float maxVisRange;
	public float maxSize;

    public int foodAmount;
	public int mutatonChanse;
	public float mutationMagnitude;
	public float generationTime;
	public GameObject fish;
    public GameObject food;
    public GameObject shark;
	public float idleTurnTime;

	public List<GameObject> fishList;
    private List<GameObject> sharkList;
    public List<Color> colorListFish;
    public List<Color> colorListShark;

    private float timer;
    private int generation = 1;
	private int foodCount;

	private List<List<List<float>>> archiveList;

	// Use this for initialization
	void Start () {

		foodCount = foodAmount;
		
        fishList = new List<GameObject> ();

		archiveList = new List<List<List<float>>>();

        World worldScript = GameObject.Find("World").GetComponent<World>();

        //Instantiate fishes and store them in a list (first generation)
        for (int i = 0; i < populationSizeFish; i++) {
            Vector3 position = new Vector3(Random.Range(-worldScript.xBoundry, worldScript.xBoundry), Random.Range(-worldScript.yBoundry, worldScript.yBoundry), 0);
            fishList.Add ((GameObject)Instantiate (fish, position, Quaternion.identity));
		}

        sharkList = new List<GameObject>();
        for (int i = 0; i < populationSizeShark; i++)
        {
            Vector3 position = new Vector3(Random.Range(-worldScript.xBoundry, worldScript.xBoundry), Random.Range(-worldScript.yBoundry, worldScript.yBoundry), 0);
            sharkList.Add((GameObject)Instantiate(shark, position, Quaternion.identity));
        }

        //Randomize first generations chromosomes
        List<float[]> randomDnaListFish = new List<float[]>();
		for (int i = 0; i < populationSizeFish; i++) {

			//TODO Replace numbers with variables

			float newSpeed = Random.Range(minSpeed, maxSpeed);
			float newTurnAngle = Random.Range(minTurnAngle, maxTurnAngle);
			float newVisRange = Random.Range(minVisRange, maxVisRange);
			float newSize = Random.Range(minSize, maxSize);

			randomDnaListFish.Add (new float[] {newSpeed, newTurnAngle, newVisRange, newSize});

			//Randomize color
            Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);

            //Assign the random chromosomes to the fish
            GameObject tempFish = fishList[i];
            tempFish.GetComponent<Fish>().chromosome = randomDnaListFish[i];
            tempFish.GetComponent<Fish>().color = newColor;
        }

        List<float[]> randomDnaListShark = new List<float[]>();
        for (int i = 0; i < populationSizeShark; i++)
        {

            //TODO Replace numbers with variables
            float newSpeed = Random.Range(minSpeed, maxSpeed);
            float newTurnAngle = Random.Range(0.01f, 180.0f);
            float newVisRange = Random.Range(0.1f, 10.0f);
            float newSize = Random.Range(minSize, maxSize - 0.5f);


            randomDnaListShark.Add(new float[] { newSpeed, newTurnAngle, newVisRange, newSize });

            //Randomize color
            Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);

            //Assign the random chromosomes to the fish
            GameObject tempShark = sharkList[i];
            tempShark.GetComponent<Shark>().chromosome = randomDnaListShark[i];
            tempShark.GetComponent<Shark>().color = newColor;
        }

        //Spawn food in a set interval
        //InvokeRepeating("SpawnFood", 0.0f, 5.0f);
        SpawnFood();

		timer = 0;
	}

	// _____ GETTERS ______

	public int getGeneration() {
		return generation;
	}

	public int getFoodCount() {
		return foodCount;
	}

    //Spawn a certain amount of food
    void SpawnFood() {
		for (int i = 0; i < foodAmount; i++) {
            //Randomize position
			World worldScript = GameObject.Find("World").GetComponent<World>();
			Vector3 position = new Vector3(Random.Range(-worldScript.xBoundry, worldScript.xBoundry), Random.Range(-worldScript.yBoundry, worldScript.yBoundry), 0);
            Instantiate(food, position, Quaternion.identity);
        }
    }

	/** Public functions **/

	public List<List<List<float>>> getArchive() {
		return this.archiveList;
	}

	public void removeFood() {
		foodCount -= 1;
	}
	
	// Update is called once per frame
	void Update () {

		//Check if fish runs out
		if(fishList.Count <= 0) {
			Debug.Log ("All fish were eaten.");
			Debug.Break ();
		}

		//Check if food runs out
		if (foodCount <= 0) {
			timer = 0;
			ReproduceGeneration ();
			foodCount = foodAmount;
		}

		//start next generation when time runs out
		timer += Time.deltaTime;
		if (timer > generationTime) {
			timer = 0;
			ReproduceGeneration ();
			foodCount = foodAmount;
		}
	}

	private void ReproduceGeneration () {

        SortFishByFitness(fishList, "fish");
        

        SortFishByFitness(sharkList, "shark");
        SpawnNextGen(GenerateChromosomes(sharkList, "shark", populationSizeShark), sharkList, "shark");

		AddStatsToArchive ();
        SpawnNextGen(GenerateChromosomes(fishList, "fish", populationSizeFish), fishList, "fish");
        GameObject.Find ("UI Controller").GetComponent<UI> ().addGenerationToDropDown (generation);

        generation++;
		GameObject.Find ("UiGenText").GetComponent<Text> ().text = "Gen " + generation;
	}

	private void SortFishByFitness(List<GameObject> speciesList, string species) {
        //sort the list by the amount of food eaten
        switch (species)
        {
            case "fish":
                speciesList.Sort(
                    delegate (GameObject x, GameObject y) {
                        return y.GetComponent<Fish>().food.CompareTo(x.GetComponent<Fish>().food);
                    }
                );
                break;
            case "shark":
                speciesList.Sort(
                    delegate (GameObject x, GameObject y) {
                        return y.GetComponent<Shark>().food.CompareTo(x.GetComponent<Shark>().food);
                    }
                );
                break;
        }
    }

	//Add old generation stats to the archive
	private void AddStatsToArchive() {
		List<List<float>> tempStatList = new List<List<float>>();

		foreach (GameObject fish in fishList) {
			List<float> tempStats = new List<float> ();

			foreach (float stat in fish.GetComponent<Fish>().chromosome) {
				tempStats.Add (stat);
			}
            //Add amount of food eaten
            tempStats.Add(fish.GetComponent<Fish>().food);
			tempStatList.Add (tempStats);
		}
		archiveList.Add(tempStatList);
	}

	//Get the poopulations total fitness
	private int GetTotalFitness(List<GameObject> speciesList, string species) {
		int totalFitness = 0;
        switch(species){
            case "fish":
                for (int i = 0; i < fishList.Count; i++)
                    totalFitness += speciesList[i].GetComponent<Fish>().food;
                break;
            case "shark":
                for (int i = 0; i < sharkList.Count ; i++)
                    totalFitness += speciesList[i].GetComponent<Shark>().food;
                break;
        }
        return totalFitness;
    }

	private List<float[]> GenerateChromosomes(List<GameObject> speciesList, string species, int populationSize) { 
        List<float[]> chromosomeList = new List<float[]>();
        for (int i = 0; i < populationSize; i++) {

            float[] fishDad = calcParent(speciesList, species);
            float[] fishMom = calcParent(speciesList, species);
            float[] fishKid = fishDad;

            int temp = Random.Range(0, fishDad.Length - 1);

            for (int k = 0; k < fishDad.Length; k++) {

                if(k > temp){
                    fishKid[k] = fishMom[k];
                }
            }

            chromosomeList.Add(fishKid);
        }

        ClearScene(species);
		return chromosomeList;
    }

	//Clear scene of gameobjects and clear lists
    private void ClearScene (string species) {

        //Destory all GameObjects with the tag food
        GameObject[] gos = GameObject.FindGameObjectsWithTag("food");
        foreach (GameObject go in gos)
            Destroy(go);

        switch (species){
            case "fish":
                if (fishList != null)
                {
                    //Destroy all fish
                    fishList.ForEach(delegate (GameObject fish) {
                        Destroy(fish);
                    });

                    //Clear fishList
                    fishList.Clear();
                }
                break;

            case "shark":
                //Destroy all sharks
                if (sharkList != null)
                {
                    sharkList.ForEach(delegate (GameObject shark) {
                        Destroy(shark);
                    });

                    //Clear sharkList
                    sharkList.Clear();
                }
                break;
        }
    }

	//Apply the new chromosomes and spawn the new generation
	private void SpawnNextGen (List<float[]> chromosomeList, List<GameObject> speciesList, string species) {

        World worldScript = GameObject.Find("World").GetComponent<World>();

        switch (species)
        {
            case "fish":
                for (int i = 0; i < populationSizeFish; i++)
                {
                    Vector3 position = new Vector3(Random.Range(-worldScript.xBoundry, worldScript.xBoundry), Random.Range(-worldScript.yBoundry, worldScript.yBoundry), 0);

                    speciesList.Add((GameObject)Instantiate(fish, position, Quaternion.identity));
                    speciesList[i].GetComponent<Fish>().chromosome = chromosomeList[i];
                    speciesList[i].GetComponent<Fish>().color = colorListFish[i + 1];
                }

                colorListFish.Clear();

                //Mutate
                Mutate(fishList, "fish");
                break;
            case "shark":
                for (int i = 0; i < populationSizeShark; i++)
                {
                    Vector3 position = new Vector3(Random.Range(-worldScript.xBoundry, worldScript.xBoundry), Random.Range(-worldScript.yBoundry, worldScript.yBoundry), 0);

                    speciesList.Add((GameObject)Instantiate(shark, position, Quaternion.identity));
                    speciesList[i].GetComponent<Shark>().chromosome = chromosomeList[i];
                    speciesList[i].GetComponent<Shark>().color = colorListShark[i + 1];
                }
                colorListShark.Clear();

                //Mutate
                Mutate(sharkList, "shark");
                break;
        }
        
		SpawnFood();
    }

	//Get a random parent based on fitness
    private float[] calcParent(List<GameObject> speciesList, string species){

        float randomSeed = 0;
        float counter = 0;

        switch (species){
            case "fish":
                randomSeed = Random.Range(0, GetTotalFitness(speciesList, "fish"));
                counter = 0;
            
                for (int i = 0; i < speciesList.Count; i++)
                {
                    counter += speciesList[i].GetComponent<Fish>().food;

                    if (randomSeed > counter)
                    {
                        continue;
                    }
                    else
                    {
                        colorListFish.Add(speciesList[i].GetComponent<Fish>().color);
                        return speciesList[i].GetComponent<Fish>().chromosome;
                    }
                }
                break;
            case "shark":
                randomSeed = Random.Range(0, GetTotalFitness(speciesList, "shark"));
                counter = 0;

                for (int i = 0; i < speciesList.Count; i++)
                {
                    counter += speciesList[i].GetComponent<Shark>().food;

                    if (randomSeed > counter)
                    {
                        continue;
                    }
                    else
                    {
                        colorListShark.Add(speciesList[i].GetComponent<Shark>().color);
                        return speciesList[i].GetComponent<Shark>().chromosome;
                    }
                }
                break;
        }

        Debug.Log("Error incest");
        return null;
    }

	private void Mutate(List<GameObject> speciesList, string species) {

        switch (species) {
            case "fish":
                for (int i = 0; i < speciesList.Count; i++)
                {
                    float[] currentChromosome = speciesList[i].GetComponent<Fish>().chromosome;
                    for (int k = 0; k < currentChromosome.Length; k++)
                    {

                        if (Random.Range(0, 100) <= mutatonChanse)
                        {

                            float mutateAmount = mutationMagnitude * currentChromosome[k];
                            currentChromosome[k] += Random.Range(-mutateAmount, mutateAmount);
                            speciesList[i].GetComponent<Fish>().mutant = true;
                        }
                        else
                            continue;
                    }
                }
                break;
            case "shark":
                for (int i = 0; i < speciesList.Count; i++)
                {

                    float[] currentChromosome = speciesList[i].GetComponent<Shark>().chromosome;
                    for (int k = 0; k < currentChromosome.Length; k++)
                    {

                        if (Random.Range(0, 100) <= mutatonChanse)
                        {

                            float mutateAmount = mutationMagnitude * currentChromosome[k];
                            currentChromosome[k] += Random.Range(-mutateAmount, mutateAmount);
                            speciesList[i].GetComponent<Shark>().mutant = true;
                        }
                        else
                            continue;
                    }
                }
                break;
        }

	}

}
