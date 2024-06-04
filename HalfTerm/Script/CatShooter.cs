using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BulletHell
{
    public class CatShooter : MonoBehaviour
    {
        public GameObject Ball;
        public float BallLife = 8f;
        public float speed = 10f;

        // Referencias a variables del script GirlMove
        public Transform bulletSpawnPoint;
        public float bulletSpeed = 5f;
        public TextMeshProUGUI bulletCounterText; // Referencia al texto de contador de balas

        // Modos de disparo
        public const string Mode1 = "Mode1";
        public const string Mode2 = "Mode2";
        public const string Mode3 = "Mode3";

        private string currentMode = Mode1;
        private float modeDuration = 10f;
        private float modeTimer = 0f;
        private int currentBulletCount = 0;
        private int maxBullets = 100;
        private bool isShooting = false; // Variable para controlar la corrutina

        void Start()
        {
            if (bulletCounterText == null)
            {
                Debug.LogError("Bullet Counter Text not assigned in the Inspector");
                return;
            }
            UpdateBulletCounter();
        }

        void Update()
        {
            modeTimer += Time.deltaTime;
            if (modeTimer >= modeDuration)
            {
                ChangeMode();
                modeTimer = 0f;
            }

            if (currentBulletCount < maxBullets && !isShooting)
            {
                StartCoroutine(Shoot());
            }
        }

        void ChangeMode()
        {
            switch (currentMode)
            {
                case Mode1:
                    currentMode = Mode2;
                    break;
                case Mode2:
                    currentMode = Mode3;
                    break;
                case Mode3:
                    break;
            }
        }

        IEnumerator Shoot()
        {
            isShooting = true; // Marca que estamos disparando

            if (currentMode == Mode1)
            {
                ShootCirclePattern();
            }
            else if (currentMode == Mode2)
            {
                ShootSpiralPattern();
            }
            else if (currentMode == Mode3)
            {
                ShootStarPattern();
            }

            yield return new WaitForSeconds(1.0f); // Ajusta el tiempo entre disparos según sea necesario
            isShooting = false; // Marca que hemos terminado de disparar
        }

        void ShootCirclePattern()
        {
            int numBullets = 10;
            float angleStep = 360f / numBullets;
            for (int i = 0; i < numBullets; i++)
            {
                Quaternion rotation = Quaternion.Euler(0f, 0f, i * angleStep);
                GameObject bullet = Instantiate(Ball, bulletSpawnPoint.position, rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = bullet.transform.right * bulletSpeed;
                }
                currentBulletCount++;
                UpdateBulletCounter();
                Destroy(bullet, BallLife); // Destruye la bala después de BallLife segundos
            }
        }

        void ShootSpiralPattern()
        {
            int numBullets = 20;
            float angleStep = 360f / numBullets;
            float radius = 1.0f; // Radio inicial de la espiral
            float radiusIncrement = 1.0f; // Incremento del radio por cada bala

            for (int i = 0; i < numBullets; i++)
            {
                float angle = i * angleStep;
                float x = bulletSpawnPoint.position.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                float y = bulletSpawnPoint.position.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad);

                GameObject bullet = Instantiate(Ball, new Vector3(x, y, bulletSpawnPoint.position.z), Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                    rb.velocity = direction * bulletSpeed;
                }

                radius += radiusIncrement; // Incrementar el radio para la próxima bala
                currentBulletCount++;
                UpdateBulletCounter();
                Destroy(bullet, BallLife); // Destruye la bala después de BallLife segundos
            }
        }

        void ShootStarPattern()
        {
            int numPoints = 5;
            int bulletsPerLine = 6;
            float angleStep = 360f / numPoints;
            float lineLength = 10f; // Incremento de la longitud de la línea para hacer la estrella más grande

            for (int i = 0; i < numPoints; i++)
            {
                float angle = i * angleStep;
                Vector3 startPosition = bulletSpawnPoint.position;
                for (int j = 0; j < bulletsPerLine; j++)
                {
                    float fraction = (float)j / (bulletsPerLine - 1);
                    float x = startPosition.x + lineLength * fraction * Mathf.Cos(angle * Mathf.Deg2Rad);
                    float y = startPosition.y + lineLength * fraction * Mathf.Sin(angle * Mathf.Deg2Rad);

                    GameObject bullet = Instantiate(Ball, new Vector3(x, y, startPosition.z), Quaternion.identity);
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), -Mathf.Abs(Mathf.Sin(angle * Mathf.Deg2Rad))); // Dispara hacia abajo
                        rb.velocity = direction * bulletSpeed;
                    }

                    currentBulletCount++;
                    UpdateBulletCounter();
                    Destroy(bullet, BallLife); // Destruye la bala después de BallLife segundos
                }
            }
        }

        void UpdateBulletCounter()
        {
            if (bulletCounterText != null)
            {
                bulletCounterText.text = "Bullets: " + currentBulletCount;
            }
        }

        public void DecreaseBulletCount()
        {
            currentBulletCount--;
            UpdateBulletCounter();
        }

        public void DecreaseBulletCountBy(int amount)
        {
            currentBulletCount -= amount;
            UpdateBulletCounter();
        }
    }
}
