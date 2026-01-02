using UnityEngine;

public class GameManager : MonoBehaviour {
    private Grid grid;
    [SerializeField] private GameObject playerPrefab;

    private void Start() {
        grid = new Grid(15, 10, 1f, new Vector3(-7.5f, -5));
        SpawnPlayerAtGridPosition(7, 0);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 56);
        }

        if (Input.GetMouseButtonDown(1)) {
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
    }

    private void SpawnPlayerAtGridPosition(int x, int y) {
        Vector3 spawnPosition = grid.GetCellCenterWorldPosition(x, y);
        Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
    }
}