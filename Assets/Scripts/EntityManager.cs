using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class EntityManager : MonoBehaviour {
    [SerializeField] private GameObject[] vehiclePrefabs;
    [SerializeField] private GameObject[] riverEntityPrefabs;

    private int spawnXleft = -5;
    private int spawnXright = 19;
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
        InitializeStartingVehicles();
        StartCoroutine(SpawnVehiclesRandomly());
        StartCoroutine(SpawnRiverEntitiesRandomly());
    }

    private void Update() {
        // NOTE: This should be optimized, but for this simple game it will do.
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

    private void InitializeStartingVehicles() {
        for (int i = 0; i < 6; i++) {
            bool spawnOnLeft = Random.value > 0.5f;

            if (spawnOnLeft) {
                int randomLeftY = vehicleLeftSpawnRows[Random.Range(0, vehicleLeftSpawnRows.Length)];
                int randomX = Random.Range(-2, 4);
                SpawnVehicle(randomX, randomLeftY);
            }
            else {
                int randomRightY = vehiclerightSpawnRows[Random.Range(0, vehiclerightSpawnRows.Length)];
                int randomX = Random.Range(8, 15);
                SpawnVehicle(randomX, randomRightY);
            }
        }
    }

    private IEnumerator SpawnVehiclesRandomly() {
        while (true) {
            yield return new WaitForSeconds(0.5f);

            bool spawnOnLeft = Random.value > 0.5f;

            if (spawnOnLeft) {
                int randomLeftY = vehicleLeftSpawnRows[Random.Range(0, vehicleLeftSpawnRows.Length)];
                SpawnVehicle(spawnXleft, randomLeftY);
            }
            else {
                int randomRightY = vehiclerightSpawnRows[Random.Range(0, vehiclerightSpawnRows.Length)];
                SpawnVehicle(spawnXright, randomRightY);
            }
        }
    }

    private IEnumerator SpawnRiverEntitiesRandomly() {
        while (true) {
            yield return new WaitForSeconds(0.8f);

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
        bool shouldRotate = x >= 0 && x <= 19;

        Quaternion rotation = shouldRotate ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        GameObject vehicle =
            Instantiate(vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)], spawnPosition, rotation);
    }

    private void SpawnRiverEntity(int x, int y) {
        Vector3 spawnPosition = GameManager.Instance.grid.GetCellCenterWorldPosition(x, y);
        bool shouldRotate = x >= 0 && x <= 19;

        Quaternion rotation = shouldRotate ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        GameObject riverEntity = Instantiate(riverEntityPrefabs[Random.Range(0, riverEntityPrefabs.Length)],
            spawnPosition, rotation);
    }

    private void MoveGameObject(GameObject objectToBeMoved) {
        float speed = 4f * Time.deltaTime;
        objectToBeMoved.transform.Translate(Vector3.right * speed);
    }
}