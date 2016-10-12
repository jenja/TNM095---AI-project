using UnityEngine;
using System.Collections;

public class CreatureUtils : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Keep creature inside world boundries
	public void KeepWithinBoundries(GameObject creature) {
		World worldScript = GameObject.Find("World").GetComponent<World> ();
		float worldX = worldScript.xBoundry;
		float worldY = worldScript.yBoundry;

		float newX = 0;
		float newY = 0;

		if (creature.transform.position.x > worldX)
			newX = worldX;
		else if (creature.transform.position.x < -worldX)
			newX = -worldX;

		if (creature.transform.position.y > worldY)
			newY = worldY;
		else if (creature.transform.position.y < -worldY)
			newY = -worldY;

		if 		(newX != 0 && newY != 0)	creature.transform.position = new Vector2 (newX, newY);
		else if (newX != 0 && newY == 0)	creature.transform.position = new Vector2 (newX, creature.transform.position.y);
		else if (newX == 0 && newY != 0)	creature.transform.position = new Vector2 (creature.transform.position.x, newY);
	}

	//Teleport to other side if leaving  boundries
	public void tpToOtherSide(GameObject creature) {
		GameObject world = GameObject.Find("World");
		World worldScript = world.GetComponent<World> ();

		float newX = 0;
		float newY = 0;

		if(creature.transform.position.x > worldScript.xBoundry)
			newX = -worldScript.xBoundry;
		else if(creature.transform.position.x < -worldScript.xBoundry)
			newX = worldScript.xBoundry;

		if (creature.transform.position.y > worldScript.yBoundry)
			newY = -worldScript.yBoundry; 
		else if (creature.transform.position.y < -worldScript.yBoundry)
			newY = worldScript.yBoundry;

		if 		(newX != 0 && newY != 0)	creature.transform.position = new Vector2 (newX, newY);
		else if (newX != 0 && newY == 0)	creature.transform.position = new Vector2 (newX, creature.transform.position.y);
		else if (newX == 0 && newY != 0)	creature.transform.position = new Vector2 (creature.transform.position.x, newY);
	}
}
