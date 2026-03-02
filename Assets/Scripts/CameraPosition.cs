using UnityEngine;

public class CameraPosition : MonoBehaviour {
    private void Start() {
        FitCameraToGrid();
    }

    private void FitCameraToGrid() {
        GameManager gm = GameManager.Instance;

        float gridWorldWidth = gm.gridWidth * gm.cellSize;
        float gridWorldHeight = gm.gridHeight * gm.cellSize;

        float screenAspect = (float)Screen.width / Screen.height;
        float gridAspect = gridWorldWidth / gridWorldHeight;

        float orthographicSize;
        if (gridAspect > screenAspect) {
            // Grid is wider than screen, fit by width
            orthographicSize = gridWorldWidth / (2f * screenAspect);
        }
        else {
            // Grid is taller than screen, fit by height
            orthographicSize = gridWorldHeight / 2f;
        }

        Camera.main.orthographicSize = orthographicSize;

        // Position camera at grid center
        Vector3 gridOrigin = new Vector3(-9.5f, -5f, 0f); // Same as in GameManager
        Vector3 gridCenter = gridOrigin + new Vector3(gridWorldWidth / 2f, gridWorldHeight / 2f, -10f);

        transform.position = gridCenter;
    }
}