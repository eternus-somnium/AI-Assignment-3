using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PieSliceSensor : MonoBehaviour 
{
    GameObject[] gameObjects;

    public List<GameObject> quadrantOne;
    public List<GameObject> quadrantTwo;
    public List<GameObject> quadrantThree;
    public List<GameObject> quadrantFour;

    public float range = 2.5f;

    public bool showPieSliceSensor = false;

	// Use this for initialization
	void Awake () 
    {
        //This will only detect active game objects that already exist when the scene starts
        gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
	}

    // Update is called once per frame
    void Update() 
    {
        VisualizePieSliceSensor();

        Collider collider;
        Vector3 vectorDistance;
        Vector3 closestPoint;
        float distance;
        float theta;

        for (int i = 0; i < gameObjects.Length; ++i)
        {
            //Just not checking unecessary objects in a messy way since we could use tags or other structures when this isnt only a prototype
            if (gameObjects[i].name == "Plane" || gameObjects[i].name == "Main Camera" || gameObjects[i].name == "User" || gameObjects[i].name == "Directional Light" || gameObjects[i].name == "AdjacencySensor" || gameObjects[i].name == "WallSensors" || gameObjects[i].name == "Pie Slice Sensor" || gameObjects[i].name == "Heading")
                continue;

            collider = gameObjects[i].GetComponent<Collider>();

            if (collider == null)
                continue;

            //Calculate the distance to the closest point of the bounding of an object
            closestPoint = collider.ClosestPointOnBounds(gameObject.transform.position);

            vectorDistance = new Vector3(closestPoint.x - gameObject.transform.position.x, closestPoint.y - gameObject.transform.position.y, closestPoint.z - gameObject.transform.position.z);

            distance = Mathf.Sqrt((vectorDistance.x * vectorDistance.x) + (vectorDistance.z * vectorDistance.z));


            int quadrantOneIndex = -1;
            int quadrantTwoIndex = -1;
            int quadrantThreeIndex = -1;
            int quadrantFourIndex = -1;

            //Scan each active list of objects in each quadrant and mark their index if found
            for (int j = 0; j < quadrantOne.Count; ++j)
            {
                if (quadrantOne[j].name == gameObjects[i].name)
                {
                    quadrantOneIndex = j;
                }
            }

            for (int j = 0; j < quadrantTwo.Count; ++j)
            {
                if (quadrantTwo[j].name == gameObjects[i].name)
                {
                    quadrantTwoIndex = j;
                }
            }

            for (int j = 0; j < quadrantThree.Count; ++j)
            {
                if (quadrantThree[j].name == gameObjects[i].name)
                {
                    quadrantThreeIndex = j;
                }
            }

            for (int j = 0; j < quadrantFour.Count; ++j)
            {
                if (quadrantFour[j].name == gameObjects[i].name)
                {
                    quadrantFourIndex = j;
                }
            }

            //If the object is found within distance ensure it is in the appropriate quadrant's list, if not, add it
            if (distance <= range)
            {
                theta = Mathf.Atan2(Vector3.Dot(gameObject.transform.up, Vector3.Cross(gameObject.transform.forward, vectorDistance)), Vector3.Dot(gameObject.transform.forward, vectorDistance)) * Mathf.Rad2Deg;

                if (theta >= 0 && theta <= 90 && quadrantOneIndex == -1)
                {
                    quadrantOne.Add(gameObjects[i]);
                }
                if (theta >= 90 && quadrantTwoIndex == -1)
                {
                    quadrantTwo.Add(gameObjects[i]);
                }
                if (theta <= -90 && quadrantThreeIndex == -1)
                {
                    quadrantThree.Add(gameObjects[i]);
                }
                if (theta <= 0 && theta >= -90 && quadrantFourIndex == -1)
                {
                    quadrantFour.Add(gameObjects[i]);
                }

                //Remove the object from any previous quadrant if the angle has changed
                if ((theta <= 0 || theta >= 90) && quadrantOneIndex != -1)
                {
                    quadrantOne.RemoveAt(quadrantOneIndex);
                }
                if (theta <= 90 && quadrantTwoIndex != -1)
                {
                    quadrantTwo.RemoveAt(quadrantTwoIndex);
                }
                if (theta >= -90 && quadrantThreeIndex != -1)
                {
                    quadrantThree.RemoveAt(quadrantThreeIndex);
                }
                if ((theta >= 0 || theta <= -90) && quadrantFourIndex != -1)
                {
                    quadrantFour.RemoveAt(quadrantFourIndex);
                }
            }
            else
            {
                //If no longer within distance, remove from the quadrant
                if (quadrantOneIndex != -1)
                {
                    quadrantOne.RemoveAt(quadrantOneIndex);
                }
                if (quadrantTwoIndex != -1)
                {
                    quadrantTwo.RemoveAt(quadrantTwoIndex);
                }
                if (quadrantThreeIndex != -1)
                {
                    quadrantThree.RemoveAt(quadrantThreeIndex);
                }
                if (quadrantFourIndex != -1)
                {
                    quadrantFour.RemoveAt(quadrantFourIndex);
                }
            }

        }
	}

    void VisualizePieSliceSensor()
    {
        if (gameObject.GetComponent<MeshRenderer>().enabled != showPieSliceSensor)
            gameObject.GetComponent<MeshRenderer>().enabled = showPieSliceSensor;

        if (showPieSliceSensor)
        {
            Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * range, Color.white);
            Debug.DrawRay(gameObject.transform.position, gameObject.transform.right * range, Color.white);
            Debug.DrawRay(gameObject.transform.position, -gameObject.transform.right * range, Color.white);
            Debug.DrawRay(gameObject.transform.position, -gameObject.transform.forward * range, Color.white);
        }
    }
}
