using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell
{
    public class HairyBall : MonoBehaviour
    {
        public Vector2 CatShoot;
        public float BallLife = 8f;
        public float speed = 15f;

        private float timer = 0f;
        private Renderer renderer; // Referencia al componente Renderer del objeto
        private CatShooter catShooter; // Referencia al script CatShooter

        void Start()
        {
            CatShoot = new Vector2(transform.position.x, transform.position.y);
            renderer = GetComponent<Renderer>(); // Obtener el componente Renderer al inicio
            catShooter = FindObjectOfType<CatShooter>(); // Obtener el componente CatShooter
        }

        void Update()
        {
            if (timer > BallLife || !IsVisible()) { // Destruir el objeto si el temporizador excede BallLife o si no es visible en la pantalla
                DestroyBall();
            }
            timer += Time.deltaTime;
            transform.position = Movement(timer);
        }

        public Vector2 Movement(float timer)
        {
            float x = timer * speed * transform.right.x;
            float y = timer * speed * transform.right.y;
            return new Vector2(x + CatShoot.x, y + CatShoot.y);
        }

        bool IsVisible()
        {
            return renderer.isVisible; // Verificar si el objeto es visible en la pantalla
        }

        void DestroyBall()
        {
            if (catShooter != null)
            {
                catShooter.DecreaseBulletCount();
            }
            Destroy(gameObject);
        }
    }
}
