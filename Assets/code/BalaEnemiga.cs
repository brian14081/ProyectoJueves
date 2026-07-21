using UnityEngine;

public class BalaEnemiga : MonoBehaviour
{
    void Start()
    {
        // Se destruye sola a los 3 segundos para no gastar memoria
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        // Esto hace que la bala avance sola apenas nace
        transform.Translate(Vector3.up * 8f * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Instanciamos al jugador
            PlayerController jugador = collision.GetComponent<PlayerController>();

            if (jugador != null)
            {
                // Ejecutamos el método vacío que restará 1 a las vidas (vidas--)
                jugador.RecibirDano();
            }

            // Destruimos la bala enemiga
            Destroy(gameObject);
        }

        // ... acá sigue la lógica de la pared que ya tenías ...
        if (collision.CompareTag("Pared") || collision.name.Contains("Pared"))
        {
            Destroy(gameObject);
        }
    }
}