using UnityEngine;
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

	private bool uiTriggered = false;

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

		//Make sure ui updates properly after first generation
		if (!uiTriggered && GameObject.Find ("Simulation").GetComponent<Simulation> ().getGeneration() == 2) {
			DisplayIndividuals ();
			uiTriggered = true;
		}
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

		//get generation from dropdown
		genDisplayChoice = genDropDown.value;

		//Check if generation exists
		if (genDisplayChoice >= archive.Count) {
			generationText.text = "No data recorded for this generation...";
			return;
		}

		string text = "";

		text += "    Speed  Angle    Range   Size   Food\n";

		//Display individuals
		foreach (List<float> fish in archive[genDisplayChoice]) {

			foreach (float stat in fish)
				//text += stat.ToString("F") + ", ";
				text += string.Format ("{0,10}", stat.ToString("F1"));
				//text += stat.ToString("F1") + ", ";
			text += "\n";
		}
		generationText.text = text;
	}

	public void DisplayGeneral() {
		
	}
}