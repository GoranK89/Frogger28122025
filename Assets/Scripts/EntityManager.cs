using UnityEngine;
using System.Collections;

public class EntityManager : MonoBehaviour {
    [SerializeField] private GameObject[] vehiclePrefabs;
    [SerializeField] private GameObject[] riverEntityPrefabs;

    [SerializeField] private float minSpawnDelay = 1f;
    [SerializeField] private float maxSpawnDelay = 2f;

    private int spawnXleft = -4;
    private int spawnXright = 18;
    private int[] vehicleLeftSpawnRows = { 1, 3, 5 };
    private int[] vehiclerightSpawnRows = { 2, 4 };
    private int[] riverEntitySpawnRows = { 7, 8, 9, 10 };

    private int carsSpawned = 0;
    private int riverEntitiesSpawned = 0;
    private int maxCars = 20;
    private int maxRiverEntities = 15;

    // Car Rotation
    private bool rotateLeft;

    private void Start() {
        StartCoroutine(SpawnVehiclesRandomly());
        StartCoroutine(SpawnRiverEntitiesRandomly());
    }

    private void Update() {
        // TODO: this is inefficient, ok for testing
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");
        GameObject[] riverEntities = GameObject.FindGameObjectsWithTag("RiverLog");

        foreach (GameObject vehicle in vehicles) {
            MoveGameObject(vehicle);
        }

        foreach (GameObject riverEntity in riverEntities) {
            MoveGameObject(riverEntity);
        }
    }

    private IEnumerator SpawnVehiclesRandomly() {
        while (carsSpawned < maxCars) {
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

            carsSpawned++;
        }
    }

    private IEnumerator SpawnRiverEntitiesRandomly() {
        while (riverEntitiesSpawned < maxRiverEntities) {
            float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(randomDelay);

            bool spawnOnLeft = Random.value > 0.5f;

            if (spawnOnLeft) {
                int randomLeftY = riverEntitySpawnRows[Random.Range(0, riverEntitySpawnRows.Length)];
                SpawnRiverEntity(spawnXleft, randomLeftY);
            }
            else {
                int randomRightY = riverEntitySpawnRows[Random.Range(0, riverEntitySpawnRows.Length)];
                SpawnRiverEntity(spawnXright, randomRightY);
            }
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