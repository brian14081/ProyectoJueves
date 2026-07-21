using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform jugador;

    void LateUpdate()
    {
        if (jugador != null)
        {
            // La cámara sigue a la nave milimétricamente sin pelear con las físicas
            transform.position = new Vector3(jugador.position.x, jugador.position.y, -10f);
        }
    }
}