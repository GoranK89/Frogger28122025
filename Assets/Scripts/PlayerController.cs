using UnityEngine;

public class PlayerController : MonoBehaviour {
    private void Update() {
        if (Input.GetKeyDown(KeyCode.W)) {
            Move(0, 1);
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            Move(0, -1);
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            Move(-1, 0);
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            Move(1, 0);
        }
    }

    private void Move(int deltaX, int deltaY) {
        GameManager gameManager = GameManager.Instance;
        int newX = gameManager.playerCurrentPositionX + deltaX;
        int newY = gameManager.playerCurrentPositionY + deltaY;

        // Check bounds
        if (newX >= 0 && newY >= 0 && newX < gameManager.gridWidth && newY < gameManager.gridHeight) {
            Vector3 newPosition = gameManager.grid.GetCellCenterWorldPosition(newX, newY);
            transform.position = newPosition;
            gameManager.playerCurrentPositionX = newX;
            gameManager.playerCurrentPositionY = newY;

            // move this to game manager
            int playerPositionCellValue = gameManager.grid.GetValue(gameManager.playerCurrentPositionX,
                gameManager.playerCurrentPositionY);
            if (playerPositionCellValue == 3) {
                Debug.Log("Player is in the river! Destroy the player.");
            }
        }
    }
}