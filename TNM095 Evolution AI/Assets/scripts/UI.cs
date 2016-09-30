using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

	public float simulationSpeed { get; set;}

	public GameObject simulationPanel;
	public GameObject showUiButton;

	private bool hide;

	void Start() {
		hide = false;
		simulationSpeed = 1;
	}

	void Update() {

		Time.timeScale = simulationSpeed;
	}

	public void HideShow() {
		hide = !hide;
		simulationPanel.SetActive (hide);
		showUiButton.SetActive (!hide);
	}
}
