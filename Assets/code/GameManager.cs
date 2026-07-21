using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    [Header("Configuración de Rondas")]
    public int rondaActual = 1;
    public float tiempoDeRonda = 30f; // Cada ronda dura 30 segundos
    private float timerRonda;

    [Header("Referencias a las Casitas")]
    public GameObject[] ganchosCasitas; // Guardaremos las posiciones para revivirlas

    [Header("UI de Mejoras")]
    public GameObject panelMejoras; // Tu panel de Canvas con los botones de mejora

    void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        timerRonda = tiempoDeRonda;
        if (panelMejoras != null) panelMejoras.SetActive(false);
    }

    void Update()
    {
        timerRonda -= Time.deltaTime;
        if (timerRonda <= 0)
        {
            FinDeRonda();
        }
    }

    void FinDeRonda()
    {
        timerRonda = tiempoDeRonda;
        rondaActual++;

        // Abre el menú para elegir mejora
        AbrirMenuMejoras();

        // Cada 3 rondas se regeneran los spawners
        if (rondaActual % 3 == 0)
        {
            RegenerarSpawners();
        }
    }

    void AbrirMenuMejoras()
    {
        if (panelMejoras != null)
        {
            panelMejoras.SetActive(true);
            Time.timeScale = 0f; // Pausa el juego para elegir
        }
    }

    // ==========================================
    // BOTONES DE MEJORA (CONECTAR EN EL UI)
    // ==========================================

    public void MejorarVelocidad()
    {
        // Acá va el código para subirle la velocidad a tu nave permanentemente
        Debug.Log("Velocidad mejorada!");
        CerrarMenuMejoras();
    }

    public void MejorarCadencia()
    {
        // Acá va el código para que dispare más rápido permanentemente
        Debug.Log("Cadencia mejorada!");
        CerrarMenuMejoras();
    }

    public void MejorarDanio()
    {
        // ¡Acá está! Poné acá la lógica para aumentar el daño de tus balas permanentemente
        Debug.Log("Daño mejorado!");
        CerrarMenuMejoras();
    }

    void CerrarMenuMejoras()
    {
        panelMejoras.SetActive(false);
        Time.timeScale = 1f; // Despausa el juego
    }

    void RegenerarSpawners()
    {
        foreach (GameObject casita in ganchosCasitas)
        {
            if (casita != null)
            {
                casita.SetActive(true); // Prende de nuevo las casitas rotas
                if (casita.TryGetComponent<Casita>(out Casita c)) c.vida = 3f;
            }
        }
    }
}