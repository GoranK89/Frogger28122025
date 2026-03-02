using UnityEngine;

public class PlayerController : MonoBehaviour {
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;

    private Vector2 movement = Vector2.zero;
    private Animator animator;
    private string currentAnimation = "";
    private bool isOnLog = false;

    private void Awake() {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        PlayerOnLogBoundsCheck();

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
        // When player is carried by a log, this position sync is required.
        Vector2Int currentGridPos = gameManager.grid.GetGridPosition(transform.position);
        gameManager.playerCurrentPositionX = currentGridPos.x;
        gameManager.playerCurrentPositionY = currentGridPos.y;

        int newX = gameManager.playerCurrentPositionX + deltaX;
        int newY = gameManager.playerCurrentPositionY + deltaY;

        int targetCellValue = gameManager.grid.GetValue(newX, newY);

        // Check bounds, can not move outside of grid or into 0 value cells
        if (targetCellValue != 0 && (newX >= 0 && newY >= 0) &&
            (newX < gameManager.gridWidth && newY < gameManager.gridHeight)) {
            Vector3 newPosition = gameManager.grid.GetCellCenterWorldPosition(newX, newY);
            transform.position = newPosition;

            gameManager.playerCurrentPositionX = newX;
            gameManager.playerCurrentPositionY = newY;

            // Trigger animation based on movement direction
            if (deltaY == 1) {
                ChangeAnimation("PlayerJumpForward", 0);
            }
            else if (deltaY == -1) {
                ChangeAnimation("PlayerJumpBackward", 0);
            }
            else if (deltaX == 1) {
                spriteRenderer.flipX = false;
                ChangeAnimation("PlayerJumpRight", 0);
            }
            else if (deltaX == -1) {
                spriteRenderer.flipX = true;
                ChangeAnimation("PlayerJumpLeft", 0);
            }

            // TODO: move this to a separate component

            transform.SetParent(null);
            isOnLog = false;

            if (targetCellValue == 3) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(newPosition, 0.2f);

                foreach (Collider2D collider in colliders) {
                    if (collider.gameObject.CompareTag("RiverLog")) {
                        transform.SetParent(collider.transform);
                        isOnLog = true;
                        break;
                    }
                }
            }

            if (targetCellValue == 3 && !isOnLog) {
                Destroy(gameObject);
            }
        }
    }

    private void PlayerOnLogBoundsCheck() {
        if (isOnLog) {
            Vector2Int currentGridPos = gameManager.grid.GetGridPosition(transform.position);

            if (currentGridPos.x < 0 || currentGridPos.x >= gameManager.gridWidth ||
                currentGridPos.y < 0 || currentGridPos.y >= gameManager.gridHeight) {
                Destroy(gameObject);
            }
        }
    }

    private void ChangeAnimation(string animation, float crossFade = 0.2f) {
        animator.Play(animation, 0, 0f);
        currentAnimation = animation;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Vehicle")) {
            Destroy(gameObject);
        }
    }
}