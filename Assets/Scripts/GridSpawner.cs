using UnityEngine;

public class GridSpawner : MonoBehaviour {
    private Grid grid;

    private void Start() {
        grid = new Grid(15, 10, 1f, new Vector3(-7.5f, -5));
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 56);
        }

        if (Input.GetMouseButtonDown(1)) {
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
    }
}