using UnityEngine;

public class BossRotate : MonoBehaviour
{
    public GameObject centerObject; // The central object to rotate around
    public GameObject objectPrefab; // The object to spawn
    public int numberOfObjects = 5; // Number of objects to spawn
    public float radius = 2.0f; // Radius of the circle
    public bool rotateCounterClockwise = false; // Whether to rotate counter-clockwise

    private GameObject[] spawnedObjects; // Array to store the spawned objects
    private float angleStep; // Angle step between each object

    void Start()
    {
        // Initialize the spawnedObjects array
        spawnedObjects = new GameObject[numberOfObjects];

        // Calculate the angle step based on the number of objects and rotation direction
        angleStep = 360.0f / numberOfObjects;

        // Spawn the objects and position them around the center
        SpawnObjects();
    }

    void SpawnObjects()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            // Calculate the angle for this object based on rotation direction
            float angle = rotateCounterClockwise ? (360 - i * angleStep) : (i * angleStep);

            // Calculate the position on the circle
            float x = centerObject.transform.position.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float y = centerObject.transform.position.y + radius * Mathf.Sin(Mathf.Deg2Rad * angle);

            // Spawn the object
            GameObject obj = Instantiate(objectPrefab, new Vector3(x, y, 0), Quaternion.identity);

            // Parent the object to the centerObject for organization
            obj.transform.SetParent(centerObject.transform);

            // Set the rotation of the object to face outward from the center
            obj.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

            // Add the spawned object to the spawnedObjects array
            spawnedObjects[i] = obj;
        }
    }

    void Update()
    {
        // Rotate the spawned objects around the center
        RotateObjects();
    }

    void RotateObjects()
    {
        // Determine the rotation direction
        int direction = rotateCounterClockwise ? -1 : 1;

        // Rotate each spawned object around the center
        foreach (GameObject obj in spawnedObjects)
        {
            obj.transform.RotateAround(centerObject.transform.position, Vector3.forward, direction * 50.0f * Time.deltaTime); // Adjust the rotation speed as needed
        }
    }
    void OnDisable()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
    }
}