using UnityEngine;
using System.Collections;

public class Shark : MonoBehaviour {

    //Attributes
    public int food;
    public Color color;
    public float[] chromosome;
    public bool mutant = false;

    private float speed;
    private float turnAngle;
    private float visabilityRange;
    private float size;
    private bool foodInRange = false;
    private float randomAngle;
    private float turnTimer;

    private Vector2 forwardDirection;
	private GameObject closestFish = null;

    // Use this for initialization
    void Start()
    {

        this.food = 0;
        //TODO move the random functionality to simulatio scripts

        turnTimer = GameObject.Find("Simulation").GetComponent<Simulation>().idleTurnTime;
        randomAngle = Random.Range(-turnAngle, turnAngle);

        //Give the fish random values
        speed = chromosome[0];
        turnAngle = chromosome[1];
        visabilityRange = chromosome[2];
        size = chromosome[3];

        this.GetComponent<SpriteRenderer>().color = color;
        this.GetComponent<Shark>().transform.localScale = new Vector3(size, size, 1);

        if (mutant)
        {
            this.GetComponent<SpriteRenderer>().color = Color.black;
        }

        //initiate forward direction
        forwardDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        forwardDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {

        //Update forward direction with a random angle within the units maximum turnAngle
		if (!foodInRange || closestFish == null) {
            if (turnTimer <= 0)
            {
                randomAngle = Random.Range(-turnAngle, turnAngle);
                turnTimer = GameObject.Find("Simulation").GetComponent<Simulation>().idleTurnTime;
            }
            forwardDirection = Quaternion.Euler (0, 0, randomAngle) * forwardDirection;
			forwardDirection.Normalize ();
            turnTimer -= Time.deltaTime;
        }

        //Target the closest food in range and turn towards it
        else {
			Vector2 foodDirection = (closestFish.transform.position - transform.position);
            
			float angleToFood = Vector2.Angle(forwardDirection, foodDirection);
            float tempTurnAngle = turnAngle;

            if (Vector3.Cross(forwardDirection, foodDirection).z < 0)
            {
                angleToFood = -angleToFood;
                tempTurnAngle = -tempTurnAngle;
            }

            if (Mathf.Abs(angleToFood) > Mathf.Abs(tempTurnAngle))
                forwardDirection = Quaternion.Euler(0, 0, tempTurnAngle * Time.deltaTime) * forwardDirection;
            else
                forwardDirection = Quaternion.Euler(0, 0, angleToFood * Time.deltaTime) * forwardDirection;
        }

        //Move individual forward with constant speed
        transform.Translate(forwardDirection * Time.deltaTime * speed);

        //teleport fish to other side when outstepping boundries
        GameObject world = GameObject.Find("World");
        World worldScript = world.GetComponent<World>();

        float newX = 0;
        float newY = 0;

        if (transform.position.x > worldScript.xBoundry || transform.position.x < -worldScript.xBoundry)
            newX = -transform.position.x;
        if (transform.position.y > worldScript.yBoundry || transform.position.y < -worldScript.yBoundry)
            newY = -transform.position.y;

        if (newX != 0 && newY != 0) transform.position = new Vector2(newX, newY);
        else if (newX != 0 && newY == 0) transform.position = new Vector2(newX, transform.position.y);
        else if (newX == 0 && newY != 0) transform.position = new Vector2(transform.position.x, newY);
	
        //Check if food is in vision
        detectFood();
    }

    private void detectFood()
    {
		closestFish = GetClosestObjectWithTag("fish");
		if (closestFish != null && Vector2.Distance (transform.position, closestFish.transform.position) <= visabilityRange)
			foodInRange = true;
        else
            foodInRange = false;
    }

    //Returns the closest gameobject of a specific tag
    private GameObject GetClosestObjectWithTag(string tag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);

        if (objectsWithTag.Length == 0)
            return null;

        GameObject closestObject = objectsWithTag[0];

        foreach (GameObject obj in objectsWithTag)
        {
            if (Vector2.Distance(transform.position, obj.transform.position) <
                Vector2.Distance(transform.position, closestObject.transform.position))
            {
                closestObject = obj;
            }
        }
        return closestObject;
    }

    //Check collision 
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "fish")
        {
            this.food = this.food + 1;
            Destroy(coll.gameObject);
            GameObject.Find("Simulation").GetComponent<Simulation>().fishList.Remove(coll.gameObject);
            //GameObject.Find("Simulation").GetComponent<Simulation>().removeFood();
        }
    }
}
