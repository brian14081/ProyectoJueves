using UnityEngine;

public class SpawnerCamara : MonoBehaviour
{
    [Header("Lista de Enemigos")]
    public GameObject[] enemigosPrefabs;
    public float tiempoEntreSpawns = 2f;

    [Header("Puntos de Spawn Seguros")]
    [Tooltip("Arrastrá acá tus 4 Casitas de la escena")]
    public Transform[] puntosSeguros;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= tiempoEntreSpawns)
        {
            if (enemigosPrefabs.Length > 0 && puntosSeguros.Length > 0)
            {
                // Elige una casita al azar
                Transform puntoElegido = puntosSeguros[Random.Range(0, puntosSeguros.Length)];
                // Elige un tipo de bicho al azar (común o rango)
                GameObject enemigoAleatorio = enemigosPrefabs[Random.Range(0, enemigosPrefabs.Length)];

                // Lo clona exactamente en la posición de la casita
                Instantiate(enemigoAleatorio, puntoElegido.position, Quaternion.identity);
            }
            timer = 0f;
        }
    }
}