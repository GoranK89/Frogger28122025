using UnityEngine;

public class PlayerController : MonoBehaviour {
    private SpriteRenderer spriteRenderer;

    private Vector2 movement = Vector2.zero;

    private Animator animator;
    private string currentAnimation = "";

    private void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

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
            int cellValue = gameManager.grid.GetValue(newX, newY);
            if (cellValue == 3) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(newPosition, 0.2f);
                bool onLog = false;
                foreach (Collider2D collider in colliders) {
                    if (collider.gameObject.CompareTag("RiverLog")) {
                        onLog = true;
                        break;
                    }
                }

                if (!onLog) {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void ChangeAnimation(string animation, float crossFade = 0.2f) {
        animator.Play(animation, 0, 0f);
        currentAnimation = animation;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Vehicle")) {
            Destroy(gameObject);
        }
    }
}