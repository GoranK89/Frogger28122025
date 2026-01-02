using UnityEngine;

public static class UtilsClass {
    private static Camera mainCamera;

    private static float screenBoundaryX;
    private static float screenBoundaryY;

    public static Vector3 GetMouseWorldPosition() {
        if (mainCamera == null) {
            mainCamera = Camera.main;
        }

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }

    public static float GetScreenBoundaryX() {
        if (mainCamera == null) {
            mainCamera = Camera.main;
        }

        if (screenBoundaryX == 0f) {
            // Calculate width based on aspect ratio
            screenBoundaryX = mainCamera.orthographicSize * mainCamera.aspect;
        }

        return screenBoundaryX;
    }

    public static float GetScreenBoundaryY() {
        if (mainCamera == null) {
            mainCamera = Camera.main;
        }

        if (screenBoundaryY == 0f) {
            // Camera orthographic size is the half-height of the visible area
            screenBoundaryY = mainCamera.orthographicSize;
        }

        return screenBoundaryY;
    }

    public static float GetColliderHalfWitdh(Collider2D collider) {
        return collider.bounds.extents.x;
    }
}