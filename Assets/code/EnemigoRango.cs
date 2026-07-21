using UnityEngine;

public class EnemigoRango : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2.5f;
    public float distanciaDeTiro = 8f; // Prob� subiendo esto a 8 o 10

    [Header("Disparo")]
    public GameObject balaEnemigaPrefab;
    public Transform puntoDisparo;
    public float tiempoEntreTiros = 1.5f;

    private Transform jugador;
    private Rigidbody2D rb; // Componente de f�sicas
    private float timerDisparo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject nave = GameObject.FindGameObjectWithTag("Player");
        if (nave != null) jugador = nave.transform;
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        // Que el enemigo siempre mire a la nave
        Vector2 direccion = (jugador.position - transform.position).normalized;
        transform.up = direccion;

        if (distancia > distanciaDeTiro)
        {
            // CAMBIO CLAVE: Usamos la velocidad del Rigidbody en vez de MoveTowards
            // Esto permite que los enemigos se choquen y se empujen entre s� sin fusionarse
            rb.linearVelocity = direccion * velocidad;
        }
        else
        {
            // Fren� en rango: Apaga la velocidad pero permite que la horda lo empuje desde atr�s
            rb.linearVelocity = Vector2.zero;

            timerDisparo += Time.deltaTime;
            if (timerDisparo >= tiempoEntreTiros)
            {
                Disparar();
                timerDisparo = 0f;
            }
        }
    }

    void Disparar()
    {
        if (balaEnemigaPrefab != null && puntoDisparo != null)
        {
            Instantiate(balaEnemigaPrefab, puntoDisparo.position, transform.rotation);
        }
    }
    // Añadimos la variable de vida para que puedas modificarla desde el Inspector si quieres
    public int vidas = 3;

    // Esta es la función que tu Bullet.cs estaba buscando desesperadamente
    public void RecibirDano()
    {
        vidas--;

        if (vidas <= 0)
        {
            // Opcional: Acá podrías poner un efecto de explosión o sonido antes de destruirlo
            Destroy(gameObject);
        }
    }
}