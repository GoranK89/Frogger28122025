using UnityEngine;
using System.Collections;

public class EntityManager : MonoBehaviour {
    [SerializeField] private GameObject[] vehiclePrefabs;
    [SerializeField] private GameObject[] riverEntityPrefabs;

    [SerializeField] private float minSpawnDelay = 1f;
    [SerializeField] private float maxSpawnDelay = 1.5f;

    private int spawnXleft = -4;
    private int spawnXright = 18;
    private int[] vehicleLeftSpawnRows = { 1, 3, 5 };
    private int[] vehiclerightSpawnRows = { 2, 4 };
    private int[] riverEntityLeftSpawnRows = { 7, 9 };
    private int[] riverEntityRightSpawnRows = { 8, 10 };

    private int vehiclesSpawned = 0;
    private int riverEntitiesSpawned = 0;
    private int maxVehicles = 20;
    private int maxRiverEntities = 15;

    // Entity rotation
    private bool rotateLeft;

    private void Start() {
        StartCoroutine(SpawnVehiclesRandomly());
        StartCoroutine(SpawnRiverEntitiesRandomly());
    }

    private void Update() {
        // NOTE: Yes, this can be optimized, but for this simple game it will do.
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");
        GameObject[] riverEntities = GameObject.FindGameObjectsWithTag("RiverLog");

        foreach (GameObject vehicle in vehicles) {
            MoveGameObject(vehicle);
        }

        foreach (GameObject riverEntity in riverEntities) {
            MoveGameObject(riverEntity);
        }

        foreach (GameObject vehicle in vehicles) {
            DestroyOutOfBoundsObjects(vehicle);
        }

        foreach (GameObject riverEntity in riverEntities) {
            DestroyOutOfBoundsObjects(riverEntity);
        }
    }

    private IEnumerator SpawnVehiclesRandomly() {
        while (vehiclesSpawned < maxVehicles) {
            float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(randomDelay);

            bool spawnOnLeft = Random.value > 0.5f;

            if (spawnOnLeft) {
                int randomLeftY = vehicleLeftSpawnRows[Random.Range(0, vehicleLeftSpawnRows.Length)];
                SpawnVehicle(spawnXleft, randomLeftY);
            }
            else {
                int randomRightY = vehiclerightSpawnRows[Random.Range(0, vehiclerightSpawnRows.Length)];
                SpawnVehicle(spawnXright, randomRightY);
            }

            vehiclesSpawned++;
        }
    }

    private IEnumerator SpawnRiverEntitiesRandomly() {
        while (riverEntitiesSpawned < maxRiverEntities) {
            float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(randomDelay);

            bool spawnOnLeft = Random.value > 0.5f;

            if (spawnOnLeft) {
                int randomLeftY = riverEntityLeftSpawnRows[Random.Range(0, riverEntityLeftSpawnRows.Length)];
                SpawnRiverEntity(spawnXleft, randomLeftY);
            }
            else {
                int randomRightY = riverEntityRightSpawnRows[Random.Range(0, riverEntityRightSpawnRows.Length)];
                SpawnRiverEntity(spawnXright, randomRightY);
            }
        }
    }

    private void DestroyOutOfBoundsObjects(GameObject objectToBeDestroyed) {
        if (Mathf.Abs(objectToBeDestroyed.transform.position.x) > 20f ||
            Mathf.Abs(objectToBeDestroyed.transform.position.x) < -10f) {
            Destroy(objectToBeDestroyed);
        }
    }

    private void SpawnVehicle(int x, int y) {
        Vector3 spawnPosition = GameManager.Instance.grid.GetCellCenterWorldPosition(x, y);
        if (x == spawnXright) {
            rotateLeft = true;
            GameObject vehicle = Instantiate(vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)], spawnPosition,
                Quaternion.Euler(0, 180, 0));
        }
        else {
            rotateLeft = false;
            GameObject vehicle = Instantiate(vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)], spawnPosition,
                Quaternion.identity);
        }
    }

    private void SpawnRiverEntity(int x, int y) {
        Vector3 spawnPosition = GameManager.Instance.grid.GetCellCenterWorldPosition(x, y);
        if (x == spawnXright) {
            rotateLeft = true;
            GameObject riverEntity = Instantiate(riverEntityPrefabs[Random.Range(0, riverEntityPrefabs.Length)],
                spawnPosition, Quaternion.Euler(0, 180, 0));
        }
        else {
            rotateLeft = false;
            GameObject riverEntity = Instantiate(riverEntityPrefabs[Random.Range(0, riverEntityPrefabs.Length)],
                spawnPosition, Quaternion.identity);
        }
    }

    private void MoveGameObject(GameObject objectToBeMoved) {
        float speed = 4f * Time.deltaTime;
        objectToBeMoved.transform.Translate(Vector3.right * speed);
    }
}