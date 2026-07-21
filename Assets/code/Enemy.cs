using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    private Transform player;

    [Header("Drops")]
    public GameObject[] powerUpPrefabs; // ACÁ ESTÁ LA LISTA []
    [Range(0f, 100f)] public float dropChance = 15f;

    public GameObject vidaPrefab;
    [Range(0f, 100f)] public float vidaDropChance = 5f;

    [Header("Movimiento Inteligente")]
    public float distanciaCerca = 4f;
    public float distanciaLejos = 15f;
    public float multiplicadorCerca = 0.6f;
    public float multiplicadorLejos = 1.5f;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distanciaAlJugador = Vector2.Distance(transform.position, player.position);
            float porcentajeDistancia = Mathf.InverseLerp(distanciaCerca, distanciaLejos, distanciaAlJugador);
            float multiplicadorActual = Mathf.Lerp(multiplicadorCerca, multiplicadorLejos, porcentajeDistancia);
            float velocidadFinal = speed * multiplicadorActual;
            transform.position = Vector2.MoveTowards(transform.position, player.position, velocidadFinal * Time.deltaTime);

            Vector2 lookDir = player.position - transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            float randomVal = Random.Range(0f, 100f);

            if (randomVal <= dropChance && powerUpPrefabs != null && powerUpPrefabs.Length > 0)
            {
                int indiceAleatorio = Random.Range(0, powerUpPrefabs.Length);
                Instantiate(powerUpPrefabs[indiceAleatorio], transform.position, Quaternion.identity);
            }
            else
            {
                float randomVida = Random.Range(0f, 100f);
                if (randomVida <= vidaDropChance && vidaPrefab != null)
                {
                    Instantiate(vidaPrefab, transform.position, Quaternion.identity);
                }
            }

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.RecibirDano();
            }
            Destroy(gameObject);
        }
    }
}