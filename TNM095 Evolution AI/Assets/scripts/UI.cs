﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI : MonoBehaviour {

	public float simulationSpeed { get; set;}
	public int genDisplayChoice { get; set;}

	public GameObject simulationPanel;
	public GameObject statPanel;
	public GameObject showUiButton;
	public Dropdown genDropDown;
	public Text generationText;

	private bool hide;
	private List<string> dropDownList;

	void Start() {
		hide = false;
		simulationSpeed = 1;
		genDisplayChoice = 0;

		simulationPanel.SetActive (false);
		statPanel.SetActive (false);

		dropDownList = new List<string> ();
	}

	void Update() {
		Time.timeScale = simulationSpeed;
	}

	public void addGenerationToDropDown(int gen) {
		dropDownList.Add ("Generation " + gen);
		genDropDown.options.Clear ();
		genDropDown.AddOptions (dropDownList);
	}

	public void HideShow() {
		hide = !hide;
		simulationPanel.SetActive (hide);
		statPanel.SetActive (hide);
		showUiButton.SetActive (!hide);
	}

	public void DisplayIndividuals() {
		List<List<List<float>>> archive = GameObject.Find ("Simulation").GetComponent<Simulation> ().getArchive ();
	
		//Check if generation exists
		if (genDisplayChoice >= archive.Count) {
			generationText.text = "No data recorded for this generation...";
			return;
		}

		string text = "";

		//Display individuals
		foreach (List<float> fish in archive[genDisplayChoice]) {
			foreach (float stat in fish)
				text += stat.ToString("F") + ", ";
			text += "\n";
		}
		generationText.text = text;
	}

	public void DisplayGeneral() {
		
	}
}
