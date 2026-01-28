using UnityEngine;
using System.Collections;

public class TrafficManager : MonoBehaviour {
    [SerializeField] private GameObject[] carPrefabs;

    [SerializeField] private float minSpawnDelay = 2f;
    [SerializeField] private float maxSpawnDelay = 5f;

    private int spawnXleft = -1;
    private int spawnXright = 16;
    private int[] leftSpawnRows = { 1, 3, 5 };
    private int[] rightSpawnRows = { 2, 4 };

    private int carsSpawned = 0;
    private int maxCars = 20;

    private void Start() {
        StartCoroutine(SpawnVehiclesRandomly());
    }

    private IEnumerator SpawnVehiclesRandomly() {
        while (carsSpawned < maxCars) {
            float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(randomDelay);

            int randomLeftY = leftSpawnRows[Random.Range(0, leftSpawnRows.Length)];
            int randomRightY = rightSpawnRows[Random.Range(0, rightSpawnRows.Length)];

            VehicleSpawner(spawnXleft, randomLeftY);
            VehicleSpawner(spawnXright, randomRightY);
            carsSpawned++;
        }
    }

    private void VehicleSpawner(int x, int y) {
        Vector3 spawnPosition = GameManager.Instance.grid.GetCellCenterWorldPosition(x, y);
        Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], spawnPosition, Quaternion.identity);
    }
}