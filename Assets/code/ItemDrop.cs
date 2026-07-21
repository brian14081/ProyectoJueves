using UnityEngine;

// Definimos los tipos de poderes posibles
public enum TipoPoder { Nuke, TripleShot, Escudo, Velocidad }

public class ItemDrop : MonoBehaviour
{
    public TipoPoder tipo; // Seleccionable en el Inspector
}