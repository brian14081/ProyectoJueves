using UnityEngine;

public class EnemigoMeleeDano : MonoBehaviour
{
    public float Dano = 1f;
    public float tiempoEntreGolpes = 0.5f;

    private float timer = 0f;


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController jugador = collision.gameObject.GetComponent<PlayerController>();
            if (jugador != null)
            {
            
                jugador.RecibirDano(); 

           
                timer = 0f;
            }
        }
    }


    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            timer += Time.deltaTime;

            if (timer >= tiempoEntreGolpes)
            {
                PlayerController jugador = collision.gameObject.GetComponent<PlayerController>();
                if (jugador != null)
                {
                    jugador.RecibirDano(); 
                }
                timer = 0f; 
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            timer = tiempoEntreGolpes;
        }
    }
}