using UnityEngine;

public class MuroMuerte : MonoBehaviour
{
    // Usamos OnTriggerEnter2D porque tus muros tienen la casilla "Is Trigger" marcada
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Verificamos si lo que tocó el muro es un Enemigo a Rango
        EnemigoRango enemigoRango = collision.GetComponent<EnemigoRango>();
        if (enemigoRango != null)
        {
            // Podés llamar a su función de daño, o directamente destruirlo. 
            // Como queremos eliminarlo de una, Destroy es más rápido y limpio:
            Destroy(collision.gameObject);
            return; // Cortamos la ejecución acá para que no siga buscando
        }

        // 2. Verificamos si lo que tocó el muro es un Enemigo Melee
        EnemigoMeleeDano enemigoMelee = collision.GetComponent<EnemigoMeleeDano>();
        if (enemigoMelee != null)
        {
            Destroy(collision.gameObject);
        }
    }
}