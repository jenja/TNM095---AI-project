using UnityEngine;
using System.Collections;

public class Population : MonoBehaviour {
	
	private Fish[] fishList;
	private int size;

	//Constructor
	public Population(int size) {
		for (int i = 0; i < size; i++) {
			fishList[i] = new Fish();
            Debug.Log(fishList[i].dna.speed);
            
		}
		this.size = size;
	}

	public int getSize() {
		return this.size;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
