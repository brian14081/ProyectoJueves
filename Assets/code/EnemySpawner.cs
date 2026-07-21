using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    [Header("Sistema de Rondas (Por Tiempo)")]
    public float duracionRonda = 30f;
    public float tiempoRestante;
    public int rondaActual = 1;

    [Header("Caos Brotato (Cantidades)")]
    public float spawnRateInicial = 0.8f;
    public float spawnRateMinimo = 0.1f;
    private float currentSpawnRate;
    private float nextSpawnTime;
    public float spawnDistance = 18f;

    [Header("Interfaz de Usuario (UI)")]
    public TextMeshProUGUI textoRonda;
    public GameObject panelMejoras;

    void Start()
    {
        IniciarRonda();
    }

    void Update()
    {
        tiempoRestante -= Time.deltaTime;

        if (textoRonda != null)
        {
            textoRonda.text = "round: " + rondaActual + "\n" + Mathf.Ceil(tiempoRestante).ToString();
        }

        if (tiempoRestante > 0)
        {
            float porcentajeCompletado = 1f - (tiempoRestante / duracionRonda);
            currentSpawnRate = Mathf.Lerp(spawnRateInicial, spawnRateMinimo, porcentajeCompletado);

            if (Time.time >= nextSpawnTime)
            {
                SpawnEnemy(porcentajeCompletado);
                nextSpawnTime = Time.time + currentSpawnRate;
            }
        }
        else
        {
            LimpiarEnemigos();
            PasarDeRonda();
        }
    }

    void IniciarRonda()
    {
        tiempoRestante = duracionRonda;
        spawnRateInicial = Mathf.Max(0.3f, 0.8f - (rondaActual * 0.05f));
        spawnRateMinimo = Mathf.Max(0.05f, 0.2f - (rondaActual * 0.02f));
    }

    void PasarDeRonda()
    {
        rondaActual++;
        if (rondaActual % 3 == 0)
        {
            if (panelMejoras != null) panelMejoras.SetActive(true);
            Time.timeScale = 0f;
        }
        IniciarRonda();
    }

    void LimpiarEnemigos()
    {
        GameObject[] enemigosEnPantalla = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemigo in enemigosEnPantalla)
        {
            Destroy(enemigo);
        }
    }

    void SpawnEnemy(float porcentaje)
    {
        float randomAngle = Random.Range(0f, 360f);
        Vector2 spawnDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
        Vector2 spawnPos = (Vector2)Camera.main.transform.position + (spawnDirection * spawnDistance);

        GameObject nuevoEnemigo = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        Enemy scriptEnemigo = nuevoEnemigo.GetComponent<Enemy>();

        if (scriptEnemigo != null)
        {

            float velocidadBase = 4.5f + (rondaActual * 0.15f);
            float velocidadExtra = porcentaje * 2.5f;

            scriptEnemigo.speed = velocidadBase + velocidadExtra;
        }
    }
}