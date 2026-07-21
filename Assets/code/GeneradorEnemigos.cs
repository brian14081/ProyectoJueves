using UnityEngine;

public class GeneradorEnemigos : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform jugador;
    public float tiempoEntreSpawns = 1.5f;

    [Header("Configuración de Pasillos")]
    public Transform[] puntosDePasillos; // Acá vas a poner los 4 pasillos vacíos

    [Header("Configuración del Centro")]
    public Vector2 limitesMapa = new Vector2(15f, 10f); // Tamaño de tu rectángulo central
    public float distanciaSegura = 8f; // Radio alrededor del jugador donde NO pueden spawnear

    private float timer = 0f;

    void Update()
    {
        if (jugador == null) return;

        timer += Time.deltaTime;
        if (timer >= tiempoEntreSpawns)
        {
            SpawnearEnemigo();
            timer = 0f;
        }
    }

    void SpawnearEnemigo()
    {
        Vector2 posicionSpawn = Vector2.zero;

        // 50% de chance de salir de un pasillo, 50% de salir del centro
        bool usarPasillo = Random.value > 0.5f;

        if (usarPasillo && puntosDePasillos.Length > 0)
        {
            // Elige un pasillo al azar
            Transform pasilloElegido = puntosDePasillos[Random.Range(0, puntosDePasillos.Length)];
            posicionSpawn = pasilloElegido.position;
        }
        else
        {
            // Lógica del profe: Spawnear en el centro pero FUERA de la cámara
            int intentos = 0;
            do
            {
                float randomX = Random.Range(-limitesMapa.x, limitesMapa.x);
                float randomY = Random.Range(-limitesMapa.y, limitesMapa.y);
                posicionSpawn = new Vector2(randomX, randomY);
                intentos++;
            }
            // El loop se repite si el enemigo cae muy cerca del jugador (distancia segura)
            // Le ponemos límite de 10 intentos para que no se cuelgue el juego
            while (Vector2.Distance(posicionSpawn, jugador.position) < distanciaSegura && intentos < 10);
        }

        // Crea al enemigo
        Instantiate(enemyPrefab, posicionSpawn, Quaternion.identity);
    }
}