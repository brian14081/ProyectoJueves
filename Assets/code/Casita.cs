using UnityEngine;

public class Casita : MonoBehaviour
{
    [Header("Configuración")]
    public float vida = 3f; // Tiros necesarios para romperla
    public GameObject enemigoPrefab;
    public float tiempoEntreSpawns = 3f; // Cada cuánto sale un enemigo

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        // Si pasa el tiempo, crea un enemigo en la posición de la casita
        if (timer >= tiempoEntreSpawns)
        {
            Instantiate(enemigoPrefab, transform.position, Quaternion.identity);
            timer = 0f;
        }
    }

    public void RecibirDano(float dano)
    {
        vida -= dano;
        if (vida <= 0)
        {
            // En vez de borrarla, la apagamos para poder regenerarla después
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si choca con una casita spawner
        if (collision.TryGetComponent<Casita>(out Casita casita))
        {
            casita.RecibirDano(1f); // Le saca 1 de vida
            Destroy(gameObject);    // Destruye la bala al impactar
            return; // Corta acá para que no siga ejecutando lo de abajo
        }

        // ... acá abajo sigue tu código viejo que destruye enemigos ...
    }
}