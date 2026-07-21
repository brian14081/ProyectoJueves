using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Tu casita (¡con el 1f de vuelta para que no salte el error de la línea 14!)
        Casita casita = collision.GetComponent<Casita>();
        if (casita != null)
        {
            casita.RecibirDano(1f);
            Destroy(gameObject);
            return;
        }

        // 2. LO NUEVO: Detectar al enemigo a distancia
        if (collision.CompareTag("Enemigo")) // Asegúrate de que el EnemigoRango tenga este Tag en el Inspector
        {
            // En lugar de "Enemy", buscamos "EnemigoRango"
            EnemigoRango enemigoRango = collision.GetComponent<EnemigoRango>();

            if (enemigoRango != null)
            {
                // Revisa dentro de tu script EnemigoRango.cs si esta función lleva () o (1f)
                enemigoRango.RecibirDano();
            }

            // Destruimos la bala tras impactar
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.up * speed;

        
        Destroy(gameObject, lifeTime);
    }


}