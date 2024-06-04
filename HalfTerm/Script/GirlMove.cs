using UnityEngine;

public class GirlMove : MonoBehaviour {
    public float movePower = 5f;
    public float slowMovePower = 2f; // Velocidad de movimiento lento
    public KeyCode slowKey = KeyCode.LeftShift; // Tecla para activar el movimiento lento

    private Rigidbody2D rb;
    private int direction = 1;
    private bool alive = true;

    private Camera mainCamera;

    public int heart = 3;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 6f;
    public float fireRate = 0.5f;
    private float nextFire = 3f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void Update() {
        if (alive) {
            Run();
            if (Input.GetKeyDown(slowKey)) {
                movePower = slowMovePower; // Activar movimiento lento
            } else if (Input.GetKeyUp(slowKey)) {
                movePower = 5f; // Volver al movimiento normal
            } if (Input.GetButton("Fire1") && Time.time > nextFire) {
                nextFire = Time.time + fireRate;
                Shoot();
            }
        }
    }

    void Run() {
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0) {
            direction = -1;
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(direction, 1, 1);
        }
        if (Input.GetAxisRaw("Horizontal") > 0) {
            direction = 1;
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(direction, 1, 1);
        }
        if (Input.GetAxisRaw("Vertical") < 0) {
            moveVelocity = Vector3.down;
        }
        if (Input.GetAxisRaw("Vertical") > 0) {
            moveVelocity = Vector3.up;
        }

        Vector3 targetPosition = transform.position + moveVelocity * movePower * Time.deltaTime;
        Vector3 clampedPosition = ClampToCameraView(targetPosition);

        transform.position = clampedPosition;
    }

    Vector3 ClampToCameraView(Vector3 targetPosition) {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(targetPosition);

        viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0.05f, 0.95f);
        viewportPosition.y = Mathf.Clamp(viewportPosition.y, 0.05f, 0.95f);

        return mainCamera.ViewportToWorldPoint(viewportPosition);
    }

    public void Damage() {
        heart -= 1;
        if (heart <= 0) {
            alive = false;
            Debug.Log("HAZ PERDIDO!!");
        }
    }

    void Shoot() {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = bulletSpawnPoint.right * bulletSpeed;

        // Asegúrate de que las balas se destruyan después de un tiempo para actualizar el contador
        Destroy(bullet, 2f);
    }
}
