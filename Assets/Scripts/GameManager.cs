using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public Grid grid;
    [SerializeField] private GameObject playerPrefab;
    public int gridWidth = 15;
    public int gridHeight = 10;
    public float cellSize = 1f;

    private int pavementTag = 1;
    private int roadTag = 2;
    private int riverTag = 3;

    public int playerCurrentPositionX;
    public int playerCurrentPositionY;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        grid = new Grid(gridWidth, gridHeight, cellSize, new Vector3(-7.5f, -5));
        SpawnPlayerAtGridPosition(7, 0);

        SetGridValues(0, 1, pavementTag); // Pavement rows
        SetGridValues(1, 4, roadTag); // Road rows
        SetGridValues(4, 5, pavementTag); // Pavement rows
        SetGridValues(5, 9, riverTag); // River rows
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
        playerCurrentPositionX = x;
        playerCurrentPositionY = y;
    }

    private void SetGridValues(int rowIndexStart, int rowIndexEnd, int value) {
        for (int x = 0; x < gridWidth; x++) {
            for (int y = rowIndexStart; y < rowIndexEnd; y++) {
                grid.SetValue(x, y, value);
            }
        }
    }
}