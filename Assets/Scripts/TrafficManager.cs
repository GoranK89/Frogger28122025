using UnityEngine;
using System.Collections;

public class TrafficManager : MonoBehaviour {
    [SerializeField] private GameObject[] vehiclePrefabs;

    [SerializeField] private float minSpawnDelay = 1f;
    [SerializeField] private float maxSpawnDelay = 2f;

    private int spawnXleft = -1;
    private int spawnXright = 16;
    private int[] leftSpawnRows = { 1, 3, 5 };
    private int[] rightSpawnRows = { 2, 4 };

    private int carsSpawned = 0;
    private int maxCars = 20;

    // Car Rotation
    private bool driveLeft;

    private void Start() {
        StartCoroutine(SpawnVehiclesRandomly());
    }

    private void Update() {
        // TODO: this is inefficient, ok for testing
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");
        foreach (GameObject vehicle in vehicles) {
            MoveVehicle(vehicle);
        }
    }

    private IEnumerator SpawnVehiclesRandomly() {
        while (carsSpawned < maxCars) {
            float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(randomDelay);

            bool spawnOnLeft = Random.value > 0.5f;

            if (spawnOnLeft) {
                int randomLeftY = leftSpawnRows[Random.Range(0, leftSpawnRows.Length)];
                VehicleSpawner(spawnXleft, randomLeftY);
            }
            else {
                int randomRightY = rightSpawnRows[Random.Range(0, rightSpawnRows.Length)];
                VehicleSpawner(spawnXright, randomRightY);
            }

            carsSpawned++;
        }
    }

    private void VehicleSpawner(int x, int y) {
        Vector3 spawnPosition = GameManager.Instance.grid.GetCellCenterWorldPosition(x, y);
        if (x == spawnXright) {
            driveLeft = true;
            GameObject vehicle = Instantiate(vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)], spawnPosition,
                Quaternion.Euler(0, 180, 0));
        }
        else {
            driveLeft = false;
            GameObject vehicle = Instantiate(vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)], spawnPosition,
                Quaternion.identity);
        }
    }

    private void MoveVehicle(GameObject vehicle) {
        float speed = 4f * Time.deltaTime;
        vehicle.transform.Translate(Vector3.right * speed);
    }
}